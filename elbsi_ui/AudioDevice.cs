using System;
using elbemu_utils;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

namespace elbsi_ui
{
    class AudioDevice : IDisposable
    {
        private bool _disposed = false;

        DirectSound _directSound;
        private PrimarySoundBuffer _primarySoundBuffer;

        public AudioDevice(IntPtr hwnd)
        {
            // initialise directsound
            _directSound = new DirectSound();
            _directSound.SetCooperativeLevel(hwnd, CooperativeLevel.Priority);

            // create primary sound buffer
            var primaryBufferDesc = new SoundBufferDescription
            {
                Flags = BufferFlags.PrimaryBuffer,
                AlgorithmFor3D = Guid.Empty
            };

            _primarySoundBuffer = new PrimarySoundBuffer(_directSound, primaryBufferDesc)
            {
                Format = new WaveFormat(),
            };
        }

        public SoundEffect LoadSoundEffect(string filename)
        {
            WaveFile wave = WaveFile.LoadFromFile(filename);

            var bufferDescription = new SoundBufferDescription
            {
                BufferBytes = wave.SampleData.Length,
                Format = new WaveFormat((int)wave.Format.SamplesPerSec, wave.Format.BitsPerSample, wave.Format.Channels),
                AlgorithmFor3D = Guid.Empty
            };

            var secondaryBuffer = new SecondarySoundBuffer(_directSound, bufferDescription);
            var capabilities = secondaryBuffer.Capabilities;

            var dataStream1 = secondaryBuffer.Lock(0, capabilities.BufferBytes, LockFlags.EntireBuffer, out var dataStream2);

            dataStream1.Write(wave.SampleData, 0, wave.SampleData.Length);

            secondaryBuffer.Unlock(dataStream1, dataStream2);

            return new SoundEffect(secondaryBuffer);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _directSound.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
