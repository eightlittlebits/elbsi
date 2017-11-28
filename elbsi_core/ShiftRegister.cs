using System;

namespace elbsi_core
{
    class ShiftRegister : IInputIODevice, IOutputIODevice
    {
        private int _shiftOffset;
        private int _shiftHi;
        private int _shiftLo;

        public ShiftRegister(Bus bus)
        {
            bus.RegisterInputIODevice(0x03, this);

            bus.RegisterOutputIODevice(0x02, this);
            bus.RegisterOutputIODevice(0x04, this);
        }

        public byte In(byte port)
        {
            switch (port)
            {
                case 0x03: // read from port 0x03 returns the shifted value
                    int value = _shiftHi << 8 | _shiftLo;
                    return (byte)((value >> (8 - _shiftOffset)) & 0xFF);

                default:
                    throw new ArgumentException("Unknown port specified", nameof(port));
            }
        }

        public void Out(byte port, byte value)
        {
            switch (port)
            {
                case 0x02: // write to port 2 sets shift amount
                    _shiftOffset = value & 0x7;
                    break;

                case 0x04:
                    _shiftLo = _shiftHi;
                    _shiftHi = value;
                    break;

                default:
                    throw new ArgumentException("Unknown port specified", nameof(port));
            }
        }
    }
}
