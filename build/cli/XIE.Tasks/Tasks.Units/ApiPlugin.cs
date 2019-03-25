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

namespace XIE.Tasks
{
	/// <summary>
	/// プラグイン
	/// </summary>
	[CxPluginClass("Tasks")]
	public static class ApiPlugin
	{
		/// <summary>
		/// 初期化
		/// </summary>
		/// <param name="units">タスクユニットのコレクション</param>
		[CxPluginSetup("Tasks")]
		public static void Setup(Dictionary<XIE.Tasks.CxTaskUnit, string> units)
		{
			XIE.Log.Api.Trace("XIE.Tasks:");

			#region Script:
			// -------------------------------------------------------
			{
				var category = AddFolder(units, "Script", "T:XIE.Tasks.CxScriptEx", "TabPage");

				category.Tasks[new XIE.Tasks.CxScriptEx(XIE.Tasks.ExLanguageType.CSharp)] = "";
				category.Tasks[new XIE.Tasks.CxScriptEx(XIE.Tasks.ExLanguageType.VisualBasic)] = "";
			}
			#endregion

			#region Ports
			{
				var category = AddFolder(units, "Ports", "", "TabPage");
				category.Tasks[new XIE.Tasks.CxTaskUnit_DataIn_Data()] = "";
				category.Tasks[new XIE.Tasks.CxTaskUnit_DataParam_Data()] = "";
				category.Tasks[new XIE.Tasks.CxTaskUnit_DataOut_Data()] = "";
			}
			#endregion

			#region AuxInfo:
			// -------------------------------------------------------
			// 注1) AuxInfo/Tasks を変更すると CxTaskflowForm.Toolbox_Setup の処理に影響する.
			{
				var category = AddFolder(units, "AuxInfo", "T:XIE.Tasks.CxAuxInfo", "TabPage");

				#region Camera
				{
					var tag = AddFolder(category, "Camera", "T:XIE.Tasks.CxAuxInfo_Camera_Controller", "Camera-Connect", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_Camera_Controller()] = "Controller";
					tag.Tasks[new XIE.Tasks.CxGrabber_OneShot()] = "";
				}
				#endregion

				#region SerialPort
				{
					var tag = AddFolder(category, "SerialPort", "T:XIE.Tasks.CxAuxInfo_SerialPort_Controller", "SerialPort-Connect", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_SerialPort_Controller()] = "Controller";
					tag.Tasks[new XIE.Tasks.CxSerialPort_Read()] = "";
					tag.Tasks[new XIE.Tasks.CxSerialPort_Readable()] = "";
					tag.Tasks[new XIE.Tasks.CxSerialPort_Write()] = "";
					tag.Tasks[new XIE.Tasks.CxSerialPort_Writeable()] = "";
				}
				#endregion

				#region TcpServer
				{
					var tag = AddFolder(category, "TcpServer", "T:XIE.Tasks.CxAuxInfo_TcpServer_Controller", "Net-Connect", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_TcpServer_Controller()] = "Controller";
					tag.Tasks[new XIE.Tasks.CxTcpServer_Connections()] = "";
					tag.Tasks[new XIE.Tasks.CxTcpServer_Stream()] = "";
					tag.Tasks[new XIE.Tasks.TxSocketStream_Read()] = "Stream.Read";
					tag.Tasks[new XIE.Tasks.TxSocketStream_Write()] = "Stream.Write";
				}
				#endregion

				#region TcpClient
				{
					var tag = AddFolder(category, "TcpClient", "T:XIE.Tasks.CxAuxInfo_TcpClient_Controller", "Net-Connect", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_TcpClient_Controller()] = "Controller";
					tag.Tasks[new XIE.Tasks.CxTcpClient_Stream()] = "";
					tag.Tasks[new XIE.Tasks.TxSocketStream_Read()] = "Stream.Read";
					tag.Tasks[new XIE.Tasks.TxSocketStream_Write()] = "Stream.Write";
				}
				#endregion

				#region Media
				{
					var tag = AddFolder(category, "Media", "T:XIE.Tasks.CxAuxInfo_Media_Player", "Media-Connect", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_Media_Player()] = "Player";
				}
				#endregion

				#region Image
				{
					var tag = AddFolder(category, "Image", "T:XIE.Tasks.CxAuxInfo_Image_Data", "Data-Image", 0);
					tag.Tasks[new XIE.Tasks.CxAuxInfo_Image_Data()] = "Data";
				}
				#endregion

				#region Tasks
				{
					var tag = AddFolder(category, "Tasks", "T:XIE.Tasks.Syntax_Class", "Service-Connect", 0);
					tag.Tasks[new XIE.Tasks.Syntax_Class()] = "New Taskflow";
				}
				#endregion
			}
			#endregion

			#region System:
			// -------------------------------------------------------
			{
				var category = AddFolder(units, "System", "", "TabPage");

				#region Syntax
				{
					var subdir = AddFolder(category, "Syntax", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Syntax_Class()] = "";
					subdir.Tasks[new XIE.Tasks.Syntax_Scope()] = "";

					subdir.Tasks[new XIE.Tasks.Syntax_If()] = "";
					subdir.Tasks[new XIE.Tasks.Syntax_ElseIf()] = "";
					subdir.Tasks[new XIE.Tasks.Syntax_Else()] = "";

					// For
					{
						var tag = new XIE.Tasks.Syntax_For();
						subdir.Tasks[tag] = "";
						tag.TaskUnits.Add(new XIE.Tasks.Syntax_For_Index() { Location = new Point(64, 64) });
					}

					// ForEach
					{
						var tag = new XIE.Tasks.Syntax_ForEach();
						subdir.Tasks[tag] = "";
						tag.TaskUnits.Add(new XIE.Tasks.Syntax_ForEach_Item() { Location = new Point(64, 64) });
					}

					subdir.Tasks[new XIE.Tasks.Syntax_Continue()] = "";
					subdir.Tasks[new XIE.Tasks.Syntax_Break()] = "";
					subdir.Tasks[new XIE.Tasks.Syntax_Return()] = "";
				}
				#endregion

				#region Collections
				{
					var subdir = AddFolder(category, "Collections", "", "TabPage");

					#region Array
					{
						var tag = AddFolder(subdir, "Array", "", "List.Number", 0);
						tag.Tasks[new XIE.Tasks.Array_ctor()] = "";
						tag.Tasks[new XIE.Tasks.Array_Length()] = "";
						// Item
						{
							var member = AddFolder(tag, "Item", "", "Unit-Property", 0);
							member.Tasks[new XIE.Tasks.Array_Item_get()] = "get";
							member.Tasks[new XIE.Tasks.Array_Item_set()] = "set";
						}
						tag.Tasks[new XIE.Tasks.Array_Copy1()] = "";
					}
					#endregion

					#region List
					{
						var tag = AddFolder(subdir, "List", "", "List.Number", 0);
						tag.Tasks[new XIE.Tasks.List_ctor()] = "";
						tag.Tasks[new XIE.Tasks.List_Add()] = "";
						tag.Tasks[new XIE.Tasks.List_AddRange()] = "";
						tag.Tasks[new XIE.Tasks.List_Clear()] = "";
						tag.Tasks[new XIE.Tasks.List_Count()] = "";
						// Item
						{
							var member = AddFolder(tag, "Item", "", "Unit-Property", 0);
							member.Tasks[new XIE.Tasks.List_Item_get()] = "get";
							member.Tasks[new XIE.Tasks.List_Item_set()] = "set";
						}
						tag.Tasks[new XIE.Tasks.List_ToArray()] = "";
					}
					#endregion

					subdir.Tasks[new XIE.Tasks.FileNameList()] = "";
				}
				#endregion

				#region Primitive
				{
					var subdir = AddFolder(category, "Primitive", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Boolean_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Byte_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.SByte_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Char_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Int16_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Int32_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Int64_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.UInt16_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.UInt32_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.UInt64_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Single_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.Double_ctor()] = "";
				}
				#endregion

				#region Structures
				{
					var subdir = AddFolder(category, "Structures", "", "TabPage");

					#region Color
					{
						var tag = AddFolder(subdir, "Color", "", "Structure", 0);
						tag.Tasks[new XIE.Tasks.Color_ctor()] = "";
						tag.Tasks[new XIE.Tasks.Color_Properties()] = "";
					}
					#endregion

					#region DateTime
					{
						var tag = AddFolder(subdir, "DateTime", "", "Structure", 0);
						tag.Tasks[new XIE.Tasks.DateTime_ctor()] = "";
						tag.Tasks[new XIE.Tasks.DateTime_Now()] = "";
						tag.Tasks[new XIE.Tasks.DateTime_UtcNow()] = "";
						tag.Tasks[new XIE.Tasks.DateTime_Properties()] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("Kind")] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("Ticks")] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("Date")] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("DayOfWeek")] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("DayOfYear")] = "";
						tag.Tasks[new XIE.Tasks.DateTime_GetProperty("TimeOfDay")] = "";
					}
					#endregion

					#region TimeSpan
					{
						var tag = AddFolder(subdir, "TimeSpan", "", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TimeSpan_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_Properties()] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("Ticks")] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("TotalDays")] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("TotalHours")] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("TotalMinutes")] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("TotalSeconds")] = "";
						tag.Tasks[new XIE.Tasks.TimeSpan_GetProperty("TotalMilliseconds")] = "";
					}
					#endregion
				}
				#endregion

				#region Comparison
				{
					var subdir = AddFolder(category, "Comparison", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.Equal)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.NotEqual)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.LessThan)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.LessThanOrEqual)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.GreaterThan)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_Comparison(ExComparisonOperatorType.GreaterThanOrEqual)] = "";
				}
				#endregion

				#region Operations
				{
					var subdir = AddFolder(category, "Operations", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Add)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Subtract)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Multiply)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Divide)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Modulus)] = "";

					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.And)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Or)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Xor)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_BinaryOperation(ExBinaryOperatorType.Nand)] = "";

					subdir.Tasks[new XIE.Tasks.Primitive_UnaryOperation(ExUnaryOperatorType.BooleanNot)] = "";
					subdir.Tasks[new XIE.Tasks.Primitive_UnaryOperation(ExUnaryOperatorType.BitwiseNot)] = "";
				}
				#endregion

				#region Math
				{
					var subdir = AddFolder(category, "Math", "", "TabPage");
					subdir.Tasks[new XIE.Tasks.System_Math_Abs()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Sign()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Truncate()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Ceiling()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Floor()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Round()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Exp()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Log()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Log10()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Sqrt()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Sin()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Cos()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Tan()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Sinh()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Cosh()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Tanh()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Asin()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Acos()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Atan()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Atan2()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Pow()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Max()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_Min()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_BigMul()] = "";
					subdir.Tasks[new XIE.Tasks.System_Math_DivRem()] = "";
				}
				#endregion

				#region Text
				{
					var subdir = AddFolder(category, "Text", "", "TabPage");

					#region String
					{
						var tag = AddFolder(subdir, "String", "", "Font", 0);
						tag.Tasks[new XIE.Tasks.String_ctor()] = "";
						tag.Tasks[new XIE.Tasks.String_GetProperty("Length")] = "";
						tag.Tasks[new XIE.Tasks.String_TrimEnd()] = "";
					}
					#endregion

					#region Encoding
					{
						var tag = AddFolder(subdir, "Encoding", "", "Font", 0);
						tag.Tasks[new XIE.Tasks.Encoding_ctor()] = "";
						tag.Tasks[new XIE.Tasks.Encoding_GetBytes()] = "";
						tag.Tasks[new XIE.Tasks.Encoding_GetString()] = "";
					}
					#endregion
				}
				#endregion

				#region Threading
				{
					var subdir = AddFolder(category, "Threading", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Thread_Sleep()] = "";
				}
				#endregion
			}
			#endregion

			#region XIE:
			// -------------------------------------------------------
			{
				var category = AddFolder(units, "XIE", "", "TabPage");

				#region CxStopwatch
				{
					var tag = AddFolder(category, "CxStopwatch", "T:XIE.CxStopwatch", "Task-Timer", 0);

					tag.Tasks[new XIE.Tasks.CxStopwatch_ctor()] = "";
					// Elapsed
					{
						var member = AddFolder(tag, "Elapsed", "F:XIE.CxStopwatch.Elapsed", "Unit-Property", 0);
						member.Tasks[new XIE.Tasks.CxStopwatch_Elapsed_get()] = "get";
						member.Tasks[new XIE.Tasks.CxStopwatch_Elapsed_set()] = "set";
					}
					// Lap
					{
						var member = AddFolder(tag, "Lap", "F:XIE.CxStopwatch.Lap", "Unit-Property", 0);
						member.Tasks[new XIE.Tasks.CxStopwatch_Lap_get()] = "get";
						member.Tasks[new XIE.Tasks.CxStopwatch_Lap_set()] = "set";
					}
					// -----
					tag.Tasks[new XIE.Tasks.CxStopwatch_Reset()] = "";
					tag.Tasks[new XIE.Tasks.CxStopwatch_Start()] = "";
					tag.Tasks[new XIE.Tasks.CxStopwatch_Stop()] = "";
				}
				#endregion

				#region CxImage
				{
					var tag = AddFolder(category, "CxImage", "T:XIE.CxImage", "Data-Image", 0);

					tag.Tasks[new XIE.Tasks.CxImage_ctor()] = "Constructor";
					// -----
					tag.Tasks[new XIE.Tasks.CxImage_ImageSize()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Size()] = "";
					// Depth
					{
						var member = AddFolder(tag, "Depth", "P:XIE.CxImage.Depth", "Unit-Property", 0);
						member.Tasks[new XIE.Tasks.CxImage_Depth_get()] = "get";
						member.Tasks[new XIE.Tasks.CxImage_Depth_set()] = "set";
					}
					// -----
					tag.Tasks[new XIE.Tasks.CxImage_Load()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Save()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Resize()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Reset()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Clear()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Clone()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Child()] = "";
					tag.Tasks[new XIE.Tasks.CxImage_Statistics()] = "";

					// Exif
					{
						var member = AddFolder(tag, "Exif", "", "Unit-Property", 0);
						member.Tasks[new XIE.Tasks.CxImage_Exif_get()] = "get";
						member.Tasks[new XIE.Tasks.CxImage_Exif_set()] = "set";
					}
					tag.Tasks[new XIE.Tasks.CxImage_ExifCopy()] = "";

					#region Filter:
					// -----
					{
						var member = AddFolder(tag, "Filter", "", "TabPage");
						member.Tasks[new XIE.Tasks.CxImageFilter_RgbToBgr()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Compare()] = "";
						// ----
						member.Tasks[new XIE.Tasks.CxImageFilter_Mirror()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Rotate()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Transpose()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Scale()] = "";
						// ----
						member.Tasks[new XIE.Tasks.CxImageFilter_Math()] = "";
						// ----
						member.Tasks[new XIE.Tasks.CxImageFilter_Add()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Sub()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Mul()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Div()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Mod()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Min()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Max()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Diff()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Pow()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Atan2()] = "";
						// ----
						member.Tasks[new XIE.Tasks.CxImageFilter_And()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Nand()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Or()] = "";
						member.Tasks[new XIE.Tasks.CxImageFilter_Xor()] = "";
					}
					#endregion

					#region Effectors:
					// -----
					{
						var member = AddFolder(tag, "Effectors", "", "TabPage");
						member.Tasks[new XIE.Tasks.CxImageEffectors_Binarize1()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_Binarize2()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_GammaConverter()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_RgbToGray()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_RgbToHsv()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_HsvToRgb()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_HsvConverter()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_RgbConverter()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_Monochrome()] = "";
						member.Tasks[new XIE.Tasks.CxImageEffectors_PartColor()] = "";
					}
					#endregion

					#region Utility:
					// -----
					{
						var member = AddFolder(tag, "Utility", "", "TabPage");
						member.Tasks[new XIE.Tasks.CxImageUtility_Split()] = "";
						member.Tasks[new XIE.Tasks.CxImageUtility_Combine()] = "";
					}
					#endregion
				}
				#endregion

				#region Structures:
				// -------------------------------------------------------
				{
					var subdir = AddFolder(category, "Structures", "", "TabPage");

					#region TxImageSize
					{
						var tag = AddFolder(subdir, "TxImageSize", "T:XIE.TxImageSize", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxImageSize_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxImageSize_Properties()] = "";
					}
					#endregion

					#region TxModel
					{
						var tag = AddFolder(subdir, "TxModel", "T:XIE.TxModel", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxModel_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxModel_Properties()] = "";
					}
					#endregion

					#region TxRGB8x3
					{
						var tag = AddFolder(subdir, "TxRGB8x3", "T:XIE.TxRGB8x3", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxRGB8x3_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxRGB8x3_Properties()] = "";
					}
					#endregion

					#region TxBGR8x3
					{
						var tag = AddFolder(subdir, "TxBGR8x3", "T:XIE.TxBGR8x3", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxBGR8x3_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxBGR8x3_Properties()] = "";
					}
					#endregion

					#region TxPointD
					{
						var tag = AddFolder(subdir, "TxPointD", "T:XIE.TxPointD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxPointD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxPointD_Properties()] = "";
					}
					#endregion

					#region TxPointI
					{
						var tag = AddFolder(subdir, "TxPointI", "T:XIE.TxPointI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxPointI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxPointI_Properties()] = "";
					}
					#endregion

					#region TxSizeD
					{
						var tag = AddFolder(subdir, "TxSizeD", "T:XIE.TxSizeD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxSizeD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxSizeD_Properties()] = "";
					}
					#endregion

					#region TxSizeI
					{
						var tag = AddFolder(subdir, "TxSizeI", "T:XIE.TxSizeI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxSizeI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxSizeI_Properties()] = "";
					}
					#endregion

					#region TxRangeD
					{
						var tag = AddFolder(subdir, "TxRangeD", "T:XIE.TxRangeD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxRangeD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxRangeD_Properties()] = "";
					}
					#endregion

					#region TxRangeI
					{
						var tag = AddFolder(subdir, "TxRangeI", "T:XIE.TxRangeI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxRangeI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxRangeI_Properties()] = "";
					}
					#endregion

					#region TxRectangleD
					{
						var tag = AddFolder(subdir, "TxRectangleD", "T:XIE.TxRectangleD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxRectangleD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxRectangleD_Properties()] = "";
					}
					#endregion

					#region TxRectangleI
					{
						var tag = AddFolder(subdir, "TxRectangleI", "T:XIE.TxRectangleI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxRectangleI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxRectangleI_Properties()] = "";
					}
					#endregion

					#region TxCircleD
					{
						var tag = AddFolder(subdir, "TxCircleD", "T:XIE.TxCircleD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxCircleD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxCircleD_Properties()] = "";
					}
					#endregion

					#region TxCircleI
					{
						var tag = AddFolder(subdir, "TxCircleI", "T:XIE.TxCircleI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxCircleI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxCircleI_Properties()] = "";
					}
					#endregion

					#region TxCircleArcD
					{
						var tag = AddFolder(subdir, "TxCircleArcD", "T:XIE.TxCircleArcD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxCircleArcD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxCircleArcD_Properties()] = "";
					}
					#endregion

					#region TxCircleArcI
					{
						var tag = AddFolder(subdir, "TxCircleArcI", "T:XIE.TxCircleArcI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxCircleArcI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxCircleArcI_Properties()] = "";
					}
					#endregion

					#region TxEllipseD
					{
						var tag = AddFolder(subdir, "TxEllipseD", "T:XIE.TxEllipseD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxEllipseD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxEllipseD_Properties()] = "";
					}
					#endregion

					#region TxEllipseI
					{
						var tag = AddFolder(subdir, "TxEllipseI", "T:XIE.TxEllipseI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxEllipseI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxEllipseI_Properties()] = "";
					}
					#endregion

					#region TxEllipseArcD
					{
						var tag = AddFolder(subdir, "TxEllipseArcD", "T:XIE.TxEllipseArcD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxEllipseArcD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxEllipseArcD_Properties()] = "";
					}
					#endregion

					#region TxEllipseArcI
					{
						var tag = AddFolder(subdir, "TxEllipseArcI", "T:XIE.TxEllipseArcI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxEllipseArcI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxEllipseArcI_Properties()] = "";
					}
					#endregion

					#region TxLineD
					{
						var tag = AddFolder(subdir, "TxLineD", "T:XIE.TxLineD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxLineD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxLineD_Properties()] = "";
					}
					#endregion

					#region TxLineI
					{
						var tag = AddFolder(subdir, "TxLineI", "T:XIE.TxLineI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxLineI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxLineI_Properties()] = "";
					}
					#endregion

					#region TxLineSegmentD
					{
						var tag = AddFolder(subdir, "TxLineSegmentD", "T:XIE.TxLineSegmentD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxLineSegmentD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxLineSegmentD_Properties()] = "";
					}
					#endregion

					#region TxLineSegmentI
					{
						var tag = AddFolder(subdir, "TxLineSegmentI", "T:XIE.TxLineSegmentI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxLineSegmentI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxLineSegmentI_Properties()] = "";
					}
					#endregion

					#region TxTrapezoidD
					{
						var tag = AddFolder(subdir, "TxTrapezoidD", "T:XIE.TxTrapezoidD", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxTrapezoidD_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxTrapezoidD_Properties()] = "";
					}
					#endregion

					#region TxTrapezoidI
					{
						var tag = AddFolder(subdir, "TxTrapezoidI", "T:XIE.TxTrapezoidI", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxTrapezoidI_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxTrapezoidI_Properties()] = "";
					}
					#endregion

					#region TxStatistics
					{
						var tag = AddFolder(subdir, "TxStatistics", "T:XIE.TxStatistics", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxStatistics_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxStatistics_Properties()] = "";
					}
					#endregion

					#region TxDateTime
					{
						var tag = AddFolder(subdir, "TxDateTime", "T:XIE.TxDateTime", "Structure", 0);
						tag.Tasks[new XIE.Tasks.TxDateTime_ctor()] = "";
						tag.Tasks[new XIE.Tasks.TxDateTime_Properties()] = "";
					}
					#endregion
				}
				#endregion

				#region Enumerations:
				// -------------------------------------------------------
				{
					var subdir = AddFolder(category, "Enumerations", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.ExBoolean_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.ExEndianType_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.ExScanDir_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.ExStatus_ctor()] = "";
					subdir.Tasks[new XIE.Tasks.ExType_ctor()] = "";
				}
				#endregion

				#region Axi:
				// -------------------------------------------------------
				{
					var subdir = AddFolder(category, "Axi", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Axi_CalcBpp()] = "";
					subdir.Tasks[new XIE.Tasks.Axi_CalcDepth()] = "";
					subdir.Tasks[new XIE.Tasks.Axi_CalcRange()] = "";
					subdir.Tasks[new XIE.Tasks.Axi_CalcScale()] = "";
					subdir.Tasks[new XIE.Tasks.Axi_CalcStride()] = "";

					subdir.Tasks[new XIE.Tasks.Axi_DegToRad()] = "";
					subdir.Tasks[new XIE.Tasks.Axi_RadToDeg()] = "";
				}
				#endregion

				#region Defs:
				// -------------------------------------------------------
				{
					var subdir = AddFolder(category, "Defs", "", "TabPage");

					subdir.Tasks[new XIE.Tasks.Defs_XIE_PI()] = "";
					subdir.Tasks[new XIE.Tasks.Defs_XIE_EPSd()] = "";
					subdir.Tasks[new XIE.Tasks.Defs_XIE_EPSf()] = "";
				}
				#endregion
				
			}
			#endregion
		}

		/// <summary>
		/// 解放
		/// </summary>
		[CxPluginTearDown("Tasks")]
		public static void TearDown()
		{
		}

		/// <summary>
		/// フォルダを追加します。
		/// </summary>
		/// <param name="units">追加先</param>
		/// <param name="name">このフォルダの名称</param>
		/// <param name="description_key">このフォルダの説明文を示すリソースキー</param>
		/// <param name="icon_key">このフォルダのアイコンを示すキー</param>
		/// <param name="default_index">既定のユニットの指標。このフォルダ配下のユニットの指標を指定してください。無ければ -1 を指定してください。</param>
		/// <returns>
		///		追加したフォルダを返します。
		/// </returns>
		private static XIE.Tasks.CxTaskFolder AddFolder(Dictionary<XIE.Tasks.CxTaskUnit, string> units, string name, string description_key, string icon_key, int default_index = -1)
		{
			var folder = new XIE.Tasks.CxTaskFolder(name, description_key, icon_key, default_index);
			units[folder] = "";
			return folder;
		}

		/// <summary>
		/// フォルダを追加します。
		/// </summary>
		/// <param name="parent">追加先</param>
		/// <param name="name">このフォルダの名称</param>
		/// <param name="description_key">このフォルダの説明文を示すリソースキー</param>
		/// <param name="icon_key">このフォルダのアイコンを示すキー</param>
		/// <param name="default_index">既定のユニットの指標。このフォルダ配下のユニットの指標を指定してください。無ければ -1 を指定してください。</param>
		/// <returns>
		///		追加したフォルダを返します。
		/// </returns>
		private static XIE.Tasks.CxTaskFolder AddFolder(XIE.Tasks.CxTaskFolder parent, string name, string description_key, string icon_key, int default_index = -1)
		{
			var folder = new XIE.Tasks.CxTaskFolder(name, description_key, icon_key, default_index);
			parent.Tasks[folder] = "";
			return folder;
		}
	}
}
