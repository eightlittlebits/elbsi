﻿using System;

namespace elbsi_core.CPU
{
    using static StatusFlags;

    class I8080
    {
        private static byte[] FlagsSZP;

        private Bus _bus;

        private Registers _r;
        private ushort _pc;
        private ushort _sp;

        internal ushort PC { get => _pc; set => _pc = value; }
        internal Registers R { get => _r; }

        static I8080()
        {
            InitStatusTables();
        }

        private static void InitStatusTables()
        {
            FlagsSZP = new byte[0x100];

            for (int i = 0; i < 0x100; i++)
            {
                int sf, zf, pf;

                sf = i & S;
                zf = i == 0 ? Z : 0;
                pf = evenParity(i) ? P : 0;

                FlagsSZP[i] = (byte)(sf | zf | B1 | pf);
            }

            bool evenParity(int v)
            {
                v ^= v >> 4;
                v ^= v >> 2;
                v ^= v >> 1;

                return (v & 1) != 1;
            }
        }

        public I8080(Bus bus)
        {
            _r = new Registers();
            _bus = bus;
        }

        #region memory access

        private byte ReadByte(ushort address)
        {
            return _bus.ReadByte(address);
        }

        private ushort ReadWord(ushort address)
        {
            byte lo = ReadByte(address);
            byte hi = ReadByte((ushort)(address + 1));

            return (ushort)(hi << 8 | lo);
        }

        private void WriteByte(ushort address, byte value)
        {
            _bus.WriteByte(address, value);
        }

        private void WriteWord(ushort address, ushort value)
        {
            WriteByte(address, (byte)value);
            WriteByte((ushort)(address + 1), (byte)(value >> 8));
        }

        #endregion

        #region stack handling

        // Push a byte on to the stack, decrement SP then write to memory
        private void PushByte(byte value)
        {
            WriteByte(--_sp, value);
        }

        // Pop a byte from the stack, read from memory then increment SP
        private byte PopByte()
        {
            return ReadByte(_sp++);
        }

        // Push a word onto the stack, high byte pushed first
        private void PushWord(ushort value)
        {
            PushByte((byte)(value >> 8));
            PushByte((byte)value);
        }

        // Pop a word from the stack, low byte popped first
        private ushort PopWord()
        {
            byte lo = PopByte();
            byte hi = PopByte();

            return (ushort)(hi << 8 | lo);
        }

        #endregion

        internal void ExecuteInstruction()
        {
            byte opcode = ReadByte(_pc++);
            ExecuteOpcode(opcode);
        }

