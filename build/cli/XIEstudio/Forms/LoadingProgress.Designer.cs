namespace XIEstudio
{
	partial class LoadingProgressForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingProgressForm));
			this.spacer1 = new System.Windows.Forms.PictureBox();
			this.labelPercentage = new System.Windows.Forms.Label();
			this.labelDescription = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			((System.ComponentModel.ISupportInitialize)(this.spacer1)).BeginInit();
			this.SuspendLayout();
			// 
			// spacer1
			// 
			this.spacer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.spacer1.BackColor = System.Drawing.Color.Blue;
			this.spacer1.Location = new System.Drawing.Point(3, 41);
			this.spacer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.spacer1.Name = "spacer1";
			this.spacer1.Size = new System.Drawing.Size(346, 5);
			this.spacer1.TabIndex = 2;
			this.spacer1.TabStop = false;
			// 
			// labelPercentage
			// 
			this.labelPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPercentage.Font = new System.Drawing.Font("Meiryo", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelPercentage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.labelPercentage.Location = new System.Drawing.Point(269, 8);
			this.labelPercentage.Name = "labelPercentage";
			this.labelPercentage.Size = new System.Drawing.Size(70, 30);
			this.labelPercentage.TabIndex = 1;
			this.labelPercentage.Text = "0%";
			this.labelPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDescription
			// 
			this.labelDescription.Font = new System.Drawing.Font("Meiryo", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.labelDescription.Location = new System.Drawing.Point(12, 8);
			this.labelDescription.Name = "labelDescription";
			this.labelDescription.Size = new System.Drawing.Size(233, 30);
			this.labelDescription.TabIndex = 0;
			this.labelDescription.Text = "Loading ...";
			this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Meiryo", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.label1.Location = new System.Drawing.Point(12, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "XIE-Studio";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// imageList16
			// 
			this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
			this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList16.Images.SetKeyName(0, "Align");
			this.imageList16.Images.SetKeyName(1, "App-Config");
			this.imageList16.Images.SetKeyName(2, "App-Task");
			this.imageList16.Images.SetKeyName(3, "Ask-Cancel");
			this.imageList16.Images.SetKeyName(4, "Ask-OK");
			this.imageList16.Images.SetKeyName(5, "Attach");
			this.imageList16.Images.SetKeyName(6, "BreakPoint");
			this.imageList16.Images.SetKeyName(7, "Chart.Org");
			this.imageList16.Images.SetKeyName(8, "CheckBox");
			this.imageList16.Images.SetKeyName(9, "cmd");
			this.imageList16.Images.SetKeyName(10, "Cursor-Next");
			this.imageList16.Images.SetKeyName(11, "Cursor-Prev");
			this.imageList16.Images.SetKeyName(12, "Font");
			this.imageList16.Images.SetKeyName(13, "GotoShortcuts");
			this.imageList16.Images.SetKeyName(14, "Grab-Oneshot");
			this.imageList16.Images.SetKeyName(15, "Grab-Pause");
			this.imageList16.Images.SetKeyName(16, "Grab-Start");
			this.imageList16.Images.SetKeyName(17, "Grab-Stop");
			this.imageList16.Images.SetKeyName(18, "List.Bullet");
			this.imageList16.Images.SetKeyName(19, "List.Legend");
			this.imageList16.Images.SetKeyName(20, "List.Number");
			this.imageList16.Images.SetKeyName(21, "List.Option");
			this.imageList16.Images.SetKeyName(22, "Node-Book-Closed");
			this.imageList16.Images.SetKeyName(23, "Node-Book-Opened");
			this.imageList16.Images.SetKeyName(24, "Node-DataIn");
			this.imageList16.Images.SetKeyName(25, "Node-DataOut");
			this.imageList16.Images.SetKeyName(26, "Node-OptionIn");
			this.imageList16.Images.SetKeyName(27, "Node-OptionOut");
			this.imageList16.Images.SetKeyName(28, "Notify-Error");
			this.imageList16.Images.SetKeyName(29, "Notify-IHelp");
			this.imageList16.Images.SetKeyName(30, "Notify-Info");
			this.imageList16.Images.SetKeyName(31, "Notify-None");
			this.imageList16.Images.SetKeyName(32, "Notify-OK");
			this.imageList16.Images.SetKeyName(33, "Notify-Question");
			this.imageList16.Images.SetKeyName(34, "Notify-Warning");
			this.imageList16.Images.SetKeyName(35, "PaintBrush");
			this.imageList16.Images.SetKeyName(36, "Record-Pause");
			this.imageList16.Images.SetKeyName(37, "Record-Start");
			this.imageList16.Images.SetKeyName(38, "Relationship");
			this.imageList16.Images.SetKeyName(39, "Security-Key2");
			this.imageList16.Images.SetKeyName(40, "Security-Key");
			this.imageList16.Images.SetKeyName(41, "Security-Locked");
			this.imageList16.Images.SetKeyName(42, "Sort");
			this.imageList16.Images.SetKeyName(43, "TabPage");
			this.imageList16.Images.SetKeyName(44, "Task-Calculator");
			this.imageList16.Images.SetKeyName(45, "Task-Editor");
			this.imageList16.Images.SetKeyName(46, "Task-Link");
			this.imageList16.Images.SetKeyName(47, "Task-Manual");
			this.imageList16.Images.SetKeyName(48, "Task-Pause");
			this.imageList16.Images.SetKeyName(49, "Task-Pushpin");
			this.imageList16.Images.SetKeyName(50, "Task-Repeat");
			this.imageList16.Images.SetKeyName(51, "Task-Start");
			this.imageList16.Images.SetKeyName(52, "Task-Step");
			this.imageList16.Images.SetKeyName(53, "Task-Stop");
			this.imageList16.Images.SetKeyName(54, "Task-Timer");
			this.imageList16.Images.SetKeyName(55, "Color-Black");
			this.imageList16.Images.SetKeyName(56, "Color-White");
			this.imageList16.Images.SetKeyName(57, "Data2d-Kernel");
			this.imageList16.Images.SetKeyName(58, "Data2d-Matrix");
			this.imageList16.Images.SetKeyName(59, "Data2d-SE");
			this.imageList16.Images.SetKeyName(60, "Data-Anchor");
			this.imageList16.Images.SetKeyName(61, "Data-Angle");
			this.imageList16.Images.SetKeyName(62, "Data-Arc");
			this.imageList16.Images.SetKeyName(63, "Data-Circle");
			this.imageList16.Images.SetKeyName(64, "Data-Color");
			this.imageList16.Images.SetKeyName(65, "Data-DateTime");
			this.imageList16.Images.SetKeyName(66, "Data-Ellipse");
			this.imageList16.Images.SetKeyName(67, "Data-GroupOff");
			this.imageList16.Images.SetKeyName(68, "Data-GroupOn");
			this.imageList16.Images.SetKeyName(69, "Data-Image");
			this.imageList16.Images.SetKeyName(70, "Data-LineSegment");
			this.imageList16.Images.SetKeyName(71, "Data-Line");
			this.imageList16.Images.SetKeyName(72, "Data-Point");
			this.imageList16.Images.SetKeyName(73, "Data-Polyline");
			this.imageList16.Images.SetKeyName(74, "Data-Rectangle");
			this.imageList16.Images.SetKeyName(75, "Data-String");
			this.imageList16.Images.SetKeyName(76, "Image-Bin");
			this.imageList16.Images.SetKeyName(77, "Image-Gray");
			this.imageList16.Images.SetKeyName(78, "Image-RGB");
			this.imageList16.Images.SetKeyName(79, "Image-Signed");
			this.imageList16.Images.SetKeyName(80, "Image-Unpacked");
			this.imageList16.Images.SetKeyName(81, "Camera-Connect");
			this.imageList16.Images.SetKeyName(82, "Camera-Disconnect");
			this.imageList16.Images.SetKeyName(83, "Camera-Pause");
			this.imageList16.Images.SetKeyName(84, "Camera-Record");
			this.imageList16.Images.SetKeyName(85, "Camera-Stop");
			this.imageList16.Images.SetKeyName(86, "IO-Connect");
			this.imageList16.Images.SetKeyName(87, "IO-Disconnect");
			this.imageList16.Images.SetKeyName(88, "IO-Stop");
			this.imageList16.Images.SetKeyName(89, "Mail-Connect");
			this.imageList16.Images.SetKeyName(90, "Mail-Disconnect");
			this.imageList16.Images.SetKeyName(91, "Mail-Stop");
			this.imageList16.Images.SetKeyName(92, "Media-Connect");
			this.imageList16.Images.SetKeyName(93, "Media-Pause");
			this.imageList16.Images.SetKeyName(94, "Media-Stop");
			this.imageList16.Images.SetKeyName(95, "Net-Connect");
			this.imageList16.Images.SetKeyName(96, "Net-Disconnect");
			this.imageList16.Images.SetKeyName(97, "Net-Stop");
			this.imageList16.Images.SetKeyName(98, "SerialPort-Connect");
			this.imageList16.Images.SetKeyName(99, "SerialPort-Disconnect");
			this.imageList16.Images.SetKeyName(100, "SerialPort-Stop");
			this.imageList16.Images.SetKeyName(101, "Service-Connect");
			this.imageList16.Images.SetKeyName(102, "Service-Disconnect");
			this.imageList16.Images.SetKeyName(103, "Service-Pause");
			this.imageList16.Images.SetKeyName(104, "Service-Run");
			this.imageList16.Images.SetKeyName(105, "Service-Stop");
			this.imageList16.Images.SetKeyName(106, "Service-Unknown");
			this.imageList16.Images.SetKeyName(107, "Switch-Connect");
			this.imageList16.Images.SetKeyName(108, "Switch-Disconnect");
			this.imageList16.Images.SetKeyName(109, "Edit-Copy2");
			this.imageList16.Images.SetKeyName(110, "Edit-Copy");
			this.imageList16.Images.SetKeyName(111, "Edit-Cut2");
			this.imageList16.Images.SetKeyName(112, "Edit-Cut");
			this.imageList16.Images.SetKeyName(113, "Edit-DeleteBk");
			this.imageList16.Images.SetKeyName(114, "Edit-Delete");
			this.imageList16.Images.SetKeyName(115, "Edit-Paste2");
			this.imageList16.Images.SetKeyName(116, "Edit-Paste");
			this.imageList16.Images.SetKeyName(117, "Edit-Redo");
			this.imageList16.Images.SetKeyName(118, "Edit-Rename");
			this.imageList16.Images.SetKeyName(119, "Edit-Split");
			this.imageList16.Images.SetKeyName(120, "Edit-Undo");
			this.imageList16.Images.SetKeyName(121, "File-Inifile1");
			this.imageList16.Images.SetKeyName(122, "File-Inifile");
			this.imageList16.Images.SetKeyName(123, "File-Layout");
			this.imageList16.Images.SetKeyName(124, "File-NewFolder");
			this.imageList16.Images.SetKeyName(125, "File-New");
			this.imageList16.Images.SetKeyName(126, "File-Open");
			this.imageList16.Images.SetKeyName(127, "File-Property");
			this.imageList16.Images.SetKeyName(128, "File-SaveAll");
			this.imageList16.Images.SetKeyName(129, "File-SaveAsWebPage");
			this.imageList16.Images.SetKeyName(130, "File-SaveWithDesign");
			this.imageList16.Images.SetKeyName(131, "File-Save");
			this.imageList16.Images.SetKeyName(132, "File-Search");
			this.imageList16.Images.SetKeyName(133, "File-SourceCode");
			this.imageList16.Images.SetKeyName(134, "File-XML");
			this.imageList16.Images.SetKeyName(135, "Lang-CPP");
			this.imageList16.Images.SetKeyName(136, "Lang-CS");
			this.imageList16.Images.SetKeyName(137, "Lang-VB");
			this.imageList16.Images.SetKeyName(138, "Overlay-Graph");
			this.imageList16.Images.SetKeyName(139, "Overlay-Grid");
			this.imageList16.Images.SetKeyName(140, "Overlay-Mask1");
			this.imageList16.Images.SetKeyName(141, "Overlay-Mask2");
			this.imageList16.Images.SetKeyName(142, "Overlay-Profile");
			this.imageList16.Images.SetKeyName(143, "Overlay-ROI-Active");
			this.imageList16.Images.SetKeyName(144, "Overlay-ROI-Fixed");
			this.imageList16.Images.SetKeyName(145, "Overlay-Trend");
			this.imageList16.Images.SetKeyName(146, "Parser-Barcode2");
			this.imageList16.Images.SetKeyName(147, "Parser-Barcode");
			this.imageList16.Images.SetKeyName(148, "Parser-ChannelUnpack");
			this.imageList16.Images.SetKeyName(149, "Parser-ColorConvert");
			this.imageList16.Images.SetKeyName(150, "Parser-DM");
			this.imageList16.Images.SetKeyName(151, "Parser-EdgeDetect");
			this.imageList16.Images.SetKeyName(152, "Parser-EnhanceBright");
			this.imageList16.Images.SetKeyName(153, "Parser-EnhanceDark");
			this.imageList16.Images.SetKeyName(154, "Parser-Enhance");
			this.imageList16.Images.SetKeyName(155, "Parser-FilterEdge");
			this.imageList16.Images.SetKeyName(156, "Parser-FilterGauss");
			this.imageList16.Images.SetKeyName(157, "Parser-FilterSharp");
			this.imageList16.Images.SetKeyName(158, "Parser-FilterSmooth");
			this.imageList16.Images.SetKeyName(159, "Parser-Formula");
			this.imageList16.Images.SetKeyName(160, "Parser-Function");
			this.imageList16.Images.SetKeyName(161, "Parser-GeoTransMirror1");
			this.imageList16.Images.SetKeyName(162, "Parser-GeoTransMirror2");
			this.imageList16.Images.SetKeyName(163, "Parser-GeoTransRotate45");
			this.imageList16.Images.SetKeyName(164, "Parser-GeoTransRotate90");
			this.imageList16.Images.SetKeyName(165, "Parser-GeoTransScale1");
			this.imageList16.Images.SetKeyName(166, "Parser-GeoTransScale2");
			this.imageList16.Images.SetKeyName(167, "Parser-IHoughDetect");
			this.imageList16.Images.SetKeyName(168, "Parser-Measure");
			this.imageList16.Images.SetKeyName(169, "Parser-MorClosing");
			this.imageList16.Images.SetKeyName(170, "Parser-MorDilation");
			this.imageList16.Images.SetKeyName(171, "Parser-MorErosion");
			this.imageList16.Images.SetKeyName(172, "Parser-MorOpening");
			this.imageList16.Images.SetKeyName(173, "Parser-Operation");
			this.imageList16.Images.SetKeyName(174, "Parser-PolarTransInvert");
			this.imageList16.Images.SetKeyName(175, "Parser-PolarTrans");
			this.imageList16.Images.SetKeyName(176, "Parser-QR");
			this.imageList16.Images.SetKeyName(177, "Parser-Search1");
			this.imageList16.Images.SetKeyName(178, "Parser-Search2");
			this.imageList16.Images.SetKeyName(179, "Mag-Actual");
			this.imageList16.Images.SetKeyName(180, "Mag-Expand");
			this.imageList16.Images.SetKeyName(181, "Mag-Reduce");
			this.imageList16.Images.SetKeyName(182, "TreeView");
			this.imageList16.Images.SetKeyName(183, "View-FitImageHeight");
			this.imageList16.Images.SetKeyName(184, "View-FitImageSize");
			this.imageList16.Images.SetKeyName(185, "View-FitImageWidth");
			this.imageList16.Images.SetKeyName(186, "View-Fullscreen");
			this.imageList16.Images.SetKeyName(187, "View-Halftone");
			this.imageList16.Images.SetKeyName(188, "View-ModeActiveL");
			this.imageList16.Images.SetKeyName(189, "View-ModeActiveR");
			this.imageList16.Images.SetKeyName(190, "View-ModeSyc");
			this.imageList16.Images.SetKeyName(191, "View-Mode");
			this.imageList16.Images.SetKeyName(192, "View-PixelMode");
			this.imageList16.Images.SetKeyName(193, "View-Pointer");
			this.imageList16.Images.SetKeyName(194, "View-Screen");
			this.imageList16.Images.SetKeyName(195, "View-Snap");
			this.imageList16.Images.SetKeyName(196, "View-Thumbnail");
			this.imageList16.Images.SetKeyName(197, "View-Zoom");
			this.imageList16.Images.SetKeyName(198, "Window-SplitterB");
			this.imageList16.Images.SetKeyName(199, "Window-SplitterL");
			this.imageList16.Images.SetKeyName(200, "Window-SplitterR");
			// 
			// LoadingProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.ClientSize = new System.Drawing.Size(353, 90);
			this.ControlBox = false;
			this.Controls.Add(this.labelPercentage);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelDescription);
			this.Controls.Add(this.spacer1);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "LoadingProgressForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Loading Progress";
			this.Load += new System.EventHandler(this.LoadingProgressForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.spacer1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox spacer1;
		private System.Windows.Forms.Label labelPercentage;
		private System.Windows.Forms.Label labelDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ImageList imageList16;
	}
}