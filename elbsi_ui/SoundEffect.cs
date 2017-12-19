using System;
using SharpDX.DirectSound;

namespace elbsi_ui
{
    class SoundEffect : IDisposable
    {
        private bool _disposed = false;

        SecondarySoundBuffer _soundBuffer;
        PlayFlags _playFlags;

        public bool Looping
        {
            set
            {
                if (value)
                {
                    _playFlags |= PlayFlags.Looping;
                }
                else
                {
                    _playFlags &= ~PlayFlags.Looping;
                }
            }
        }

        public SoundEffect(SecondarySoundBuffer soundBuffer)
        {
            _soundBuffer = soundBuffer;
        }

        public void Play()
        {
            _soundBuffer.CurrentPosition = 0;
            _soundBuffer.Play(0, _playFlags);
        }

        public void Stop()
        {
            _soundBuffer.Stop();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _soundBuffer.Dispose();
                }
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
