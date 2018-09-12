/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#if defined(_MSC_VER)
	#include "stdafx.h"
#else
	#include <stdio.h>
	#include <stdlib.h>
	#include <stddef.h>
#endif

#include "xie_core.h"
#include "xie_high.h"
#include "xie.h"

#include "Core/xie_core_math.h"

void test01();
void dump(const xie::CxExif& exif);
void modify(xie::CxExif exif, xie::TxSizeI size);
xie::CxStringA GetNickName(unsigned short id, int mode = 1);

// ============================================================
/*!
	@brief	EntryPoint
*/
int main(int argc, const char* argv[])
{
	xie::Axi::Setup();

	#if defined(_MSC_VER)
	::CreateDirectoryA( "Results", NULL );
	#endif

	try
	{
		test01();
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}

	xie::Axi::TearDown();

	return 0;
}

// ============================================================
/*!
	@brief	How to use Exif
*/
void test01()
{
	printf("%-20s:\n", __FUNCTION__);

	LPCSTR filenames[] = 
	{
		"asahi.jpg",
		"fern.jpg",
		"floppy1.jpg",
		"hamster.jpg",
		"iris_japonica.jpg",
		"zaurus1.jpg",
		NULL
	};

	for(int i=0 ; filenames[i]!=NULL ; i++)
	{
		auto filename = filenames[i];
		auto filepath = xie::CxString::Format("TestFiles/%s", filename);

		printf("%s\n", filename);

		xie::CxStopwatch watch;
		xie::CxImage src(filepath, false);
		xie::CxImage dst;

		auto mag_x = 320.0 / src.Width();
		auto mag_y = 240.0 / src.Height();
		auto mag = xie::Axi::Min(mag_x, mag_y);
		dst.Filter().Scale(src, mag, mag, 2);

		watch.Start();
		auto src_exif = xie::CxExif::FromTag(src.Exif());
		auto dst_exif = src_exif.GetPurgedExif();
		watch.Stop();
		printf("%-20s: %9.3f msec\n", __FUNCTION__, watch.Lap);

		dump(dst_exif);
		modify(dst_exif, dst.Size());

		dst.Exif(dst_exif.Tag());
		dst.Save(xie::CxString::Format("Results/%s", filename));
	}
}

// ============================================================
/*!
	@brief	How to use Exif (GetValue)
*/
template<class TE> xie::CxString format(xie::TxScanner1D<TE>& scan)
{
	xie::CxString strValue;
	if (scan.Model.Pack == 1)
	{
		scan.ForEach([&strValue](int x, TE* addr)
		{
			if (x == 0)
				strValue += xie::CxString::Format("%d", *addr);
			else
				strValue += xie::CxString::Format(",%d", *addr);
		});
	}
	else if (scan.Model.Pack == 2)
	{
		scan.ForEach([&strValue](int x, TE* addr)
		{
			if (x == 0)
				strValue += xie::CxString::Format("%d/%d", addr[0], addr[1]);
			else
				strValue += xie::CxString::Format(",%d/%d", addr[0], addr[1]);
		});
	}
	return strValue;
}

// ============================================================
/*!
	@brief	How to use Exif (GetItems, GetValue)
*/
void dump(const xie::CxExif& exif)
{
	auto items = exif.GetItems();

	for(int k=0 ; k<items.Length() ; k++)
	{
		auto item = items[k];
		auto nickname = GetNickName(item.ID);
		xie::CxString strValue = "";

		switch(item.Type)
		{
		case 1:		// BYTE
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned char>());
			}
			break;
		case 7:		// UNDEFINED
			switch(item.ID)
			{
			case 0x9000:	// Exif version
			case 0xA000:	// Supported FlashPix version
				{
					xie::CxArray value;
					exif.GetValue(item, value);
					auto scan = value.Scanner<unsigned char>();
					if (scan.Length == 4)
						strValue = xie::CxString::Format("%c%c%c%c", scan[0], scan[1], scan[2], scan[3]);
				}
				break;
			}
			break;
		case 2:		// ASCII
			{
				xie::CxStringA value;
				exif.GetValue(item, value);
				strValue = value;
			}
			break;
		case 3:		// SHORT
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned short>());
			}
			break;
		case 4:		// LONG
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned int>());
			}
			break;
		case 9:		// SLONG 32bit
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<int>());
			}
			break;
		case 5:		// RATIONAL
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<unsigned int>());
			}
			break;
		case 10:	// SRATIONAL
			{
				xie::CxArray value;
				exif.GetValue(item, value);
				strValue = format(value.Scanner<int>());
			}
			break;
		}

		printf("[%08x] %04x %2d count=%6d value=%12d (%08x) [%-32s] %s\n", item.Offset, item.ID, item.Type, item.Count, item.ValueOrIndex, item.ValueOrIndex, (LPCSTR)strValue, (LPCSTR)nickname);
	}
}

