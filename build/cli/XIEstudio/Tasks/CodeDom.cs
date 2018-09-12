/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Threading;
using System.Diagnostics;
using System.CodeDom;

namespace XIEstudio
{
	/// <summary>
	/// リテラル表現
	/// </summary>
	class CodeLiteral
	{
		#region null のリテラル表現:

		/// <summary>
		/// null のリテラル表現を生成します。
		/// </summary>
		/// <returns>
		///		生成された CodeExpression オブジェクトを返します。
		/// </returns>
		public static CodeExpression Null
		{
			get
			{
				return new CodePrimitiveExpression(null);
			}
		}

		#endregion

		#region IntPtr.Zero のリテラル表現:

		/// <summary>
		/// IntPtr.Zero のリテラル表現を生成します。
		/// </summary>
		/// <returns>
		///		生成された CodeExpression オブジェクトを返します。
		/// </returns>
		public static CodeExpression Zero
		{
			get
			{
				return new CodeExtraType(typeof(IntPtr)).Ref("Zero");
			}
		}

		#endregion

		#region 指定された値のリテラル表現:

		/// <summary>
		/// 指定された値のリテラル表現を生成します。
		/// </summary>
		/// <param name="value">値</param>
		/// <returns>
		///		生成された CodeExpression オブジェクトを返します。
		/// </returns>
		public static CodeExpression From(object value)
		{
			if (value == null)
				return new CodePrimitiveExpression(null);

			Type type = value.GetType();
			if (type.IsEnum)
			{
				return new CodeExtraType(value.GetType()).Ref(value.ToString());
			}
			else if (type == typeof(IntPtr))
			{
				#region IntPtr
				var src = (IntPtr)value;
				if (src == IntPtr.Zero)
					return new CodeExtraType(typeof(IntPtr)).Ref("Zero");
				else
					return new CodeObjectCreateExpression(type, From(src.ToInt64()));
				#endregion
			}
			else if (type == typeof(string))
			{
				return new CodePrimitiveExpression(value);
			}
			else if (type == typeof(System.Drawing.Color))
			{
				#region Color
				var src = (System.Drawing.Color)value;
				var color = new CodeExtraType(type);
				if (src.A == 255)
					return color.Call("FromArgb", CodeLiteral.From(src.R), CodeLiteral.From(src.G), CodeLiteral.From(src.B));
				else
					return color.Call("FromArgb", CodeLiteral.From(src.A), CodeLiteral.From(src.R), CodeLiteral.From(src.G), CodeLiteral.From(src.B));
				#endregion
			}
			else if (typeof(Encoding).IsAssignableFrom(type))
			{
				#region Encoding:
				var encoding = (Encoding)value;
				if (encoding == Encoding.Default)
					return new CodeExtraType(typeof(Encoding)).Ref("Default");
				if (encoding == Encoding.ASCII)
					return new CodeExtraType(typeof(Encoding)).Ref("ASCII");
				if (encoding == Encoding.UTF8)
					return new CodeExtraType(typeof(Encoding)).Ref("UTF8");
				if (encoding == Encoding.UTF7)
					return new CodeExtraType(typeof(Encoding)).Ref("UTF7");
				if (encoding == Encoding.UTF32)
					return new CodeExtraType(typeof(Encoding)).Ref("UTF32");
				if (encoding == Encoding.Unicode)
					return new CodeExtraType(typeof(Encoding)).Ref("Unicode");
				if (encoding == Encoding.BigEndianUnicode)
					return new CodeExtraType(typeof(Encoding)).Ref("BigEndianUnicode");
				return new CodeExtraType(typeof(Encoding)).Ref("Default");
				#endregion
			}
			else if (type.IsPrimitive)
			{
				return new CodePrimitiveExpression(value);
			}
			else if (type.IsClass)
			{
				return new CodeObjectCreateExpression(type);
			}
			else
			{
				#region 既知の構造体:
				if (type == typeof(XIE.TxModel))
				{
					var src = (XIE.TxModel)value;
					return new CodeObjectCreateExpression(type, From(src.Type), From(src.Pack));
				}
				else if (type == typeof(XIE.TxPointI))
				{
					var src = (XIE.TxPointI)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y));
				}
				else if (type == typeof(XIE.TxPointD))
				{
					var src = (XIE.TxPointD)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y));
				}
				else if (type == typeof(XIE.TxSizeI))
				{
					var src = (XIE.TxSizeI)value;
					return new CodeObjectCreateExpression(type, From(src.Width), From(src.Height));
				}
				else if (type == typeof(XIE.TxSizeD))
				{
					var src = (XIE.TxSizeD)value;
					return new CodeObjectCreateExpression(type, From(src.Width), From(src.Height));
				}
				else if (type == typeof(XIE.TxRangeI))
				{
					var src = (XIE.TxRangeI)value;
					return new CodeObjectCreateExpression(type, From(src.Lower), From(src.Upper));
				}
				else if (type == typeof(XIE.TxRangeD))
				{
					var src = (XIE.TxRangeD)value;
					return new CodeObjectCreateExpression(type, From(src.Lower), From(src.Upper));
				}
				else if (type == typeof(XIE.TxRectangleI))
				{
					var src = (XIE.TxRectangleI)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.Width), From(src.Height));
				}
				else if (type == typeof(XIE.TxRectangleD))
				{
					var src = (XIE.TxRectangleD)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.Width), From(src.Height));
				}
				else if (type == typeof(XIE.TxLineI))
				{
					var src = (XIE.TxLineI)value;
					return new CodeObjectCreateExpression(type, From(src.A), From(src.B), From(src.C));
				}
				else if (type == typeof(XIE.TxLineD))
				{
					var src = (XIE.TxLineD)value;
					return new CodeObjectCreateExpression(type, From(src.A), From(src.B), From(src.C));
				}
				else if (type == typeof(XIE.TxLineSegmentI))
				{
					var src = (XIE.TxLineSegmentI)value;
					return new CodeObjectCreateExpression(type, From(src.X1), From(src.Y1), From(src.X2), From(src.Y2));
				}
				else if (type == typeof(XIE.TxLineSegmentD))
				{
					var src = (XIE.TxLineSegmentD)value;
					return new CodeObjectCreateExpression(type, From(src.X1), From(src.Y1), From(src.X2), From(src.Y2));
				}
				else if (type == typeof(XIE.TxCircleI))
				{
					var src = (XIE.TxCircleI)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.Radius));
				}
				else if (type == typeof(XIE.TxCircleD))
				{
					var src = (XIE.TxCircleD)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.Radius));
				}
				else if (type == typeof(XIE.TxEllipseI))
				{
					var src = (XIE.TxEllipseI)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.RadiusX), From(src.RadiusY));
				}
				else if (type == typeof(XIE.TxEllipseD))
				{
					var src = (XIE.TxEllipseD)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.RadiusX), From(src.RadiusY));
				}
				else if (type == typeof(XIE.TxEllipseArcI))
				{
					var src = (XIE.TxEllipseArcI)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.RadiusX), From(src.RadiusY), From(src.StartAngle), From(src.SweepAngle));
				}
				else if (type == typeof(XIE.TxEllipseArcD))
				{
					var src = (XIE.TxEllipseArcD)value;
					return new CodeObjectCreateExpression(type, From(src.X), From(src.Y), From(src.RadiusX), From(src.RadiusY), From(src.StartAngle), From(src.SweepAngle));
				}
				else if (type == typeof(XIE.TxImageSize))
				{
					var src = (XIE.TxImageSize)value;
					return new CodeObjectCreateExpression(
							type,
							CodeLiteral.From(src.Width),
							CodeLiteral.From(src.Height),
							CodeLiteral.From(src.Model),
							CodeLiteral.From(src.Channels),
							CodeLiteral.From(src.Depth)
						);
				}
				else if (type == typeof(XIE.TxStatistics))
				{
					var src = (XIE.TxStatistics)value;
					return new CodeObjectCreateExpression(
							type,
							CodeLiteral.From(src.Sum1),
							CodeLiteral.From(src.Sum2),
							CodeLiteral.From(src.Min),
							CodeLiteral.From(src.Max),
							CodeLiteral.From(src.Count)
						);
				}
				#endregion

				return new CodeObjectCreateExpression(type);
			}
		}

		/// <summary>
		/// 指定された型のリテラル表現を生成します。
		/// </summary>
		/// <typeparam name="TE">型</typeparam>
		/// <returns>
		///		生成された CodeExpression オブジェクトを返します。
		/// </returns>
		public static CodeExpression From<TE>()
		{
			Type type = typeof(TE);
			if (type.IsEnum)
			{
				object value = default(TE);
				return new CodeExtraType(value.GetType()).Ref(value.ToString());
			}
			else if (type.IsPrimitive)
			{
				return new CodePrimitiveExpression(default(TE));
			}
			else if (type.IsClass)
			{
				return new CodeObjectCreateExpression(type);
			}
			else
			{
				return new CodeObjectCreateExpression(type);
			}
		}

		#endregion
	}

	/// <summary>
	/// オブジェクト表現の拡張
	/// </summary>
	class CodeObjectExtension
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CodeObjectExtension()
		{
			Target = null;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="target">対象の表現</param>
		public CodeObjectExtension(CodeExpression target)
		{
			Target = target;
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 対象のオブジェクト表現
		/// </summary>
		public CodeExpression Target;

		#endregion

		#region メソッド: (プロパティ参照)

		/// <summary>
		/// プロパティ参照の表現を生成します。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraProperty Ref(string name)
		{
			return new CodeExtraProperty(this.Target, name);
		}

		#endregion

		#region メソッド: (メソッド実行)

		/// <summary>
		/// メソッド実行の表現を生成します。
		/// </summary>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraMethod Call(string name, params CodeExpression[] parameters)
		{
			return new CodeExtraMethod(this.Target, name, parameters);
		}

		#endregion

		#region メソッド: (オペレータ)

		/// <summary>
		/// 算術演算子（＋）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Add(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.Add, value);
		}

		/// <summary>
		/// 算術演算子（－）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Subtract(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.Subtract, value);
		}

		/// <summary>
		/// 算術演算子（＊）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Multiply(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.Multiply, value);
		}

		/// <summary>
		/// 算術演算子（／）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Divide(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.Divide, value);
		}

		/// <summary>
		/// 算術演算子（％）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Modulus(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.Modulus, value);
		}

		/// <summary>
		/// 比較演算子（！＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression NotEqual(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.IdentityInequality, value);
		}

		/// <summary>
		/// 比較演算子（＝＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Equal(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.ValueEquality, value);
		}

		/// <summary>
		/// 論理演算子（｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseOr(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.BitwiseOr, value);
		}

		/// <summary>
		/// 論理演算子（＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseAnd(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.BitwiseAnd, value);
		}

		/// <summary>
		/// 論理演算子（｜｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanOr(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.BooleanOr, value);
		}

		/// <summary>
		/// 論理演算子（＆＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanAnd(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.BooleanAnd, value);
		}

		/// <summary>
		/// 比較演算子（＜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThan(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.LessThan, value);
		}

		/// <summary>
		/// 比較演算子（≦）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThanOrEqual(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.LessThanOrEqual, value);
		}

		/// <summary>
		/// 比較演算子（＞）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThan(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.GreaterThan, value);
		}

		/// <summary>
		/// 比較演算子（≧）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThanOrEqual(CodeExpression value)
		{
			return new CodeBinaryOperatorExpression(this.Target, CodeBinaryOperatorType.GreaterThanOrEqual, value);
		}

		#endregion
	}

	/// <summary>
	/// 拡張版の型参照表現
	/// </summary>
	class CodeExtraType : CodeTypeReferenceExpression
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">型</param>
		public CodeExtraType(Type type)
			: base(type)
		{
			Extension = new CodeObjectExtension(this);
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// オブジェクト表現の拡張
		/// </summary>
		private CodeObjectExtension Extension;

		#endregion

		#region メソッド: (生成)

		/// <summary>
		/// オブジェクト生成の表現を生成します。
		/// </summary>
		/// <param name="parameters">コンストラクタに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeObjectCreateExpression New(params CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(this.Type, parameters);
		}

		#endregion

		#region メソッド: (プロパティ参照)

		/// <summary>
		/// プロパティ参照の表現を生成します。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraProperty Ref(string name)
		{
			return this.Extension.Ref(name);
		}

		#endregion

		#region メソッド: (メソッド実行)

		/// <summary>
		/// メソッド実行の表現を生成します。
		/// </summary>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraMethod Call(string name, params CodeExpression[] parameters)
		{
			return this.Extension.Call(name, parameters);
		}

		#endregion

		#region メソッド: (オペレータ)

		/// <summary>
		/// 算術演算子（＋）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Add(CodeExpression value)
		{
			return this.Extension.Add(value);
		}

		/// <summary>
		/// 算術演算子（－）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Subtract(CodeExpression value)
		{
			return this.Extension.Subtract(value);
		}

		/// <summary>
		/// 算術演算子（＊）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Multiply(CodeExpression value)
		{
			return this.Extension.Multiply(value);
		}

		/// <summary>
		/// 算術演算子（／）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Divide(CodeExpression value)
		{
			return this.Extension.Divide(value);
		}

		/// <summary>
		/// 算術演算子（％）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Modulus(CodeExpression value)
		{
			return this.Extension.Modulus(value);
		}

		/// <summary>
		/// 比較演算子（！＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression NotEqual(CodeExpression value)
		{
			return this.Extension.NotEqual(value);
		}

		/// <summary>
		/// 比較演算子（＝＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Equal(CodeExpression value)
		{
			return this.Extension.Equal(value);
		}

		/// <summary>
		/// 論理演算子（｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseOr(CodeExpression value)
		{
			return this.Extension.BitwiseOr(value);
		}

		/// <summary>
		/// 論理演算子（＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseAnd(CodeExpression value)
		{
			return this.Extension.BitwiseAnd(value);
		}

		/// <summary>
		/// 論理演算子（｜｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanOr(CodeExpression value)
		{
			return this.Extension.BooleanOr(value);
		}

		/// <summary>
		/// 論理演算子（＆＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanAnd(CodeExpression value)
		{
			return this.Extension.BooleanAnd(value);
		}

		/// <summary>
		/// 比較演算子（＜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThan(CodeExpression value)
		{
			return this.Extension.LessThan(value);
		}

		/// <summary>
		/// 比較演算子（≦）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThanOrEqual(CodeExpression value)
		{
			return this.Extension.LessThanOrEqual(value);
		}

		/// <summary>
		/// 比較演算子（＞）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThan(CodeExpression value)
		{
			return this.Extension.GreaterThan(value);
		}

		/// <summary>
		/// 比較演算子（≧）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThanOrEqual(CodeExpression value)
		{
			return this.Extension.GreaterThanOrEqual(value);
		}

		#endregion
	}

	/// <summary>
	/// 拡張版の変数参照表現
	/// </summary>
	class CodeExtraVariable : CodeVariableReferenceExpression
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">変数名</param>
		/// <param name="type">変数の型</param>
		public CodeExtraVariable(string name, Type type)
			: base(name)
		{
			Type = type;
			TypeRef = new CodeTypeReference(type);
			Extension = new CodeObjectExtension(this);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="src">変数名との型</param>
		public CodeExtraVariable(KeyValuePair<string, Type> src)
			: base(src.Key)
		{
			Type = src.Value;
			TypeRef = new CodeTypeReference(src.Value);
			Extension = new CodeObjectExtension(this);
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// 変数の型
		/// </summary>
		public Type Type;

		/// <summary>
		/// 変数の型の参照
		/// </summary>
		public CodeTypeReference TypeRef;

		/// <summary>
		/// オブジェクト表現の拡張
		/// </summary>
		private CodeObjectExtension Extension;

		#endregion

		#region メソッド: (宣言と代入)

		/// <summary>
		/// 変数宣言構文を生成します。
		/// </summary>
		/// <param name="expression">初期化表現</param>
		/// <returns>
		///		生成した CodeStatement を返します。
		/// </returns>
		public CodeVariableDeclarationStatement Declare(CodeExpression expression = null)
		{
			if (expression == null)
				return new CodeVariableDeclarationStatement(this.Type, this.VariableName);
			else
				return new CodeVariableDeclarationStatement(this.Type, this.VariableName, expression);
		}

		/// <summary>
		/// 代入構文を生成します。
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeStatement を返します。
		/// </returns>
		public CodeAssignStatement Assign(CodeExpression value)
		{
			return new CodeAssignStatement(this, value);
		}

		#endregion

		#region メソッド: (生成)

		/// <summary>
		/// オブジェクト生成の表現を生成します。
		/// </summary>
		/// <param name="parameters">コンストラクタに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeObjectCreateExpression New(params CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(this.Type, parameters);
		}

		#endregion

		#region メソッド: (参照方向)

		/// <summary>
		/// オブジェクトの参照方向表現を生成します。
		/// </summary>
		/// <param name="mode">モード [0:ref, 1:out]</param>
		/// <returns>
		///		オブジェクトの参照方向表現を返します。
		/// </returns>
		public CodeDirectionExpression Out(int mode = 0)
		{
			switch (mode)
			{
				default:
				case 0:
					return new CodeDirectionExpression(FieldDirection.Ref, this);
				case 1:
					return new CodeDirectionExpression(FieldDirection.Out, this);
			}
		}

		#endregion

		#region メソッド: (プロパティ参照)

		/// <summary>
		/// プロパティ参照の表現を生成します。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraProperty Ref(string name)
		{
			return this.Extension.Ref(name);
		}

		#endregion

		#region メソッド: (メソッド実行)

		/// <summary>
		/// メソッド実行の表現を生成します。
		/// </summary>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraMethod Call(string name, params CodeExpression[] parameters)
		{
			return this.Extension.Call(name, parameters);
		}

		#endregion

		#region メソッド: (オペレータ)

		/// <summary>
		/// 算術演算子（＋）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Add(CodeExpression value)
		{
			return this.Extension.Add(value);
		}

		/// <summary>
		/// 算術演算子（－）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Subtract(CodeExpression value)
		{
			return this.Extension.Subtract(value);
		}

		/// <summary>
		/// 算術演算子（＊）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Multiply(CodeExpression value)
		{
			return this.Extension.Multiply(value);
		}

		/// <summary>
		/// 算術演算子（／）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Divide(CodeExpression value)
		{
			return this.Extension.Divide(value);
		}

		/// <summary>
		/// 算術演算子（％）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Modulus(CodeExpression value)
		{
			return this.Extension.Modulus(value);
		}

		/// <summary>
		/// 比較演算子（！＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression NotEqual(CodeExpression value)
		{
			return this.Extension.NotEqual(value);
		}

		/// <summary>
		/// 比較演算子（＝＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Equal(CodeExpression value)
		{
			return this.Extension.Equal(value);
		}

		/// <summary>
		/// 論理演算子（｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseOr(CodeExpression value)
		{
			return this.Extension.BitwiseOr(value);
		}

		/// <summary>
		/// 論理演算子（＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseAnd(CodeExpression value)
		{
			return this.Extension.BitwiseAnd(value);
		}

		/// <summary>
		/// 論理演算子（｜｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanOr(CodeExpression value)
		{
			return this.Extension.BooleanOr(value);
		}

		/// <summary>
		/// 論理演算子（＆＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanAnd(CodeExpression value)
		{
			return this.Extension.BooleanAnd(value);
		}

		/// <summary>
		/// 比較演算子（＜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThan(CodeExpression value)
		{
			return this.Extension.LessThan(value);
		}

		/// <summary>
		/// 比較演算子（≦）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThanOrEqual(CodeExpression value)
		{
			return this.Extension.LessThanOrEqual(value);
		}

		/// <summary>
		/// 比較演算子（＞）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThan(CodeExpression value)
		{
			return this.Extension.GreaterThan(value);
		}

		/// <summary>
		/// 比較演算子（≧）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThanOrEqual(CodeExpression value)
		{
			return this.Extension.GreaterThanOrEqual(value);
		}

		#endregion
	}

	/// <summary>
	/// 拡張版のメソッド実行表現
	/// </summary>
	class CodeExtraMethod : CodeMethodInvokeExpression
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="method">メソッド参照表現</param>
		/// <param name="parameters">メソッドへ渡す引数</param>
		public CodeExtraMethod(CodeMethodReferenceExpression method, params CodeExpression[] parameters)
			: base(method, parameters)
		{
			Extension = new CodeObjectExtension(this);
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="target">対象のオブジェクト参照表現</param>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドへ渡す引数</param>
		public CodeExtraMethod(CodeExpression target, string name, params CodeExpression[] parameters)
			: base(target, name, parameters)
		{
			Extension = new CodeObjectExtension(this);
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// オブジェクト表現の拡張
		/// </summary>
		private CodeObjectExtension Extension;

		#endregion

		#region メソッド: (プロパティ参照)

		/// <summary>
		/// プロパティ参照の表現を生成します。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraProperty Ref(string name)
		{
			return this.Extension.Ref(name);
		}

		#endregion

		#region メソッド: (メソッド実行)

		/// <summary>
		/// メソッド実行の表現を生成します。
		/// </summary>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraMethod Call(string name, params CodeExpression[] parameters)
		{
			return this.Extension.Call(name, parameters);
		}

		#endregion

		#region メソッド: (オペレータ)

		/// <summary>
		/// 算術演算子（＋）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Add(CodeExpression value)
		{
			return this.Extension.Add(value);
		}

		/// <summary>
		/// 算術演算子（－）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Subtract(CodeExpression value)
		{
			return this.Extension.Subtract(value);
		}

		/// <summary>
		/// 算術演算子（＊）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Multiply(CodeExpression value)
		{
			return this.Extension.Multiply(value);
		}

		/// <summary>
		/// 算術演算子（／）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Divide(CodeExpression value)
		{
			return this.Extension.Divide(value);
		}

		/// <summary>
		/// 算術演算子（％）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Modulus(CodeExpression value)
		{
			return this.Extension.Modulus(value);
		}

		/// <summary>
		/// 比較演算子（！＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression NotEqual(CodeExpression value)
		{
			return this.Extension.NotEqual(value);
		}

		/// <summary>
		/// 比較演算子（＝＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Equal(CodeExpression value)
		{
			return this.Extension.Equal(value);
		}

		/// <summary>
		/// 論理演算子（｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseOr(CodeExpression value)
		{
			return this.Extension.BitwiseOr(value);
		}

		/// <summary>
		/// 論理演算子（＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseAnd(CodeExpression value)
		{
			return this.Extension.BitwiseAnd(value);
		}

		/// <summary>
		/// 論理演算子（｜｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanOr(CodeExpression value)
		{
			return this.Extension.BooleanOr(value);
		}

		/// <summary>
		/// 論理演算子（＆＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanAnd(CodeExpression value)
		{
			return this.Extension.BooleanAnd(value);
		}

		/// <summary>
		/// 比較演算子（＜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThan(CodeExpression value)
		{
			return this.Extension.LessThan(value);
		}

		/// <summary>
		/// 比較演算子（≦）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThanOrEqual(CodeExpression value)
		{
			return this.Extension.LessThanOrEqual(value);
		}

		/// <summary>
		/// 比較演算子（＞）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThan(CodeExpression value)
		{
			return this.Extension.GreaterThan(value);
		}

		/// <summary>
		/// 比較演算子（≧）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThanOrEqual(CodeExpression value)
		{
			return this.Extension.GreaterThanOrEqual(value);
		}

		#endregion
	}

	/// <summary>
	/// 拡張版のプロパティ参照表現
	/// </summary>
	class CodeExtraProperty : CodePropertyReferenceExpression
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="target">対象のオブジェクト参照表現</param>
		/// <param name="name">プロパティ名</param>
		public CodeExtraProperty(CodeExpression target, string name)
			: base(target, name)
		{
			Extension = new CodeObjectExtension(this);
		}

		#endregion

		#region フィールド:

		/// <summary>
		/// オブジェクト表現の拡張
		/// </summary>
		private CodeObjectExtension Extension;

		#endregion

		#region メソッド: (プロパティ参照)

		/// <summary>
		/// プロパティ参照の表現を生成します。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraProperty Ref(string name)
		{
			return this.Extension.Ref(name);
		}

		#endregion

		#region メソッド: (メソッド実行)

		/// <summary>
		/// メソッド実行の表現を生成します。
		/// </summary>
		/// <param name="name">メソッド名</param>
		/// <param name="parameters">メソッドに渡す引数</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeExtraMethod Call(string name, params CodeExpression[] parameters)
		{
			return this.Extension.Call(name, parameters);
		}

		#endregion

		#region メソッド: (オペレータ)

		/// <summary>
		/// 算術演算子（＋）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Add(CodeExpression value)
		{
			return this.Extension.Add(value);
		}

		/// <summary>
		/// 算術演算子（－）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Subtract(CodeExpression value)
		{
			return this.Extension.Subtract(value);
		}

		/// <summary>
		/// 算術演算子（＊）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Multiply(CodeExpression value)
		{
			return this.Extension.Multiply(value);
		}

		/// <summary>
		/// 算術演算子（／）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Divide(CodeExpression value)
		{
			return this.Extension.Divide(value);
		}

		/// <summary>
		/// 算術演算子（％）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Modulus(CodeExpression value)
		{
			return this.Extension.Modulus(value);
		}

		/// <summary>
		/// 比較演算子（！＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression NotEqual(CodeExpression value)
		{
			return this.Extension.NotEqual(value);
		}

		/// <summary>
		/// 比較演算子（＝＝）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression Equal(CodeExpression value)
		{
			return this.Extension.Equal(value);
		}

		/// <summary>
		/// 論理演算子（｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseOr(CodeExpression value)
		{
			return this.Extension.BitwiseOr(value);
		}

		/// <summary>
		/// 論理演算子（＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BitwiseAnd(CodeExpression value)
		{
			return this.Extension.BitwiseAnd(value);
		}

		/// <summary>
		/// 論理演算子（｜｜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanOr(CodeExpression value)
		{
			return this.Extension.BooleanOr(value);
		}

		/// <summary>
		/// 論理演算子（＆＆）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression BooleanAnd(CodeExpression value)
		{
			return this.Extension.BooleanAnd(value);
		}

		/// <summary>
		/// 比較演算子（＜）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThan(CodeExpression value)
		{
			return this.Extension.LessThan(value);
		}

		/// <summary>
		/// 比較演算子（≦）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression LessThanOrEqual(CodeExpression value)
		{
			return this.Extension.LessThanOrEqual(value);
		}

		/// <summary>
		/// 比較演算子（＞）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThan(CodeExpression value)
		{
			return this.Extension.GreaterThan(value);
		}

		/// <summary>
		/// 比較演算子（≧）
		/// </summary>
		/// <param name="value">右辺値</param>
		/// <returns>
		///		生成した CodeExpression を返します。
		/// </returns>
		public CodeBinaryOperatorExpression GreaterThanOrEqual(CodeExpression value)
		{
			return this.Extension.GreaterThanOrEqual(value);
		}

		#endregion
	}

	/// <summary>
	/// 拡張版の例外発行構文
	/// </summary>
	class CodeExtraThrow : CodeThrowExceptionStatement
	{
		#region コンストラクタ:

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">例外クラスの型</param>
		/// <param name="parameters">例外クラスのコンストラクタに渡す引数</param>
		public CodeExtraThrow(Type type, params CodeExpression[] parameters)
			: base(new CodeObjectCreateExpression(type, parameters))
		{
		}

		#endregion
	}
}
