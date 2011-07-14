namespace SamplePluggableApp
{
    partial class Console
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rtxtConsole = new System.Windows.Forms.RichTextBox();
            this.cmMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyConsoleText = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopyVersionInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWordWrap = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkMenu = new System.Windows.Forms.LinkLabel();
            this.txtAssemblyVersion = new System.Windows.Forms.TextBox();
            this.cmMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtConsole
            // 
            this.rtxtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtConsole.BackColor = System.Drawing.Color.Black;
            this.rtxtConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtConsole.ContextMenuStrip = this.cmMenu;
            this.rtxtConsole.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtConsole.ForeColor = System.Drawing.Color.LimeGreen;
            this.rtxtConsole.Location = new System.Drawing.Point(0, 22);
            this.rtxtConsole.Name = "rtxtConsole";
            this.rtxtConsole.ReadOnly = true;
            this.rtxtConsole.Size = new System.Drawing.Size(463, 95);
            this.rtxtConsole.TabIndex = 13;
            this.rtxtConsole.Text = "";
            this.rtxtConsole.WordWrap = false;
            // 
            // cmMenu
            // 
            this.cmMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopy,
            this.mnuClear,
            this.mnuSaveToFile,
            this.mnuWordWrap});
            this.cmMenu.Name = "cmMenu";
            this.cmMenu.Size = new System.Drawing.Size(153, 114);
            // 
            // mnuCopy
            // 
            this.mnuCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCopyConsoleText,
            this.mnuCopyVersionInfo});
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Size = new System.Drawing.Size(152, 22);
            this.mnuCopy.Text = "Copy";
            // 
            // mnuCopyConsoleText
            // 
            this.mnuCopyConsoleText.Name = "mnuCopyConsoleText";
            this.mnuCopyConsoleText.Size = new System.Drawing.Size(142, 22);
            this.mnuCopyConsoleText.Text = "Console Text";
            this.mnuCopyConsoleText.Click += new System.EventHandler(this.mnuCopyConsoleText_Click);
            // 
            // mnuCopyVersionInfo
            // 
            this.mnuCopyVersionInfo.Name = "mnuCopyVersionInfo";
            this.mnuCopyVersionInfo.Size = new System.Drawing.Size(142, 22);
            this.mnuCopyVersionInfo.Text = "Version Info";
            this.mnuCopyVersionInfo.Click += new System.EventHandler(this.mnuCopyVersionInfo_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(152, 22);
            this.mnuClear.Text = "Clear";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // mnuSaveToFile
            // 
            this.mnuSaveToFile.Name = "mnuSaveToFile";
            this.mnuSaveToFile.Size = new System.Drawing.Size(152, 22);
            this.mnuSaveToFile.Text = "Save To File";
            this.mnuSaveToFile.Click += new System.EventHandler(this.mnuSaveToFile_Click);
            // 
            // mnuWordWrap
            // 
            this.mnuWordWrap.CheckOnClick = true;
            this.mnuWordWrap.Name = "mnuWordWrap";
            this.mnuWordWrap.Size = new System.Drawing.Size(152, 22);
            this.mnuWordWrap.Text = "Word Wrap";
            this.mnuWordWrap.CheckedChanged += new System.EventHandler(this.mnuWordWrap_CheckedChanged);
            // 
            // lnkMenu
            // 
            this.lnkMenu.AutoSize = true;
            this.lnkMenu.BackColor = System.Drawing.SystemColors.Control;
            this.lnkMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkMenu.Location = new System.Drawing.Point(-1, 1);
            this.lnkMenu.Name = "lnkMenu";
            this.lnkMenu.Size = new System.Drawing.Size(39, 15);
            this.lnkMenu.TabIndex = 15;
            this.lnkMenu.TabStop = true;
            this.lnkMenu.Text = "Menu";
            this.lnkMenu.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkMenu.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMenu_LinkClicked);
            // 
            // txtAssemblyVersion
            // 
            this.txtAssemblyVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAssemblyVersion.BackColor = System.Drawing.SystemColors.Control;
            this.txtAssemblyVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAssemblyVersion.Location = new System.Drawing.Point(44, 3);
            this.txtAssemblyVersion.Name = "txtAssemblyVersion";
            this.txtAssemblyVersion.ReadOnly = true;
            this.txtAssemblyVersion.Size = new System.Drawing.Size(416, 13);
            this.txtAssemblyVersion.TabIndex = 16;
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtAssemblyVersion);
            this.Controls.Add(this.rtxtConsole);
            this.Controls.Add(this.lnkMenu);
            this.Name = "Console";
            this.Size = new System.Drawing.Size(463, 120);
            this.Load += new System.EventHandler(this.Console_Load);
            this.cmMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtConsole;
        private System.Windows.Forms.ContextMenuStrip cmMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuCopy;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
        private System.Windows.Forms.ToolStripMenuItem mnuWordWrap;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveToFile;
        private System.Windows.Forms.LinkLabel lnkMenu;
        private System.Windows.Forms.TextBox txtAssemblyVersion;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyConsoleText;
        private System.Windows.Forms.ToolStripMenuItem mnuCopyVersionInfo;
    }
}
