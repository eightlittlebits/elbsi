namespace elbsi_ui
{
    partial class RomSelectForm
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label1;
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SlotHLocation = new System.Windows.Forms.TextBox();
            this.SlotELocation = new System.Windows.Forms.TextBox();
            this.SlotGLocation = new System.Windows.Forms.TextBox();
            this.SlotFLocation = new System.Windows.Forms.TextBox();
            this.SlotHBrowseButton = new System.Windows.Forms.Button();
            this.SlotGBrowseButton = new System.Windows.Forms.Button();
            this.SlotEBrowseButton = new System.Windows.Forms.Button();
            this.SlotFBrowseButton = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 44);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(36, 13);
            label2.TabIndex = 11;
            label2.Text = "Slot G";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 73);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(34, 13);
            label3.TabIndex = 12;
            label3.Text = "Slot F";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 102);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 13);
            label4.TabIndex = 13;
            label4.Text = "Slot E";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 13);
            label1.TabIndex = 14;
            label1.Text = "Slot H";
            // 
            // Ok
            // 
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Ok.Location = new System.Drawing.Point(115, 125);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(196, 125);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // SlotHLocation
            // 
            this.SlotHLocation.Location = new System.Drawing.Point(54, 12);
            this.SlotHLocation.Name = "SlotHLocation";
            this.SlotHLocation.Size = new System.Drawing.Size(283, 20);
            this.SlotHLocation.TabIndex = 2;
            this.SlotHLocation.Tag = "H";
            // 
            // SlotELocation
            // 
            this.SlotELocation.Location = new System.Drawing.Point(54, 99);
            this.SlotELocation.Name = "SlotELocation";
            this.SlotELocation.Size = new System.Drawing.Size(283, 20);
            this.SlotELocation.TabIndex = 3;
            this.SlotELocation.Tag = "E";
            // 
            // SlotGLocation
            // 
            this.SlotGLocation.Location = new System.Drawing.Point(54, 41);
            this.SlotGLocation.Name = "SlotGLocation";
            this.SlotGLocation.Size = new System.Drawing.Size(283, 20);
            this.SlotGLocation.TabIndex = 4;
            this.SlotGLocation.Tag = "G";
            // 
            // SlotFLocation
            // 
            this.SlotFLocation.Location = new System.Drawing.Point(54, 70);
            this.SlotFLocation.Name = "SlotFLocation";
            this.SlotFLocation.Size = new System.Drawing.Size(283, 20);
            this.SlotFLocation.TabIndex = 5;
            this.SlotFLocation.Tag = "F";
            // 
            // SlotHBrowseButton
            // 
            this.SlotHBrowseButton.Location = new System.Drawing.Point(343, 10);
            this.SlotHBrowseButton.Name = "SlotHBrowseButton";
            this.SlotHBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.SlotHBrowseButton.TabIndex = 6;
            this.SlotHBrowseButton.Text = "...";
            this.SlotHBrowseButton.UseVisualStyleBackColor = true;
            this.SlotHBrowseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // SlotGBrowseButton
            // 
            this.SlotGBrowseButton.Location = new System.Drawing.Point(343, 39);
            this.SlotGBrowseButton.Name = "SlotGBrowseButton";
            this.SlotGBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.SlotGBrowseButton.TabIndex = 7;
            this.SlotGBrowseButton.Text = "...";
            this.SlotGBrowseButton.UseVisualStyleBackColor = true;
            this.SlotGBrowseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // SlotEBrowseButton
            // 
            this.SlotEBrowseButton.Location = new System.Drawing.Point(343, 97);
            this.SlotEBrowseButton.Name = "SlotEBrowseButton";
            this.SlotEBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.SlotEBrowseButton.TabIndex = 8;
            this.SlotEBrowseButton.Text = "...";
            this.SlotEBrowseButton.UseVisualStyleBackColor = true;
            this.SlotEBrowseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // SlotFBrowseButton
            // 
            this.SlotFBrowseButton.Location = new System.Drawing.Point(343, 68);
            this.SlotFBrowseButton.Name = "SlotFBrowseButton";
            this.SlotFBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.SlotFBrowseButton.TabIndex = 9;
            this.SlotFBrowseButton.Text = "...";
            this.SlotFBrowseButton.UseVisualStyleBackColor = true;
            this.SlotFBrowseButton.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // RomSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(386, 158);
            this.Controls.Add(label1);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.SlotFBrowseButton);
            this.Controls.Add(this.SlotEBrowseButton);
            this.Controls.Add(this.SlotGBrowseButton);
            this.Controls.Add(this.SlotHBrowseButton);
            this.Controls.Add(this.SlotFLocation);
            this.Controls.Add(this.SlotGLocation);
            this.Controls.Add(this.SlotELocation);
            this.Controls.Add(this.SlotHLocation);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RomSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select ROMs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox SlotHLocation;
        private System.Windows.Forms.TextBox SlotELocation;
        private System.Windows.Forms.TextBox SlotGLocation;
        private System.Windows.Forms.TextBox SlotFLocation;
        private System.Windows.Forms.Button SlotHBrowseButton;
        private System.Windows.Forms.Button SlotGBrowseButton;
        private System.Windows.Forms.Button SlotEBrowseButton;
        private System.Windows.Forms.Button SlotFBrowseButton;
    }
}