        private int ExecuteOpcode(byte opcode)
        {
            int cycles;

            switch (opcode)
            {
                case 0x00: goto default;
                case 0x01: _r.BC = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI B,a16
                case 0x02: goto default;
                case 0x03: goto default;
                case 0x04: goto default;
                case 0x05: goto default;
                case 0x06: goto default;
                case 0x07: goto default;
                case 0x08: goto default;
                case 0x09: goto default;
                case 0x0A: goto default;
                case 0x0B: goto default;
                case 0x0C: goto default;
                case 0x0D: goto default;
                case 0x0E: goto default;
                case 0x0F: goto default;
                case 0x10: goto default;
                case 0x11: _r.DE = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI D,a16
                case 0x12: goto default;
                case 0x13: goto default;
                case 0x14: goto default;
                case 0x15: goto default;
                case 0x16: goto default;
                case 0x17: goto default;
                case 0x18: goto default;
                case 0x19: goto default;
                case 0x1A: goto default;
                case 0x1B: goto default;
                case 0x1C: goto default;
                case 0x1D: goto default;
                case 0x1E: goto default;
                case 0x1F: goto default;
                case 0x20: goto default;
                case 0x21: _r.HL = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI H,a16
                case 0x22: goto default;
                case 0x23: goto default;
                case 0x24: goto default;
                case 0x25: goto default;
                case 0x26: goto default;
                case 0x27: goto default;
                case 0x28: goto default;
                case 0x29: goto default;
                case 0x2A: goto default;
                case 0x2B: goto default;
                case 0x2C: goto default;
                case 0x2D: goto default;
                case 0x2E: goto default;
                case 0x2F: goto default;
                case 0x30: goto default;
                case 0x31: _sp = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI SP,d16
                case 0x32: goto default;
                case 0x33: goto default;
                case 0x34: goto default;
                case 0x35: goto default;
                case 0x36: goto default;
                case 0x37: goto default;
                case 0x38: goto default;
                case 0x39: goto default;
                case 0x3A: _r.A = ReadByte(ReadWord(_pc)); _pc += 2; cycles = 13; break; // LDA a16
                case 0x3B: goto default;
                case 0x3C: goto default;
                case 0x3D: goto default;
                case 0x3E: _r.A = ReadByte(_pc++); cycles = 7; break; // MVI A,d8
                case 0x3F: goto default;
                case 0x40: goto default;
                case 0x41: goto default;
                case 0x42: goto default;
                case 0x43: goto default;
                case 0x44: goto default;
                case 0x45: goto default;
                case 0x46: _r.B = ReadByte(_r.HL); cycles = 7; break; // MOV B,M
                case 0x47: goto default;
                case 0x48: goto default;
                case 0x49: goto default;
                case 0x4A: goto default;
                case 0x4B: goto default;
                case 0x4C: goto default;
                case 0x4D: goto default;
                case 0x4E: _r.C = ReadByte(_r.HL); cycles = 7; break; // MOV C,M
                case 0x4F: goto default;
                case 0x50: goto default;
                case 0x51: goto default;
                case 0x52: goto default;
                case 0x53: goto default;
                case 0x54: goto default;
                case 0x55: goto default;
                case 0x56: _r.D = ReadByte(_r.HL); cycles = 7; break; // MOV D,M
                case 0x57: goto default;
                case 0x58: goto default;
                case 0x59: goto default;
                case 0x5A: goto default;
                case 0x5B: goto default;
                case 0x5C: goto default;
                case 0x5D: goto default;
                case 0x5E: _r.E = ReadByte(_r.HL); cycles = 7; break; // MOV E,M
                case 0x5F: goto default;
                case 0x60: goto default;
                case 0x61: goto default;
                case 0x62: goto default;
                case 0x63: goto default;
                case 0x64: goto default;
                case 0x65: goto default;
                case 0x66: _r.H = ReadByte(_r.HL); cycles = 7; break; // MOV H,M
                case 0x67: goto default;
                case 0x68: goto default;
                case 0x69: goto default;
                case 0x6A: goto default;
                case 0x6B: goto default;
                case 0x6C: goto default;
                case 0x6D: goto default;
                case 0x6E: _r.L = ReadByte(_r.HL); cycles = 7; break; // MOV L,M
                case 0x6F: goto default;
                case 0x70: goto default;
                case 0x71: goto default;
                case 0x72: goto default;
                case 0x73: goto default;
                case 0x74: goto default;
                case 0x75: goto default;
                case 0x76: goto default;
                case 0x77: goto default;
                case 0x78: goto default;
                case 0x79: goto default;
                case 0x7A: goto default;
                case 0x7B: goto default;
                case 0x7C: _r.A = _r.H; cycles = 5; break; // MOV A,H
                case 0x7D: _r.A = _r.L; cycles = 5; break; // MOV A,L
                case 0x7E: _r.A = ReadByte(_r.HL); cycles = 7; break; // MOV A,M
                case 0x7F: goto default;
                case 0x80: goto default;
                case 0x81: goto default;
                case 0x82: goto default;
                case 0x83: goto default;
                case 0x84: goto default;
                case 0x85: goto default;
                case 0x86: goto default;
                case 0x87: goto default;
                case 0x88: goto default;
                case 0x89: goto default;
                case 0x8A: goto default;
                case 0x8B: goto default;
                case 0x8C: goto default;
                case 0x8D: goto default;
                case 0x8E: goto default;
                case 0x8F: goto default;
                case 0x90: goto default;
                case 0x91: goto default;
                case 0x92: goto default;
                case 0x93: goto default;
                case 0x94: goto default;
                case 0x95: goto default;
                case 0x96: goto default;
                case 0x97: goto default;
                case 0x98: goto default;
                case 0x99: goto default;
                case 0x9A: goto default;
                case 0x9B: goto default;
                case 0x9C: goto default;
                case 0x9D: goto default;
                case 0x9E: goto default;
                case 0x9F: goto default;
                case 0xA0: goto default;
                case 0xA1: goto default;
                case 0xA2: goto default;
                case 0xA3: goto default;
                case 0xA4: goto default;
                case 0xA5: goto default;
                case 0xA6: goto default;
                case 0xA7: goto default;
                case 0xA8: goto default;
                case 0xA9: goto default;
                case 0xAA: goto default;
                case 0xAB: goto default;
                case 0xAC: goto default;
                case 0xAD: goto default;
                case 0xAE: goto default;
                case 0xAF: goto default;
                case 0xB0: goto default;
                case 0xB1: goto default;
                case 0xB2: goto default;
                case 0xB3: goto default;
                case 0xB4: goto default;
                case 0xB5: goto default;
                case 0xB6: goto default;
                case 0xB7: goto default;
                case 0xB8: goto default;
                case 0xB9: goto default;
                case 0xBA: goto default;
                case 0xBB: goto default;
                case 0xBC: goto default;
                case 0xBD: goto default;
                case 0xBE: goto default;
                case 0xBF: goto default;
                case 0xC0: goto default;
                case 0xC1: _r.BC = PopWord(); cycles = 10; break; // POP B

                case 0xC4: goto default;
                case 0xC5: PushWord(_r.BC); cycles = 11; break; // PUSH B
                case 0xC6: goto default;
                case 0xC7: goto default;
                case 0xC8: goto default;
                case 0xC9: Return(); cycles = 10; break; // RET

                #region branch group

                case 0xC3: JumpImmediate(); cycles = 10; break; // JMP a16
                case 0xCA: JumpImmediate(_r.F[Z]); cycles = 10; break; // JZ a16
                case 0xC2: JumpImmediate(!_r.F[Z]); cycles = 10; break; // JNZ a16

                case 0xCD: CallImmediate(); cycles = 17; break; // CALL a16

                #endregion

                case 0xCB: goto default;
                case 0xCC: goto default;

                case 0xCE: goto default;
                case 0xCF: goto default;
                case 0xD0: goto default;
                case 0xD1:  _r.DE = PopWord(); cycles = 10; break; // POP D
                case 0xD2: goto default;
                case 0xD3: goto default;
                case 0xD4: goto default;
                case 0xD5: PushWord(_r.DE); cycles = 11; break; // PUSH D
                case 0xD6: goto default;
                case 0xD7: goto default;
                case 0xD8: goto default;
                case 0xD9: goto default;
                case 0xDA: goto default;
                case 0xDB: goto default;
                case 0xDC: goto default;
                case 0xDD: goto default;
                case 0xDE: goto default;
                case 0xDF: goto default;
                case 0xE0: goto default;
                case 0xE1: _r.HL = PopWord(); cycles = 10; break; // POP H
                case 0xE2: goto default;
                case 0xE3: goto default;
                case 0xE4: goto default;
                case 0xE5: PushWord(_r.HL); cycles = 11; break; // PUSH H
                case 0xE6: goto default;
                case 0xE7: goto default;
                case 0xE8: goto default;
                case 0xE9: goto default;
                case 0xEA: goto default;
                case 0xEB: goto default;
                case 0xEC: goto default;
                case 0xED: goto default;
                case 0xEE: goto default;
                case 0xEF: goto default;
                case 0xF0: goto default;
                case 0xF1: _r.PSW = PopWord(); cycles = 10; break; // POP PSW
                case 0xF2: goto default;
                case 0xF3: goto default;
                case 0xF4: goto default;
                case 0xF5: PushWord(_r.PSW); cycles = 11; break; // PUSH PSW
                case 0xF6: goto default;
                case 0xF7: goto default;
                case 0xF8: goto default;
                case 0xF9: goto default;
                case 0xFA: goto default;
                case 0xFB: goto default;
                case 0xFC: goto default;
                case 0xFD: goto default;
                case 0xFE: _r.F = Compare8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // CPI d8
                case 0xFF: goto default;

                default: throw new NotImplementedException($"Unimplemented opcode: 0x{opcode:X2} at address 0x{_pc - 1:X4}");
            }

            return cycles;
        }

