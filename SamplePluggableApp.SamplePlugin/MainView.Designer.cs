namespace SamplePluggableApp.SamplePlugin
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
            this.button1 = new System.Windows.Forms.Button();
            this.lbParameterDisplay = new System.Windows.Forms.ListBox();
            this.btnException = new System.Windows.Forms.Button();
            this.btnCheckDependancy = new System.Windows.Forms.Button();
            this.btnReadFromConfigFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(120, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lbParameterDisplay
            // 
            this.lbParameterDisplay.FormattingEnabled = true;
            this.lbParameterDisplay.Location = new System.Drawing.Point(12, 12);
            this.lbParameterDisplay.Name = "lbParameterDisplay";
            this.lbParameterDisplay.Size = new System.Drawing.Size(219, 82);
            this.lbParameterDisplay.TabIndex = 3;
            // 
            // btnException
            // 
            this.btnException.Location = new System.Drawing.Point(237, 12);
            this.btnException.Name = "btnException";
            this.btnException.Size = new System.Drawing.Size(159, 23);
            this.btnException.TabIndex = 4;
            this.btnException.Text = "Raise Unhandled Exception";
            this.btnException.UseVisualStyleBackColor = true;
            this.btnException.Click += new System.EventHandler(this.btnException_Click);
            // 
            // btnCheckDependancy
            // 
            this.btnCheckDependancy.Location = new System.Drawing.Point(237, 41);
            this.btnCheckDependancy.Name = "btnCheckDependancy";
            this.btnCheckDependancy.Size = new System.Drawing.Size(159, 23);
            this.btnCheckDependancy.TabIndex = 5;
            this.btnCheckDependancy.Text = "Check Dependancy";
            this.btnCheckDependancy.UseVisualStyleBackColor = true;
            this.btnCheckDependancy.Click += new System.EventHandler(this.btnCheckDependancy_Click);
            // 
            // btnReadFromConfigFile
            // 
            this.btnReadFromConfigFile.Location = new System.Drawing.Point(237, 70);
            this.btnReadFromConfigFile.Name = "btnReadFromConfigFile";
            this.btnReadFromConfigFile.Size = new System.Drawing.Size(159, 23);
            this.btnReadFromConfigFile.TabIndex = 6;
            this.btnReadFromConfigFile.Text = "Read App.Config";
            this.btnReadFromConfigFile.UseVisualStyleBackColor = true;
            this.btnReadFromConfigFile.Click += new System.EventHandler(this.btnReadFromConfigFile_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 113);
            this.Controls.Add(this.btnReadFromConfigFile);
            this.Controls.Add(this.btnCheckDependancy);
            this.Controls.Add(this.btnException);
            this.Controls.Add(this.lbParameterDisplay);
            this.Name = "MainView";
            this.Load += new System.EventHandler(this.MainView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbParameterDisplay;
        private System.Windows.Forms.Button btnException;
        private System.Windows.Forms.Button btnCheckDependancy;
        private System.Windows.Forms.Button btnReadFromConfigFile;

    }
}