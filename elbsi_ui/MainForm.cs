using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using elbemu_utils;
using elbsi_core;

namespace elbsi_ui
{
    public partial class MainForm : Form
    {
        private const int _targetFramesPerSecond = 60;
        private const int _screenWidth = 224;
        private const int _screenHeight = 256;

        private MessagePump _messagePump;

        private readonly double _stopwatchFrequency;

        private readonly double _targetFrameTicks;
        private long _lastFrameTimestamp;

        private bool _limitFrameRate = true;

        private GdiRenderer _renderer;
        private DirectBitmap _displayBuffer;
        private byte[] _display;

        private SpaceInvaders _invaders;

        public MainForm()
        {
            InitializeComponent();

            _stopwatchFrequency = Stopwatch.Frequency;
            _targetFrameTicks = _stopwatchFrequency / _targetFramesPerSecond;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _messagePump = new MessagePump();

            ClientSize = new Size(_screenWidth * 2, _screenHeight * 2);

            _renderer = new GdiRenderer(_displayPanel);
            _displayBuffer = new DirectBitmap(_screenWidth, _screenHeight);
            _display = new byte[0x1C00];

            _invaders = new SpaceInvaders();

            _invaders.LoadRom(0x0000, File.ReadAllBytes(@"roms\invaders\invaders.h"));
            _invaders.LoadRom(0x0800, File.ReadAllBytes(@"roms\invaders\invaders.g"));
            _invaders.LoadRom(0x1000, File.ReadAllBytes(@"roms\invaders\invaders.f"));
            _invaders.LoadRom(0x1800, File.ReadAllBytes(@"roms\invaders\invaders.e"));

            _messagePump.RunWhileIdle(Frame);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            _messagePump.Resume();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            _messagePump.Pause();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _messagePump.Stop();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // returns true if the key was processed, if not pass to 
            // default handler
            if (!ProcessKeyboard(e.KeyCode, true))
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            // returns true if the key was processed, if not pass to 
            // default handler
            if (!ProcessKeyboard(e.KeyCode, false))
            {
                base.OnKeyUp(e);
            }
        }

        private bool ProcessKeyboard(Keys key, bool pressed)
        {
            switch (key)
            {
                case Keys.Tab:
                    _limitFrameRate = !pressed;
                    break;

                default:
                    return false;
            }

            return true;
        }

        private void Frame()
        {
            long updateTimeStart = Stopwatch.GetTimestamp();

            _invaders.RunFrame();

            long updateTimeEnd = Stopwatch.GetTimestamp();

            long renderTimeStart = updateTimeEnd;

            RenderDisplayToDisplayBuffer();
            _renderer.DrawBitmap(_displayBuffer.Bitmap);

            long renderTimeEnd = Stopwatch.GetTimestamp();

            long currentTimeStamp = Stopwatch.GetTimestamp();
            long elapsedTicks = currentTimeStamp - _lastFrameTimestamp;

            if (_limitFrameRate && elapsedTicks < _targetFrameTicks)
            {
                // get ms to sleep for, cast to int to truncate to nearest millisecond
                // take 1 ms off the sleep time as we don't always hit the sleep exactly, trade
                // burning extra cpu in the spin loop for accuracy
                int sleepMilliseconds = (int)((_targetFrameTicks - elapsedTicks) * 1000 / _stopwatchFrequency) - 1;

                if (sleepMilliseconds > 0)
                {
                    Thread.Sleep(sleepMilliseconds);
                }

                // spin for the remaining partial millisecond to hit target frame rate
                while ((Stopwatch.GetTimestamp() - _lastFrameTimestamp) < _targetFrameTicks) ;
            }

            long endFrameTimestamp = Stopwatch.GetTimestamp();

            long totalFrameTicks = endFrameTimestamp - _lastFrameTimestamp;

            _lastFrameTimestamp = endFrameTimestamp;

            double updateTime = (updateTimeEnd - updateTimeStart) * 1000 / _stopwatchFrequency;
            double renderTime = (renderTimeEnd - renderTimeStart) * 1000 / _stopwatchFrequency;

            double frameTime = totalFrameTicks * 1000 / _stopwatchFrequency;

            this.Text = $"elbsi - {updateTime:00.000}ms {renderTime:00.000}ms {frameTime:00.0000}ms";
        }

        private unsafe void RenderDisplayToDisplayBuffer()
        {
            const int displayWidth = 32; // bytes, expands into 256 pixels
            const int displayHeight = 224;

            _invaders.GetDisplayMemory(_display);

            uint* output = ((uint*)_displayBuffer.BitmapData);

            // convert 32x224 display into 224*256 display (rotates 90 degrees counter clockwise)
            for (int y = 0; y < displayHeight; y++)
            {
                for (int x = 0; x < displayWidth; x++)
                {
                    int outx = y;
                    int outy = x*8;
                                        
                    byte input = _display[y * 32 + x];

                    for (int i = 0; i < 8; i++)
                    {
                        //output[((outy + i) * 224) + outx] = ((input >> 7 - i) & 0x01) == 0x01 ? 0xFFFFFFFF : 0xFF000000; // <-- upside down fun!
                        output[((255 - outy - i) * 224) + outx] = ((input >> i) & 0x01) == 0x01 ? 0xFFFFFFFF : 0xFF000000;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _displayBuffer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
