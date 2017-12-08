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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.slotHLocation = new System.Windows.Forms.TextBox();
            this.slotELocation = new System.Windows.Forms.TextBox();
            this.slotGLocation = new System.Windows.Forms.TextBox();
            this.slotFLocation = new System.Windows.Forms.TextBox();
            this.slotHBrowseButton = new System.Windows.Forms.Button();
            this.slotGBrowseButton = new System.Windows.Forms.Button();
            this.slotEBrowseButton = new System.Windows.Forms.Button();
            this.slotFBrowseButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 13);
            label1.TabIndex = 10;
            label1.Text = "Slot H";
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
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(115, 125);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(196, 125);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // slotHLocation
            // 
            this.slotHLocation.Location = new System.Drawing.Point(54, 12);
            this.slotHLocation.Name = "slotHLocation";
            this.slotHLocation.Size = new System.Drawing.Size(283, 20);
            this.slotHLocation.TabIndex = 2;
            // 
            // slotELocation
            // 
            this.slotELocation.Location = new System.Drawing.Point(54, 99);
            this.slotELocation.Name = "slotELocation";
            this.slotELocation.Size = new System.Drawing.Size(283, 20);
            this.slotELocation.TabIndex = 3;
            // 
            // slotGLocation
            // 
            this.slotGLocation.Location = new System.Drawing.Point(54, 41);
            this.slotGLocation.Name = "slotGLocation";
            this.slotGLocation.Size = new System.Drawing.Size(283, 20);
            this.slotGLocation.TabIndex = 4;
            // 
            // slotFLocation
            // 
            this.slotFLocation.Location = new System.Drawing.Point(54, 70);
            this.slotFLocation.Name = "slotFLocation";
            this.slotFLocation.Size = new System.Drawing.Size(283, 20);
            this.slotFLocation.TabIndex = 5;
            // 
            // slotHBrowseButton
            // 
            this.slotHBrowseButton.Location = new System.Drawing.Point(343, 10);
            this.slotHBrowseButton.Name = "slotHBrowseButton";
            this.slotHBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.slotHBrowseButton.TabIndex = 6;
            this.slotHBrowseButton.Text = "...";
            this.slotHBrowseButton.UseVisualStyleBackColor = true;
            this.slotHBrowseButton.Click += new System.EventHandler(this.SlotHBrowseButtonClick);
            // 
            // slotGBrowseButton
            // 
            this.slotGBrowseButton.Location = new System.Drawing.Point(343, 39);
            this.slotGBrowseButton.Name = "slotGBrowseButton";
            this.slotGBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.slotGBrowseButton.TabIndex = 7;
            this.slotGBrowseButton.Text = "...";
            this.slotGBrowseButton.UseVisualStyleBackColor = true;
            this.slotGBrowseButton.Click += new System.EventHandler(this.SlotGBrowseButtonClick);
            // 
            // slotEBrowseButton
            // 
            this.slotEBrowseButton.Location = new System.Drawing.Point(343, 97);
            this.slotEBrowseButton.Name = "slotEBrowseButton";
            this.slotEBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.slotEBrowseButton.TabIndex = 8;
            this.slotEBrowseButton.Text = "...";
            this.slotEBrowseButton.UseVisualStyleBackColor = true;
            this.slotEBrowseButton.Click += new System.EventHandler(this.SlotEBrowseButtonClick);
            // 
            // slotFBrowseButton
            // 
            this.slotFBrowseButton.Location = new System.Drawing.Point(343, 68);
            this.slotFBrowseButton.Name = "slotFBrowseButton";
            this.slotFBrowseButton.Size = new System.Drawing.Size(31, 23);
            this.slotFBrowseButton.TabIndex = 9;
            this.slotFBrowseButton.Text = "...";
            this.slotFBrowseButton.UseVisualStyleBackColor = true;
            this.slotFBrowseButton.Click += new System.EventHandler(this.SlotFBrowseButtonClick);
            // 
            // RomSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 158);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.slotFBrowseButton);
            this.Controls.Add(this.slotEBrowseButton);
            this.Controls.Add(this.slotGBrowseButton);
            this.Controls.Add(this.slotHBrowseButton);
            this.Controls.Add(this.slotFLocation);
            this.Controls.Add(this.slotGLocation);
            this.Controls.Add(this.slotELocation);
            this.Controls.Add(this.slotHLocation);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RomSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select ROM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox slotHLocation;
        private System.Windows.Forms.TextBox slotELocation;
        private System.Windows.Forms.TextBox slotGLocation;
        private System.Windows.Forms.TextBox slotFLocation;
        private System.Windows.Forms.Button slotHBrowseButton;
        private System.Windows.Forms.Button slotGBrowseButton;
        private System.Windows.Forms.Button slotEBrowseButton;
        private System.Windows.Forms.Button slotFBrowseButton;
    }
}