        #region arithmetic group handlers

        private static (byte, StatusFlags) And8Bit(byte a, byte b)
        {
            int result = a & b;

            StatusFlags flags = FlagsSZP[result] | ((a | b) & B3);

            return ((byte)result, flags);
        }

        // https://stackoverflow.com/questions/8034566/overflow-and-carry-flags-on-z80/8037485#8037485
        private static (byte, StatusFlags) Add8Bit(byte a, byte b, bool carry = false)
        {
            int result = a + b + (carry ? 1 : 0);

            int carryIn = result ^ a ^ b;

            result &= 0xFF;

            StatusFlags flags = FlagsSZP[result];

            flags[H] = (carryIn & 0x10) == 0x10;
            flags[C] = (carryIn & 0x100) == 0x100;

            return ((byte)result, flags);
        }

        // https://stackoverflow.com/questions/8034566/overflow-and-carry-flags-on-z80/8037485#8037485
        private static (byte, StatusFlags) Sub8Bit(byte a, byte b, bool carry = false)
        {
            // a - b - c = a + ~b + 1 - c = a + ~b + !c
            var (result, flags) = Add8Bit(a, (byte)(~b), !carry);

            flags ^= C | H;

            return (result, flags);
        }

        private static (byte, StatusFlags) Or8Bit(byte a, byte b)
        {
            var result = a | b;

            StatusFlags flags = FlagsSZP[result];

            return ((byte)result, flags);
        }

