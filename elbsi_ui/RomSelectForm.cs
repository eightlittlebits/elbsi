using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace elbsi_ui
{
    public partial class RomSelectForm : Form
    {
        public string SlotHFilename { get; set; }
        public string SlotGFilename { get; set; }
        public string SlotFFilename { get; set; }
        public string SlotEFilename { get; set; }

        public string[] DefaultPaths { get; internal set; }

        private Dictionary<Button, TextBox> _buttonTextBoxMap;

        public RomSelectForm()
        {
            InitializeComponent();

            SlotHLocation.DataBindings.Add("Text", this, "SlotHFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            SlotGLocation.DataBindings.Add("Text", this, "SlotGFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            SlotFLocation.DataBindings.Add("Text", this, "SlotFFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            SlotELocation.DataBindings.Add("Text", this, "SlotEFilename", false, DataSourceUpdateMode.OnPropertyChanged);

            _buttonTextBoxMap = new Dictionary<Button, TextBox>()
            {
                { SlotHBrowseButton, SlotHLocation },
                { SlotGBrowseButton, SlotGLocation },
                { SlotFBrowseButton, SlotFLocation },
                { SlotEBrowseButton, SlotELocation }
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // add any defaults provided
            SlotHLocation.Text = DefaultPaths[0];
            SlotGLocation.Text = DefaultPaths[1];
            SlotFLocation.Text = DefaultPaths[2];
            SlotELocation.Text = DefaultPaths[3];
        }

        private void BrowseButtonClick(object sender, System.EventArgs e)
        {
            var textBox = _buttonTextBoxMap[(Button)sender];

            var (result, filename) = BrowseForRomFile((string)textBox.Tag);

            if (result)
            {
                textBox.Text = filename;
            }
        }

        private (bool result, string filename) BrowseForRomFile(string slotName)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Select ROM for slot " + slotName,
                Filter = "ROM files (*.h;*.g;*.f;*.e;*.rom;*.bin)|*.h;*.g;*.f;*.e;*.rom;*.bin|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return (true, openFileDialog.FileName);
            }

            return (false, string.Empty);
        }
    }
}
