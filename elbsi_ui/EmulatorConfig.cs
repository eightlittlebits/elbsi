using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using elbemu_utils.Configuration;

namespace elbsi_ui
{
    public sealed class EmulatorConfig : XmlConfiguration<EmulatorConfig>, INotifyPropertyChanged
    {
        private bool _limitFrameRate = true;
        private bool _pauseWhenFocusLost = true;

        public bool LimitFrameRate
        {
            get => _limitFrameRate;
            set
            {
                if (_limitFrameRate != value)
                {
                    _limitFrameRate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool PauseWhenFocusLost
        {
            get => _pauseWhenFocusLost;
            set
            {
                if (_pauseWhenFocusLost != value)
                {
                    _pauseWhenFocusLost = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
