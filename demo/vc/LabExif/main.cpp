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
			case 0x100: name = "�摜�̕�"; break;
			case 0x101: name = "�摜�̍���"; break;
			case 0x102: name = "�r�b�g�̐[��"; break;
			case 0x103: name = "���k�̎��"; break;
			case 0x106: name = "��f����"; break;
			case 0x10E: name = "�^�C�g��"; break;
			case 0x10F: name = "�J�����̃��[�J�["; break;
			case 0x110: name = "�J�����̃��f��"; break;
			case 0x111: name = "���P�[�V����"; break;
			case 0x112: name = "�摜�̕���"; break;
			case 0x115: name = "�R���|�[�l���g��"; break;
			case 0x116: name = "�X�g���b�v���̍s��"; break;
			case 0x117: name = "���k�X�g���b�v���̃o�C�g��"; break;
			case 0x11A: name = "���������̉𑜓x"; break;
			case 0x11B: name = "���������̉𑜓x"; break;
			case 0x11C: name = "�摜�f�[�^�̔z�u"; break;
			case 0x128: name = "�𑜓x�̒P��"; break;
			case 0x12D: name = "Transfer function"; break;
			case 0x131: name = "�\�t�g�E�F�A��"; break;
			case 0x132: name = "�ύX����"; break;
			case 0x13B: name = "�쐬��"; break;
			case 0x13E: name = "���F�_�̐F�x"; break;
			case 0x13F: name = "���F�̐F�x"; break;
			case 0x201: name = "JPEG SOI �I�t�Z�b�g"; break;
			case 0x202: name = "JPEG �f�[�^�I�t�Z�b�g"; break;
			case 0x211: name = "�F��ԕϊ��s��W��"; break;
			case 0x212: name = "Subsampling ration of Y to C"; break;
			case 0x213: name = "Y and C positioning"; break;
			case 0x214: name = "�����_�̊�l"; break;
			case 0x8298: name = "�����"; break;
			case 0x8769: name = "Exif IFD �|�C���^"; break;
			case 0x8825: name = "GPS IFD �|�C���^"; break;
			// Table 18
			case 0x829A: name = "�I������"; break;
			case 0x829D: name = "F �l"; break;
			case 0x8822: name = "�I�o���[�h"; break;
			case 0x8824: name = "�������x"; break;
			case 0x8827: name = "ISO ���x�ݒ�"; break;
			case 0x8828: name = "���d�ϊ��W��"; break;
			case 0x8830: name = "���x���"; break;
			case 0x8831: name = "�W���o�͊��x"; break;
			case 0x8832: name = "�����I���w��"; break;
			case 0x8833: name = "ISO ���x"; break;
			case 0x8834: name = "ISO ���x �ܓx yyy"; break;
			case 0x8835: name = "ISO ���x �ܓx zzz"; break;
			case 0x9000: name = "Exif �o�[�W����"; break;
			case 0x9003: name = "�B�e����"; break;
			case 0x9004: name = "�쐬����"; break;
			case 0x9101: name = "�e�R���|�[�l���g�̈Ӗ�"; break;
			case 0x9102: name = "�摜���k���[�h"; break;
			case 0x9201: name = "�V���b�^�[�X�s�[�h"; break;
			case 0x9202: name = "�i��l"; break;
			case 0x9203: name = "���邳"; break;
			case 0x9204: name = "�I�o�␳"; break;
			case 0x9205: name = "�ő�i��"; break;
			case 0x9206: name = "��ʑ̋���"; break;
			case 0x9207: name = "�������[�h"; break;
			case 0x9208: name = "����"; break;
			case 0x9209: name = "�t���b�V��"; break;
			case 0x920A: name = "�œ_����"; break;
			case 0x9214: name = "��ʑ̖ʐ�"; break;
			case 0x927C: name = "���[�J�[�m�[�g"; break;
			case 0x9286: name = "�R�����g"; break;
			case 0x9290: name = "�쐬�����T�u�b"; break;
			case 0x9291: name = "�쐬�����T�u�b (original)"; break;
			case 0x9292: name = "�쐬�����T�u�b (digitized)"; break;
			case 0xA000: name = "�Ή� FlashPix �o�[�W����"; break;
			case 0xA001: name = "�F��ԏ��"; break;
			case 0xA002: name = "�����摜��"; break;
			case 0xA003: name = "�����摜����"; break;
			case 0xA004: name = "�����t�@�C��"; break;
			case 0xA005: name = "�݊��� IFD �|�C���^"; break;
			case 0xA20B: name = "�t���b�V�����[�h"; break;
			case 0xA20C: name = "��Ԏ��g������"; break;
			case 0xA20E: name = "�œ_�� X �𑜓x"; break;
			case 0xA20F: name = "�œ_�� Y �𑜓x"; break;
			case 0xA210: name = "�œ_�ʉ𑜓x�P��"; break;
			case 0xA214: name = "��ʑ̈ʒu"; break;
			case 0xA215: name = "�I���w�W"; break;
			case 0xA217: name = "�Z���T�[����"; break;
			case 0xA300: name = "�t�@�C���\�[�X"; break;
			case 0xA301: name = "�V�[�����"; break;
			case 0xA302: name = "CFA pattern"; break;
			case 0xA401: name = "�J�X�^���C���[�W����"; break;
			case 0xA402: name = "�I�����[�h"; break;
			case 0xA403: name = "�z���C�g�o�����X"; break;
			case 0xA404: name = "�f�W�^���Y�[��"; break;
			case 0xA405: name = "�œ_�����i35mm ���Z�j"; break;
			case 0xA406: name = "�V�[���B�e���[�h"; break;
			case 0xA407: name = "�Q�C������"; break;
			case 0xA408: name = "�R���g���X�g"; break;
			case 0xA409: name = "�ʓx"; break;
			case 0xA40A: name = "���ēx"; break;
			case 0xA40B: name = "�f�o�C�X�ݒ����"; break;
			case 0xA40C: name = "��ʑ̋����͈�"; break;
			case 0xA420: name = "Unique image ID"; break;
			case 0xA430: name = "�J�������L��"; break;
			case 0xA431: name = "�J���������ԍ�"; break;
			case 0xA432: name = "�����Y�d�l"; break;
			case 0xA433: name = "�����Y���[�J�["; break;
			case 0xA434: name = "�����Y���f��"; break;
			case 0xA435: name = "�����Y�����ԍ�"; break;
			case 0xA500: name = "�K���}�l"; break;
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