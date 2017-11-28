using System;

namespace elbsi_core
{
    class Bus
    {
        byte[] _memory;

        IInputIODevice[] _inputIO;
        IOutputIODevice[] _outputIO;

        public Bus()
        {
            _memory = new byte[0x10000];

            _inputIO = new IInputIODevice[0x100];
            _outputIO = new IOutputIODevice[0x100];
        }

        internal void Load(int address, byte[] rom) => Array.Copy(rom, 0, _memory, address, rom.Length);

        internal byte ReadByte(ushort address) => _memory[address & 0xFFFF];
        internal void WriteByte(ushort address, byte value) => _memory[address & 0xFFFF] = value;

        internal void RegisterInputIODevice(byte port, IInputIODevice device) => _inputIO[port] = device;
        internal void RegisterOutputIODevice(byte port, IOutputIODevice device) => _outputIO[port] = device;

        internal byte In(byte port) => _inputIO[port]?.In(port) ?? 0x00;
        internal void Out(byte port, byte value) => _outputIO[port]?.Out(port, value);
    }
}