// ============================================================
/*!
	@brief	How to use Exif
*/
void modify(xie::CxExif exif, xie::TxSizeI size)
{
	auto items = exif.GetItems();

	for(int k=0 ; k<items.Length() ; k++)
	{
		auto item = items[k];

		switch (item.ID)
		{
			case 0x0132:	// File change date and time
				if (item.Type == 2 && item.Count >= 19)
				{
					auto now = xie::TxDateTime::Now(true);
					auto value = xie::CxString::Format(
						"%04d:%02d:%02d %02d:%02d:%02d",
						now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
					exif.SetValue(item, value);
				}
				break;
			case 0xA002:	// Valid image width
				if (item.Type == 3 && item.Count == 1)
				{
					typedef unsigned short TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)size.Width;
					exif.SetValue(item, value);
				}
				else if (item.Type == 4 && item.Count == 1)
				{
					typedef unsigned int TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)size.Width;
					exif.SetValue(item, value);
				}
				break;
			case 0xA003:	// Valid image height
				if (item.Type == 3 && item.Count == 1)
				{
					typedef unsigned short TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)size.Height;
					exif.SetValue(item, value);
				}
				else if (item.Type == 4 && item.Count == 1)
				{
					typedef unsigned int TE;
					xie::CxArray value(1, xie::ModelOf<TE>());
					auto ptr = (TE*)value.Address();
					ptr[0] = (TE)size.Height;
					exif.SetValue(item, value);
				}
				break;
		}
	}
}

