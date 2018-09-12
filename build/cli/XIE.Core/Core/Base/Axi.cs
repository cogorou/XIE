/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using XIE.Ptr;

namespace XIE
{
	/// <summary>
	/// 補助関数群
	/// </summary>
	public static partial class Axi : System.Object
	{
		#region コンストラクタ: 

		/// <summary>
		/// スタティックコンストラクタ
		/// </summary>
		static Axi()
		{
		}

		#endregion

		#region 初期化と解放:

		/// <summary>
		/// ライブラリの初期化
		/// </summary>
		/// <param name="options">オプション。(未使用)</param>
		public static void Setup(params object[] options)
		{
			xie_core.xie_core_setup();
			xie_high.xie_high_setup_ex("File");
			xie_high.xie_high_setup_ex("GDI");
			xie_high.xie_high_setup_ex("IO");
			xie_high.xie_high_setup_ex("Net");
			xie_high.xie_high_setup_ex("Media");
		}

		/// <summary>
		/// ライブラリの解放
		/// </summary>
		public static void TearDown()
		{
			xie_high.xie_high_teardown();
			xie_core.xie_core_teardown();
		}

		#endregion

		#region デバッガ:

		/// <summary>
		/// トレースの ON/OFF の設定
		/// </summary>
		/// <param name="value">トレースの ON/OFF [0:OFF,1~:ON]</param>
		public static void TraceLevel(int value)
		{
			xie_core.fnXIE_Core_TraceLevel_Set(value);
		}

		/// <summary>
		/// トレースの ON/OFF の設定
		/// </summary>
		/// <returns>
		///		現在設定されている値を返します。
		///		0 はトレース OFF を示します。
		///		1 以上はトレース ON を示します。
		///		既定では 0 です。
		/// </returns>
		public static int TraceLevel()
		{
			return xie_core.fnXIE_Core_TraceLevel_Get();
		}

		#endregion

		#region AppDomain 関連. (アセンブリまたは型の解決)

