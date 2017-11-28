namespace elbsi_ui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._displayPanel = new elbemu_utils.Components.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // _displayPanel
            // 
            this._displayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._displayPanel.Location = new System.Drawing.Point(0, 0);
            this._displayPanel.Name = "_displayPanel";
            this._displayPanel.Size = new System.Drawing.Size(284, 262);
            this._displayPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this._displayPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "elbsi";
            this.ResumeLayout(false);

        }

        #endregion

        private elbemu_utils.Components.DoubleBufferedPanel _displayPanel;
    }
}

