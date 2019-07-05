namespace XIEstudio
{
	partial class CxAuxInfoForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CxAuxInfoForm));
			this.statusbar = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusSpacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolImageName = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolImagePrev = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolImageNext = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusMouseInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusViewInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusWorkingSet = new System.Windows.Forms.ToolStripDropDownButton();
			this.statusGCcollect = new System.Windows.Forms.ToolStripMenuItem();
			this.splitVert = new System.Windows.Forms.SplitContainer();
			this.treeAux = new XIEstudio.CxAuxTreeView();
			this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
			this.NetworkMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuNetworkOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuNetworkClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator41 = new System.Windows.Forms.ToolStripSeparator();
			this.menuNetworkProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.SerialPortMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuSerialPortOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSerialPortClose = new System.Windows.Forms.ToolStripMenuItem();
			this.DataMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuDataOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDataSave = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDataClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.menuDataThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDataProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.MediaMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuMediaStart = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMediaPause = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMediaStop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.menuMediaSaveGRF = new System.Windows.Forms.ToolStripMenuItem();
			this.menuMediaClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.menuMediaProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraSaveGRF = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraClose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuCameraRecord = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraStart = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraStop = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
			this.menuCameraProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraPropertyAudio = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCameraPropertyVideo = new System.Windows.Forms.ToolStripMenuItem();
			this.CameraMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.TaskMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuTaskNew = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTaskOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTaskSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.menuTaskClose = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTaskDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuTaskThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTaskProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.toolbarView = new XIE.Forms.CxToolStripEx();
			this.toolLogForm = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.toolFileOpen = new System.Windows.Forms.ToolStripButton();
			this.toolAudioInput = new System.Windows.Forms.ToolStripButton();
			this.toolAudioOutput = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewClipboardObserver = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewCopy = new System.Windows.Forms.ToolStripButton();
			this.toolViewPaste = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewROI = new System.Windows.Forms.ToolStripButton();
			this.toolViewExif = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewFitImageSize = new System.Windows.Forms.ToolStripSplitButton();
			this.toolViewFitImageWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewFitImageHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale = new System.Windows.Forms.ToolStripSplitButton();
			this.toolViewScale0010 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0025 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0050 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0075 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0100 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0200 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0400 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale0800 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale1600 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScale3200 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewScaleDown = new System.Windows.Forms.ToolStripButton();
			this.toolViewScaleUp = new System.Windows.Forms.ToolStripButton();
			this.toolFullscreen = new System.Windows.Forms.ToolStripButton();
			this.toolPreview = new System.Windows.Forms.ToolStripButton();
			this.toolViewHalftone = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewDepth = new System.Windows.Forms.ToolStripSplitButton();
			this.toolViewDepthDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewUnpack = new System.Windows.Forms.ToolStripButton();
			this.toolViewChannelPrev = new System.Windows.Forms.ToolStripButton();
			this.toolViewChannelNo = new System.Windows.Forms.ToolStripLabel();
			this.toolViewChannelNext = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewProfile = new System.Windows.Forms.ToolStripButton();
			this.toolViewGrid = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.toolViewSnapshot = new System.Windows.Forms.ToolStripSplitButton();
			this.toolViewSnapshotOverlayMode0 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewSnapshotOverlayMode1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolViewSnapshotOverlayMode2 = new System.Windows.Forms.ToolStripMenuItem();
			this.ImageViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuFullscreen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuPreview = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewROI = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewExif = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewFitImageSize = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewFitImageSizeBoth = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewFitImageSizeWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewFitImageSizeHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0010 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0025 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0050 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0075 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0100 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0200 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0400 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale0800 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale1600 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScale3200 = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScaleReset = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScaleDown = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewScaleUp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewHalftone = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewDepth = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewDepthFit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewDepthDefault = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewUnpack = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewChannelPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewChannelNext = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator26 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewProfile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.statusbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitVert)).BeginInit();
			this.splitVert.Panel1.SuspendLayout();
			this.splitVert.SuspendLayout();
			this.NetworkMenu.SuspendLayout();
			this.SerialPortMenu.SuspendLayout();
			this.DataMenu.SuspendLayout();
			this.MediaMenu.SuspendLayout();
			this.CameraMenu.SuspendLayout();
			this.TaskMenu.SuspendLayout();
			this.toolbarView.SuspendLayout();
			this.ImageViewMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusbar
			// 
			this.statusbar.AutoSize = false;
			this.statusbar.BackColor = System.Drawing.SystemColors.Control;
			this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusSpacer,
            this.toolImageName,
            this.toolImagePrev,
            this.toolImageNext,
            this.statusMouseInfo,
            this.statusViewInfo,
            this.statusWorkingSet});
			this.statusbar.Location = new System.Drawing.Point(0, 647);
			this.statusbar.Name = "statusbar";
			this.statusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusbar.Size = new System.Drawing.Size(1264, 35);
			this.statusbar.TabIndex = 1;
			this.statusbar.Text = "statusbar";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(13, 30);
			this.statusMessage.Text = "-";
			// 
			// statusSpacer
			// 
			this.statusSpacer.Name = "statusSpacer";
			this.statusSpacer.Size = new System.Drawing.Size(530, 30);
			this.statusSpacer.Spring = true;
			// 
			// toolImageName
			// 
			this.toolImageName.Name = "toolImageName";
			this.toolImageName.Size = new System.Drawing.Size(43, 30);
			this.toolImageName.Text = "Name";
			// 
			// toolImagePrev
			// 
			this.toolImagePrev.AutoSize = false;
			this.toolImagePrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolImagePrev.Image = ((System.Drawing.Image)(resources.GetObject("toolImagePrev.Image")));
			this.toolImagePrev.Name = "toolImagePrev";
			this.toolImagePrev.Size = new System.Drawing.Size(32, 30);
			this.toolImagePrev.Text = "Prev";
			this.toolImagePrev.Click += new System.EventHandler(this.toolImagePrev_Click);
			// 
			// toolImageNext
			// 
			this.toolImageNext.AutoSize = false;
			this.toolImageNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolImageNext.Image = ((System.Drawing.Image)(resources.GetObject("toolImageNext.Image")));
			this.toolImageNext.Name = "toolImageNext";
			this.toolImageNext.Size = new System.Drawing.Size(32, 30);
			this.toolImageNext.Text = "Next";
			this.toolImageNext.Click += new System.EventHandler(this.toolImageNext_Click);
			// 
			// statusMouseInfo
			// 
			this.statusMouseInfo.AutoSize = false;
			this.statusMouseInfo.Name = "statusMouseInfo";
			this.statusMouseInfo.Size = new System.Drawing.Size(250, 30);
			this.statusMouseInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// statusViewInfo
			// 
			this.statusViewInfo.AutoSize = false;
			this.statusViewInfo.Font = new System.Drawing.Font("Meiryo", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.statusViewInfo.Name = "statusViewInfo";
			this.statusViewInfo.Size = new System.Drawing.Size(250, 30);
			// 
			// statusWorkingSet
			// 
			this.statusWorkingSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statusWorkingSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusGCcollect});
			this.statusWorkingSet.Image = ((System.Drawing.Image)(resources.GetObject("statusWorkingSet.Image")));
			this.statusWorkingSet.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.statusWorkingSet.Name = "statusWorkingSet";
			this.statusWorkingSet.Size = new System.Drawing.Size(97, 33);
			this.statusWorkingSet.Text = "(xxx,xxx KB)";
			this.statusWorkingSet.ToolTipText = "WorkingSet";
			// 
			// statusGCcollect
			// 
			this.statusGCcollect.Image = ((System.Drawing.Image)(resources.GetObject("statusGCcollect.Image")));
			this.statusGCcollect.Name = "statusGCcollect";
			this.statusGCcollect.Size = new System.Drawing.Size(136, 22);
			this.statusGCcollect.Text = "GC.Collect";
			this.statusGCcollect.Click += new System.EventHandler(this.statusGCcollect_Click);
			// 
			// splitVert
			// 
			this.splitVert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitVert.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitVert.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitVert.Location = new System.Drawing.Point(0, 35);
			this.splitVert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitVert.Name = "splitVert";
			// 
			// splitVert.Panel1
			// 
			this.splitVert.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitVert.Panel1.Controls.Add(this.treeAux);
			this.splitVert.Size = new System.Drawing.Size(1264, 612);
			this.splitVert.SplitterDistance = 300;
			this.splitVert.SplitterWidth = 8;
			this.splitVert.TabIndex = 2;
			// 
			// treeAux
			// 
			this.treeAux.BackColor = System.Drawing.SystemColors.Control;
			this.treeAux.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeAux.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeAux.FullRowSelect = true;
			this.treeAux.HideSelection = false;
			this.treeAux.ImageKey = "Node.Book.Closed";
			this.treeAux.Location = new System.Drawing.Point(0, 0);
			this.treeAux.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.treeAux.Name = "treeAux";
			this.treeAux.SelectedImageKey = "Node.Book.Closed";
			this.treeAux.Size = new System.Drawing.Size(298, 610);
			this.treeAux.TabIndex = 1;
			this.treeAux.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeAux_AfterSelect);
			this.treeAux.DoubleClick += new System.EventHandler(this.treeAux_DoubleClick);
			this.treeAux.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeAux_KeyDown);
			this.treeAux.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeAux_KeyPress);
			this.treeAux.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeAux_MouseDown);
			// 
			// timerUpdateUI
			// 
			this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
			// 
			// NetworkMenu
			// 
			this.NetworkMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNetworkOpen,
            this.menuNetworkClose,
            this.toolStripSeparator41,
            this.menuNetworkProperty});
			this.NetworkMenu.Name = "DioMenu";
			this.NetworkMenu.Size = new System.Drawing.Size(149, 76);
			this.NetworkMenu.Opened += new System.EventHandler(this.NetworkMenu_Opened);
			// 
			// menuNetworkOpen
			// 
			this.menuNetworkOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuNetworkOpen.Image")));
			this.menuNetworkOpen.Name = "menuNetworkOpen";
			this.menuNetworkOpen.Size = new System.Drawing.Size(148, 22);
			this.menuNetworkOpen.Text = "Open (&O)";
			this.menuNetworkOpen.Click += new System.EventHandler(this.menuNetworkOpen_Click);
			// 
			// menuNetworkClose
			// 
			this.menuNetworkClose.Image = ((System.Drawing.Image)(resources.GetObject("menuNetworkClose.Image")));
			this.menuNetworkClose.Name = "menuNetworkClose";
			this.menuNetworkClose.Size = new System.Drawing.Size(148, 22);
			this.menuNetworkClose.Text = "Close (&C)";
			this.menuNetworkClose.Click += new System.EventHandler(this.menuNetworkClose_Click);
			// 
			// toolStripSeparator41
			// 
			this.toolStripSeparator41.Name = "toolStripSeparator41";
			this.toolStripSeparator41.Size = new System.Drawing.Size(145, 6);
			// 
			// menuNetworkProperty
			// 
			this.menuNetworkProperty.Image = ((System.Drawing.Image)(resources.GetObject("menuNetworkProperty.Image")));
			this.menuNetworkProperty.Name = "menuNetworkProperty";
			this.menuNetworkProperty.Size = new System.Drawing.Size(148, 22);
			this.menuNetworkProperty.Text = "Property (&R)";
			this.menuNetworkProperty.Click += new System.EventHandler(this.menuNetworkSetting_Click);
			// 
			// SerialPortMenu
			// 
			this.SerialPortMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSerialPortOpen,
            this.menuSerialPortClose});
			this.SerialPortMenu.Name = "DioMenu";
			this.SerialPortMenu.Size = new System.Drawing.Size(130, 48);
			this.SerialPortMenu.Opened += new System.EventHandler(this.SerialPortMenu_Opened);
			// 
			// menuSerialPortOpen
			// 
			this.menuSerialPortOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuSerialPortOpen.Image")));
			this.menuSerialPortOpen.Name = "menuSerialPortOpen";
			this.menuSerialPortOpen.Size = new System.Drawing.Size(129, 22);
			this.menuSerialPortOpen.Text = "Open (&O)";
			this.menuSerialPortOpen.Click += new System.EventHandler(this.toolSerialPortOpen_Click);
			// 
			// menuSerialPortClose
			// 
			this.menuSerialPortClose.Image = ((System.Drawing.Image)(resources.GetObject("menuSerialPortClose.Image")));
			this.menuSerialPortClose.Name = "menuSerialPortClose";
			this.menuSerialPortClose.Size = new System.Drawing.Size(129, 22);
			this.menuSerialPortClose.Text = "Close (&C)";
			this.menuSerialPortClose.Click += new System.EventHandler(this.toolSerialPortClose_Click);
			// 
			// DataMenu
			// 
			this.DataMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDataOpen,
            this.menuDataSave,
            this.menuDataClose,
            this.toolStripSeparator10,
            this.menuDataThumbnail,
            this.menuDataProperty});
			this.DataMenu.Name = "CameraParamMenu";
			this.DataMenu.Size = new System.Drawing.Size(185, 120);
			this.DataMenu.Opened += new System.EventHandler(this.DataMenu_Opened);
			// 
			// menuDataOpen
			// 
			this.menuDataOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuDataOpen.Image")));
			this.menuDataOpen.Name = "menuDataOpen";
			this.menuDataOpen.Size = new System.Drawing.Size(184, 22);
			this.menuDataOpen.Text = "Open (&O)";
			this.menuDataOpen.Click += new System.EventHandler(this.menuDataOpen_Click);
			// 
			// menuDataSave
			// 
			this.menuDataSave.Image = ((System.Drawing.Image)(resources.GetObject("menuDataSave.Image")));
			this.menuDataSave.Name = "menuDataSave";
			this.menuDataSave.ShortcutKeyDisplayString = "[Ctrl+S]";
			this.menuDataSave.Size = new System.Drawing.Size(184, 22);
			this.menuDataSave.Text = "Save (&S)";
			this.menuDataSave.Click += new System.EventHandler(this.menuDataSave_Click);
			// 
			// menuDataClose
			// 
			this.menuDataClose.Image = ((System.Drawing.Image)(resources.GetObject("menuDataClose.Image")));
			this.menuDataClose.Name = "menuDataClose";
			this.menuDataClose.Size = new System.Drawing.Size(184, 22);
			this.menuDataClose.Text = "Close (&C)";
			this.menuDataClose.Click += new System.EventHandler(this.menuDataClose_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(181, 6);
			// 
			// menuDataThumbnail
			// 
			this.menuDataThumbnail.Image = ((System.Drawing.Image)(resources.GetObject("menuDataThumbnail.Image")));
			this.menuDataThumbnail.Name = "menuDataThumbnail";
			this.menuDataThumbnail.Size = new System.Drawing.Size(184, 22);
			this.menuDataThumbnail.Text = "Thumbnail";
			this.menuDataThumbnail.Click += new System.EventHandler(this.menuDataThumbnail_Click);
			// 
			// menuDataProperty
			// 
			this.menuDataProperty.Image = ((System.Drawing.Image)(resources.GetObject("menuDataProperty.Image")));
			this.menuDataProperty.Name = "menuDataProperty";
			this.menuDataProperty.Size = new System.Drawing.Size(184, 22);
			this.menuDataProperty.Text = "Property (&R)";
			this.menuDataProperty.Click += new System.EventHandler(this.menuDataProperty_Click);
			// 
			// MediaMenu
			// 
			this.MediaMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMediaStart,
            this.menuMediaPause,
            this.menuMediaStop,
            this.toolStripSeparator13,
            this.menuMediaSaveGRF,
            this.menuMediaClose,
            this.toolStripSeparator14,
            this.menuMediaProperty});
			this.MediaMenu.Name = "CameraParamMenu";
			this.MediaMenu.Size = new System.Drawing.Size(149, 148);
			this.MediaMenu.Opened += new System.EventHandler(this.MediaMenu_Opened);
			// 
			// menuMediaStart
			// 
			this.menuMediaStart.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaStart.Image")));
			this.menuMediaStart.Name = "menuMediaStart";
			this.menuMediaStart.Size = new System.Drawing.Size(148, 22);
			this.menuMediaStart.Text = "Start (&A)";
			this.menuMediaStart.Click += new System.EventHandler(this.menuMediaStart_Click);
			// 
			// menuMediaPause
			// 
			this.menuMediaPause.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaPause.Image")));
			this.menuMediaPause.Name = "menuMediaPause";
			this.menuMediaPause.Size = new System.Drawing.Size(148, 22);
			this.menuMediaPause.Text = "Pause";
			this.menuMediaPause.Click += new System.EventHandler(this.menuMediaPause_Click);
			// 
			// menuMediaStop
			// 
			this.menuMediaStop.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaStop.Image")));
			this.menuMediaStop.Name = "menuMediaStop";
			this.menuMediaStop.Size = new System.Drawing.Size(148, 22);
			this.menuMediaStop.Text = "Stop (&T)";
			this.menuMediaStop.Click += new System.EventHandler(this.menuMediaStop_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(145, 6);
			// 
			// menuMediaSaveGRF
			// 
			this.menuMediaSaveGRF.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaSaveGRF.Image")));
			this.menuMediaSaveGRF.Name = "menuMediaSaveGRF";
			this.menuMediaSaveGRF.Size = new System.Drawing.Size(148, 22);
			this.menuMediaSaveGRF.Text = "Save GRF";
			this.menuMediaSaveGRF.Click += new System.EventHandler(this.menuMediaSaveGRF_Click);
			// 
			// menuMediaClose
			// 
			this.menuMediaClose.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaClose.Image")));
			this.menuMediaClose.Name = "menuMediaClose";
			this.menuMediaClose.Size = new System.Drawing.Size(148, 22);
			this.menuMediaClose.Text = "Close (&C)";
			this.menuMediaClose.Click += new System.EventHandler(this.menuMediaClose_Click);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(145, 6);
			// 
			// menuMediaProperty
			// 
			this.menuMediaProperty.Image = ((System.Drawing.Image)(resources.GetObject("menuMediaProperty.Image")));
			this.menuMediaProperty.Name = "menuMediaProperty";
			this.menuMediaProperty.Size = new System.Drawing.Size(148, 22);
			this.menuMediaProperty.Text = "Property (&R)";
			this.menuMediaProperty.Click += new System.EventHandler(this.menuMediaProperty_Click);
			// 
			// menuCameraOpen
			// 
			this.menuCameraOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraOpen.Image")));
			this.menuCameraOpen.Name = "menuCameraOpen";
			this.menuCameraOpen.Size = new System.Drawing.Size(172, 22);
			this.menuCameraOpen.Text = "Open (&O)";
			this.menuCameraOpen.Click += new System.EventHandler(this.menuCameraOpen_Click);
			// 
			// menuCameraSaveGRF
			// 
			this.menuCameraSaveGRF.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraSaveGRF.Image")));
			this.menuCameraSaveGRF.Name = "menuCameraSaveGRF";
			this.menuCameraSaveGRF.Size = new System.Drawing.Size(172, 22);
			this.menuCameraSaveGRF.Text = "Save GRF";
			this.menuCameraSaveGRF.Click += new System.EventHandler(this.menuCameraSaveGRF_Click);
			// 
			// menuCameraClose
			// 
			this.menuCameraClose.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraClose.Image")));
			this.menuCameraClose.Name = "menuCameraClose";
			this.menuCameraClose.Size = new System.Drawing.Size(172, 22);
			this.menuCameraClose.Text = "Close (&C)";
			this.menuCameraClose.Click += new System.EventHandler(this.menuCameraClose_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
			// 
			// menuCameraRecord
			// 
			this.menuCameraRecord.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraRecord.Image")));
			this.menuCameraRecord.Name = "menuCameraRecord";
			this.menuCameraRecord.Size = new System.Drawing.Size(172, 22);
			this.menuCameraRecord.Text = "Record";
			this.menuCameraRecord.Click += new System.EventHandler(this.menuCameraRecord_Click);
			// 
			// menuCameraStart
			// 
			this.menuCameraStart.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraStart.Image")));
			this.menuCameraStart.Name = "menuCameraStart";
			this.menuCameraStart.Size = new System.Drawing.Size(172, 22);
			this.menuCameraStart.Text = "Start (&A)";
			this.menuCameraStart.Click += new System.EventHandler(this.menuCameraStart_Click);
			// 
			// menuCameraStop
			// 
			this.menuCameraStop.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraStop.Image")));
			this.menuCameraStop.Name = "menuCameraStop";
			this.menuCameraStop.Size = new System.Drawing.Size(172, 22);
			this.menuCameraStop.Text = "Stop (&T)";
			this.menuCameraStop.Click += new System.EventHandler(this.menuCameraStop_Click);
			// 
			// toolStripSeparator24
			// 
			this.toolStripSeparator24.Name = "toolStripSeparator24";
			this.toolStripSeparator24.Size = new System.Drawing.Size(169, 6);
			// 
			// menuCameraProperty
			// 
			this.menuCameraProperty.Image = ((System.Drawing.Image)(resources.GetObject("menuCameraProperty.Image")));
			this.menuCameraProperty.Name = "menuCameraProperty";
			this.menuCameraProperty.Size = new System.Drawing.Size(172, 22);
			this.menuCameraProperty.Text = "Property (&R)";
			this.menuCameraProperty.Click += new System.EventHandler(this.menuCameraProperty_Click);
			// 
			// menuCameraPropertyAudio
			// 
			this.menuCameraPropertyAudio.Name = "menuCameraPropertyAudio";
			this.menuCameraPropertyAudio.Size = new System.Drawing.Size(172, 22);
			this.menuCameraPropertyAudio.Text = "Property (Audio)";
			this.menuCameraPropertyAudio.Click += new System.EventHandler(this.menuCameraPropertyAudio_Click);
			// 
			// menuCameraPropertyVideo
			// 
			this.menuCameraPropertyVideo.Name = "menuCameraPropertyVideo";
			this.menuCameraPropertyVideo.Size = new System.Drawing.Size(172, 22);
			this.menuCameraPropertyVideo.Text = "Property (Video)";
			this.menuCameraPropertyVideo.Click += new System.EventHandler(this.menuCameraPropertyVideo_Click);
			// 
			// CameraMenu
			// 
			this.CameraMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCameraOpen,
            this.menuCameraSaveGRF,
            this.menuCameraClose,
            this.toolStripSeparator1,
            this.menuCameraRecord,
            this.menuCameraStart,
            this.menuCameraStop,
            this.toolStripSeparator24,
            this.menuCameraProperty,
            this.menuCameraPropertyAudio,
            this.menuCameraPropertyVideo});
			this.CameraMenu.Name = "CameraMenu";
			this.CameraMenu.Size = new System.Drawing.Size(173, 214);
			this.CameraMenu.Opened += new System.EventHandler(this.CameraMenu_Opened);
			// 
			// TaskMenu
			// 
			this.TaskMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTaskNew,
            this.menuTaskOpen,
            this.menuTaskSave,
            this.toolStripSeparator9,
            this.menuTaskClose,
            this.menuTaskDelete,
            this.toolStripSeparator3,
            this.menuTaskThumbnail,
            this.menuTaskProperty});
			this.TaskMenu.Name = "CameraParamMenu";
			this.TaskMenu.Size = new System.Drawing.Size(185, 170);
			this.TaskMenu.Opened += new System.EventHandler(this.TaskMenu_Opened);
			// 
			// menuTaskNew
			// 
			this.menuTaskNew.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskNew.Image")));
			this.menuTaskNew.Name = "menuTaskNew";
			this.menuTaskNew.Size = new System.Drawing.Size(184, 22);
			this.menuTaskNew.Text = "New (&N)";
			this.menuTaskNew.Click += new System.EventHandler(this.menuTaskNew_Click);
			// 
			// menuTaskOpen
			// 
			this.menuTaskOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskOpen.Image")));
			this.menuTaskOpen.Name = "menuTaskOpen";
			this.menuTaskOpen.Size = new System.Drawing.Size(184, 22);
			this.menuTaskOpen.Text = "Open (&O)";
			this.menuTaskOpen.Click += new System.EventHandler(this.menuTaskOpen_Click);
			// 
			// menuTaskSave
			// 
			this.menuTaskSave.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskSave.Image")));
			this.menuTaskSave.Name = "menuTaskSave";
			this.menuTaskSave.ShortcutKeyDisplayString = "[Ctrl+S]";
			this.menuTaskSave.Size = new System.Drawing.Size(184, 22);
			this.menuTaskSave.Text = "Save (&S)";
			this.menuTaskSave.Click += new System.EventHandler(this.menuTaskSave_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(181, 6);
			// 
			// menuTaskClose
			// 
			this.menuTaskClose.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskClose.Image")));
			this.menuTaskClose.Name = "menuTaskClose";
			this.menuTaskClose.Size = new System.Drawing.Size(184, 22);
			this.menuTaskClose.Text = "Close (&C)";
			this.menuTaskClose.Click += new System.EventHandler(this.menuTaskClose_Click);
			// 
			// menuTaskDelete
			// 
			this.menuTaskDelete.Name = "menuTaskDelete";
			this.menuTaskDelete.Size = new System.Drawing.Size(184, 22);
			this.menuTaskDelete.Text = "Delete";
			this.menuTaskDelete.Click += new System.EventHandler(this.menuTaskDelete_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
			// 
			// menuTaskThumbnail
			// 
			this.menuTaskThumbnail.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskThumbnail.Image")));
			this.menuTaskThumbnail.Name = "menuTaskThumbnail";
			this.menuTaskThumbnail.Size = new System.Drawing.Size(184, 22);
			this.menuTaskThumbnail.Text = "Thumbnail";
			this.menuTaskThumbnail.Click += new System.EventHandler(this.menuTaskThumbnail_Click);
			// 
			// menuTaskProperty
			// 
			this.menuTaskProperty.Image = ((System.Drawing.Image)(resources.GetObject("menuTaskProperty.Image")));
			this.menuTaskProperty.Name = "menuTaskProperty";
			this.menuTaskProperty.Size = new System.Drawing.Size(184, 22);
			this.menuTaskProperty.Text = "Property (&R)";
			this.menuTaskProperty.Click += new System.EventHandler(this.menuTaskProperty_Click);
			// 
			// toolbarView
			// 
			this.toolbarView.BackColor = System.Drawing.SystemColors.Control;
			this.toolbarView.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolbarView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLogForm,
            this.toolStripSeparator11,
            this.toolFileOpen,
            this.toolAudioInput,
            this.toolAudioOutput,
            this.toolStripSeparator7,
            this.toolViewClipboardObserver,
            this.toolStripSeparator2,
            this.toolViewCopy,
            this.toolViewPaste,
            this.toolStripSeparator4,
            this.toolViewROI,
            this.toolViewExif,
            this.toolStripSeparator17,
            this.toolViewFitImageSize,
            this.toolViewScale,
            this.toolViewScaleDown,
            this.toolViewScaleUp,
            this.toolFullscreen,
            this.toolPreview,
            this.toolViewHalftone,
            this.toolStripSeparator6,
            this.toolViewDepth,
            this.toolViewUnpack,
            this.toolViewChannelPrev,
            this.toolViewChannelNo,
            this.toolViewChannelNext,
            this.toolStripSeparator5,
            this.toolViewProfile,
            this.toolViewGrid,
            this.toolStripSeparator8,
            this.toolViewSnapshot});
			this.toolbarView.Location = new System.Drawing.Point(0, 0);
			this.toolbarView.Name = "toolbarView";
			this.toolbarView.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolbarView.Size = new System.Drawing.Size(1264, 35);
			this.toolbarView.TabIndex = 0;
			this.toolbarView.Text = "toolbarView";
			// 
			// toolLogForm
			// 
			this.toolLogForm.AutoSize = false;
			this.toolLogForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolLogForm.Image = ((System.Drawing.Image)(resources.GetObject("toolLogForm.Image")));
			this.toolLogForm.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolLogForm.Name = "toolLogForm";
			this.toolLogForm.Size = new System.Drawing.Size(32, 32);
			this.toolLogForm.Text = "toolStripButton1";
			this.toolLogForm.ToolTipText = "Log";
			this.toolLogForm.Click += new System.EventHandler(this.toolLogForm_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(6, 35);
			// 
			// toolFileOpen
			// 
			this.toolFileOpen.AutoSize = false;
			this.toolFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolFileOpen.Image")));
			this.toolFileOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFileOpen.Name = "toolFileOpen";
			this.toolFileOpen.Size = new System.Drawing.Size(32, 32);
			this.toolFileOpen.Text = "Open";
			this.toolFileOpen.Click += new System.EventHandler(this.toolFileOpen_Click);
			// 
			// toolAudioInput
			// 
			this.toolAudioInput.AutoSize = false;
			this.toolAudioInput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAudioInput.Image = ((System.Drawing.Image)(resources.GetObject("toolAudioInput.Image")));
			this.toolAudioInput.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolAudioInput.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAudioInput.Name = "toolAudioInput";
			this.toolAudioInput.Size = new System.Drawing.Size(32, 32);
			this.toolAudioInput.Text = "Audio Input";
			this.toolAudioInput.Click += new System.EventHandler(this.toolAudioInput_Click);
			// 
			// toolAudioOutput
			// 
			this.toolAudioOutput.AutoSize = false;
			this.toolAudioOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolAudioOutput.Image = ((System.Drawing.Image)(resources.GetObject("toolAudioOutput.Image")));
			this.toolAudioOutput.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolAudioOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolAudioOutput.Name = "toolAudioOutput";
			this.toolAudioOutput.Size = new System.Drawing.Size(32, 32);
			this.toolAudioOutput.Text = "Audio Output";
			this.toolAudioOutput.Click += new System.EventHandler(this.toolAudioOutput_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewClipboardObserver
			// 
			this.toolViewClipboardObserver.AutoSize = false;
			this.toolViewClipboardObserver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewClipboardObserver.Image = ((System.Drawing.Image)(resources.GetObject("toolViewClipboardObserver.Image")));
			this.toolViewClipboardObserver.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewClipboardObserver.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewClipboardObserver.Name = "toolViewClipboardObserver";
			this.toolViewClipboardObserver.Size = new System.Drawing.Size(32, 32);
			this.toolViewClipboardObserver.Text = "Clipboard Observer";
			this.toolViewClipboardObserver.Click += new System.EventHandler(this.toolViewClipboardObserver_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewCopy
			// 
			this.toolViewCopy.AutoSize = false;
			this.toolViewCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolViewCopy.Image")));
			this.toolViewCopy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewCopy.Name = "toolViewCopy";
			this.toolViewCopy.Size = new System.Drawing.Size(32, 32);
			this.toolViewCopy.Text = "Copy";
			this.toolViewCopy.Click += new System.EventHandler(this.toolViewCopy_Click);
			// 
			// toolViewPaste
			// 
			this.toolViewPaste.AutoSize = false;
			this.toolViewPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolViewPaste.Image")));
			this.toolViewPaste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewPaste.Name = "toolViewPaste";
			this.toolViewPaste.Size = new System.Drawing.Size(32, 32);
			this.toolViewPaste.Text = "Paste";
			this.toolViewPaste.Click += new System.EventHandler(this.toolViewPaste_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewROI
			// 
			this.toolViewROI.AutoSize = false;
			this.toolViewROI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewROI.Image = ((System.Drawing.Image)(resources.GetObject("toolViewROI.Image")));
			this.toolViewROI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewROI.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewROI.Name = "toolViewROI";
			this.toolViewROI.Size = new System.Drawing.Size(32, 32);
			this.toolViewROI.Text = "ROI";
			this.toolViewROI.Click += new System.EventHandler(this.toolViewROI_Click);
			// 
			// toolViewExif
			// 
			this.toolViewExif.AutoSize = false;
			this.toolViewExif.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewExif.Image = ((System.Drawing.Image)(resources.GetObject("toolViewExif.Image")));
			this.toolViewExif.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewExif.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewExif.Name = "toolViewExif";
			this.toolViewExif.Size = new System.Drawing.Size(32, 32);
			this.toolViewExif.Text = "Exif";
			this.toolViewExif.Click += new System.EventHandler(this.toolViewExif_Click);
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewFitImageSize
			// 
			this.toolViewFitImageSize.AutoSize = false;
			this.toolViewFitImageSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewFitImageSize.DropDownButtonWidth = 16;
			this.toolViewFitImageSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolViewFitImageWidth,
            this.toolViewFitImageHeight});
			this.toolViewFitImageSize.Image = ((System.Drawing.Image)(resources.GetObject("toolViewFitImageSize.Image")));
			this.toolViewFitImageSize.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewFitImageSize.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewFitImageSize.Name = "toolViewFitImageSize";
			this.toolViewFitImageSize.Size = new System.Drawing.Size(48, 32);
			this.toolViewFitImageSize.Text = "Fit Image Size";
			this.toolViewFitImageSize.ButtonClick += new System.EventHandler(this.toolViewFitImageSize_ButtonClick);
			// 
			// toolViewFitImageWidth
			// 
			this.toolViewFitImageWidth.Image = ((System.Drawing.Image)(resources.GetObject("toolViewFitImageWidth.Image")));
			this.toolViewFitImageWidth.Name = "toolViewFitImageWidth";
			this.toolViewFitImageWidth.Size = new System.Drawing.Size(175, 22);
			this.toolViewFitImageWidth.Text = "Fit Image Width";
			this.toolViewFitImageWidth.Click += new System.EventHandler(this.toolViewFitImageWidth_Click);
			// 
			// toolViewFitImageHeight
			// 
			this.toolViewFitImageHeight.Image = ((System.Drawing.Image)(resources.GetObject("toolViewFitImageHeight.Image")));
			this.toolViewFitImageHeight.Name = "toolViewFitImageHeight";
			this.toolViewFitImageHeight.Size = new System.Drawing.Size(175, 22);
			this.toolViewFitImageHeight.Text = "Fit Image Height";
			this.toolViewFitImageHeight.Click += new System.EventHandler(this.toolViewFitImageHeight_Click);
			// 
			// toolViewScale
			// 
			this.toolViewScale.AutoSize = false;
			this.toolViewScale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewScale.DropDownButtonWidth = 16;
			this.toolViewScale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolViewScale0010,
            this.toolViewScale0025,
            this.toolViewScale0050,
            this.toolViewScale0075,
            this.toolViewScale0100,
            this.toolViewScale0200,
            this.toolViewScale0400,
            this.toolViewScale0800,
            this.toolViewScale1600,
            this.toolViewScale3200});
			this.toolViewScale.Image = ((System.Drawing.Image)(resources.GetObject("toolViewScale.Image")));
			this.toolViewScale.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewScale.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewScale.Name = "toolViewScale";
			this.toolViewScale.Size = new System.Drawing.Size(48, 32);
			this.toolViewScale.Tag = "100";
			this.toolViewScale.Text = "Adjust Scale";
			this.toolViewScale.ButtonClick += new System.EventHandler(this.toolViewScale_ButtonClick);
			this.toolViewScale.DropDownOpened += new System.EventHandler(this.toolViewScale_DropDownOpened);
			// 
			// toolViewScale0010
			// 
			this.toolViewScale0010.Name = "toolViewScale0010";
			this.toolViewScale0010.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0010.Tag = "10";
			this.toolViewScale0010.Text = "10%";
			this.toolViewScale0010.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0025
			// 
			this.toolViewScale0025.Name = "toolViewScale0025";
			this.toolViewScale0025.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0025.Tag = "25";
			this.toolViewScale0025.Text = "25%";
			this.toolViewScale0025.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0050
			// 
			this.toolViewScale0050.Name = "toolViewScale0050";
			this.toolViewScale0050.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0050.Tag = "50";
			this.toolViewScale0050.Text = "50%";
			this.toolViewScale0050.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0075
			// 
			this.toolViewScale0075.Name = "toolViewScale0075";
			this.toolViewScale0075.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0075.Tag = "75";
			this.toolViewScale0075.Text = "75%";
			this.toolViewScale0075.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0100
			// 
			this.toolViewScale0100.Name = "toolViewScale0100";
			this.toolViewScale0100.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0100.Tag = "100";
			this.toolViewScale0100.Text = "100%";
			this.toolViewScale0100.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0200
			// 
			this.toolViewScale0200.Name = "toolViewScale0200";
			this.toolViewScale0200.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0200.Tag = "200";
			this.toolViewScale0200.Text = "200%";
			this.toolViewScale0200.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0400
			// 
			this.toolViewScale0400.Name = "toolViewScale0400";
			this.toolViewScale0400.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0400.Tag = "400";
			this.toolViewScale0400.Text = "400%";
			this.toolViewScale0400.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale0800
			// 
			this.toolViewScale0800.Name = "toolViewScale0800";
			this.toolViewScale0800.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale0800.Tag = "800";
			this.toolViewScale0800.Text = "800%";
			this.toolViewScale0800.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale1600
			// 
			this.toolViewScale1600.Name = "toolViewScale1600";
			this.toolViewScale1600.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale1600.Tag = "1600";
			this.toolViewScale1600.Text = "1600%";
			this.toolViewScale1600.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScale3200
			// 
			this.toolViewScale3200.Name = "toolViewScale3200";
			this.toolViewScale3200.Size = new System.Drawing.Size(116, 22);
			this.toolViewScale3200.Tag = "3200";
			this.toolViewScale3200.Text = "3200%";
			this.toolViewScale3200.Click += new System.EventHandler(this.toolViewScale_ButtonClick);
			// 
			// toolViewScaleDown
			// 
			this.toolViewScaleDown.AutoSize = false;
			this.toolViewScaleDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewScaleDown.Image = ((System.Drawing.Image)(resources.GetObject("toolViewScaleDown.Image")));
			this.toolViewScaleDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewScaleDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewScaleDown.Name = "toolViewScaleDown";
			this.toolViewScaleDown.Size = new System.Drawing.Size(32, 32);
			this.toolViewScaleDown.Text = "Scale Down";
			this.toolViewScaleDown.Click += new System.EventHandler(this.toolViewScaleDown_Click);
			// 
			// toolViewScaleUp
			// 
			this.toolViewScaleUp.AutoSize = false;
			this.toolViewScaleUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewScaleUp.Image = ((System.Drawing.Image)(resources.GetObject("toolViewScaleUp.Image")));
			this.toolViewScaleUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewScaleUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewScaleUp.Name = "toolViewScaleUp";
			this.toolViewScaleUp.Size = new System.Drawing.Size(32, 32);
			this.toolViewScaleUp.Text = "Scale Up";
			this.toolViewScaleUp.Click += new System.EventHandler(this.toolViewScaleUp_Click);
			// 
			// toolFullscreen
			// 
			this.toolFullscreen.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolFullscreen.AutoSize = false;
			this.toolFullscreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolFullscreen.Image = ((System.Drawing.Image)(resources.GetObject("toolFullscreen.Image")));
			this.toolFullscreen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolFullscreen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolFullscreen.Name = "toolFullscreen";
			this.toolFullscreen.Size = new System.Drawing.Size(32, 32);
			this.toolFullscreen.Text = "Fullscreen";
			this.toolFullscreen.Click += new System.EventHandler(this.toolFullscreen_Click);
			// 
			// toolPreview
			// 
			this.toolPreview.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolPreview.AutoSize = false;
			this.toolPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolPreview.Image")));
			this.toolPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolPreview.Name = "toolPreview";
			this.toolPreview.Size = new System.Drawing.Size(32, 32);
			this.toolPreview.Text = "Preview";
			this.toolPreview.Click += new System.EventHandler(this.toolPreview_Click);
			// 
			// toolViewHalftone
			// 
			this.toolViewHalftone.AutoSize = false;
			this.toolViewHalftone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewHalftone.Image = ((System.Drawing.Image)(resources.GetObject("toolViewHalftone.Image")));
			this.toolViewHalftone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewHalftone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewHalftone.Name = "toolViewHalftone";
			this.toolViewHalftone.Size = new System.Drawing.Size(32, 32);
			this.toolViewHalftone.Text = "Halftone";
			this.toolViewHalftone.Click += new System.EventHandler(this.toolViewHalftone_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewDepth
			// 
			this.toolViewDepth.AutoSize = false;
			this.toolViewDepth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewDepth.DropDownButtonWidth = 16;
			this.toolViewDepth.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolViewDepthDefault});
			this.toolViewDepth.Image = ((System.Drawing.Image)(resources.GetObject("toolViewDepth.Image")));
			this.toolViewDepth.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewDepth.Name = "toolViewDepth";
			this.toolViewDepth.Size = new System.Drawing.Size(48, 32);
			this.toolViewDepth.Text = "Depth";
			this.toolViewDepth.ButtonClick += new System.EventHandler(this.toolViewDepth_ButtonClick);
			// 
			// toolViewDepthDefault
			// 
			this.toolViewDepthDefault.Name = "toolViewDepthDefault";
			this.toolViewDepthDefault.Size = new System.Drawing.Size(118, 22);
			this.toolViewDepthDefault.Text = "Default";
			this.toolViewDepthDefault.Click += new System.EventHandler(this.toolViewDepthDefault_Click);
			// 
			// toolViewUnpack
			// 
			this.toolViewUnpack.AutoSize = false;
			this.toolViewUnpack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewUnpack.Image = ((System.Drawing.Image)(resources.GetObject("toolViewUnpack.Image")));
			this.toolViewUnpack.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewUnpack.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewUnpack.Name = "toolViewUnpack";
			this.toolViewUnpack.Size = new System.Drawing.Size(32, 32);
			this.toolViewUnpack.Text = "Unpack";
			this.toolViewUnpack.Click += new System.EventHandler(this.toolViewUnpack_Click);
			// 
			// toolViewChannelPrev
			// 
			this.toolViewChannelPrev.AutoSize = false;
			this.toolViewChannelPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewChannelPrev.Image = ((System.Drawing.Image)(resources.GetObject("toolViewChannelPrev.Image")));
			this.toolViewChannelPrev.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewChannelPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewChannelPrev.Name = "toolViewChannelPrev";
			this.toolViewChannelPrev.Size = new System.Drawing.Size(32, 32);
			this.toolViewChannelPrev.Text = "Previous Channel";
			this.toolViewChannelPrev.Click += new System.EventHandler(this.toolViewChannelPrev_Click);
			// 
			// toolViewChannelNo
			// 
			this.toolViewChannelNo.AutoSize = false;
			this.toolViewChannelNo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewChannelNo.Name = "toolViewChannelNo";
			this.toolViewChannelNo.Size = new System.Drawing.Size(32, 32);
			this.toolViewChannelNo.Text = "00";
			// 
			// toolViewChannelNext
			// 
			this.toolViewChannelNext.AutoSize = false;
			this.toolViewChannelNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewChannelNext.Image = ((System.Drawing.Image)(resources.GetObject("toolViewChannelNext.Image")));
			this.toolViewChannelNext.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewChannelNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewChannelNext.Name = "toolViewChannelNext";
			this.toolViewChannelNext.Size = new System.Drawing.Size(32, 32);
			this.toolViewChannelNext.Text = "Next Channel";
			this.toolViewChannelNext.Click += new System.EventHandler(this.toolViewChannelNext_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewProfile
			// 
			this.toolViewProfile.AutoSize = false;
			this.toolViewProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewProfile.Image = ((System.Drawing.Image)(resources.GetObject("toolViewProfile.Image")));
			this.toolViewProfile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewProfile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewProfile.Name = "toolViewProfile";
			this.toolViewProfile.Size = new System.Drawing.Size(32, 32);
			this.toolViewProfile.Text = "Profile";
			this.toolViewProfile.Click += new System.EventHandler(this.toolViewProfile_Click);
			// 
			// toolViewGrid
			// 
			this.toolViewGrid.AutoSize = false;
			this.toolViewGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolViewGrid.Image")));
			this.toolViewGrid.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewGrid.Name = "toolViewGrid";
			this.toolViewGrid.Size = new System.Drawing.Size(32, 32);
			this.toolViewGrid.Text = "Grid";
			this.toolViewGrid.Click += new System.EventHandler(this.toolViewGrid_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 35);
			// 
			// toolViewSnapshot
			// 
			this.toolViewSnapshot.AutoSize = false;
			this.toolViewSnapshot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolViewSnapshot.DropDownButtonWidth = 16;
			this.toolViewSnapshot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolViewSnapshotOverlayMode0,
            this.toolViewSnapshotOverlayMode1,
            this.toolViewSnapshotOverlayMode2});
			this.toolViewSnapshot.Image = ((System.Drawing.Image)(resources.GetObject("toolViewSnapshot.Image")));
			this.toolViewSnapshot.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolViewSnapshot.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolViewSnapshot.Name = "toolViewSnapshot";
			this.toolViewSnapshot.Size = new System.Drawing.Size(48, 32);
			this.toolViewSnapshot.Text = "Snapshot";
			this.toolViewSnapshot.ButtonClick += new System.EventHandler(this.toolViewSnapshot_ButtonClick);
			this.toolViewSnapshot.DropDownOpened += new System.EventHandler(this.toolViewSnapshot_DropDownOpened);
			// 
			// toolViewSnapshotOverlayMode0
			// 
			this.toolViewSnapshotOverlayMode0.Name = "toolViewSnapshotOverlayMode0";
			this.toolViewSnapshotOverlayMode0.Size = new System.Drawing.Size(261, 22);
			this.toolViewSnapshotOverlayMode0.Tag = "0";
			this.toolViewSnapshotOverlayMode0.Text = "including Overlay";
			this.toolViewSnapshotOverlayMode0.Click += new System.EventHandler(this.toolViewSnapshotOverlayMode_Click);
			// 
			// toolViewSnapshotOverlayMode1
			// 
			this.toolViewSnapshotOverlayMode1.Name = "toolViewSnapshotOverlayMode1";
			this.toolViewSnapshotOverlayMode1.Size = new System.Drawing.Size(261, 22);
			this.toolViewSnapshotOverlayMode1.Tag = "1";
			this.toolViewSnapshotOverlayMode1.Text = "including Overlay (Visible Rect)";
			this.toolViewSnapshotOverlayMode1.Click += new System.EventHandler(this.toolViewSnapshotOverlayMode_Click);
			// 
			// toolViewSnapshotOverlayMode2
			// 
			this.toolViewSnapshotOverlayMode2.Name = "toolViewSnapshotOverlayMode2";
			this.toolViewSnapshotOverlayMode2.Size = new System.Drawing.Size(261, 22);
			this.toolViewSnapshotOverlayMode2.Tag = "2";
			this.toolViewSnapshotOverlayMode2.Text = "including Overlay (Display Rect)";
			this.toolViewSnapshotOverlayMode2.Click += new System.EventHandler(this.toolViewSnapshotOverlayMode_Click);
			// 
			// ImageViewMenu
			// 
			this.ImageViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFullscreen,
            this.menuPreview,
            this.toolStripSeparator15,
            this.menuViewCopy,
            this.menuViewPaste,
            this.toolStripSeparator12,
            this.menuViewROI,
            this.menuViewExif,
            this.toolStripSeparator23,
            this.menuViewFitImageSize,
            this.menuViewScale,
            this.menuViewScaleReset,
            this.menuViewScaleDown,
            this.menuViewScaleUp,
            this.menuViewHalftone,
            this.toolStripSeparator25,
            this.menuViewDepth,
            this.menuViewUnpack,
            this.menuViewChannelPrev,
            this.menuViewChannelNext,
            this.toolStripSeparator26,
            this.menuViewProfile,
            this.menuViewGrid});
			this.ImageViewMenu.Name = "ImageViewMenu";
			this.ImageViewMenu.Size = new System.Drawing.Size(176, 430);
			this.ImageViewMenu.Opened += new System.EventHandler(this.ImageViewMenu_Opened);
			// 
			// menuFullscreen
			// 
			this.menuFullscreen.Name = "menuFullscreen";
			this.menuFullscreen.Size = new System.Drawing.Size(175, 22);
			this.menuFullscreen.Text = "Fullscreen";
			this.menuFullscreen.Click += new System.EventHandler(this.menuFullscreen_Click);
			// 
			// menuPreview
			// 
			this.menuPreview.Name = "menuPreview";
			this.menuPreview.Size = new System.Drawing.Size(175, 22);
			this.menuPreview.Text = "Preview";
			this.menuPreview.Click += new System.EventHandler(this.menuPreview_Click);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(172, 6);
			// 
			// menuViewCopy
			// 
			this.menuViewCopy.Name = "menuViewCopy";
			this.menuViewCopy.Size = new System.Drawing.Size(175, 22);
			this.menuViewCopy.Text = "Copy";
			this.menuViewCopy.Click += new System.EventHandler(this.menuViewCopy_Click);
			// 
			// menuViewPaste
			// 
			this.menuViewPaste.Name = "menuViewPaste";
			this.menuViewPaste.Size = new System.Drawing.Size(175, 22);
			this.menuViewPaste.Text = "Paste";
			this.menuViewPaste.Click += new System.EventHandler(this.menuViewPaste_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(172, 6);
			// 
			// menuViewROI
			// 
			this.menuViewROI.Name = "menuViewROI";
			this.menuViewROI.Size = new System.Drawing.Size(175, 22);
			this.menuViewROI.Text = "ROI";
			this.menuViewROI.Click += new System.EventHandler(this.menuViewROI_Click);
			// 
			// menuViewExif
			// 
			this.menuViewExif.Name = "menuViewExif";
			this.menuViewExif.Size = new System.Drawing.Size(175, 22);
			this.menuViewExif.Text = "Exif";
			this.menuViewExif.Click += new System.EventHandler(this.menuViewExif_Click);
			// 
			// toolStripSeparator23
			// 
			this.toolStripSeparator23.Name = "toolStripSeparator23";
			this.toolStripSeparator23.Size = new System.Drawing.Size(172, 6);
			// 
			// menuViewFitImageSize
			// 
			this.menuViewFitImageSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewFitImageSizeBoth,
            this.menuViewFitImageSizeWidth,
            this.menuViewFitImageSizeHeight});
			this.menuViewFitImageSize.Name = "menuViewFitImageSize";
			this.menuViewFitImageSize.Size = new System.Drawing.Size(175, 22);
			this.menuViewFitImageSize.Text = "Fit Image Size";
			// 
			// menuViewFitImageSizeBoth
			// 
			this.menuViewFitImageSizeBoth.Name = "menuViewFitImageSizeBoth";
			this.menuViewFitImageSizeBoth.Size = new System.Drawing.Size(114, 22);
			this.menuViewFitImageSizeBoth.Text = "Both";
			this.menuViewFitImageSizeBoth.Click += new System.EventHandler(this.menuViewFitImageSizeBoth_Click);
			// 
			// menuViewFitImageSizeWidth
			// 
			this.menuViewFitImageSizeWidth.Name = "menuViewFitImageSizeWidth";
			this.menuViewFitImageSizeWidth.Size = new System.Drawing.Size(114, 22);
			this.menuViewFitImageSizeWidth.Text = "Width";
			this.menuViewFitImageSizeWidth.Click += new System.EventHandler(this.menuViewFitImageSizeWidth_Click);
			// 
			// menuViewFitImageSizeHeight
			// 
			this.menuViewFitImageSizeHeight.Name = "menuViewFitImageSizeHeight";
			this.menuViewFitImageSizeHeight.Size = new System.Drawing.Size(114, 22);
			this.menuViewFitImageSizeHeight.Text = "Height";
			this.menuViewFitImageSizeHeight.Click += new System.EventHandler(this.menuViewFitImageSizeHeight_Click);
			// 
			// menuViewScale
			// 
			this.menuViewScale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewScale0010,
            this.menuViewScale0025,
            this.menuViewScale0050,
            this.menuViewScale0075,
            this.menuViewScale0100,
            this.menuViewScale0200,
            this.menuViewScale0400,
            this.menuViewScale0800,
            this.menuViewScale1600,
            this.menuViewScale3200});
			this.menuViewScale.Name = "menuViewScale";
			this.menuViewScale.Size = new System.Drawing.Size(175, 22);
			this.menuViewScale.Text = "Scale";
			this.menuViewScale.DropDownOpened += new System.EventHandler(this.menuViewScale_DropDownOpened);
			// 
			// menuViewScale0010
			// 
			this.menuViewScale0010.Name = "menuViewScale0010";
			this.menuViewScale0010.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0010.Tag = "10";
			this.menuViewScale0010.Text = "10%";
			this.menuViewScale0010.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0025
			// 
			this.menuViewScale0025.Name = "menuViewScale0025";
			this.menuViewScale0025.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0025.Tag = "25";
			this.menuViewScale0025.Text = "25%";
			this.menuViewScale0025.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0050
			// 
			this.menuViewScale0050.Name = "menuViewScale0050";
			this.menuViewScale0050.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0050.Tag = "50";
			this.menuViewScale0050.Text = "50%";
			this.menuViewScale0050.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0075
			// 
			this.menuViewScale0075.Name = "menuViewScale0075";
			this.menuViewScale0075.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0075.Tag = "75";
			this.menuViewScale0075.Text = "75%";
			this.menuViewScale0075.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0100
			// 
			this.menuViewScale0100.Name = "menuViewScale0100";
			this.menuViewScale0100.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0100.Tag = "100";
			this.menuViewScale0100.Text = "100%";
			this.menuViewScale0100.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0200
			// 
			this.menuViewScale0200.Name = "menuViewScale0200";
			this.menuViewScale0200.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0200.Tag = "200";
			this.menuViewScale0200.Text = "200%";
			this.menuViewScale0200.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0400
			// 
			this.menuViewScale0400.Name = "menuViewScale0400";
			this.menuViewScale0400.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0400.Tag = "400";
			this.menuViewScale0400.Text = "400%";
			this.menuViewScale0400.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale0800
			// 
			this.menuViewScale0800.Name = "menuViewScale0800";
			this.menuViewScale0800.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale0800.Tag = "800";
			this.menuViewScale0800.Text = "800%";
			this.menuViewScale0800.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale1600
			// 
			this.menuViewScale1600.Name = "menuViewScale1600";
			this.menuViewScale1600.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale1600.Tag = "1600";
			this.menuViewScale1600.Text = "1600%";
			this.menuViewScale1600.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScale3200
			// 
			this.menuViewScale3200.Name = "menuViewScale3200";
			this.menuViewScale3200.Size = new System.Drawing.Size(116, 22);
			this.menuViewScale3200.Tag = "3200";
			this.menuViewScale3200.Text = "3200%";
			this.menuViewScale3200.Click += new System.EventHandler(this.menuViewScale_Click);
			// 
			// menuViewScaleReset
			// 
			this.menuViewScaleReset.Name = "menuViewScaleReset";
			this.menuViewScaleReset.Size = new System.Drawing.Size(175, 22);
			this.menuViewScaleReset.Text = "Scale Reset";
			this.menuViewScaleReset.Click += new System.EventHandler(this.menuViewScaleReset_Click);
			// 
			// menuViewScaleDown
			// 
			this.menuViewScaleDown.Name = "menuViewScaleDown";
			this.menuViewScaleDown.Size = new System.Drawing.Size(175, 22);
			this.menuViewScaleDown.Text = "Scale Down";
			this.menuViewScaleDown.Click += new System.EventHandler(this.menuViewScaleDown_Click);
			// 
			// menuViewScaleUp
			// 
			this.menuViewScaleUp.Name = "menuViewScaleUp";
			this.menuViewScaleUp.Size = new System.Drawing.Size(175, 22);
			this.menuViewScaleUp.Text = "Scale Up";
			this.menuViewScaleUp.Click += new System.EventHandler(this.menuViewScaleUp_Click);
			// 
			// menuViewHalftone
			// 
			this.menuViewHalftone.Name = "menuViewHalftone";
			this.menuViewHalftone.Size = new System.Drawing.Size(175, 22);
			this.menuViewHalftone.Text = "Halftone";
			this.menuViewHalftone.Click += new System.EventHandler(this.menuViewHalftone_Click);
			// 
			// toolStripSeparator25
			// 
			this.toolStripSeparator25.Name = "toolStripSeparator25";
			this.toolStripSeparator25.Size = new System.Drawing.Size(172, 6);
			// 
			// menuViewDepth
			// 
			this.menuViewDepth.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewDepthFit,
            this.menuViewDepthDefault});
			this.menuViewDepth.Name = "menuViewDepth";
			this.menuViewDepth.Size = new System.Drawing.Size(175, 22);
			this.menuViewDepth.Text = "Depth";
			// 
			// menuViewDepthFit
			// 
			this.menuViewDepthFit.Name = "menuViewDepthFit";
			this.menuViewDepthFit.Size = new System.Drawing.Size(118, 22);
			this.menuViewDepthFit.Text = "Fit";
			this.menuViewDepthFit.Click += new System.EventHandler(this.menuViewDepthFit_Click);
			// 
			// menuViewDepthDefault
			// 
			this.menuViewDepthDefault.Name = "menuViewDepthDefault";
			this.menuViewDepthDefault.Size = new System.Drawing.Size(118, 22);
			this.menuViewDepthDefault.Text = "Default";
			this.menuViewDepthDefault.Click += new System.EventHandler(this.menuViewDepthDefault_Click);
			// 
			// menuViewUnpack
			// 
			this.menuViewUnpack.Name = "menuViewUnpack";
			this.menuViewUnpack.Size = new System.Drawing.Size(175, 22);
			this.menuViewUnpack.Text = "Unpack";
			this.menuViewUnpack.Click += new System.EventHandler(this.menuViewUnpack_Click);
			// 
			// menuViewChannelPrev
			// 
			this.menuViewChannelPrev.Name = "menuViewChannelPrev";
			this.menuViewChannelPrev.Size = new System.Drawing.Size(175, 22);
			this.menuViewChannelPrev.Text = "Previous Channel";
			this.menuViewChannelPrev.Click += new System.EventHandler(this.menuViewChannelPrev_Click);
			// 
			// menuViewChannelNext
			// 
			this.menuViewChannelNext.Name = "menuViewChannelNext";
			this.menuViewChannelNext.Size = new System.Drawing.Size(175, 22);
			this.menuViewChannelNext.Text = "Next Channel";
			this.menuViewChannelNext.Click += new System.EventHandler(this.menuViewChannelNext_Click);
			// 
			// toolStripSeparator26
			// 
			this.toolStripSeparator26.Name = "toolStripSeparator26";
			this.toolStripSeparator26.Size = new System.Drawing.Size(172, 6);
			// 
			// menuViewProfile
			// 
			this.menuViewProfile.Name = "menuViewProfile";
			this.menuViewProfile.Size = new System.Drawing.Size(175, 22);
			this.menuViewProfile.Text = "Profile";
			this.menuViewProfile.Click += new System.EventHandler(this.menuViewProfile_Click);
			// 
			// menuViewGrid
			// 
			this.menuViewGrid.Name = "menuViewGrid";
			this.menuViewGrid.Size = new System.Drawing.Size(175, 22);
			this.menuViewGrid.Text = "Grid";
			this.menuViewGrid.Click += new System.EventHandler(this.menuViewGrid_Click);
			// 
			// CxAuxInfoForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1264, 682);
			this.Controls.Add(this.splitVert);
			this.Controls.Add(this.toolbarView);
			this.Controls.Add(this.statusbar);
			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "CxAuxInfoForm";
			this.Text = "AuxInfo";
			this.Activated += new System.EventHandler(this.CxAuxInfoForm_Activated);
			this.Deactivate += new System.EventHandler(this.CxAuxInfoForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CxAuxInfoForm_FormClosing);
			this.Load += new System.EventHandler(this.CxAuxInfoForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CxAuxInfoForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CxAuxInfoForm_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CxAuxInfoForm_KeyDown);
			this.Move += new System.EventHandler(this.CxAuxInfoForm_Move);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CxAuxInfoForm_PreviewKeyDown);
			this.Resize += new System.EventHandler(this.CxAuxInfoForm_Resize);
			this.statusbar.ResumeLayout(false);
			this.statusbar.PerformLayout();
			this.splitVert.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitVert)).EndInit();
			this.splitVert.ResumeLayout(false);
			this.NetworkMenu.ResumeLayout(false);
			this.SerialPortMenu.ResumeLayout(false);
			this.DataMenu.ResumeLayout(false);
			this.MediaMenu.ResumeLayout(false);
			this.CameraMenu.ResumeLayout(false);
			this.TaskMenu.ResumeLayout(false);
			this.toolbarView.ResumeLayout(false);
			this.toolbarView.PerformLayout();
			this.ImageViewMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusbar;
		private System.Windows.Forms.SplitContainer splitVert;
		private System.Windows.Forms.Timer timerUpdateUI;
		private System.Windows.Forms.ToolStripStatusLabel statusMessage;
		private System.Windows.Forms.ToolStripStatusLabel statusSpacer;
		private XIE.Forms.CxToolStripEx toolbarView;
		private System.Windows.Forms.ToolStripSplitButton toolViewFitImageSize;
		private System.Windows.Forms.ToolStripMenuItem toolViewFitImageWidth;
		private System.Windows.Forms.ToolStripMenuItem toolViewFitImageHeight;
		private System.Windows.Forms.ToolStripSplitButton toolViewScale;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0010;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0025;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0050;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0075;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0100;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0200;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0400;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale0800;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale1600;
		private System.Windows.Forms.ToolStripMenuItem toolViewScale3200;
		private System.Windows.Forms.ToolStripButton toolViewScaleDown;
		private System.Windows.Forms.ToolStripButton toolViewScaleUp;
		private System.Windows.Forms.ContextMenuStrip NetworkMenu;
		private System.Windows.Forms.ToolStripMenuItem menuNetworkOpen;
		private System.Windows.Forms.ToolStripMenuItem menuNetworkClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator41;
		private System.Windows.Forms.ToolStripMenuItem menuNetworkProperty;
		private System.Windows.Forms.ContextMenuStrip SerialPortMenu;
		private System.Windows.Forms.ToolStripMenuItem menuSerialPortOpen;
		private System.Windows.Forms.ToolStripMenuItem menuSerialPortClose;
		private System.Windows.Forms.ContextMenuStrip DataMenu;
		private System.Windows.Forms.ToolStripMenuItem menuDataOpen;
		private System.Windows.Forms.ToolStripMenuItem menuDataClose;
		private System.Windows.Forms.ToolStripStatusLabel statusViewInfo;
		private System.Windows.Forms.ToolStripDropDownButton statusWorkingSet;
		private System.Windows.Forms.ToolStripMenuItem statusGCcollect;
		private CxAuxTreeView treeAux;
		private System.Windows.Forms.ToolStripButton toolFullscreen;
		private System.Windows.Forms.ToolStripButton toolPreview;
		private System.Windows.Forms.ToolStripButton toolViewHalftone;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem menuDataSave;
		private System.Windows.Forms.ToolStripButton toolViewCopy;
		private System.Windows.Forms.ToolStripButton toolViewPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripStatusLabel statusMouseInfo;
		private System.Windows.Forms.ToolStripButton toolViewROI;
		private System.Windows.Forms.ToolStripButton toolViewProfile;
		private System.Windows.Forms.ToolStripButton toolViewGrid;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton toolViewUnpack;
		private System.Windows.Forms.ToolStripButton toolViewChannelPrev;
		private System.Windows.Forms.ToolStripButton toolViewChannelNext;
		private System.Windows.Forms.ToolStripButton toolViewClipboardObserver;
		private System.Windows.Forms.ToolStripButton toolAudioInput;
		private System.Windows.Forms.ToolStripButton toolFileOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripLabel toolViewChannelNo;
		private System.Windows.Forms.ToolStripSplitButton toolViewSnapshot;
		private System.Windows.Forms.ToolStripMenuItem toolViewSnapshotOverlayMode0;
		private System.Windows.Forms.ToolStripMenuItem toolViewSnapshotOverlayMode1;
		private System.Windows.Forms.ToolStripMenuItem toolViewSnapshotOverlayMode2;
		private System.Windows.Forms.ContextMenuStrip MediaMenu;
		private System.Windows.Forms.ToolStripMenuItem menuMediaStart;
		private System.Windows.Forms.ToolStripMenuItem menuMediaPause;
		private System.Windows.Forms.ToolStripMenuItem menuMediaStop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem menuMediaClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripMenuItem menuMediaProperty;
		private System.Windows.Forms.ToolStripButton toolAudioOutput;
		private System.Windows.Forms.ToolStripMenuItem menuMediaSaveGRF;
		private System.Windows.Forms.ToolStripMenuItem menuCameraOpen;
		private System.Windows.Forms.ToolStripMenuItem menuCameraSaveGRF;
		private System.Windows.Forms.ToolStripMenuItem menuCameraClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuCameraRecord;
		private System.Windows.Forms.ToolStripMenuItem menuCameraStart;
		private System.Windows.Forms.ToolStripMenuItem menuCameraStop;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
		private System.Windows.Forms.ToolStripMenuItem menuCameraProperty;
		private System.Windows.Forms.ToolStripMenuItem menuCameraPropertyAudio;
		private System.Windows.Forms.ToolStripMenuItem menuCameraPropertyVideo;
		private System.Windows.Forms.ContextMenuStrip CameraMenu;
		private System.Windows.Forms.ContextMenuStrip TaskMenu;
		private System.Windows.Forms.ToolStripMenuItem menuTaskNew;
		private System.Windows.Forms.ToolStripMenuItem menuTaskOpen;
		private System.Windows.Forms.ToolStripMenuItem menuTaskSave;
		private System.Windows.Forms.ToolStripMenuItem menuTaskClose;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem menuTaskProperty;
		private System.Windows.Forms.ToolStripButton toolLogForm;
		private System.Windows.Forms.ToolStripMenuItem menuTaskThumbnail;
		private System.Windows.Forms.ToolStripSplitButton toolViewDepth;
		private System.Windows.Forms.ToolStripMenuItem toolViewDepthDefault;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem menuTaskDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem menuDataThumbnail;
		private System.Windows.Forms.ToolStripButton toolViewExif;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
		private System.Windows.Forms.ToolStripStatusLabel toolImagePrev;
		private System.Windows.Forms.ToolStripStatusLabel toolImageName;
		private System.Windows.Forms.ToolStripStatusLabel toolImageNext;
		private System.Windows.Forms.ContextMenuStrip ImageViewMenu;
		private System.Windows.Forms.ToolStripMenuItem menuViewROI;
		private System.Windows.Forms.ToolStripMenuItem menuViewExif;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
		private System.Windows.Forms.ToolStripMenuItem menuViewFitImageSize;
		private System.Windows.Forms.ToolStripMenuItem menuViewFitImageSizeBoth;
		private System.Windows.Forms.ToolStripMenuItem menuViewFitImageSizeWidth;
		private System.Windows.Forms.ToolStripMenuItem menuViewFitImageSizeHeight;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0010;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0025;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0050;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0075;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0100;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0200;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0400;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale0800;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale1600;
		private System.Windows.Forms.ToolStripMenuItem menuViewScale3200;
		private System.Windows.Forms.ToolStripMenuItem menuViewScaleDown;
		private System.Windows.Forms.ToolStripMenuItem menuViewScaleUp;
		private System.Windows.Forms.ToolStripMenuItem menuViewHalftone;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator25;
		private System.Windows.Forms.ToolStripMenuItem menuViewDepth;
		private System.Windows.Forms.ToolStripMenuItem menuViewDepthFit;
		private System.Windows.Forms.ToolStripMenuItem menuViewDepthDefault;
		private System.Windows.Forms.ToolStripMenuItem menuViewUnpack;
		private System.Windows.Forms.ToolStripMenuItem menuViewChannelPrev;
		private System.Windows.Forms.ToolStripMenuItem menuViewChannelNext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator26;
		private System.Windows.Forms.ToolStripMenuItem menuViewProfile;
		private System.Windows.Forms.ToolStripMenuItem menuViewGrid;
		private System.Windows.Forms.ToolStripMenuItem menuViewScaleReset;
		private System.Windows.Forms.ToolStripMenuItem menuViewCopy;
		private System.Windows.Forms.ToolStripMenuItem menuViewPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem menuFullscreen;
		private System.Windows.Forms.ToolStripMenuItem menuPreview;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.ToolStripMenuItem menuDataProperty;
	}
}

