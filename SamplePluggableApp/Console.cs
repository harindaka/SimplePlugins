using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace SamplePluggableApp
{
    public partial class Console : UserControl
    {
        private enum MessageTypes { Informational, Warning, Exception }

        public Console()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get { return rtxtConsole.Text; }
            set { rtxtConsole.Text = value; }
        }

        private void Write(string message, MessageTypes messageType)
        {
            Color fontColor;
            if (messageType == MessageTypes.Informational)
                fontColor = Color.LimeGreen;
            else if (messageType == MessageTypes.Warning)
                fontColor = Color.CornflowerBlue;
            else
                fontColor = Color.Red;

            this.rtxtConsole.SelectionStart = this.rtxtConsole.TextLength;
            this.rtxtConsole.SelectionColor = fontColor;
            this.rtxtConsole.SelectedText = DateTime.Now.ToString("hh:mm:ss tt") + " > " + message + "\n";

            this.rtxtConsole.Select(rtxtConsole.Text.Length - 1, 0);
            this.rtxtConsole.ScrollToCaret();
            this.rtxtConsole.Refresh();
        }
        public void WriteException(Exception ex)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(delegate() { this.WriteException(ex); }));
            else
                this.Write("The following exception occurred: " + ex.ToString(), MessageTypes.Exception);
        }
        public void WriteException(string message, Exception ex)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(delegate() { this.WriteException(message, ex); }));
            else
                this.Write(message + ex.ToString(), MessageTypes.Exception);
        }
        public void WriteException(string message)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(delegate() { this.WriteException(message); }));
            else
                this.Write(message, MessageTypes.Exception);
        }
        public void WriteWarning(string message)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(delegate() { this.WriteWarning(message); }));
            else
                this.Write("Warning!: " + message, MessageTypes.Warning);
        }
        public void WriteInfo(string message)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(delegate() { this.WriteInfo(message); }));
            else
                this.Write("Info: " + message, MessageTypes.Informational);
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            rtxtConsole.Text = "";
        }

        private void mnuWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            rtxtConsole.WordWrap = mnuWordWrap.Checked;
        }

        private void mnuSaveToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Files|*.rtf";

            try
            {
                if (dlg.ShowDialog(this.Parent) == DialogResult.OK)
                    rtxtConsole.SaveFile(dlg.FileName);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
            }
        }

        private void lnkMenu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cmMenu.Show(lnkMenu, new System.Drawing.Point(0, lnkMenu.Height));
        }

        private void Console_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                Assembly asm = Assembly.GetAssembly(this.ParentForm.GetType());
                AssemblyName asmName = asm.GetName();
                txtAssemblyVersion.Text = "About: \"" + asmName.Name + "\", v" + asmName.Version.ToString() + ", " + new FileInfo(asm.Location).LastWriteTime.ToString("dd-MMM-yyyy hh:mm:ss tt");
            }
        }

        private void mnuCopyConsoleText_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(rtxtConsole.SelectedText))
            {
                rtxtConsole.SelectAll();
                rtxtConsole.Copy();
                rtxtConsole.SelectedText = "";
            }
            else
                rtxtConsole.Copy();
        }

        private void mnuCopyVersionInfo_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtAssemblyVersion.Text);
        }
    }
}