// ============================================================
/*!
	@brief	NickName
*/
xie::CxStringA GetNickName(unsigned short id, int mode)
{
	xie::CxStringA name = "";

	if (mode == 1)
	{
		switch (id)
		{
			// 4.6.8 Tag Support Levels
			// Table 17
			case 0x100: name = "画像の幅"; break;
			case 0x101: name = "画像の高さ"; break;
			case 0x102: name = "ビットの深さ"; break;
			case 0x103: name = "圧縮の種類"; break;
			case 0x106: name = "画素合成"; break;
			case 0x10E: name = "タイトル"; break;
			case 0x10F: name = "カメラのメーカー"; break;
			case 0x110: name = "カメラのモデル"; break;
			case 0x111: name = "ロケーション"; break;
			case 0x112: name = "画像の方向"; break;
			case 0x115: name = "コンポーネント数"; break;
			case 0x116: name = "ストリップ毎の行数"; break;
			case 0x117: name = "圧縮ストリップ毎のバイト数"; break;
			case 0x11A: name = "水平方向の解像度"; break;
			case 0x11B: name = "垂直方向の解像度"; break;
			case 0x11C: name = "画像データの配置"; break;
			case 0x128: name = "解像度の単位"; break;
			case 0x12D: name = "Transfer function"; break;
			case 0x131: name = "ソフトウェア名"; break;
			case 0x132: name = "変更日時"; break;
			case 0x13B: name = "作成者"; break;
			case 0x13E: name = "白色点の色度"; break;
			case 0x13F: name = "原色の色度"; break;
			case 0x201: name = "JPEG SOI オフセット"; break;
			case 0x202: name = "JPEG データオフセット"; break;
			case 0x211: name = "色空間変換行列係数"; break;
			case 0x212: name = "Subsampling ration of Y to C"; break;
			case 0x213: name = "Y and C positioning"; break;
			case 0x214: name = "黒白点の基準値"; break;
			case 0x8298: name = "著作者"; break;
			case 0x8769: name = "Exif IFD ポインタ"; break;
			case 0x8825: name = "GPS IFD ポインタ"; break;
			// Table 18
			case 0x829A: name = "露光時間"; break;
			case 0x829D: name = "F 値"; break;
			case 0x8822: name = "露出モード"; break;
			case 0x8824: name = "分光感度"; break;
			case 0x8827: name = "ISO 感度設定"; break;
			case 0x8828: name = "光電変換係数"; break;
			case 0x8830: name = "感度種別"; break;
			case 0x8831: name = "標準出力感度"; break;
			case 0x8832: name = "推奨露光指数"; break;
			case 0x8833: name = "ISO 感度"; break;
			case 0x8834: name = "ISO 感度 緯度 yyy"; break;
			case 0x8835: name = "ISO 感度 緯度 zzz"; break;
			case 0x9000: name = "Exif バージョン"; break;
			case 0x9003: name = "撮影日時"; break;
			case 0x9004: name = "作成日時"; break;
			case 0x9101: name = "各コンポーネントの意味"; break;
			case 0x9102: name = "画像圧縮モード"; break;
			case 0x9201: name = "シャッタースピード"; break;
			case 0x9202: name = "絞り値"; break;
			case 0x9203: name = "明るさ"; break;
			case 0x9204: name = "露出補正"; break;
			case 0x9205: name = "最大絞り"; break;
			case 0x9206: name = "被写体距離"; break;
			case 0x9207: name = "測光モード"; break;
			case 0x9208: name = "光源"; break;
			case 0x9209: name = "フラッシュ"; break;
			case 0x920A: name = "焦点距離"; break;
			case 0x9214: name = "被写体面積"; break;
			case 0x927C: name = "メーカーノート"; break;
			case 0x9286: name = "コメント"; break;
			case 0x9290: name = "作成日時サブ秒"; break;
			case 0x9291: name = "作成日時サブ秒 (original)"; break;
			case 0x9292: name = "作成日時サブ秒 (digitized)"; break;
			case 0xA000: name = "対応 FlashPix バージョン"; break;
			case 0xA001: name = "色空間情報"; break;
			case 0xA002: name = "実効画像幅"; break;
			case 0xA003: name = "実効画像高さ"; break;
			case 0xA004: name = "音声ファイル"; break;
			case 0xA005: name = "互換性 IFD ポインタ"; break;
			case 0xA20B: name = "フラッシュモード"; break;
			case 0xA20C: name = "空間周波数応答"; break;
			case 0xA20E: name = "焦点面 X 解像度"; break;
			case 0xA20F: name = "焦点面 Y 解像度"; break;
			case 0xA210: name = "焦点面解像度単位"; break;
			case 0xA214: name = "被写体位置"; break;
			case 0xA215: name = "露光指標"; break;
			case 0xA217: name = "センサー方式"; break;
			case 0xA300: name = "ファイルソース"; break;
			case 0xA301: name = "シーン種別"; break;
			case 0xA302: name = "CFA pattern"; break;
			case 0xA401: name = "カスタムイメージ処理"; break;
			case 0xA402: name = "露光モード"; break;
			case 0xA403: name = "ホワイトバランス"; break;
			case 0xA404: name = "デジタルズーム"; break;
			case 0xA405: name = "焦点距離（35mm 換算）"; break;
			case 0xA406: name = "シーン撮影モード"; break;
			case 0xA407: name = "ゲイン調整"; break;
			case 0xA408: name = "コントラスト"; break;
			case 0xA409: name = "彩度"; break;
			case 0xA40A: name = "明瞭度"; break;
			case 0xA40B: name = "デバイス設定説明"; break;
			case 0xA40C: name = "被写体距離範囲"; break;
			case 0xA420: name = "Unique image ID"; break;
			case 0xA430: name = "カメラ所有者"; break;
			case 0xA431: name = "カメラ製造番号"; break;
			case 0xA432: name = "レンズ仕様"; break;
			case 0xA433: name = "レンズメーカー"; break;
			case 0xA434: name = "レンズモデル"; break;
			case 0xA435: name = "レンズ製造番号"; break;
			case 0xA500: name = "ガンマ値"; break;
		}
	}
	else
	{
		switch (id)
		{
			// 4.6.8 Tag Support Levels
			// Table 17
			case 0x100: name = "Image width"; break;
			case 0x101: name = "Image height"; break;
			case 0x102: name = "Number of bits per component"; break;
			case 0x103: name = "Compression scheme"; break;
			case 0x106: name = "Pixel composition"; break;
			case 0x10E: name = "Image title"; break;
			case 0x10F: name = "Manufacturer of image input equipment"; break;
			case 0x110: name = "Model of image input equipment"; break;
			case 0x111: name = "Image data location"; break;
			case 0x112: name = "Orientation of image"; break;
			case 0x115: name = "Number of components"; break;
			case 0x116: name = "Number of rows per strip"; break;
			case 0x117: name = "Bytes per compressed strip"; break;
			case 0x11A: name = "Image resolution in width direction"; break;
			case 0x11B: name = "Image resolution in height direction"; break;
			case 0x11C: name = "Image data arrangement"; break;
			case 0x128: name = "Unit of X and Y resolution"; break;
			case 0x12D: name = "Transfer function"; break;
			case 0x131: name = "Software used"; break;
			case 0x132: name = "File change date and time"; break;
			case 0x13B: name = "Person who created the image"; break;
			case 0x13E: name = "White point chromaticity"; break;
			case 0x13F: name = "Chromaticities of primaries"; break;
			case 0x201: name = "Offset to JPEG SOI"; break;
			case 0x202: name = "Offset to JPEG data"; break;
			case 0x211: name = "Color space transformation matrix coefficients"; break;
			case 0x212: name = "Subsampling ration of Y to C"; break;
			case 0x213: name = "Y and C positioning"; break;
			case 0x214: name = "Pair of black and white reference values"; break;
			case 0x8298: name = "Copyright holder"; break;
			case 0x8769: name = "Exif IFD pointer"; break;
			case 0x8825: name = "GPS IFD pointer"; break;
			// Table 18
			case 0x829A: name = "Exposure time"; break;
			case 0x829D: name = "F number"; break;
			case 0x8822: name = "Exposure program"; break;
			case 0x8824: name = "Spectral sensitivity"; break;
			case 0x8827: name = "ISO speed ratings"; break;
			case 0x8828: name = "Optoelectric conversion factor"; break;
			case 0x8830: name = "Sensitivity Type"; break;
			case 0x8831: name = "Standard Output Sensitivity"; break;
			case 0x8832: name = "Recommended Exposure Index"; break;
			case 0x8833: name = "ISO Speed"; break;
			case 0x8834: name = "ISO Speed Latitude yyy"; break;
			case 0x8835: name = "ISO Speed Latitude zzz"; break;
			case 0x9000: name = "Exif version"; break;
			case 0x9003: name = "Date and time of original data generation"; break;
			case 0x9004: name = "Date and time of digital data generation"; break;
			case 0x9101: name = "Meaning of each component"; break;
			case 0x9102: name = "Image compression mode"; break;
			case 0x9201: name = "Shutter speed"; break;
			case 0x9202: name = "Aperture"; break;
			case 0x9203: name = "Brightness"; break;
			case 0x9204: name = "Exposure bias"; break;
			case 0x9205: name = "Maximul lens aperture"; break;
			case 0x9206: name = "Subject distance"; break;
			case 0x9207: name = "Metering mode"; break;
			case 0x9208: name = "Light source"; break;
			case 0x9209: name = "Flash"; break;
			case 0x920A: name = "Lens focal length"; break;
			case 0x9214: name = "Subject area"; break;
			case 0x927C: name = "Manufacturer note"; break;
			case 0x9286: name = "User comments"; break;
			case 0x9290: name = "Date time subseconds"; break;
			case 0x9291: name = "Date time original subseconds"; break;
			case 0x9292: name = "Date time digitized subseconds"; break;
			case 0xA000: name = "Supported Flashpix version"; break;
			case 0xA001: name = "Color space information"; break;
			case 0xA002: name = "Valid image width"; break;
			case 0xA003: name = "Valid image height"; break;
			case 0xA004: name = "Related audio file"; break;
			case 0xA005: name = "Interoperatibility IFD pointer"; break;
			case 0xA20B: name = "Flash energy"; break;
			case 0xA20C: name = "Spatial frequency response"; break;
			case 0xA20E: name = "Focal plane X resolution"; break;
			case 0xA20F: name = "Focal plane Y resolution"; break;
			case 0xA210: name = "Focal plane resolution unit"; break;
			case 0xA214: name = "Subject location"; break;
			case 0xA215: name = "Exposure index"; break;
			case 0xA217: name = "Sensing method"; break;
			case 0xA300: name = "File source"; break;
			case 0xA301: name = "Scene type"; break;
			case 0xA302: name = "CFA pattern"; break;
			case 0xA401: name = "Custom image processing"; break;
			case 0xA402: name = "Exposure mode"; break;
			case 0xA403: name = "White balance"; break;
			case 0xA404: name = "Digital zoom ratio"; break;
			case 0xA405: name = "Focal length in 35mm film"; break;
			case 0xA406: name = "Scene capture type"; break;
			case 0xA407: name = "Gain control"; break;
			case 0xA408: name = "Contrast"; break;
			case 0xA409: name = "Saturation"; break;
			case 0xA40A: name = "Sharpness"; break;
			case 0xA40B: name = "Device settings description"; break;
			case 0xA40C: name = "Subject distance range"; break;
			case 0xA420: name = "Unique image ID"; break;
			case 0xA430: name = "Camera Owner Name"; break;
			case 0xA431: name = "Body Serial Number"; break;
			case 0xA432: name = "Lens Specification"; break;
			case 0xA433: name = "Lens Make"; break;
			case 0xA434: name = "Lens Model"; break;
			case 0xA435: name = "Lens Serial Number"; break;
			case 0xA500: name = "Gamma"; break;
		}
	}

	return name;
}