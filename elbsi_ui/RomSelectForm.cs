using System.IO;
using System.Windows.Forms;

namespace elbsi_ui
{
    public partial class RomSelectForm : Form
    {
        private string _lastDirectory = string.Empty;

        public string SlotHFilename { get; set; }
        public string SlotGFilename { get; set; }
        public string SlotFFilename { get; set; }
        public string SlotEFilename { get; set; }

        public RomSelectForm()
        {
            InitializeComponent();

            slotHLocation.DataBindings.Add("Text", this, "SlotHFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            slotGLocation.DataBindings.Add("Text", this, "SlotGFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            slotFLocation.DataBindings.Add("Text", this, "SlotFFilename", false, DataSourceUpdateMode.OnPropertyChanged);
            slotELocation.DataBindings.Add("Text", this, "SlotEFilename", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void SlotHBrowseButtonClick(object sender, System.EventArgs e)
        {
            var result = BrowseForRomFile("H");
            if (result.result)
            {
                slotHLocation.Text = result.filename;
            }
        }

        private void SlotGBrowseButtonClick(object sender, System.EventArgs e)
        {
            var result = BrowseForRomFile("G");
            if (result.result)
            {
                slotGLocation.Text = result.filename;
            }
        }

        private void SlotFBrowseButtonClick(object sender, System.EventArgs e)
        {
            var result = BrowseForRomFile("F");
            if (result.result)
            {
                slotFLocation.Text = result.filename;
            }
        }

        private void SlotEBrowseButtonClick(object sender, System.EventArgs e)
        {
            var result = BrowseForRomFile("E");
            if (result.result)
            {
                slotELocation.Text = result.filename;
            }
        }

        private (bool result, string filename) BrowseForRomFile(string slotName)
        {
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = _lastDirectory,
                Title = "Open rom for slot " + slotName,
                Filter = "rom files (*.h;*.g;*.f;*.e;*.rom;*.bin)|*.h;*.g;*.f;*.e;*.rom;*.bin|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;

                _lastDirectory = Path.GetDirectoryName(filename);

                return (true, filename);
            }

            return (false, string.Empty);
        }
    }
}
