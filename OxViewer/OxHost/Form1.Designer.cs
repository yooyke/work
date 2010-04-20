namespace OxHost
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            Exit();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.chatTab = new System.Windows.Forms.TabPage();
            this.chatTypeLabel = new System.Windows.Forms.Label();
            this.chatMessageLabel = new System.Windows.Forms.Label();
            this.chatButton = new System.Windows.Forms.Button();
            this.chatMessageTB = new System.Windows.Forms.TextBox();
            this.chatTypeCB = new System.Windows.Forms.ComboBox();
            this.chatLB = new System.Windows.Forms.ListBox();
            this.imTab = new System.Windows.Forms.TabPage();
            this.imMessageLabel = new System.Windows.Forms.Label();
            this.imIDLabel = new System.Windows.Forms.Label();
            this.imIDTB = new System.Windows.Forms.TextBox();
            this.imButton = new System.Windows.Forms.Button();
            this.imMessageTB = new System.Windows.Forms.TextBox();
            this.imLB = new System.Windows.Forms.ListBox();
            this.telTab = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.telRegionLabel = new System.Windows.Forms.TextBox();
            this.telZLabel = new System.Windows.Forms.TextBox();
            this.telYLabel = new System.Windows.Forms.TextBox();
            this.telXLabel = new System.Windows.Forms.TextBox();
            this.Teleport = new System.Windows.Forms.Button();
            this.moveTab = new System.Windows.Forms.TabPage();
            this.gestureLabel = new System.Windows.Forms.Label();
            this.gestureButton = new System.Windows.Forms.Button();
            this.gestureCB = new System.Windows.Forms.ComboBox();
            this.standupButton = new System.Windows.Forms.Button();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.loginWaitLabel = new System.Windows.Forms.TextBox();
            this.uriTB = new System.Windows.Forms.TextBox();
            this.loginWait = new System.Windows.Forms.TrackBar();
            this.locationTB = new System.Windows.Forms.TextBox();
            this.passTB = new System.Windows.Forms.TextBox();
            this.lastTB = new System.Windows.Forms.TextBox();
            this.firstTB = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.objectPanel = new System.Windows.Forms.Panel();
            this.objectScaleTB = new System.Windows.Forms.TextBox();
            this.objectPositionTB = new System.Windows.Forms.TextBox();
            this.objectRotationTB = new System.Windows.Forms.TextBox();
            this.objectClickTB = new System.Windows.Forms.TextBox();
            this.objectScaleLabel = new System.Windows.Forms.Label();
            this.objectTypeTB = new System.Windows.Forms.TextBox();
            this.objectPositionLabel = new System.Windows.Forms.Label();
            this.objectRotationLabel = new System.Windows.Forms.Label();
            this.objectIDTB = new System.Windows.Forms.TextBox();
            this.objectClickLabel = new System.Windows.Forms.Label();
            this.objectTypeLabel = new System.Windows.Forms.Label();
            this.objectIDLabel = new System.Windows.Forms.Label();
            this.objectNameTB = new System.Windows.Forms.TextBox();
            this.objectNameLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.chatTab.SuspendLayout();
            this.imTab.SuspendLayout();
            this.telTab.SuspendLayout();
            this.moveTab.SuspendLayout();
            this.loginPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loginWait)).BeginInit();
            this.objectPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Location = new System.Drawing.Point(6, 7);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(640, 480);
            this.panel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.chatTab);
            this.tabControl1.Controls.Add(this.imTab);
            this.tabControl1.Controls.Add(this.telTab);
            this.tabControl1.Controls.Add(this.moveTab);
            this.tabControl1.Location = new System.Drawing.Point(652, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(360, 480);
            this.tabControl1.TabIndex = 3;
            // 
            // chatTab
            // 
            this.chatTab.Controls.Add(this.chatTypeLabel);
            this.chatTab.Controls.Add(this.chatMessageLabel);
            this.chatTab.Controls.Add(this.chatButton);
            this.chatTab.Controls.Add(this.chatMessageTB);
            this.chatTab.Controls.Add(this.chatTypeCB);
            this.chatTab.Controls.Add(this.chatLB);
            this.chatTab.Location = new System.Drawing.Point(4, 24);
            this.chatTab.Name = "chatTab";
            this.chatTab.Padding = new System.Windows.Forms.Padding(3);
            this.chatTab.Size = new System.Drawing.Size(352, 452);
            this.chatTab.TabIndex = 0;
            this.chatTab.Text = "Chat";
            this.chatTab.UseVisualStyleBackColor = true;
            // 
            // chatTypeLabel
            // 
            this.chatTypeLabel.AutoSize = true;
            this.chatTypeLabel.Location = new System.Drawing.Point(111, 395);
            this.chatTypeLabel.Name = "chatTypeLabel";
            this.chatTypeLabel.Size = new System.Drawing.Size(36, 12);
            this.chatTypeLabel.TabIndex = 10;
            this.chatTypeLabel.Text = "Type :";
            // 
            // chatMessageLabel
            // 
            this.chatMessageLabel.AutoSize = true;
            this.chatMessageLabel.Location = new System.Drawing.Point(6, 421);
            this.chatMessageLabel.Name = "chatMessageLabel";
            this.chatMessageLabel.Size = new System.Drawing.Size(56, 12);
            this.chatMessageLabel.TabIndex = 9;
            this.chatMessageLabel.Text = "Message :";
            // 
            // chatButton
            // 
            this.chatButton.Location = new System.Drawing.Point(302, 392);
            this.chatButton.Name = "chatButton";
            this.chatButton.Size = new System.Drawing.Size(43, 45);
            this.chatButton.TabIndex = 3;
            this.chatButton.Text = "Send";
            this.chatButton.UseVisualStyleBackColor = true;
            this.chatButton.Click += new System.EventHandler(this.chatButton_Click);
            // 
            // chatMessageTB
            // 
            this.chatMessageTB.Location = new System.Drawing.Point(68, 418);
            this.chatMessageTB.Name = "chatMessageTB";
            this.chatMessageTB.Size = new System.Drawing.Size(228, 19);
            this.chatMessageTB.TabIndex = 2;
            this.chatMessageTB.TextChanged += new System.EventHandler(this.chatB_TextChanged);
            // 
            // chatTypeCB
            // 
            this.chatTypeCB.FormattingEnabled = true;
            this.chatTypeCB.Items.AddRange(new object[] {
            "Whisper",
            "Normal",
            "Shout"});
            this.chatTypeCB.Location = new System.Drawing.Point(153, 392);
            this.chatTypeCB.Name = "chatTypeCB";
            this.chatTypeCB.Size = new System.Drawing.Size(143, 20);
            this.chatTypeCB.TabIndex = 1;
            // 
            // chatLB
            // 
            this.chatLB.FormattingEnabled = true;
            this.chatLB.ItemHeight = 12;
            this.chatLB.Location = new System.Drawing.Point(5, 6);
            this.chatLB.Name = "chatLB";
            this.chatLB.Size = new System.Drawing.Size(340, 376);
            this.chatLB.TabIndex = 0;
            // 
            // imTab
            // 
            this.imTab.Controls.Add(this.imMessageLabel);
            this.imTab.Controls.Add(this.imIDLabel);
            this.imTab.Controls.Add(this.imIDTB);
            this.imTab.Controls.Add(this.imButton);
            this.imTab.Controls.Add(this.imMessageTB);
            this.imTab.Controls.Add(this.imLB);
            this.imTab.Location = new System.Drawing.Point(4, 24);
            this.imTab.Name = "imTab";
            this.imTab.Padding = new System.Windows.Forms.Padding(3);
            this.imTab.Size = new System.Drawing.Size(352, 452);
            this.imTab.TabIndex = 3;
            this.imTab.Text = "IM";
            this.imTab.UseVisualStyleBackColor = true;
            // 
            // imMessageLabel
            // 
            this.imMessageLabel.AutoSize = true;
            this.imMessageLabel.Location = new System.Drawing.Point(6, 421);
            this.imMessageLabel.Name = "imMessageLabel";
            this.imMessageLabel.Size = new System.Drawing.Size(56, 12);
            this.imMessageLabel.TabIndex = 102;
            this.imMessageLabel.Text = "Message :";
            // 
            // imIDLabel
            // 
            this.imIDLabel.AutoSize = true;
            this.imIDLabel.Location = new System.Drawing.Point(40, 395);
            this.imIDLabel.Name = "imIDLabel";
            this.imIDLabel.Size = new System.Drawing.Size(22, 12);
            this.imIDLabel.TabIndex = 101;
            this.imIDLabel.Text = "ID :";
            // 
            // imIDTB
            // 
            this.imIDTB.Location = new System.Drawing.Point(68, 392);
            this.imIDTB.Name = "imIDTB";
            this.imIDTB.Size = new System.Drawing.Size(228, 19);
            this.imIDTB.TabIndex = 1;
            // 
            // imButton
            // 
            this.imButton.Location = new System.Drawing.Point(302, 392);
            this.imButton.Name = "imButton";
            this.imButton.Size = new System.Drawing.Size(43, 45);
            this.imButton.TabIndex = 3;
            this.imButton.Text = "Send";
            this.imButton.UseVisualStyleBackColor = true;
            this.imButton.Click += new System.EventHandler(this.imButton_Click);
            // 
            // imMessageTB
            // 
            this.imMessageTB.Location = new System.Drawing.Point(68, 418);
            this.imMessageTB.Name = "imMessageTB";
            this.imMessageTB.Size = new System.Drawing.Size(228, 19);
            this.imMessageTB.TabIndex = 2;
            // 
            // imLB
            // 
            this.imLB.FormattingEnabled = true;
            this.imLB.ItemHeight = 12;
            this.imLB.Location = new System.Drawing.Point(5, 6);
            this.imLB.Name = "imLB";
            this.imLB.Size = new System.Drawing.Size(340, 376);
            this.imLB.TabIndex = 0;
            // 
            // telTab
            // 
            this.telTab.Controls.Add(this.listBox1);
            this.telTab.Controls.Add(this.label4);
            this.telTab.Controls.Add(this.label3);
            this.telTab.Controls.Add(this.label2);
            this.telTab.Controls.Add(this.label1);
            this.telTab.Controls.Add(this.telRegionLabel);
            this.telTab.Controls.Add(this.telZLabel);
            this.telTab.Controls.Add(this.telYLabel);
            this.telTab.Controls.Add(this.telXLabel);
            this.telTab.Controls.Add(this.Teleport);
            this.telTab.Location = new System.Drawing.Point(4, 24);
            this.telTab.Name = "telTab";
            this.telTab.Padding = new System.Windows.Forms.Padding(3);
            this.telTab.Size = new System.Drawing.Size(352, 452);
            this.telTab.TabIndex = 1;
            this.telTab.Text = "Teleport";
            this.telTab.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(5, 6);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(343, 376);
            this.listBox1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 394);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sim";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(178, 394);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 394);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Z";
            // 
            // telRegionLabel
            // 
            this.telRegionLabel.Location = new System.Drawing.Point(30, 391);
            this.telRegionLabel.Name = "telRegionLabel";
            this.telRegionLabel.Size = new System.Drawing.Size(142, 19);
            this.telRegionLabel.TabIndex = 1;
            // 
            // telZLabel
            // 
            this.telZLabel.Location = new System.Drawing.Point(308, 391);
            this.telZLabel.Name = "telZLabel";
            this.telZLabel.Size = new System.Drawing.Size(32, 19);
            this.telZLabel.TabIndex = 4;
            // 
            // telYLabel
            // 
            this.telYLabel.Location = new System.Drawing.Point(252, 391);
            this.telYLabel.Name = "telYLabel";
            this.telYLabel.Size = new System.Drawing.Size(32, 19);
            this.telYLabel.TabIndex = 3;
            // 
            // telXLabel
            // 
            this.telXLabel.Location = new System.Drawing.Point(196, 391);
            this.telXLabel.Name = "telXLabel";
            this.telXLabel.Size = new System.Drawing.Size(32, 19);
            this.telXLabel.TabIndex = 2;
            // 
            // Teleport
            // 
            this.Teleport.Location = new System.Drawing.Point(122, 421);
            this.Teleport.Name = "Teleport";
            this.Teleport.Size = new System.Drawing.Size(96, 28);
            this.Teleport.TabIndex = 5;
            this.Teleport.Text = "Teleport";
            this.Teleport.UseVisualStyleBackColor = true;
            this.Teleport.Click += new System.EventHandler(this.Teleport_Click);
            // 
            // moveTab
            // 
            this.moveTab.Controls.Add(this.gestureLabel);
            this.moveTab.Controls.Add(this.gestureButton);
            this.moveTab.Controls.Add(this.gestureCB);
            this.moveTab.Controls.Add(this.standupButton);
            this.moveTab.Location = new System.Drawing.Point(4, 24);
            this.moveTab.Name = "moveTab";
            this.moveTab.Padding = new System.Windows.Forms.Padding(3);
            this.moveTab.Size = new System.Drawing.Size(352, 452);
            this.moveTab.TabIndex = 2;
            this.moveTab.Text = "Movement";
            this.moveTab.UseVisualStyleBackColor = true;
            // 
            // gestureLabel
            // 
            this.gestureLabel.AutoSize = true;
            this.gestureLabel.Location = new System.Drawing.Point(6, 72);
            this.gestureLabel.Name = "gestureLabel";
            this.gestureLabel.Size = new System.Drawing.Size(45, 12);
            this.gestureLabel.TabIndex = 1207;
            this.gestureLabel.Text = "Gesture";
            // 
            // gestureButton
            // 
            this.gestureButton.Location = new System.Drawing.Point(159, 82);
            this.gestureButton.Name = "gestureButton";
            this.gestureButton.Size = new System.Drawing.Size(51, 28);
            this.gestureButton.TabIndex = 3;
            this.gestureButton.Text = "Action";
            this.gestureButton.UseVisualStyleBackColor = true;
            this.gestureButton.Click += new System.EventHandler(this.gestureButton_Click);
            // 
            // gestureCB
            // 
            this.gestureCB.FormattingEnabled = true;
            this.gestureCB.Location = new System.Drawing.Point(6, 87);
            this.gestureCB.Name = "gestureCB";
            this.gestureCB.Size = new System.Drawing.Size(147, 20);
            this.gestureCB.TabIndex = 2;
            // 
            // standupButton
            // 
            this.standupButton.Location = new System.Drawing.Point(6, 6);
            this.standupButton.Name = "standupButton";
            this.standupButton.Size = new System.Drawing.Size(78, 28);
            this.standupButton.TabIndex = 1;
            this.standupButton.Text = "Standup";
            this.standupButton.UseVisualStyleBackColor = true;
            this.standupButton.Click += new System.EventHandler(this.standupButton_Click);
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.loginWaitLabel);
            this.loginPanel.Controls.Add(this.uriTB);
            this.loginPanel.Controls.Add(this.loginWait);
            this.loginPanel.Controls.Add(this.locationTB);
            this.loginPanel.Controls.Add(this.passTB);
            this.loginPanel.Controls.Add(this.lastTB);
            this.loginPanel.Controls.Add(this.firstTB);
            this.loginPanel.Location = new System.Drawing.Point(1, 493);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(546, 106);
            this.loginPanel.TabIndex = 1;
            // 
            // loginWaitLabel
            // 
            this.loginWaitLabel.Location = new System.Drawing.Point(499, 62);
            this.loginWaitLabel.Name = "loginWaitLabel";
            this.loginWaitLabel.ReadOnly = true;
            this.loginWaitLabel.Size = new System.Drawing.Size(36, 19);
            this.loginWaitLabel.TabIndex = 1100;
            this.loginWaitLabel.Text = "0";
            this.loginWaitLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // uriTB
            // 
            this.uriTB.Location = new System.Drawing.Point(5, 37);
            this.uriTB.Name = "uriTB";
            this.uriTB.Size = new System.Drawing.Size(530, 19);
            this.uriTB.TabIndex = 104;
            this.uriTB.Text = "http://127.0.0.1:10001";
            this.uriTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // loginWait
            // 
            this.loginWait.Location = new System.Drawing.Point(5, 58);
            this.loginWait.Maximum = 60;
            this.loginWait.Name = "loginWait";
            this.loginWait.Size = new System.Drawing.Size(488, 45);
            this.loginWait.TabIndex = 106;
            this.loginWait.Scroll += new System.EventHandler(this.loginWait_Scroll);
            // 
            // locationTB
            // 
            this.locationTB.Location = new System.Drawing.Point(407, 12);
            this.locationTB.Name = "locationTB";
            this.locationTB.Size = new System.Drawing.Size(128, 19);
            this.locationTB.TabIndex = 103;
            this.locationTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // passTB
            // 
            this.passTB.Location = new System.Drawing.Point(273, 12);
            this.passTB.Name = "passTB";
            this.passTB.PasswordChar = '*';
            this.passTB.Size = new System.Drawing.Size(128, 19);
            this.passTB.TabIndex = 102;
            this.passTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // lastTB
            // 
            this.lastTB.Location = new System.Drawing.Point(139, 12);
            this.lastTB.Name = "lastTB";
            this.lastTB.Size = new System.Drawing.Size(128, 19);
            this.lastTB.TabIndex = 101;
            this.lastTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // firstTB
            // 
            this.firstTB.Location = new System.Drawing.Point(5, 12);
            this.firstTB.Name = "firstTB";
            this.firstTB.Size = new System.Drawing.Size(128, 19);
            this.firstTB.TabIndex = 100;
            this.firstTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.passTB_KeyUp);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(553, 493);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(93, 106);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // objectPanel
            // 
            this.objectPanel.Controls.Add(this.objectNameLabel);
            this.objectPanel.Controls.Add(this.objectNameTB);
            this.objectPanel.Controls.Add(this.objectScaleTB);
            this.objectPanel.Controls.Add(this.objectPositionTB);
            this.objectPanel.Controls.Add(this.objectRotationTB);
            this.objectPanel.Controls.Add(this.objectClickTB);
            this.objectPanel.Controls.Add(this.objectScaleLabel);
            this.objectPanel.Controls.Add(this.objectTypeTB);
            this.objectPanel.Controls.Add(this.objectPositionLabel);
            this.objectPanel.Controls.Add(this.objectRotationLabel);
            this.objectPanel.Controls.Add(this.objectIDTB);
            this.objectPanel.Controls.Add(this.objectClickLabel);
            this.objectPanel.Controls.Add(this.objectTypeLabel);
            this.objectPanel.Controls.Add(this.objectIDLabel);
            this.objectPanel.Location = new System.Drawing.Point(653, 493);
            this.objectPanel.Name = "objectPanel";
            this.objectPanel.Size = new System.Drawing.Size(358, 105);
            this.objectPanel.TabIndex = 4;
            // 
            // objectScaleTB
            // 
            this.objectScaleTB.Location = new System.Drawing.Point(245, 78);
            this.objectScaleTB.Name = "objectScaleTB";
            this.objectScaleTB.ReadOnly = true;
            this.objectScaleTB.Size = new System.Drawing.Size(107, 19);
            this.objectScaleTB.TabIndex = 6;
            // 
            // objectPositionTB
            // 
            this.objectPositionTB.Location = new System.Drawing.Point(245, 58);
            this.objectPositionTB.Name = "objectPositionTB";
            this.objectPositionTB.ReadOnly = true;
            this.objectPositionTB.Size = new System.Drawing.Size(107, 19);
            this.objectPositionTB.TabIndex = 4;
            // 
            // objectRotationTB
            // 
            this.objectRotationTB.Location = new System.Drawing.Point(49, 78);
            this.objectRotationTB.Name = "objectRotationTB";
            this.objectRotationTB.ReadOnly = true;
            this.objectRotationTB.Size = new System.Drawing.Size(107, 19);
            this.objectRotationTB.TabIndex = 5;
            // 
            // objectClickTB
            // 
            this.objectClickTB.Location = new System.Drawing.Point(245, 37);
            this.objectClickTB.Name = "objectClickTB";
            this.objectClickTB.ReadOnly = true;
            this.objectClickTB.Size = new System.Drawing.Size(107, 19);
            this.objectClickTB.TabIndex = 2;
            // 
            // objectScaleLabel
            // 
            this.objectScaleLabel.AutoSize = true;
            this.objectScaleLabel.Location = new System.Drawing.Point(202, 81);
            this.objectScaleLabel.Name = "objectScaleLabel";
            this.objectScaleLabel.Size = new System.Drawing.Size(30, 12);
            this.objectScaleLabel.TabIndex = 106;
            this.objectScaleLabel.Text = "Sca :";
            // 
            // objectTypeTB
            // 
            this.objectTypeTB.Location = new System.Drawing.Point(49, 37);
            this.objectTypeTB.Name = "objectTypeTB";
            this.objectTypeTB.ReadOnly = true;
            this.objectTypeTB.Size = new System.Drawing.Size(107, 19);
            this.objectTypeTB.TabIndex = 1;
            // 
            // objectPositionLabel
            // 
            this.objectPositionLabel.AutoSize = true;
            this.objectPositionLabel.Location = new System.Drawing.Point(202, 61);
            this.objectPositionLabel.Name = "objectPositionLabel";
            this.objectPositionLabel.Size = new System.Drawing.Size(30, 12);
            this.objectPositionLabel.TabIndex = 104;
            this.objectPositionLabel.Text = "Pos :";
            // 
            // objectRotationLabel
            // 
            this.objectRotationLabel.AutoSize = true;
            this.objectRotationLabel.Location = new System.Drawing.Point(6, 81);
            this.objectRotationLabel.Name = "objectRotationLabel";
            this.objectRotationLabel.Size = new System.Drawing.Size(29, 12);
            this.objectRotationLabel.TabIndex = 105;
            this.objectRotationLabel.Text = "Rot :";
            // 
            // objectIDTB
            // 
            this.objectIDTB.Location = new System.Drawing.Point(34, 12);
            this.objectIDTB.Name = "objectIDTB";
            this.objectIDTB.ReadOnly = true;
            this.objectIDTB.Size = new System.Drawing.Size(318, 19);
            this.objectIDTB.TabIndex = 0;
            // 
            // objectClickLabel
            // 
            this.objectClickLabel.AutoSize = true;
            this.objectClickLabel.Location = new System.Drawing.Point(202, 40);
            this.objectClickLabel.Name = "objectClickLabel";
            this.objectClickLabel.Size = new System.Drawing.Size(37, 12);
            this.objectClickLabel.TabIndex = 102;
            this.objectClickLabel.Text = "Click :";
            // 
            // objectTypeLabel
            // 
            this.objectTypeLabel.AutoSize = true;
            this.objectTypeLabel.Location = new System.Drawing.Point(6, 40);
            this.objectTypeLabel.Name = "objectTypeLabel";
            this.objectTypeLabel.Size = new System.Drawing.Size(36, 12);
            this.objectTypeLabel.TabIndex = 101;
            this.objectTypeLabel.Text = "Type :";
            // 
            // objectIDLabel
            // 
            this.objectIDLabel.AutoSize = true;
            this.objectIDLabel.Location = new System.Drawing.Point(6, 15);
            this.objectIDLabel.Name = "objectIDLabel";
            this.objectIDLabel.Size = new System.Drawing.Size(22, 12);
            this.objectIDLabel.TabIndex = 100;
            this.objectIDLabel.Text = "ID :";
            // 
            // objectNameTB
            // 
            this.objectNameTB.Location = new System.Drawing.Point(49, 58);
            this.objectNameTB.Name = "objectNameTB";
            this.objectNameTB.ReadOnly = true;
            this.objectNameTB.Size = new System.Drawing.Size(107, 19);
            this.objectNameTB.TabIndex = 3;
            // 
            // objectNameLabel
            // 
            this.objectNameLabel.AutoSize = true;
            this.objectNameLabel.Location = new System.Drawing.Point(6, 61);
            this.objectNameLabel.Name = "objectNameLabel";
            this.objectNameLabel.Size = new System.Drawing.Size(40, 12);
            this.objectNameLabel.TabIndex = 103;
            this.objectNameLabel.Text = "Name :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 602);
            this.Controls.Add(this.objectPanel);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginPanel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.chatTab.ResumeLayout(false);
            this.chatTab.PerformLayout();
            this.imTab.ResumeLayout(false);
            this.imTab.PerformLayout();
            this.telTab.ResumeLayout(false);
            this.telTab.PerformLayout();
            this.moveTab.ResumeLayout(false);
            this.moveTab.PerformLayout();
            this.loginPanel.ResumeLayout(false);
            this.loginPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loginWait)).EndInit();
            this.objectPanel.ResumeLayout(false);
            this.objectPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage chatTab;
        private System.Windows.Forms.TabPage telTab;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.TextBox uriTB;
        private System.Windows.Forms.TextBox locationTB;
        private System.Windows.Forms.TextBox passTB;
        private System.Windows.Forms.TextBox lastTB;
        private System.Windows.Forms.TextBox firstTB;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.ListBox chatLB;
        private System.Windows.Forms.ComboBox chatTypeCB;
        private System.Windows.Forms.Button chatButton;
        private System.Windows.Forms.TextBox chatMessageTB;
        private System.Windows.Forms.TextBox telRegionLabel;
        private System.Windows.Forms.TextBox telZLabel;
        private System.Windows.Forms.TextBox telYLabel;
        private System.Windows.Forms.TextBox telXLabel;
        private System.Windows.Forms.Button Teleport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TrackBar loginWait;
        private System.Windows.Forms.TextBox loginWaitLabel;
        private System.Windows.Forms.TabPage moveTab;
        private System.Windows.Forms.Button standupButton;
        private System.Windows.Forms.Panel objectPanel;
        private System.Windows.Forms.TextBox objectIDTB;
        private System.Windows.Forms.Label objectIDLabel;
        private System.Windows.Forms.TextBox objectClickTB;
        private System.Windows.Forms.TextBox objectTypeTB;
        private System.Windows.Forms.Label objectClickLabel;
        private System.Windows.Forms.Label objectTypeLabel;
        private System.Windows.Forms.TextBox objectScaleTB;
        private System.Windows.Forms.TextBox objectPositionTB;
        private System.Windows.Forms.TextBox objectRotationTB;
        private System.Windows.Forms.Label objectScaleLabel;
        private System.Windows.Forms.Label objectPositionLabel;
        private System.Windows.Forms.Label objectRotationLabel;
        private System.Windows.Forms.Button gestureButton;
        private System.Windows.Forms.ComboBox gestureCB;
        private System.Windows.Forms.Label gestureLabel;
        private System.Windows.Forms.TabPage imTab;
        private System.Windows.Forms.ListBox imLB;
        private System.Windows.Forms.Label imMessageLabel;
        private System.Windows.Forms.Label imIDLabel;
        private System.Windows.Forms.TextBox imIDTB;
        private System.Windows.Forms.Button imButton;
        private System.Windows.Forms.TextBox imMessageTB;
        private System.Windows.Forms.Label chatMessageLabel;
        private System.Windows.Forms.Label chatTypeLabel;
        private System.Windows.Forms.Label objectNameLabel;
        private System.Windows.Forms.TextBox objectNameTB;
    }
}

