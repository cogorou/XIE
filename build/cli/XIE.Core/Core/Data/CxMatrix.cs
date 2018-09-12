/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XIE
{
	/// <summary>
	/// 行列オブジェクトクラス
	/// </summary>
	/// <example>
	/// <code lang="C#" source="examples/Core/CxMatrix/CxMatrix_01.cs"/>
	/// </example>
	[Serializable]
	[TypeConverter(typeof(CxSortingConverter))]
	public unsafe class CxMatrix : System.Object
		, IxModule
		, IDisposable
		, ICloneable
		, IxEquatable
		, IxFileAccess
		, IxAttachable
		, ISerializable
		, IXmlSerializable
	{
		#region IxModule の実装: (ハンドル)

		[NonSerialized]
		private HxModule m_Handle = IntPtr.Zero;
		[NonSerialized]
		private bool m_IsDisposable = false;

		/// <summary>
		/// ハンドルの取得
		/// </summary>
		/// <returns>
		///		保有しているハンドルを返します。
		/// </returns>
		HxModule IxModule.GetHandle()
		{
			return m_Handle;
		}

		/// <summary>
		/// ハンドルの設定
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		void IxModule.SetHandle(HxModule handle, bool disposable)
		{
			m_Handle = handle;
			m_IsDisposable = disposable;

			if (m_Handle == IntPtr.Zero)
				m_Tag = null;
			else
				m_Tag = (TxMatrix*)m_Handle.TagPtr().ToPointer();
		}

		/// <summary>
		/// 破棄
		/// </summary>
		void IxModule.Destroy()
		{
			if (m_IsDisposable)
				m_Handle.Destroy();
			m_Handle = IntPtr.Zero;
		}

		#endregion

		#region 構造体.

		/// <summary>
		/// 情報構造体へのポインタ
		/// </summary>
		[NonSerialized]
		private unsafe TxMatrix* m_Tag = null;

		/// <summary>
		/// データ構造の取得
		/// </summary>
		/// <returns>
		///		自身のデータ構造の複製を返します。
		/// </returns>
		public unsafe virtual TxMatrix Tag()
		{
			if (this.m_Tag == null)
				return new TxMatrix();
			return *this.m_Tag;
		}

		#endregion

		#region 生成関数:

		/// <summary>
		/// 行列オブジェクトハンドルからの生成
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxMatrix FromHandle(HxModule handle, bool disposable)
		{
			return new CxMatrix(handle, disposable);
		}

		/// <summary>
		/// アタッチ (情報構造体指定)
		/// </summary>
		/// <param name="tag">アタッチ先</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxMatrix FromTag(TxMatrix tag)
		{
			var dst = new CxMatrix();
			dst.Attach(tag);
			return dst;
		}

		/// <summary>
		/// IEnumerable からの変換
		/// </summary>
		/// <param name="rows">行数 [1~]</param>
		/// <param name="cols">列数 [1~]</param>
		/// <param name="src">変換元</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public static CxMatrix From(int rows, int cols, IEnumerable src)
		{
			int src_count = Axi.GetElementCount((IEnumerable)src);
			Type src_type = src.GetType().GetElementType();
			var model = ModelOf.From(src_type);
			if (model.Type == ExType.None || model.Pack == 0)
				throw new CxException(ExStatus.InvalidParam, "Model for Element was not found.");
			var dst = new CxMatrix(rows, cols, model);
			if (src_count > 0)
				dst.Scanner().Copy(src);
			return dst;
		}

		#endregion

		#region 生成関数: (Preset)

		/// <summary>
		/// 回転行列の生成
		/// </summary>
		/// <param name="degree">回転角 (degree)</param>
		/// <param name="axis_x">回転の基軸(X)</param>
		/// <param name="axis_y">回転の基軸(Y)</param>
		/// <returns>
		///		新しく生成した行列オブジェクトを返します。
		/// </returns>
		public static CxMatrix PresetRotate(double degree, double axis_x, double axis_y)
		{
			CxMatrix dst = new CxMatrix();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Preset_Rotate(((IxModule)dst).GetHandle(), degree, axis_x, axis_y);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		/// <summary>
		/// スケール変換行列の生成
		/// </summary>
		/// <param name="sx">X 方向倍率 [1.0 = 等倍]</param>
		/// <param name="sy">Y 方向倍率 [1.0 = 等倍]</param>
		/// <returns>
		///		新しく生成した行列オブジェクトを返します。
		/// </returns>
		public static CxMatrix PresetScale(double sx, double sy)
		{
			CxMatrix dst = new CxMatrix();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Preset_Scale(((IxModule)dst).GetHandle(), sx, sy);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		/// <summary>
		/// 平行移動行列の生成
		/// </summary>
		/// <param name="tx">X 方向移動量</param>
		/// <param name="ty">Y 方向移動量</param>
		/// <returns>
		///		新しく生成した行列オブジェクトを返します。
		/// </returns>
		public static CxMatrix PresetTranslate(double tx, double ty)
		{
			CxMatrix dst = new CxMatrix();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Preset_Translate(((IxModule)dst).GetHandle(), tx, ty);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		/// <summary>
		/// せん断行列の生成
		/// </summary>
		/// <param name="degree_x">X 方向せん断角度 (degree)</param>
		/// <param name="degree_y">Y 方向せん断角度 (degree)</param>
		/// <returns>
		///		新しく生成した行列オブジェクトを返します。
		/// </returns>
		public static CxMatrix PresetShear(double degree_x, double degree_y)
		{
			CxMatrix dst = new CxMatrix();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Preset_Shear(((IxModule)dst).GetHandle(), degree_x, degree_y);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		#endregion

		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ用初期化関数
		/// </summary>
		private void _Constructor()
		{
			HxModule handle = xie_core.fnXIE_Core_Module_Create("CxMatrix");
			if (handle == IntPtr.Zero)
				throw new CxException(ExStatus.MemoryError);
			((IxModule)this).SetHandle(handle, true);
		}

		/// <summary>
		/// コンストラクタ (ハンドル指定)
		/// </summary>
		/// <param name="handle">ハンドル</param>
		/// <param name="disposable">解放の指示 (true の場合は Destroy で解放されます。)</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		private CxMatrix(HxModule handle, bool disposable)
		{
			((IxModule)this).SetHandle(handle, disposable);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CxMatrix()
		{
			_Constructor();
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="size">サイズ [1~] ※幅/高さ共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型 [既定値:TxModel.F64(1)]</param>
		public CxMatrix(TxSizeI size, TxModel model = default(TxModel))
		{
			_Constructor();
			this.Resize(size, model);
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="rows">行数 [1~] ※行列数が共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="cols">列数 [1~] ※行列数が共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型 [既定値:TxModel.F64(1)]</param>
		public CxMatrix(int rows, int cols, TxModel model = default(TxModel))
		{
			_Constructor();
			this.Resize(rows, cols, model);
		}

		/// <summary>
		/// ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名 [拡張子: raw]</param>
		/// <param name="options">オプション。(未使用)</param>
		/// <returns>
		///		新しく生成したオブジェクトを返します。
		/// </returns>
		public CxMatrix(string filename, params object[] options)
		{
			_Constructor();
			Load(filename, options);
		}

		#endregion

		#region デストラクタ:

		/// <summary>
		/// デストラクタ
		/// </summary>
		~CxMatrix()
		{
			((IxModule)this).Destroy();
		}

		#endregion

		#region IDisposable の実装: (解放)

		/// <summary>
		/// 解放
		/// </summary>
		public virtual void Dispose()
		{
			((IxModule)this).GetHandle().Dispose();
		}

		/// <summary>
		/// 有効性 [初期値:false]
		/// </summary>
		/// <returns>
		///		現在のオブジェクトの内部リソースが有効な場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsValid
		{
			get { return ((IxModule)this).GetHandle().IsValid(); }
		}

		#endregion

		#region ICloneable の実装:

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		public virtual object Clone()
		{
			var clone = new CxMatrix();
			clone.CopyFrom(this);
			return clone;
		}

		#endregion

		#region IxEquatable の実装:

		/// <summary>
		/// オブジェクトの内容の複製
		/// </summary>
		/// <param name="src">複製元</param>
		public virtual void CopyFrom(object src)
		{
			if (ReferenceEquals(src, this)) return;
			if (ReferenceEquals(src, null)) return;
			if (src is IxModule)
			{
				((IxModule)this).GetHandle().CopyFrom(((IxModule)src).GetHandle());
				return;
			}
			if (src is IxConvertible)
			{
				((IxConvertible)src).CopyTo(this);
				return;
			}
			throw new CxException(ExStatus.Unsupported);
		}

		/// <summary>
		/// オブジェクトの内容の比較
		/// </summary>
		/// <param name="src">比較対象</param>
		/// <returns>
		///		内容が一致する場合は true 、それ以外は false を返します。
		/// </returns>
		public virtual bool ContentEquals(object src)
		{
			if (ReferenceEquals(src, null)) return false;
			if (ReferenceEquals(src, this)) return true;
			if (this.GetType().IsInstanceOfType(src) == false) return false;

			try
			{
				var _src = (CxMatrix)src;
				return ((IxModule)this).GetHandle().ContentEquals( ((IxModule)_src).GetHandle());
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		#endregion

		#region IxFileAccess の実装: (ファイルアクセス)

		/// <summary>
		/// IxFileAccess の実装: ファイル読み込み
		/// </summary>
		/// <param name="filename">ファイル名。(raw)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Load(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);
			filename = System.IO.Path.GetFullPath(filename);
			if (System.IO.File.Exists(filename) == false)
				throw new System.IO.FileNotFoundException();

			ExStatus status = xie_core.fnXIE_Core_FileAccess_Load(((IxModule)this).GetHandle(), filename, IntPtr.Zero, TxModel.Default);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// IxFileAccess の実装: ファイル保存
		/// </summary>
		/// <param name="filename">ファイル名。(raw)</param>
		/// <param name="options">オプション。(未使用)</param>
		public virtual void Save(string filename, params object[] options)
		{
			if (string.IsNullOrEmpty(filename))
				throw new CxException(ExStatus.InvalidParam);

			ExStatus status = xie_core.fnXIE_Core_FileAccess_Save(((IxModule)this).GetHandle(), filename, IntPtr.Zero, TxModel.Default);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region IxAttachable の実装:

		/// <summary>
		/// アタッチ状態 [初期値:false] (true の場合 Dispose で Handle を解放しません。)
		/// </summary>
		/// <returns>
		///		現在のオブジェクトが外部リソースを参照している場合は true を返します。
		///		それ以外は false を返します。
		/// </returns>
		[XmlIgnore]
		[ReadOnly(true)]
		public virtual bool IsAttached
		{
			get { return ((IxModule)this).GetHandle().IsAttached(); }
		}

		/// <summary>
		/// アタッチ
		/// </summary>
		/// <param name="src">アタッチ先</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Attach(object src)
		{
			if (src is IxModule)
			{
				ExStatus status = xie_core.fnXIE_Core_Attachable_Attach(((IxModule)this).GetHandle(), ((IxModule)src).GetHandle());
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}

			if (src is TxMatrix)
			{
				ExStatus status = xie_core.fnXIE_Core_Matrix_Attach(((IxModule)this).GetHandle(), (TxMatrix)src);
				if (status != ExStatus.Success)
					throw new CxException(status);
				return;
			}

			throw new CxException(ExStatus.Unsupported);
		}

		#endregion

		#region インデクサ:

		/// <summary>
		/// 領域の先頭アドレス
		/// </summary>
		/// <returns>
		///		領域の先頭アドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr Address()
		{
			if (this.m_Tag == null)
				return IntPtr.Zero;
			return this.m_Tag->Address;
		}

		/// <summary>
		/// 行の先頭アドレス
		/// </summary>
		/// <param name="row">行指標 [0~(Rows-1)]</param>
		/// <returns>
		///		指定位置の行の先頭アドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr this[int row]
		{
			get
			{
				TxMatrix* tag = this.m_Tag;
				if (tag == null ||
					tag->Address == IntPtr.Zero)
					throw new CxException(ExStatus.InvalidObject);

				if (row < 0)
					throw new CxException(ExStatus.InvalidParam);
				if (!(row < tag->Rows))
					throw new CxException(ExStatus.InvalidParam);

				double* addr = (double*)tag->Address.ToPointer();
				return new XIE.Ptr.DoublePtr(addr + (row * tag->Columns));
			}
		}

		/// <summary>
		/// 要素のアドレス
		/// </summary>
		/// <param name="row">行指標 [0~(Rows-1)]</param>
		/// <param name="col">列指標 [col=0~(Columns-1)]</param>
		/// <returns>
		///		指定位置の要素のアドレスを返します。
		/// </returns>
		public virtual unsafe IntPtr this[int row, int col]
		{
			get
			{
				TxMatrix* tag = this.m_Tag;
				if (tag == null ||
					tag->Address == IntPtr.Zero)
					throw new CxException(ExStatus.InvalidObject);

				if (row < 0 || col < 0)
					throw new CxException(ExStatus.InvalidParam);
				if (!(row < tag->Rows))
					throw new CxException(ExStatus.InvalidParam);
				if (!(col < tag->Columns))
					throw new CxException(ExStatus.InvalidParam);

				byte* addr = (byte*)tag->Address.ToPointer();
				return new IntPtr(addr + (row * tag->Stride) + (col * this.Model.Size));
			}
		}

		#endregion

		#region プロパティ:

		/// <summary>
		/// 行数 [0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxMatrix.Rows")]
		public virtual unsafe int Rows
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Rows;
			}
		}

		/// <summary>
		/// 列数 [0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxMatrix.Columns")]
		public virtual unsafe int Columns
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Columns;
			}
		}

		/// <summary>
		/// 要素の型
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxMatrix.Model")]
		public virtual unsafe TxModel Model
		{
			get
			{
				if (this.m_Tag == null)
					return TxModel.Default;
				return this.m_Tag->Model;
			}
		}

		/// <summary>
		/// 水平方向サイズ (bytes) [範囲:0~]
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxMatrix.Stride")]
		public virtual unsafe int Stride
		{
			get
			{
				if (this.m_Tag == null)
					return 0;
				return this.m_Tag->Stride;
			}
		}

		#endregion

		#region プロパティ:(拡張)

		/// <summary>
		/// 幅と高さ
		/// </summary>
		[XmlIgnore]
		[ReadOnly(true)]
		[CxCategory("Information")]
		[CxDescription("P:XIE.CxMatrix.Size")]
		public virtual TxSizeI Size
		{
			get
			{
				return new TxSizeI(this.Columns, this.Rows);
			}
		}

		#endregion

		#region メソッド: (Resize)

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="size">サイズ [1~] ※幅/高さ共に 0 の場合はハンドルは生成されますが領域は未確保になります。</param>
		/// <param name="model">要素の型 [既定値:TxModel.F64(1)]</param>
		public virtual void Resize(TxSizeI size, TxModel model = default(TxModel))
		{
			Resize(size.Height, size.Width, model);
		}

		/// <summary>
		/// 領域の確保
		/// </summary>
		/// <param name="rows">行数 [1~] ※行列数が共に 0 の場合は解放します。</param>
		/// <param name="cols">列数 [1~] ※行列数が共に 0 の場合は解放します。</param>
		/// <param name="model">要素の型 [既定値:TxModel.F64(1)]</param>
		public virtual void Resize(int rows, int cols, TxModel model = default(TxModel))
		{
			if (((IxModule)this).GetHandle() == IntPtr.Zero)
				_Constructor();

			ExStatus status = xie_core.fnXIE_Core_Matrix_Resize(((IxModule)this).GetHandle(), rows, cols, model);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// リセット (単位行列にリセットします。事前に正方行列にリサイズしてください。)
		/// </summary>
		public virtual void Reset()
		{
			ExStatus status = xie_core.fnXIE_Core_Matrix_Reset(((IxModule)this).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Clone)

		/// <summary>
		/// オブジェクトのクローンの生成
		/// </summary>
		/// <param name="model">出力配列の型</param>
		/// <returns>
		///		新しく生成したオブジェクトに自身の内容を複製して返します。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public virtual CxMatrix Clone(TxModel model)
		{
			if (this.IsValid == false)
				throw new CxException(ExStatus.InvalidObject);

			TxModel dst_model = new TxModel();
			dst_model.Type = (model.Type != ExType.None) ? model.Type : this.Model.Type;
			dst_model.Pack = (model.Pack != 0) ? model.Pack : this.Model.Pack;
			CxMatrix clone = new CxMatrix(this.Size, dst_model);
			clone.Filter().Copy(this);
			return clone;
		}

		#endregion

		#region メソッド: (Clear)

		/// <summary>
		/// 要素のクリア
		/// </summary>
		/// <param name="value">値</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual unsafe void Clear(object value)
		{
			if (value == null)
				throw new CxException(ExStatus.InvalidParam);

			HxModule hdst = ((IxModule)this).GetHandle();
			ExStatus status = ExStatus.Unsupported;
			TxModel model = ModelOf.From(value.GetType());
			switch (model.Type)
			{
				case ExType.None:
					break;
				case ExType.Ptr:
					break;
				default:
					using (CxArray temp = new CxArray(1, model))
					{
						IntPtr addr = temp.Address();
						System.Runtime.InteropServices.Marshal.StructureToPtr(value, addr, false);
						status = xie_core.fnXIE_Core_Matrix_Clear(hdst, addr, model);
					}
					break;
			}
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		/// <summary>
		/// 対角成分の初期化 
		/// </summary>
		/// <param name="value">値</param>
		/// <param name="mode">モード [範囲:0,1,2] ※ 1 は対角成分の初期化のみ、2 はそれ以外の 0 初期化のみ、0 は両方を行います。</param>
		/// <exception cref="T:XIE.CxException"/>
		public virtual void Eye(double value, int mode)
		{
			HxModule hdst = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Eye(hdst, value, mode);
			if (status != ExStatus.Success)
				throw new CxException(status);
		}

		#endregion

		#region メソッド: (Measure)

		/// <summary>
		/// 統計
		/// </summary>
		/// <returns>
		///		統計データを返します。
		///		未確保の場合は例外を発行します。<br/>
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public TxStatistics Statistics()
		{
			var result = new TxStatistics();
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Statistics(hsrc, 0, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 抽出
		/// </summary>
		/// <param name="sy">抽出位置(Y) [0~Height-1]</param>
		/// <param name="sx">抽出位置(X) [0~Width-1]</param>
		/// <param name="length">抽出する長さ [範囲:0~]</param>
		/// <param name="dir">走査方向 [X=行抽出、Y=列抽出]</param>
		/// <returns>
		///		指定された範囲の要素を格納して返します。
		///		配列の要素は自身と同一型です。要素数は抽出する長さと同一です。
		/// </returns>
		/// <exception cref="T:XIE.CxException"/>
		public CxArray Extract(int sy, int sx, int length, ExScanDir dir)
		{
			CxArray result = new CxArray();
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hresult = ((IxModule)result).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Extract(hsrc, sy, sx, length, dir, hresult);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 行列式の算出
		/// </summary>
		/// <returns>
		///		計算結果を返します。
		/// </returns>
		public double Det()
		{
			double result;
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Det(hsrc, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// 対角要素の和
		/// </summary>
		/// <returns>
		///		計算結果を返します。
		/// </returns>
		public double Trace()
		{
			double result;
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Trace(hsrc, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		/// <summary>
		/// スケール変換係数の抽出
		/// </summary>
		/// <returns>
		///		抽出したスケール変換係数を返します。
		/// </returns>
		public TxSizeD ScaleFactor()
		{
			TxSizeD result;
			HxModule hsrc = ((IxModule)this).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_ScaleFactor(hsrc, out result);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return result;
		}

		#endregion

		#region メソッド: (Filter)

		/// <summary>
		/// フィルタの取得
		/// </summary>
		/// <returns>
		///		行列オブジェクトフィルタを返します。
		/// </returns>
		public virtual CxMatrixFilter Filter()
		{
			return new CxMatrixFilter(this);
		}

		#endregion

		#region メソッド: (Scanner)

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <returns>
		///		２次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner2D Scanner()
		{
			return new TxScanner2D(this.Address(), this.Columns, this.Rows, this.Model, this.Stride);
		}

		/// <summary>
		/// 配列走査構造体の取得
		/// </summary>
		/// <param name="bounds">始点とサイズ</param>
		/// <returns>
		///		２次元配列走査構造体を返します。
		/// </returns>
		public virtual TxScanner2D Scanner(TxRectangleI bounds)
		{
			if (bounds.X < 0 || bounds.Y < 0)
				throw new CxException(ExStatus.InvalidParam);
			if (!(bounds.X + bounds.Width <= this.Columns))
				throw new CxException(ExStatus.InvalidParam);
			if (!(bounds.Y + bounds.Height <= this.Rows))
				throw new CxException(ExStatus.InvalidParam);
			return new TxScanner2D(this[bounds.Y, bounds.X], bounds.Width, bounds.Height, this.Model, this.Stride);
		}

		#endregion

		#region メソッド: (Linear)

		/// <summary>
		/// 逆行列
		/// </summary>
		/// <returns>
		///		自身の逆行列を返します。
		/// </returns>
		public CxMatrix Invert()
		{
			CxMatrix dst = new CxMatrix();
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hdst = ((IxModule)dst).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Invert(hdst, hsrc);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		/// <summary>
		/// 小行列
		/// </summary>
		/// <param name="row">除去対象の行指標 [0~]</param>
		/// <param name="col">除去対象の列指標 [0~]</param>
		/// <returns>
		///		自身の小行列を返します。
		/// </returns>
		public CxMatrix Submatrix(int row, int col)
		{
			CxMatrix dst = new CxMatrix();
			HxModule hsrc = ((IxModule)this).GetHandle();
			HxModule hdst = ((IxModule)dst).GetHandle();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Submatrix(hdst, hsrc, row, col);
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		#endregion

		#region オペレータ: (行列の積)

		/// <summary>
		/// 行列の積
		/// </summary>
		/// <param name="src">左辺値</param>
		/// <param name="val">右辺値</param>
		/// <returns>
		///		行列の積を返します。
		/// </returns>
		public static CxMatrix operator *(CxMatrix src, CxMatrix val)
		{
			CxMatrix dst = new CxMatrix();
			ExStatus status = xie_core.fnXIE_Core_Matrix_Product(((IxModule)dst).GetHandle(), ((IxModule)src).GetHandle(), ((IxModule)val).GetHandle());
			if (status != ExStatus.Success)
				throw new CxException(status);
			return dst;
		}

		#endregion

		#region ISerializable の実装:

		/// <summary>
		/// ISerializable の実装: シグネチャコンストラクタ
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public unsafe CxMatrix(SerializationInfo info, StreamingContext context)
		{
			_Constructor();

			int rows = 0;
			int cols = 0;
			TxModel model = TxModel.Default;
			double[] items = new double[0];

			// --------------------------------------------------
			foreach (SerializationEntry entry in info)
			{
				switch (entry.Name)
				{
					case "Rows":
						rows = (int)info.GetValue(entry.Name, typeof(int));
						break;
					case "Columns":
						cols = (int)info.GetValue(entry.Name, typeof(int));
						break;
					case "Model":
						model = (TxModel)info.GetValue(entry.Name, typeof(TxModel));
						break;
					case "Items":
						items = (double[])info.GetValue(entry.Name, typeof(double[]));
						break;
				}
			}

			// --------------------------------------------------
			if (rows > 0 && cols > 0 && model == TxModel.F64(1))
			{
				this.Resize(rows, cols);
				int count = rows * cols;
				if (this.Address() != IntPtr.Zero && items.Length == count)
				{
					double* addr = (double*)this.Address();
					for (int i = 0; i < items.Length; i++)
						addr[i] = items[i];
				}
			}
		}

		/// <summary>
		/// ISerializable の実装: SerializationInfo に、オブジェクトをシリアル化するために必要なデータを設定します。
		/// </summary>
		/// <param name="info">データ読み込み先の SerializationInfo</param>
		/// <param name="context">シリアル化先</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		unsafe void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int rows = this.Rows;
			int cols = this.Columns;
			TxModel model = this.Model;
			int count = rows * cols;
			double[] items = new double[count];
			if (this.Address() != IntPtr.Zero)
			{
				double* addr = (double*)this.Address();
				for (int i = 0; i < count; i++)
					items[i] = addr[i];
			}
			info.AddValue("Rows", rows);
			info.AddValue("Columns", cols);
			info.AddValue("Model", model);
			info.AddValue("Items", items);
		}

		#endregion

		#region IXmlSerializable の実装:

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトの XML 表現からオブジェクトを生成します。
		/// </summary>
		/// <param name="reader">オブジェクトの逆シリアル化元である XmlReader ストリーム。</param>
		unsafe void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			// --------------------------------------------------
			int rows = 0;
			int cols = 0;
			TxModel model = TxModel.Default;
			if (reader.Name == "Rows")
				rows = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			if (reader.Name == "Columns")
				cols = XIE.AxiXml.GetValue<int>(reader, reader.Name);
			if (reader.Name == "Model")
				model = XIE.AxiXml.GetValue<TxModel>(reader, reader.Name);
			// --------------------------------------------------
			if (rows > 0 && cols > 0 && model.Type != ExType.None && model.Pack > 0)
				this.Resize(rows, cols);
			else
				this.Dispose();
			// --------------------------------------------------
			for (int y = 0; y < this.Rows; y++)
			{
				if (reader.Name == string.Format("Items{0}", y))
				{
					if (reader.IsEmptyElement)
					{
						reader.ReadStartElement(reader.Name);
					}
					else
					{
						reader.ReadStartElement(reader.Name);
						{
							Type item_type = Axi.TypeOf(model.Type);
							Type ptr_type = Axi.PointerOf(model.Type);
							if (item_type != null && ptr_type != null)
							{
								XmlSerializer xs = new XmlSerializer(item_type.MakeArrayType());
								object items = xs.Deserialize(reader);
								object ptr = Activator.CreateInstance(ptr_type, new object[] { this[y] });
								ptr_type.GetMethod("CopyFrom").Invoke(ptr, new object[] { items });
							}
						}
						reader.ReadEndElement();
					}
				}
			}
			// --------------------------------------------------
			reader.ReadEndElement();
		}

		/// <summary>
		/// IXmlSerializable の実装: オブジェクトを XML 表現に変換します。
		/// </summary>
		/// <param name="writer">オブジェクトのシリアル化先の XmlWriter ストリーム。</param>
		unsafe void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			XIE.AxiXml.AddValue(writer, "Rows", this.Rows);
			XIE.AxiXml.AddValue(writer, "Columns", this.Columns);
			XIE.AxiXml.AddValue(writer, "Model", this.Model);

			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");	// 各項目にプレフィックスと名前空間を入れない.

			Type ptr_type = Axi.PointerOf(this.Model.Type);
			if (ptr_type != null && this.Model.Pack > 0)
			{
				for (int y = 0; y < this.Rows; y++)
				{
					writer.WriteStartElement(string.Format("Items{0}", y));

					int count = this.Columns * this.Model.Pack;
					object ptr = Activator.CreateInstance(ptr_type, new object[] { this[y] });
					object buf = ptr_type.GetMethod("ToArray").Invoke(ptr, new object[] { count });
					XmlSerializer xs = new XmlSerializer(buf.GetType());
					xs.Serialize(writer, buf, ns);

					writer.WriteEndElement();
				}
			}
		}

		/// <summary>
		/// IXmlSerializable の実装: 処理対象オブジェクトの XML 表現を記述する XmlSchema
		/// </summary>
		/// <returns>
		///		常に null を返します。
		/// </returns>
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		#endregion
	}
}
