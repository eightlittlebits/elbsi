using elbsi_core.CPU;

namespace elbsi_core
{
    public class SpaceInvaders
    {
        private Bus _bus;
        private I8080 _i8080;
        private ShiftRegister _shiftRegister;

        private int _cycles;

        public SpaceInvaders()
        {
            _bus = new Bus();
            _i8080 = new I8080(_bus);
            _shiftRegister = new ShiftRegister(_bus);

            _cycles = 0;
        }

        public void LoadRom(ushort address, byte[] rom) => _bus.Load(address, rom);

        public void RunFrame()
        {
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
    }
}
