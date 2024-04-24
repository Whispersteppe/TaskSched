namespace TaskScheduler.WinForm
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            _notifyIcon = new NotifyIcon(components);
            _notifyIconMenu = new ContextMenuStrip(components);
            _notifyContextShow = new ToolStripMenuItem();
            _notifyContextCreate = new ToolStripMenuItem();
            _notifyContextStart = new ToolStripMenuItem();
            _notifyContextStop = new ToolStripMenuItem();
            _notifyContextExit = new ToolStripMenuItem();
            _statusStrip = new StatusStrip();
            _toolStrip = new ToolStrip();
            _menuStrip = new MenuStrip();
            tsMenuControl = new ToolStripMenuItem();
            tsStart = new ToolStripMenuItem();
            tsStop = new ToolStripMenuItem();
            tsExit = new ToolStripMenuItem();
            tsMenuManage = new ToolStripMenuItem();
            tsAddEvent = new ToolStripMenuItem();
            tsMenuAddFolder = new ToolStripMenuItem();
            tsMenuAddActivity = new ToolStripMenuItem();
            mainSplit = new SplitContainer();
            schedulerTreeView = new Controls.SchedulerTreeView();
            canvasSelector = new Controls.CanvasSelector();
            _notifyIconMenu.SuspendLayout();
            _menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainSplit).BeginInit();
            mainSplit.Panel1.SuspendLayout();
            mainSplit.Panel2.SuspendLayout();
            mainSplit.SuspendLayout();
            SuspendLayout();
            // 
            // _notifyIcon
            // 
            _notifyIcon.ContextMenuStrip = _notifyIconMenu;
            _notifyIcon.Icon = (Icon)resources.GetObject("_notifyIcon.Icon");
            _notifyIcon.Text = "Scheduler Stopped";
            _notifyIcon.Visible = true;
            _notifyIcon.Click += notifyIcon_Click;
            // 
            // _notifyIconMenu
            // 
            _notifyIconMenu.Items.AddRange(new ToolStripItem[] { _notifyContextShow, _notifyContextCreate, _notifyContextStart, _notifyContextStop, _notifyContextExit });
            _notifyIconMenu.Name = "_notifyIconMenu";
            _notifyIconMenu.Size = new Size(141, 114);
            // 
            // _notifyContextShow
            // 
            _notifyContextShow.Name = "_notifyContextShow";
            _notifyContextShow.Size = new Size(140, 22);
            _notifyContextShow.Text = "Show";
            _notifyContextShow.Click += notifyContextShow_Click;
            // 
            // _notifyContextCreate
            // 
            _notifyContextCreate.Name = "_notifyContextCreate";
            _notifyContextCreate.Size = new Size(140, 22);
            _notifyContextCreate.Text = "Create Event";
            _notifyContextCreate.Click += notifyContextCreate_Click;
            // 
            // _notifyContextStart
            // 
            _notifyContextStart.Name = "_notifyContextStart";
            _notifyContextStart.Size = new Size(140, 22);
            _notifyContextStart.Text = "Start";
            _notifyContextStart.Click += notifyContextStart_Click;
            // 
            // _notifyContextStop
            // 
            _notifyContextStop.Enabled = false;
            _notifyContextStop.Name = "_notifyContextStop";
            _notifyContextStop.Size = new Size(140, 22);
            _notifyContextStop.Text = "Stop";
            _notifyContextStop.Click += notifyContextStop_Click;
            // 
            // _notifyContextExit
            // 
            _notifyContextExit.Name = "_notifyContextExit";
            _notifyContextExit.Size = new Size(140, 22);
            _notifyContextExit.Text = "Exit";
            _notifyContextExit.Click += notifyContextExit_Click;
            // 
            // _statusStrip
            // 
            _statusStrip.Location = new Point(0, 428);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new Size(800, 22);
            _statusStrip.TabIndex = 1;
            _statusStrip.Text = "_statusStrip";
            // 
            // _toolStrip
            // 
            _toolStrip.Location = new Point(0, 24);
            _toolStrip.Name = "_toolStrip";
            _toolStrip.Size = new Size(800, 25);
            _toolStrip.TabIndex = 2;
            _toolStrip.Text = "toolStrip1";
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] { tsMenuControl, tsMenuManage });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(800, 24);
            _menuStrip.TabIndex = 3;
            _menuStrip.Text = "menuStrip1";
            // 
            // tsMenuControl
            // 
            tsMenuControl.DropDownItems.AddRange(new ToolStripItem[] { tsStart, tsStop, tsExit });
            tsMenuControl.Name = "tsMenuControl";
            tsMenuControl.Size = new Size(59, 20);
            tsMenuControl.Text = "Control";
            // 
            // tsStart
            // 
            tsStart.Name = "tsStart";
            tsStart.Size = new Size(98, 22);
            tsStart.Text = "Start";
            tsStart.Click += tsStart_Click;
            // 
            // tsStop
            // 
            tsStop.Name = "tsStop";
            tsStop.Size = new Size(98, 22);
            tsStop.Text = "Stop";
            tsStop.Click += tsStop_Click;
            // 
            // tsExit
            // 
            tsExit.Name = "tsExit";
            tsExit.Size = new Size(98, 22);
            tsExit.Text = "Exit";
            tsExit.Click += tsExit_Click;
            // 
            // tsMenuManage
            // 
            tsMenuManage.DropDownItems.AddRange(new ToolStripItem[] { tsAddEvent, tsMenuAddFolder, tsMenuAddActivity });
            tsMenuManage.Name = "tsMenuManage";
            tsMenuManage.Size = new Size(62, 20);
            tsMenuManage.Text = "Manage";
            // 
            // tsAddEvent
            // 
            tsAddEvent.Name = "tsAddEvent";
            tsAddEvent.Size = new Size(139, 22);
            tsAddEvent.Text = "Add Event";
            // 
            // tsMenuAddFolder
            // 
            tsMenuAddFolder.Name = "tsMenuAddFolder";
            tsMenuAddFolder.Size = new Size(139, 22);
            tsMenuAddFolder.Text = "Add Folder";
            // 
            // tsMenuAddActivity
            // 
            tsMenuAddActivity.Name = "tsMenuAddActivity";
            tsMenuAddActivity.Size = new Size(139, 22);
            tsMenuAddActivity.Text = "Add Activity";
            // 
            // mainSplit
            // 
            mainSplit.Dock = DockStyle.Fill;
            mainSplit.Location = new Point(0, 49);
            mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            mainSplit.Panel1.Controls.Add(schedulerTreeView);
            // 
            // mainSplit.Panel2
            // 
            mainSplit.Panel2.Controls.Add(canvasSelector);
            mainSplit.Size = new Size(800, 379);
            mainSplit.SplitterDistance = 174;
            mainSplit.TabIndex = 4;
            // 
            // schedulerTreeView
            // 
            schedulerTreeView.Dock = DockStyle.Fill;
            schedulerTreeView.Location = new Point(0, 0);
            schedulerTreeView.Name = "schedulerTreeView";
            schedulerTreeView.Size = new Size(174, 379);
            schedulerTreeView.TabIndex = 0;
            // 
            // canvasSelector
            // 
            canvasSelector.Dock = DockStyle.Fill;
            canvasSelector.Location = new Point(0, 0);
            canvasSelector.Name = "canvasSelector";
            canvasSelector.Size = new Size(622, 379);
            canvasSelector.TabIndex = 0;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(mainSplit);
            Controls.Add(_toolStrip);
            Controls.Add(_statusStrip);
            Controls.Add(_menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = _menuStrip;
            Name = "FrmMain";
            Text = "Task Scheduler";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            _notifyIconMenu.ResumeLayout(false);
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            mainSplit.Panel1.ResumeLayout(false);
            mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplit).EndInit();
            mainSplit.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _notifyIconMenu;
        private ToolStripMenuItem _notifyContextShow;
        private ToolStripMenuItem _notifyContextCreate;
        private ToolStripMenuItem _notifyContextStart;
        private ToolStripMenuItem _notifyContextStop;
        private ToolStripMenuItem _notifyContextExit;
        private StatusStrip _statusStrip;
        private ToolStrip _toolStrip;
        private MenuStrip _menuStrip;
        private SplitContainer mainSplit;
        private Controls.SchedulerTreeView schedulerTreeView;
        private Controls.CanvasSelector canvasSelector;
        private ToolStripMenuItem tsMenuControl;
        private ToolStripMenuItem tsStart;
        private ToolStripMenuItem tsStop;
        private ToolStripMenuItem tsExit;
        private ToolStripMenuItem tsMenuManage;
        private ToolStripMenuItem tsAddEvent;
        private ToolStripMenuItem tsMenuAddFolder;
        private ToolStripMenuItem tsMenuAddActivity;
    }
}
