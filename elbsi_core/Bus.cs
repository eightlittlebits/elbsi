using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elbsi_core
{
    class Bus
    {
        byte[] _memory;

        public Bus()
        {
            _memory = new byte[0x10000];
        }

        internal void Load(int address, byte[] rom)
        {
            Array.Copy(rom, 0, _memory, address, rom.Length);
        }

        internal byte ReadByte(ushort address)
        {
            return _memory[address & 0xFFFF];
        }

        internal void WriteByte(ushort address, byte value)
        {
            _memory[address & 0xFFFF] = value;
        }       
    }
}
