using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using elbemu_utils;
using elbsi_core;
using elbsi_core.CPU;

namespace elbsi_ui
{
    public partial class MainForm : Form
    {
        private const int _targetFramesPerSecond = 60;
        private const int _screenWidth = 224;
        private const int _screenHeight = 256;

        private MessagePump _messagePump;

        private AudioDevice _audioDevice;
        private SoundEffect[] _soundEffects;

        private readonly double _stopwatchFrequency;

        private readonly double _targetFrameTicks;
        private long _lastFrameTimestamp;

        private bool _limitFrameRate = true;

        private GdiRenderer _renderer;
        private DirectBitmap _displayBuffer;
        private byte[] _display;
        private uint[] _overlay;

        private SpaceInvaders _invaders;
        private InputState _input;

        private bool _running = false;

        public MainForm()
        {
            _stopwatchFrequency = Stopwatch.Frequency;
            _targetFrameTicks = _stopwatchFrequency / _targetFramesPerSecond;

            InitializeComponent();
            BuildMainMenu();
            SetFormStyles();

            _messagePump = new MessagePump();

            _audioDevice = new AudioDevice(this.Handle);
            _soundEffects = LoadSoundEffects();

            _renderer = new GdiRenderer(this);
            _displayBuffer = new DirectBitmap(_screenWidth, _screenHeight);
            _display = new byte[0x1C00];

            _invaders = new SpaceInvaders(PlaySound);
            _input = new InputState();

            InitializeOverlay();
        }

        private SoundEffect[] LoadSoundEffects()
        {
            SoundEffect[] soundEffects = new SoundEffect[9];

            for (int i = 0; i < 9; i++)
            {
                soundEffects[i] = _audioDevice.LoadSoundEffect($"sounds\\{i}.wav");
            }

            soundEffects[0].Looping = true;

            return soundEffects;
        }

        private void PlaySound(int sound, bool play)
        {
            if (play)
            {
                _soundEffects[sound].Play();
            }
            else
            {
                _soundEffects[sound].Stop();
            }
        }

        private void InitializeOverlay()
        {
            _overlay = new uint[_screenWidth * _screenHeight];

            for (int y = 0; y < _screenHeight; y++)
            {
                for (int x = 0; x < _screenWidth; x++)
                {
                    if (y >= 32 && y < 64)
                    {
                        _overlay[y * _screenWidth + x] = 0xFFFF0000;
                    }
                    else if (y >= 184 && (y <= 240 || (x >= 16 && x < 134)))
                    {
                        _overlay[y * _screenWidth + x] = 0xFF00FF00;
                    }
                    else
                    {
                        _overlay[y * _screenWidth + x] = 0xFFFFFFFF;
                    }
                }
            }
        }

        private void BuildMainMenu()
        {
            MainMenu menu = new MainMenu(new[] {
                                             new MenuItem("&File", new[] {
                                                 new MenuItem("&Open", OpenRom),
                                                 new MenuItem("-"),
                                                 new MenuItem("E&xit", CloseForm),
                                             }),
                                         });

            this.Menu = menu;
        }

        private void OpenRom(object sender, EventArgs e)
        {
            using (RomSelectForm romSelect = new RomSelectForm())
            {
                romSelect.Owner = this;
                romSelect.DefaultPaths = new[]
                {
                    @"roms\invaders\invaders.h",
                    @"roms\invaders\invaders.g",
                    @"roms\invaders\invaders.f",
                    @"roms\invaders\invaders.e"
                };

                if (romSelect.ShowDialog() == DialogResult.OK)
                {
                    // load rom files into memory
                    LoadRom(0x0000, romSelect.SlotHFilename);
                    LoadRom(0x0800, romSelect.SlotGFilename);
                    LoadRom(0x1000, romSelect.SlotFFilename);
                    LoadRom(0x1800, romSelect.SlotEFilename);

                    // begin running
                    _messagePump.RunWhileIdle(Frame);
                    _running = true;
                }
            }
        }

        private void LoadRom(ushort address, string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException($"ROM file {Path.GetFileName(filename)} not found.", filename);
            }

            byte[] romData = File.ReadAllBytes(filename);

            // each rom must be 2k
            if (romData.Length != 0x800)
            {
                throw new InvalidDataException($"ROM file {Path.GetFileName(filename)} is incorrect size, expected 2048 bytes.");
            }

            _invaders.LoadRom(address, romData);
        }

        private void SetFormStyles()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // these settings as applied below conflict with the documents,
            // we're ok in this instance though as we're rendering the control
            // ourselves constantly. We don't need windows to be raising either
            // the WM_ERASEBKGND or WM_PAINT events. Seetting 
            // AllPaintingInWmPaint to true prevents WM_ERASEBKGND and setting
            // UserPaint to false prevents WM_PAINT
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, false);

            UpdateStyles();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ClientSize = new Size(_screenWidth * 2, _screenHeight * 2);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_running)
            {
                _messagePump.Resume();
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if (_running)
            {
                _messagePump.Pause();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_running)
            {
                _messagePump.Stop();
            }
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
                case Keys.Space:
                    _input.Credit = pressed;
                    break;

                case Keys.D1:
                    _input.P1Start = pressed;
                    break;

                case Keys.Left:
                    _input.P1Left = pressed;
                    break;

                case Keys.Right:
                    _input.P1Right = pressed;
                    break;

                case Keys.Up:
                    _input.P1Shot = pressed;
                    break;

                case Keys.ShiftKey:
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

            try
            {
                _invaders.RunFrame(_input);
            }
            catch (InfiniteLoopException)
            {
                _messagePump.Stop();
            }

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
                    byte input = _display[y * 32 + x];
                    int outx = y;

                    for (int i = 0; i < 8; i++)
                    {
                        int outy = 255 - (x * 8) - i;
                        output[(outy * 224) + outx] = ((input >> i) & 0x01) == 0x01 ? _overlay[(outy * 224) + outx] : 0xFF000000;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < _soundEffects.Length; i++)
                {
                    _soundEffects[i].Dispose();
                }
                _audioDevice.Dispose();
                components?.Dispose();
                _displayBuffer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
