namespace Notepad.UI
{
    partial class NotepadFrame
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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scOuter = new System.Windows.Forms.SplitContainer();
            this.splitControlArea = new System.Windows.Forms.SplitContainer();
            this.bottomTabs = new System.Windows.Forms.TabControl();
            this.bottomTabArea = new System.Windows.Forms.TabControl();
            this.loggingTabPage = new System.Windows.Forms.TabPage();
            this.skinningManager1 = new SkinFramework.SkinningManager();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOuter)).BeginInit();
            this.scOuter.Panel1.SuspendLayout();
            this.scOuter.Panel2.SuspendLayout();
            this.scOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitControlArea)).BeginInit();
            this.splitControlArea.Panel1.SuspendLayout();
            this.splitControlArea.SuspendLayout();
            this.bottomTabArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(806, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem1.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // scOuter
            // 
            this.scOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scOuter.IsSplitterFixed = true;
            this.scOuter.Location = new System.Drawing.Point(0, 24);
            this.scOuter.Name = "scOuter";
            this.scOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scOuter.Panel1
            // 
            this.scOuter.Panel1.Controls.Add(this.splitControlArea);
            // 
            // scOuter.Panel2
            // 
            this.scOuter.Panel2.Controls.Add(this.bottomTabArea);
            this.scOuter.Size = new System.Drawing.Size(806, 431);
            this.scOuter.SplitterDistance = 277;
            this.scOuter.SplitterWidth = 1;
            this.scOuter.TabIndex = 1;
            // 
            // splitControlArea
            // 
            this.splitControlArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitControlArea.Location = new System.Drawing.Point(0, 0);
            this.splitControlArea.Name = "splitControlArea";
            // 
            // splitControlArea.Panel1
            // 
            this.splitControlArea.Panel1.Controls.Add(this.bottomTabs);
            this.splitControlArea.Size = new System.Drawing.Size(806, 277);
            this.splitControlArea.SplitterDistance = 268;
            this.splitControlArea.TabIndex = 4;
            // 
            // bottomTabs
            // 
            this.bottomTabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.bottomTabs.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.bottomTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomTabs.Location = new System.Drawing.Point(0, 0);
            this.bottomTabs.Multiline = true;
            this.bottomTabs.Name = "bottomTabs";
            this.bottomTabs.Padding = new System.Drawing.Point(1, 1);
            this.bottomTabs.SelectedIndex = 0;
            this.bottomTabs.Size = new System.Drawing.Size(268, 277);
            this.bottomTabs.TabIndex = 0;
            // 
            // bottomTabArea
            // 
            this.bottomTabArea.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.bottomTabArea.Controls.Add(this.loggingTabPage);
            this.bottomTabArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomTabArea.Location = new System.Drawing.Point(0, 0);
            this.bottomTabArea.Name = "bottomTabArea";
            this.bottomTabArea.SelectedIndex = 0;
            this.bottomTabArea.Size = new System.Drawing.Size(806, 153);
            this.bottomTabArea.TabIndex = 0;
            // 
            // loggingTabPage
            // 
            this.loggingTabPage.Location = new System.Drawing.Point(4, 4);
            this.loggingTabPage.Name = "loggingTabPage";
            this.loggingTabPage.Size = new System.Drawing.Size(798, 127);
            this.loggingTabPage.TabIndex = 0;
            this.loggingTabPage.Text = "Log";
            this.loggingTabPage.UseVisualStyleBackColor = true;
            // 
            // skinningManager1
            // 
            this.skinningManager1.DefaultSkin = SkinFramework.DefaultSkin.Office2007Luna;
            this.skinningManager1.ParentForm = this;
            // 
            // NotepadFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 455);
            this.Controls.Add(this.scOuter);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "NotepadFrame";
            this.Text = "IronText";
            this.Load += new System.EventHandler(this.NotepadFrame_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.scOuter.Panel1.ResumeLayout(false);
            this.scOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOuter)).EndInit();
            this.scOuter.ResumeLayout(false);
            this.splitControlArea.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitControlArea)).EndInit();
            this.splitControlArea.ResumeLayout(false);
            this.bottomTabArea.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        public System.Windows.Forms.SplitContainer splitControlArea;
        private System.Windows.Forms.TabControl bottomTabs;
        public System.Windows.Forms.SplitContainer scOuter;
        public System.Windows.Forms.TabControl bottomTabArea;
        private System.Windows.Forms.TabPage loggingTabPage;
        public SkinFramework.SkinningManager skinningManager1;
    }
}