		/// <summary>
		/// デシリアライズ時にアセンブリの解決が失敗したときに発生します。(seealso:AppDomain.CurrentDomain.AssemblyResolve)
		/// </summary>
		/// <param name="sender">送信元</param>
		/// <param name="args">アセンブリ名</param>
		/// <returns>
		///		指定されたアセンブリ名に該当するアセンブリを返します。
		/// </returns>
		static Assembly AssemblyResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (name == assembly.FullName.Split(',')[0])
					return assembly;
			}
			return null;
		}

		/// <summary>
		/// Type.GetType() で型の解決が失敗したときに発生します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		/// <returns>
		///		見つかったアセンブリを返します。
		/// </returns>
		/// <remarks>
		/// この処理は、下記で示されている方法とは異なります。
		/// <![CDATA[
		/// http://msdn.microsoft.com/ja-jp/library/system.appdomain.typeresolve(v=vs.80).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-4
		/// ]]>
		/// </remarks>
		static Assembly TypeResolving(object sender, ResolveEventArgs args)
		{
			string name = args.Name.Split(',')[0];
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (name == type.FullName)
						return assembly;
				}
			}
			return null;
		}

		#endregion

		#region 比較.

		/// <summary>
		/// ２つのオブジェクトの内容を比較します。
		/// </summary>
		/// <param name="left">左辺値</param>
		/// <param name="right">右辺値</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public static bool ContentEquals(object left, object right)
		{
			if (ReferenceEquals(left, null))
				return ReferenceEquals(right, null);
			else if (ReferenceEquals(right, null))
				return false;
			if (ReferenceEquals(left, right)) return true;
			if (left.GetType().IsInstanceOfType(right) == false) return false;
			if (left is IxEquatable)
				return ((IxEquatable)left).ContentEquals(right);
			return left.Equals(right);
		}

		#endregion

		#region 単純計算.

		/// <summary>
		/// Degree から Radian に変換します。
		/// </summary>
		/// <param name="degree">Degree 単位の角度</param>
		/// <returns>
		///		変換した結果を返します。
		/// </returns>
		public static double DegToRad(double degree)
		{
			return degree * System.Math.PI / 180;
		}

		/// <summary>
		/// Radian から Degree に変換します。
		/// </summary>
		/// <param name="radian">Radian 単位の角度</param>
		/// <returns>
		///		変換した結果を返します。
		/// </returns>
		public static double RadToDeg(double radian)
		{
			return radian * 180 / System.Math.PI;
		}

		/// <summary>
		/// 座標の回転
		/// </summary>
		/// <param name="src">回転対象の座標</param>
		/// <param name="axis">回転の機軸</param>
		/// <param name="angle">回転角 (degree)</param>
		/// <returns>
		///		回転後の座標を返します。
		/// </returns>
		public static TxPointD Rotate(TxPointD src, TxPointD axis, double angle)
		{
			var dst = new TxPointD();
			var R = angle * System.Math.PI / 180;
			var dx = src.X - axis.X;
			var dy = src.Y - axis.Y;
			dst.X = axis.X + (dx * System.Math.Cos(R)) - (dy * System.Math.Sin(R));
			dst.Y = axis.Y + (dx * System.Math.Sin(R)) + (dy * System.Math.Cos(R));
			return dst;
		}

		#endregion

		#region 変換系: (PointerOf)

		/// <summary>
		/// ExType に対応するポインタ型の Type の取得
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		ポインタ型の Type を返します。<br/>
		///		該当しない場合は null を返します。
		/// </returns>
		public static Type PointerOf(ExType type)
		{
			switch (type)
			{
				case ExType.U8: return typeof(BytePtr);
				case ExType.U16: return typeof(UInt16Ptr);
				case ExType.U32: return typeof(UInt32Ptr);
				case ExType.U64: return typeof(UInt64Ptr);
				case ExType.S8: return typeof(SBytePtr);
				case ExType.S16: return typeof(Int16Ptr);
				case ExType.S32: return typeof(Int32Ptr);
				case ExType.S64: return typeof(Int64Ptr);
				case ExType.F32: return typeof(SinglePtr);
				case ExType.F64: return typeof(DoublePtr);
			}
			return null;
		}

		/// <summary>
		/// Type に対応するポインタ型の Type の取得
		/// </summary>
		/// <param name="type">型 (Primitive 型または既知の構造体のみサポートします)</param>
		/// <returns>
		///		ポインタ型の Type を返します。<br/>
		///		該当しない場合は null を返します。
		/// </returns>
		public static Type PointerOf(Type type)
		{
			if (type == typeof(Byte)) return typeof(BytePtr);
			if (type == typeof(SByte)) return typeof(SBytePtr);
			if (type == typeof(Int16)) return typeof(Int16Ptr);
			if (type == typeof(Int32)) return typeof(Int32Ptr);
			if (type == typeof(Int64)) return typeof(Int64Ptr);
			if (type == typeof(UInt16)) return typeof(UInt16Ptr);
			if (type == typeof(UInt32)) return typeof(UInt32Ptr);
			if (type == typeof(UInt64)) return typeof(UInt64Ptr);
			if (type == typeof(Single)) return typeof(SinglePtr);
			if (type == typeof(Double)) return typeof(DoublePtr);
			if (type == typeof(Boolean)) return typeof(SBytePtr);
			if (type == typeof(TxRGB8x3)) return typeof(TxRGB8x3Ptr);
			if (type == typeof(TxRGB8x4)) return typeof(TxRGB8x4Ptr);
			if (type == typeof(TxBGR8x3)) return typeof(TxBGR8x3Ptr);
			if (type == typeof(TxBGR8x4)) return typeof(TxBGR8x4Ptr);
			if (type == typeof(TxCircleD)) return typeof(TxCircleDPtr);
			if (type == typeof(TxCircleI)) return typeof(TxCircleIPtr);
			if (type == typeof(TxEllipseD)) return typeof(TxEllipseDPtr);
			if (type == typeof(TxEllipseI)) return typeof(TxEllipseIPtr);
			if (type == typeof(TxLineD)) return typeof(TxLineDPtr);
			if (type == typeof(TxLineI)) return typeof(TxLineIPtr);
			if (type == typeof(TxLineSegmentD)) return typeof(TxLineSegmentDPtr);
			if (type == typeof(TxLineSegmentI)) return typeof(TxLineSegmentIPtr);
			if (type == typeof(TxPointD)) return typeof(TxPointDPtr);
			if (type == typeof(TxPointI)) return typeof(TxPointIPtr);
			if (type == typeof(TxRangeD)) return typeof(TxRangeDPtr);
			if (type == typeof(TxRangeI)) return typeof(TxRangeIPtr);
			if (type == typeof(TxRectangleD)) return typeof(TxRectangleDPtr);
			if (type == typeof(TxRectangleI)) return typeof(TxRectangleIPtr);
			if (type == typeof(TxSizeD)) return typeof(TxSizeDPtr);
			if (type == typeof(TxSizeI)) return typeof(TxSizeIPtr);
			if (type == typeof(TxStatistics)) return typeof(TxStatisticsPtr);
			if (type == typeof(TxTrapezoidD)) return typeof(TxTrapezoidDPtr);
			if (type == typeof(TxTrapezoidI)) return typeof(TxTrapezoidIPtr);
			return null;
		}

		#endregion

		#region 変換系: (TypeOf)

		/// <summary>
		/// ExType に対応する Type の取得
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		Type を返します。<br/>
		///		該当しない場合は null を返します。
		/// </returns>
		public static Type TypeOf(ExType type)
		{
			switch (type)
			{
				case ExType.Ptr: return typeof(IntPtr);
				case ExType.U8: return typeof(Byte);
				case ExType.U16: return typeof(UInt16);
				case ExType.U32: return typeof(UInt32);
				case ExType.U64: return typeof(UInt64);
				case ExType.S8: return typeof(SByte);
				case ExType.S16: return typeof(Int16);
				case ExType.S32: return typeof(Int32);
				case ExType.S64: return typeof(Int64);
				case ExType.F32: return typeof(Single);
				case ExType.F64: return typeof(Double);
			}
			return null;
		}

		/// <summary>
		/// Type に対応する ExType の取得
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		ExType を返します。<br/>
		///		該当しない場合は ExType.None を返します。
		/// </returns>
		public static ExType TypeOf(Type type)
		{
			if (type == null) return ExType.None;
			if (type == typeof(IntPtr)) return ExType.Ptr;
			if (type == typeof(HxModule)) return ExType.Ptr;
			if (type == typeof(SByte)) return ExType.S8;
			if (type == typeof(Byte)) return ExType.U8;
			if (type == typeof(Int16)) return ExType.S16;
			if (type == typeof(UInt16)) return ExType.U16;
			if (type == typeof(Int32)) return ExType.S32;
			if (type == typeof(UInt32)) return ExType.U32;
			if (type == typeof(Int64)) return ExType.S64;
			if (type == typeof(UInt64)) return ExType.U64;
			if (type == typeof(Single)) return ExType.F32;
			if (type == typeof(Double)) return ExType.F64;
			if (type == typeof(Boolean)) return ExType.S8;
			return ExType.None;
		}

		#endregion

		#region 変換系: (IEnumerable)

		/// <summary>
		/// 指定された列挙型の要素数を取得します。
		/// </summary>
		/// <param name="src">列挙型</param>
		/// <returns>
		///		指定された列挙型の要素数を返します。\n
		/// </returns>
		internal static int GetElementCount(System.Collections.IEnumerable src)
		{
			#region 非ジェネリックの要素数は Count() では取得できない.
			int src_count = 0;
			var src_iter = src.GetEnumerator();
			while (src_iter.MoveNext())
				src_count++;
			return src_count;
			#endregion
		}

		#endregion

		#region Memory Alloc/Free:

		/// <summary>
		/// ヒープメモリの確保
		/// </summary>
		/// <param name="size">要素のサイズ [1~]</param>
		/// <param name="zero_clear">0 初期化の指示</param>
		/// <returns>
		///		確保したヒープメモリの先頭アドレスを返します。<br/>
		///		使用後は MemoryFree で解放する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.Axi.BufferFree(System.IntPtr)"/>
		public static IntPtr MemoryAlloc(SIZE_T size, bool zero_clear = false)
		{
			return xie_core.fnXIE_Core_Axi_MemoryAlloc(size, zero_clear ? ExBoolean.True : ExBoolean.False);
		}

		/// <summary>
		/// ヒープメモリの解放
		/// </summary>
		/// <param name="ptr">ヒープメモリの先頭アドレス</param>
		/// <seealso cref="M:XIE.Axi.MemoryAlloc(XIE.SIZE_T,System.Boolean)"/>
		public static void MemoryFree(IntPtr ptr)
		{
			xie_core.fnXIE_Core_Axi_MemoryFree(ptr);
		}

		#endregion

		#region Memory Map/Unmap

		/// <summary>
		/// メモリマップ
		/// </summary>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		確保したメモリの先頭アドレスを返します。
		///		使用後は MemoryUnmap で解放する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.Axi.MemoryMap(System.IntPtr,XIE.SIZE_T)"/>
		public static IntPtr MemoryMap(SIZE_T size)
		{
			return xie_core.fnXIE_Core_Axi_MemoryMap(size);
		}

		/// <summary>
		/// メモリマップ解放
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		public static void MemoryUnmap(IntPtr ptr, SIZE_T size)
		{
			xie_core.fnXIE_Core_Axi_MemoryUnmap(ptr, size);
		}

		#endregion

		#region Memory Lock/Unlock

		/// <summary>
		/// メモリロック
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		正常の場合は 0 を返します。異常の場合はそれ以外を返します。
		///		使用後は MemoryUnlock で解除する必要があります。
		/// </returns>
		/// <seealso cref="M:XIE.Axi.MemoryUnlock(System.IntPtr,XIE.SIZE_T)"/>
		public static int MemoryLock(IntPtr ptr, SIZE_T size)
		{
			return xie_core.fnXIE_Core_Axi_MemoryLock(ptr, size);
		}

		/// <summary>
		/// メモリロック解除
		/// </summary>
		/// <param name="ptr">領域の先頭アドレス</param>
		/// <param name="size">領域のサイズ [1~]</param>
		/// <returns>
		///		正常の場合は 0 を返します。異常の場合はそれ以外を返します。
		/// </returns>
		public static int MemoryUnlock(IntPtr ptr, SIZE_T size)
		{
			return xie_core.fnXIE_Core_Axi_MemoryUnlock(ptr, size);
		}

		#endregion

		#region Model:

		/// <summary>
		/// 型のサイズ (bytes) の計算
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		指定された型のサイズ (bytes) を返します。
		/// </returns>
		public static int SizeOf(ExType type)
		{
			return xie_core.fnXIE_Core_Axi_SizeOf(type);
		}

		/// <summary>
		/// 型のサイズ (bits) の計算
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		指定された型のサイズ (bits) を返します。
		/// </returns>
		public static int CalcBpp(ExType type)
		{
			return xie_core.fnXIE_Core_Axi_CalcBpp(type);
		}

		/// <summary>
		/// ビット深度の計算
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>
		///		指定された型が表わすことができる最大のビット深度を計算します。<br/>
		///		対応しない型が指定された場合は 0 を返します。<br/>
		/// </returns>
		public static int CalcDepth(ExType type)
		{
			return xie_core.fnXIE_Core_Axi_CalcDepth(type);
		}

		/// <summary>
		/// 型のレンジの計算
		/// </summary>
		/// <param name="type">型</param>
		/// <param name="depth">ビット深度 (bits) [範囲:0=最大値、1~=指定値]</param>
		/// <returns>
		///		指定された型が表わすことができる値の範囲を計算します。<br/>
		///		depth に指定された値が範囲外の場合は最大値に補正します。<br/>
		///		対応しない型が指定された場合は上下限共に 0 になります。<br/>
		/// </returns>
		///	<remarks>
		///		上下限値は、要素の型が符号無しか符号有りかによって異なります。<br/>
		///		例えば、U16 の depth=15 は 0~32767 、S16 の depth=15 は -32767~32767 となります。<br/>
		///		S16 (System.Int16) の最小値は -32768 ですが、画像の濃度値としては範囲外ですので、
		///		処理の過程で -32767 に飽和されることになります。<br/>
		///		<br/>
		///		<list type="table">
		///			<listheader>
		///				<term>符号</term>
		///				<lower>lower</lower>
		///				<upper>upper</upper>
		///			</listheader>
		///			<item>
		///				<term>無し</term>
		///				<lower>0</lower>
		///				<upper>+(2<sup>depth</sup> -1)</upper>
		///			</item>
		///			<item>
		///				<term>有り</term>
		///				<lower>-(2<sup>depth</sup> -1)</lower>
		///				<upper>+(2<sup>depth</sup> -1)</upper>
		///			</item>
		///		</list>
		///	</remarks>
		public static TxRangeD CalcRange(ExType type, int depth)
		{
			TxRangeD result = new TxRangeD();
			xie_core.fnXIE_Core_Axi_CalcRange(type, depth, out result);
			return result;
		}

		/// <summary>
		/// 濃度値のスケーリング値の計算
		/// </summary>
		/// <param name="src_type">入力画像の要素の型</param>
		/// <param name="src_depth">入力画像のビット深度 (bits) [0=最大値、1~=指定値]</param>
		/// <param name="dst_type">出力画像の要素の型</param>
		/// <param name="dst_depth">出力画像のビット深度 (bits) [0=最大値、1~=指定値]</param>
		/// <returns>
		///		ビット深度が異なる画像間で濃度値をスケーリングする際の倍率を計算して返します。<br/>
		///		範囲外の値が指定された場合は最大のビット深度に補正します。<br/>
		///	</returns>
		///	<remarks>
		///		下式で計算します。<br/>
		///		式) scale = (2<sup>dst_depth</sup> -1) / (2<sup>src_depth</sup> -1) <br/>
		///		<br/>
		///		例えば、depth=8 を depth=10 にスケーリングする場合は、src_depth=8、dst_depth=10 を指定してください。<br/>
		///		下記のように計算して 4.011764705882 を返します。<br/>
		///		<pre>
		///		src_max = 2<sup>8</sup> -1 … 255
		///		dst_max = 2<sup>10</sup> -1 … 1023
		///		scale = dst_max / srcmax … 4.011764705882
		///		</pre>
		///		濃度値をスケーリングする場合は、dst(ch,y,x) = src(ch,y,x) * scale とします。<br/>
		/// </remarks>
		public static double CalcScale(ExType src_type, int src_depth, ExType dst_type, int dst_depth)
		{
			return xie_core.fnXIE_Core_Axi_CalcScale(src_type, src_depth, dst_type, dst_depth);
		}

		/// <summary>
		/// ２次元領域の水平方向サイズ (bytes) の計算
		/// </summary>
		/// <param name="model">要素モデル</param>
		/// <param name="width">幅 (pixels)</param>
		/// <param name="packing_size">パッキングサイズ (bytes) [1,2,4,8,16]</param>
		/// <returns>
		///		指定された要素モデル(型×パック数)と幅から計算されたサイズ (bytes) が 
		///		packing_size で割り切れるように
		///		パディングを含めたサイズ (bytes) を計算します。<br/>
		///		対応しない型が指定された場合は 0 を返します。<br/>
		///		CxImage の水平方向サイズは 4 bytes (Axi.XIE_IMAGE_PACKING_SIZE) でパッキングされています。
		/// </returns>
		public static int CalcStride(TxModel model, int width, int packing_size)
		{
			return xie_core.fnXIE_Core_Axi_CalcStride(model, width, packing_size);
		}

		#endregion

		#region MP:

		/// <summary>
		/// 論理プロセッサー数の取得
		/// </summary>
		/// <returns>
		///		論理プロセッサー数 (1~) を返します。
		///		失敗した場合は例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public static int ProcessorNum()
		{
			int num = 0;
			ExStatus status = xie_core.fnXIE_Core_Axi_ProcessorNum_Get(out num);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return num;
		}

		/// <summary>
		/// 並列化処理の並列数の取得
		/// </summary>
		/// <returns>
		///		並列数 (1~) を返します。
		///		失敗した場合は例外を発行します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public static int ParallelNum()
		{
			int num = 0;
			ExStatus status = xie_core.fnXIE_Core_Axi_ParallelNum_Get(out num);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return num;
		}

		/// <summary>
		/// 並列化処理の並列数の設定
		/// </summary>
		/// <param name="num">並列数 [1~]</param>
		/// <exception cref="T:XIE.CxException"/>
		public static void ParallelNum(int num)
		{
			ExStatus status = xie_core.fnXIE_Core_Axi_ParallelNum_Set(num);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region DateTime

		/// <summary>
		/// 現在時刻の取得 (バイナリ日時)
		/// </summary>
		/// <returns>
		///		バイナリ日時を返します。<br/>
		///		Windows では FileTime、Linux では timespec をキャストしたものです。
		/// </returns>
		public static ulong	GetTime()
		{
			ulong result = 0;
			ExStatus status = xie_core.fnXIE_Core_Axi_GetTime(out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region 入出力系: (raw)

		/// <summary>
		/// ファイルヘッダー確認 (raw)
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		Raw フォーマットファイルヘッダー情報を返します。
		/// </returns>
		public static TxRawFileHeader CheckRaw(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);
			filename = System.IO.Path.GetFullPath(filename);
			if (System.IO.File.Exists(filename) == false)
				    throw new FileNotFoundException();

			var header = new TxRawFileHeader();
			ExStatus status = xie_core.fnXIE_Core_File_CheckRaw(out header, filename);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return header;
		}

		/// <summary>
		/// ファイル読み込み (raw)
		/// </summary>
		/// <param name="dst">読み込み先</param>
		/// <param name="filename">ファイル名</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void LoadRaw(IxModule dst, string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);
			filename = System.IO.Path.GetFullPath(filename);
			if (System.IO.File.Exists(filename) == false)
				    throw new FileNotFoundException();

			ExStatus status = xie_core.fnXIE_Core_File_LoadRaw(((IxModule)dst).GetHandle(), filename);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return;
		}

		/// <summary>
		/// ファイル保存 (raw)
		/// </summary>
		/// <param name="src">保存対象</param>
		/// <param name="filename">ファイル名</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void SaveRaw(string filename, IxModule src, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);

			ExStatus status = xie_core.fnXIE_Core_File_SaveRaw(((IxModule)src).GetHandle(), filename);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region 入出力系: (Marker)

		/// <summary>
		/// Jpeg Markers 複製
		/// </summary>
		/// <param name="src_file">入力ファイル</param>
		/// <param name="dst_file">出力ファイル</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void CopyJpegMarkers(string src_file, string dst_file, params object[] options)
		{
			if (string.IsNullOrEmpty(src_file))
				throw new CxException(ExStatus.InvalidParam);
			if (string.IsNullOrEmpty(dst_file))
				throw new CxException(ExStatus.InvalidParam);

			ExStatus status = xie_high.fnXIE_File_CopyJpegMarkers(src_file, dst_file, IntPtr.Zero);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region 入出力系: (BinaryFormat)

		/// <summary>
		/// BinaryFormat: ファイルからの読み込み
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		復元したオブジェクトを返します。
		/// </returns>
		public static object ReadAsBinary(string filename, params object[] options)
		{
			using (FileStream stream = new FileStream(System.IO.Path.GetFullPath(filename), System.IO.FileMode.Open))
			{
				BinaryFormatter bf = new BinaryFormatter();
				object src = bf.Deserialize(stream);
				return src;
			}
		}

		/// <summary>
		/// BinaryFormat: ストリームからの読み込み
		/// </summary>
		/// <param name="stream">読み込み先のストリーム</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		復元したオブジェクトを返します。
		/// </returns>
		public static object ReadAsBinary(System.IO.Stream stream, params object[] options)
		{
			BinaryFormatter bf = new BinaryFormatter();
			object src = bf.Deserialize(stream);
			return src;
		}

		/// <summary>
		/// BinaryFormat: ファイルへの書き込み
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="target">書き込み対象</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void WriteAsBinary(string filename, object target, params object[] options)
		{
			using (FileStream stream = new FileStream(System.IO.Path.GetFullPath(filename), System.IO.FileMode.Create))
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(stream, target);
			}
		}

		/// <summary>
		/// BinaryFormat: ストリームへの書き込み
		/// </summary>
		/// <param name="stream">書き込み先のストリーム</param>
		/// <param name="target">書き込み対象</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void WriteAsBinary(System.IO.Stream stream, object target, params object[] options)
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(stream, target);
		}

		#endregion

		#region 入出力系: (XmlSerializer)

		/// <summary>
		/// XmlSerializer: ファイルからの読み込み
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="type">復元するクラスの型</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		復元したオブジェクトを返します。
		/// </returns>
		public static object ReadAsXml(string filename, Type type, params object[] options)
		{
			FileStream fs = null;
			try
			{
				XmlSerializer xml = new XmlSerializer(type);
				fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
				object result = xml.Deserialize(fs);
				return result;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
		}

		/// <summary>
		/// XmlSerializer: ストリームからの読み込み
		/// </summary>
		/// <param name="stream">読み込み先のストリーム</param>
		/// <param name="type">復元するクラスの型</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		復元したオブジェクトを返します。
		/// </returns>
		public static object ReadAsXml(System.IO.Stream stream, Type type, params object[] options)
		{
			XmlSerializer xml = new XmlSerializer(type);
			object result = xml.Deserialize(stream);
			return result;
		}

		/// <summary>
		/// XmlSerializer: ファイルへの書き込み
		/// </summary>
		/// <param name="filename">ファイル名</param>
		/// <param name="target">書き込み対象</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void WriteAsXml(string filename, object target, params object[] options)
		{
			using (StreamWriter stream = new StreamWriter(filename, false, Encoding.UTF8))
			{
				XmlSerializer xml = new XmlSerializer(target.GetType());
				xml.Serialize(stream, target);
			}
		}

		/// <summary>
		/// XmlSerializer: ストリームへの書き込み
		/// </summary>
		/// <param name="stream">書き込み先のストリーム</param>
		/// <param name="target">書き込み対象</param>
		/// <param name="options">オプション。(未使用)</param>
		public static void WriteAsXml(System.IO.Stream stream, object target, params object[] options)
		{
			XmlSerializer xml = new XmlSerializer(target.GetType());
			xml.Serialize(stream, target);
		}

		#endregion
	}
}
