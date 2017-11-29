using elbsi_core.CPU;

namespace elbsi_core
{
    public class SpaceInvaders : IInputIODevice
    {
        private Bus _bus;
        private I8080 _i8080;
        private ShiftRegister _shiftRegister;

        private int _cycles;

        private InputState _input;

        public SpaceInvaders()
        {
            _bus = new Bus();
            _i8080 = new I8080(_bus);
            _shiftRegister = new ShiftRegister(_bus);

            _cycles = 0;

            _bus.RegisterInputIODevice(0x01, this);
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
    }
}
