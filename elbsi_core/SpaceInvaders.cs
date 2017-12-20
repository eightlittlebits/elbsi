using System;
using elbsi_core.CPU;

namespace elbsi_core
{
    public class SpaceInvaders : IInputIODevice, IOutputIODevice
    {
        private Bus _bus;
        private I8080 _i8080;
        private ShiftRegister _shiftRegister;

        private int _cycles;

        private InputState _input;

        private byte _lastPort3Write, _lastPort5Write;

        Action<int, bool> _playSound;

        public SpaceInvaders(Action<int, bool> playSound)
        {
            _bus = new Bus();
            _i8080 = new I8080(_bus);
            _shiftRegister = new ShiftRegister(_bus);

            _cycles = 0;

            _playSound = playSound;

            _bus.RegisterInputIODevice(0x01, this);
            _bus.RegisterOutputIODevice(0x03, this);
            _bus.RegisterOutputIODevice(0x05, this);
        }

        public void LoadRom(ushort address, byte[] rom) => _bus.Load(address, rom);

        public void RunFrame(InputState input)
        {
            _input = input;

            while (_cycles < 16666)
            {
                _cycles += _i8080.ExecuteInstruction();
            }

            _i8080.RequestInterrupt(1);
            _cycles -= 16666;

            while (_cycles < 16667)
            {
                _cycles += _i8080.ExecuteInstruction();
            }

            _i8080.RequestInterrupt(2);
            _cycles -= 16667;
        }

        public void GetDisplayMemory(byte[] display)
        {
            _bus.CopyMemory(display, 0x2400, 0x1C00);
        }

        public byte In(byte port)
        {
            int portValue = 0x00;

            switch (port)
            {
                case 0x01:
                    portValue = 0x08 |
                                (_input.Credit ? (1 << 0) : 0) |
                                (_input.P1Start ? (1 << 2) : 0) |
                                (_input.P1Shot ? (1 << 4) : 0) |
                                (_input.P1Left ? (1 << 5) : 0) |
                                (_input.P1Right ? (1 << 6) : 0);
                    break;
            }

            return (byte)portValue;
        }

        public void Out(byte port, byte value)
        {
            switch (port)
            {
                case 0x03:
                    // Port 3:
                    //bit 0=UFO (repeats)        SX0 0.raw
                    if (((value & 0x01) & ~_lastPort3Write) == 0x01)
                    {
                        _playSound(0, true);
                    }
                    else if (((~value & 0x01) & _lastPort3Write) == 0x01)
                    {
                        _playSound(0, false);
                    }

                    //bit 1=Shot                 SX1 1.raw
                    if (((value & 0x02) & ~_lastPort3Write) == 0x02)
                    {
                        _playSound(1, true);
                    }

                    //bit 2=Flash (player die)   SX2 2.raw
                    if (((value & 0x04) & ~_lastPort3Write) == 0x04)
                    {
                        _playSound(2, true);
                    }

                    //bit 3=Invader die          SX3 3.raw
                    if (((value & 0x08) & ~_lastPort3Write) == 0x08)
                    {
                        _playSound(3, true);
                    }

                    _lastPort3Write = value;
                    break;

                case 0x05:
                    //Port 5:
                    //bit 0=Fleet movement 1     SX6 4.raw
                    if (((value & 0x01) & ~_lastPort5Write) == 0x01)
                    {
                        _playSound(4, true);
                    }

                    //bit 1=Fleet movement 2     SX7 5.raw
                    if (((value & 0x02) & ~_lastPort5Write) == 0x02)
                    {
                        _playSound(5, true);
                    }

                    //bit 2=Fleet movement 3     SX8 6.raw
                    if (((value & 0x04) & ~_lastPort5Write) == 0x04)
                    {
                        _playSound(6, true);
                    }

                    //bit 3=Fleet movement 4     SX9 7.raw
                    if (((value & 0x08) & ~_lastPort5Write) == 0x08)
                    {
                        _playSound(7, true);
                    }

                    //bit 4=UFO Hit              SX10 8.raw
                    if (((value & 0x10) & ~_lastPort5Write) == 0x10)
                    {
                        _playSound(8, true);
                    }

                    _lastPort5Write = value;
                    break;
            }
        }
    }
}