        private static (byte, StatusFlags) Xor8Bit(byte a, byte b)
        {
            var result = a ^ b;

            StatusFlags flags = FlagsSZP[result];

            return ((byte)result, flags);
        }

        private static StatusFlags Compare8Bit(byte a, byte b)
        {
            var (_, flags) = Sub8Bit(a, b);

            return flags;
        }

        private byte Inc8Bit(byte a)
        {
            var (result, flags) = Add8Bit(a, 1);

            _r.F &= C;
            _r.F |= (flags & ~C);

            return result;
        }

        private byte Dec8Bit(byte a)
        {
            var (result, flags) = Sub8Bit(a, 1);

            _r.F &= C;
            _r.F |= (flags & ~C);

            return result;
        }

        #endregion

        #region branch group handlers

        private void JumpImmediate()
        {
            ushort address = ReadWord(_pc);

            if (address == _pc - 1)
            {
                throw new InfiniteLoopException();
            }

            _pc = address;
        }

        private void JumpImmediate(bool condition)
        {
            ushort address = ReadWord(_pc);

            if (condition)
            {
                _pc = address;
            }
            else
            {
                _pc += 2;
            }
        }

        private void CallImmediate()
        {
            ushort address = ReadWord(_pc);
            _pc += 2;

            PushWord(_pc);
            _pc = address;
        }

        private void CallImmediate(bool condition)
        {
            ushort address = ReadWord(_pc);
            _pc += 2;

            if (condition)
            {
                PushWord(_pc);
                _pc = address;
            }
        }

        private void Return()
        {
            _pc = PopWord();
        }

        private void Return(bool condition)
        {
            if (condition)
            {
                _pc = PopWord();
            }
        }

        #endregion
    }
}
