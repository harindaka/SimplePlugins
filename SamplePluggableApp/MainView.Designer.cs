namespace SamplePluggableApp
{
    partial class MainView
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
            this.lbPlugins = new System.Windows.Forms.ListBox();
            this.btnUnloadAll = new System.Windows.Forms.Button();
            this.btnShowLoadedAssemblies = new System.Windows.Forms.Button();
            this.cslConsole = new SamplePluggableApp.Console();
            this.btnShowLoadedPlugins = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbPlugins
            // 
            this.lbPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlugins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPlugins.FormattingEnabled = true;
            this.lbPlugins.HorizontalScrollbar = true;
            this.lbPlugins.Location = new System.Drawing.Point(8, 12);
            this.lbPlugins.Name = "lbPlugins";
            this.lbPlugins.Size = new System.Drawing.Size(535, 80);
            this.lbPlugins.TabIndex = 3;
            this.lbPlugins.DoubleClick += new System.EventHandler(this.lbPlugins_DoubleClick);
            // 
            // btnUnloadAll
            // 
            this.btnUnloadAll.Location = new System.Drawing.Point(8, 104);
            this.btnUnloadAll.Name = "btnUnloadAll";
            this.btnUnloadAll.Size = new System.Drawing.Size(87, 23);
            this.btnUnloadAll.TabIndex = 4;
            this.btnUnloadAll.Text = "Unload All";
            this.btnUnloadAll.UseVisualStyleBackColor = true;
            this.btnUnloadAll.Click += new System.EventHandler(this.btnUnloadAll_Click);
            // 
            // btnShowLoadedAssemblies
            // 
            this.btnShowLoadedAssemblies.Location = new System.Drawing.Point(101, 104);
            this.btnShowLoadedAssemblies.Name = "btnShowLoadedAssemblies";
            this.btnShowLoadedAssemblies.Size = new System.Drawing.Size(147, 23);
            this.btnShowLoadedAssemblies.TabIndex = 5;
            this.btnShowLoadedAssemblies.Text = "Show Loaded Assemblies";
            this.btnShowLoadedAssemblies.UseVisualStyleBackColor = true;
            this.btnShowLoadedAssemblies.Click += new System.EventHandler(this.btnShowLoadedAssemblies_Click);
            // 
            // cslConsole
            // 
            this.cslConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cslConsole.Location = new System.Drawing.Point(10, 143);
            this.cslConsole.Name = "cslConsole";
            this.cslConsole.Size = new System.Drawing.Size(531, 254);
            this.cslConsole.TabIndex = 7;
            // 
            // btnShowLoadedPlugins
            // 
            this.btnShowLoadedPlugins.Location = new System.Drawing.Point(254, 104);
            this.btnShowLoadedPlugins.Name = "btnShowLoadedPlugins";
            this.btnShowLoadedPlugins.Size = new System.Drawing.Size(147, 23);
            this.btnShowLoadedPlugins.TabIndex = 8;
            this.btnShowLoadedPlugins.Text = "Show Loaded Plugins";
            this.btnShowLoadedPlugins.UseVisualStyleBackColor = true;
            this.btnShowLoadedPlugins.Click += new System.EventHandler(this.btnShowLoadedPlugins_Click);
            // 
            // MainView
            // 
            this.ClientSize = new System.Drawing.Size(555, 409);
            this.Controls.Add(this.btnShowLoadedPlugins);
            this.Controls.Add(this.cslConsole);
            this.Controls.Add(this.btnShowLoadedAssemblies);
            this.Controls.Add(this.btnUnloadAll);
            this.Controls.Add(this.lbPlugins);
            this.Name = "MainView";
            this.Text = "Sample Pluggable App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainView_FormClosing);
            this.Load += new System.EventHandler(this.MainView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbPlugins;
        private System.Windows.Forms.Button btnUnloadAll;
        private System.Windows.Forms.Button btnShowLoadedAssemblies;
        private Console cslConsole;
        private System.Windows.Forms.Button btnShowLoadedPlugins;
    }
}
