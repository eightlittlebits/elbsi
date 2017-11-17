using System;

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

#pragma warning disable CS0414
        private bool _interruptEnabled;
#pragma warning restore CS0414
        private bool _halted;

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
            if (!_halted)
            {
                byte opcode = ReadByte(_pc++);
                ExecuteOpcode(opcode);
            }
        }

        private int ExecuteOpcode(byte opcode)
        {
            int cycles;

            switch (opcode)
            {
                #region data transfer group

                case 0x40: cycles = 5; break; // MOV B,B
                case 0x41: _r.B = _r.C; cycles = 5; break; // MOV B,C
                case 0x42: _r.B = _r.D; cycles = 5; break; // MOV B,D
                case 0x43: _r.B = _r.E; cycles = 5; break; // MOV B,E
                case 0x44: _r.B = _r.H; cycles = 5; break; // MOV B,H
                case 0x45: _r.B = _r.L; cycles = 5; break; // MOV B,L
                case 0x46: _r.B = ReadByte(_r.HL); cycles = 7; break; // MOV B,M
                case 0x47: _r.B = _r.A; cycles = 5; break; // MOV B,A

                case 0x48: _r.C = _r.B; cycles = 5; break; // MOV C,B
                case 0x49: cycles = 5; break; // MOV C,C
                case 0x4A: _r.C = _r.D; cycles = 5; break; // MOV C,D
                case 0x4B: _r.C = _r.E; cycles = 5; break; // MOV C,E
                case 0x4C: _r.C = _r.H; cycles = 5; break; // MOV C,H
                case 0x4D: _r.C = _r.L; cycles = 5; break; // MOV C,L
                case 0x4E: _r.C = ReadByte(_r.HL); cycles = 7; break; // MOV C,M
                case 0x4F: _r.C = _r.A; cycles = 5; break; // MOV C,A

                case 0x50: _r.D = _r.B; cycles = 5; break; // MOV D,B
                case 0x51: _r.D = _r.C; cycles = 5; break; // MOV D,C
                case 0x52: cycles = 5; break; // MOV D,D
                case 0x53: _r.D = _r.E; cycles = 5; break; // MOV D,E
                case 0x54: _r.D = _r.H; cycles = 5; break; // MOV D,H
                case 0x55: _r.D = _r.L; cycles = 5; break; // MOV D,L
                case 0x56: _r.D = ReadByte(_r.HL); cycles = 7; break; // MOV D,M
                case 0x57: _r.D = _r.A; cycles = 5; break; // MOV D,A

                case 0x58: _r.E = _r.B; cycles = 5; break; // MOV E,B
                case 0x59: _r.E = _r.C; cycles = 5; break; // MOV E,C
                case 0x5A: _r.E = _r.D; cycles = 5; break; // MOV E,D
                case 0x5B: cycles = 5; break; // MOV E,E
                case 0x5C: _r.E = _r.H; cycles = 5; break; // MOV E,H
                case 0x5D: _r.E = _r.L; cycles = 5; break; // MOV E,L
                case 0x5E: _r.E = ReadByte(_r.HL); cycles = 7; break; // MOV E,M
                case 0x5F: _r.E = _r.A; cycles = 5; break; // MOV E,A

                case 0x60: _r.H = _r.B; cycles = 5; break; // MOV H,B
                case 0x61: _r.H = _r.C; cycles = 5; break; // MOV H,C
                case 0x62: _r.H = _r.D; cycles = 5; break; // MOV H,D
                case 0x63: _r.H = _r.E; cycles = 5; break; // MOV H,E
                case 0x64: cycles = 5; break; // MOV H,H
                case 0x65: _r.H = _r.L; cycles = 5; break; // MOV H,L
                case 0x66: _r.H = ReadByte(_r.HL); cycles = 7; break; // MOV H,M
                case 0x67: _r.H = _r.A; cycles = 5; break; // MOV H,A

                case 0x68: _r.L = _r.B; cycles = 5; break; // MOV L,B
                case 0x69: _r.L = _r.C; cycles = 5; break; // MOV L,C
                case 0x6A: _r.L = _r.D; cycles = 5; break; // MOV L,D
                case 0x6B: _r.L = _r.E; cycles = 5; break; // MOV L,E
                case 0x6C: _r.L = _r.H; cycles = 5; break; // MOV L,H
                case 0x6D: cycles = 5; break; // MOV L,L
                case 0x6E: _r.L = ReadByte(_r.HL); cycles = 7; break; // MOV L,M
                case 0x6F: _r.L = _r.A; cycles = 5; break; // MOV L,A

                case 0x70: WriteByte(_r.HL, _r.B); cycles = 7; break; // MOV M,B
                case 0x71: WriteByte(_r.HL, _r.C); cycles = 7; break; // MOV M,C
                case 0x72: WriteByte(_r.HL, _r.D); cycles = 7; break; // MOV M,D
                case 0x73: WriteByte(_r.HL, _r.E); cycles = 7; break; // MOV M,E
                case 0x74: WriteByte(_r.HL, _r.H); cycles = 7; break; // MOV M,H
                case 0x75: WriteByte(_r.HL, _r.L); cycles = 7; break; // MOV M,L
                case 0x77: WriteByte(_r.HL, _r.A); cycles = 7; break; // MOV M,A

                case 0x78: _r.A = _r.B; cycles = 5; break; // MOV A,B
                case 0x79: _r.A = _r.C; cycles = 5; break; // MOV A,C
                case 0x7A: _r.A = _r.D; cycles = 5; break; // MOV A,D
                case 0x7B: _r.A = _r.E; cycles = 5; break; // MOV A,E
                case 0x7C: _r.A = _r.H; cycles = 5; break; // MOV A,H
                case 0x7D: _r.A = _r.L; cycles = 5; break; // MOV A,L
                case 0x7E: _r.A = ReadByte(_r.HL); cycles = 7; break; // MOV A,M
                case 0x7F: cycles = 5; break; // MOV A,A

                case 0x06: _r.B = ReadByte(_pc++); cycles = 7; break; // MVI B,d8
                case 0x0E: _r.C = ReadByte(_pc++); cycles = 7; break; // MVI C,d8
                case 0x16: _r.D = ReadByte(_pc++); cycles = 7; break; // MVI D,d8
                case 0x1E: _r.E = ReadByte(_pc++); cycles = 7; break; // MVI E,d8
                case 0x26: _r.H = ReadByte(_pc++); cycles = 7; break; // MVI C,d8
                case 0x2E: _r.L = ReadByte(_pc++); cycles = 7; break; // MVI L,d8
                case 0x36: WriteByte(_r.HL, ReadByte(_pc++)); cycles = 10; break; // MVI M,d8
                case 0x3E: _r.A = ReadByte(_pc++); cycles = 7; break; // MVI A,d8

                case 0x3A: _r.A = ReadByte(ReadWord(_pc)); _pc += 2; cycles = 13; break; // LDA a16
                case 0x32: WriteByte(ReadWord(_pc), _r.A); _pc += 2; cycles = 13; break; // STA a16

                case 0x2A: _r.HL = ReadWord(ReadWord(_pc)); _pc += 2; cycles = 16; break; // LHLD
                case 0x22: WriteWord(ReadWord(_pc), _r.HL); _pc += 2; cycles = 16; break; // SHLD

                case 0x01: _r.BC = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI B,a16
                case 0x11: _r.DE = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI D,a16
                case 0x21: _r.HL = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI H,a16
                case 0x31: _sp = ReadWord(_pc); _pc += 2; cycles = 10; break; // LXI SP,d16

                case 0x0A: _r.A = ReadByte(_r.BC); cycles = 7; break; // LDAX B
                case 0x1A: _r.A = ReadByte(_r.DE); cycles = 7; break; // LDAX D

                case 0x02: WriteByte(_r.BC, _r.A); cycles = 7; break; // STAX B
                case 0x12: WriteByte(_r.DE, _r.A); cycles = 7; break; // STAX D

                case 0xEB: { ushort temp = _r.DE; _r.DE = _r.HL; _r.HL = temp; } cycles = 5; break; // XCHG;
                case 0xE3: { ushort temp = ReadWord(_sp); WriteWord(_sp, _r.HL); _r.HL = temp; } cycles = 18; break; // XTHL

                #endregion

                #region arithmetic group

                case 0x80: (_r.A, _r.F) = Add8Bit(_r.A, _r.B); cycles = 5; break; // ADD B
                case 0x81: (_r.A, _r.F) = Add8Bit(_r.A, _r.C); cycles = 5; break; // ADD C
                case 0x82: (_r.A, _r.F) = Add8Bit(_r.A, _r.D); cycles = 5; break; // ADD D
                case 0x83: (_r.A, _r.F) = Add8Bit(_r.A, _r.E); cycles = 5; break; // ADD E
                case 0x84: (_r.A, _r.F) = Add8Bit(_r.A, _r.H); cycles = 5; break; // ADD H
                case 0x85: (_r.A, _r.F) = Add8Bit(_r.A, _r.L); cycles = 5; break; // ADD L
                case 0x86: (_r.A, _r.F) = Add8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // ADD M
                case 0x87: (_r.A, _r.F) = Add8Bit(_r.A, _r.A); cycles = 5; break; // ADD A

                case 0xC6: (_r.A, _r.F) = Add8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // ADI d8

                case 0x88: (_r.A, _r.F) = Add8Bit(_r.A, _r.B, _r.F[C]); cycles = 5; break; // ADC B
                case 0x89: (_r.A, _r.F) = Add8Bit(_r.A, _r.C, _r.F[C]); cycles = 5; break; // ADC C
                case 0x8A: (_r.A, _r.F) = Add8Bit(_r.A, _r.D, _r.F[C]); cycles = 5; break; // ADC D
                case 0x8B: (_r.A, _r.F) = Add8Bit(_r.A, _r.E, _r.F[C]); cycles = 5; break; // ADC E
                case 0x8C: (_r.A, _r.F) = Add8Bit(_r.A, _r.H, _r.F[C]); cycles = 5; break; // ADC H
                case 0x8D: (_r.A, _r.F) = Add8Bit(_r.A, _r.L, _r.F[C]); cycles = 5; break; // ADC L
                case 0x8E: (_r.A, _r.F) = Add8Bit(_r.A, ReadByte(_r.HL), _r.F[C]); cycles = 7; break; // ADC M
                case 0x8F: (_r.A, _r.F) = Add8Bit(_r.A, _r.A, _r.F[C]); cycles = 5; break; // ADC A

                case 0xCE: (_r.A, _r.F) = Add8Bit(_r.A, ReadByte(_pc++), _r.F[C]); cycles = 7; break; // ACI d8

                case 0x90: (_r.A, _r.F) = Sub8Bit(_r.A, _r.B); cycles = 5; break; // SUB B
                case 0x91: (_r.A, _r.F) = Sub8Bit(_r.A, _r.C); cycles = 5; break; // SUB C
                case 0x92: (_r.A, _r.F) = Sub8Bit(_r.A, _r.D); cycles = 5; break; // SUB D
                case 0x93: (_r.A, _r.F) = Sub8Bit(_r.A, _r.E); cycles = 5; break; // SUB E
                case 0x94: (_r.A, _r.F) = Sub8Bit(_r.A, _r.H); cycles = 5; break; // SUB H
                case 0x95: (_r.A, _r.F) = Sub8Bit(_r.A, _r.L); cycles = 5; break; // SUB L
                case 0x96: (_r.A, _r.F) = Sub8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // SUB M
                case 0x97: (_r.A, _r.F) = Sub8Bit(_r.A, _r.A); cycles = 5; break; // SUB A

                case 0xD6: (_r.A, _r.F) = Sub8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // SUI d8

                case 0x98: (_r.A, _r.F) = Sub8Bit(_r.A, _r.B, _r.F[C]); cycles = 5; break; // SBB B
                case 0x99: (_r.A, _r.F) = Sub8Bit(_r.A, _r.C, _r.F[C]); cycles = 5; break; // SBB C
                case 0x9A: (_r.A, _r.F) = Sub8Bit(_r.A, _r.D, _r.F[C]); cycles = 5; break; // SBB D
                case 0x9B: (_r.A, _r.F) = Sub8Bit(_r.A, _r.E, _r.F[C]); cycles = 5; break; // SBB E
                case 0x9C: (_r.A, _r.F) = Sub8Bit(_r.A, _r.H, _r.F[C]); cycles = 5; break; // SBB H
                case 0x9D: (_r.A, _r.F) = Sub8Bit(_r.A, _r.L, _r.F[C]); cycles = 5; break; // SBB L
                case 0x9E: (_r.A, _r.F) = Sub8Bit(_r.A, ReadByte(_r.HL), _r.F[C]); cycles = 7; break; // SBB M
                case 0x9F: (_r.A, _r.F) = Sub8Bit(_r.A, _r.A, _r.F[C]); cycles = 5; break; // SBB A 

                case 0xDE: (_r.A, _r.F) = Sub8Bit(_r.A, ReadByte(_pc++), _r.F[C]); cycles = 7; break; // SBI d8

                case 0x04: _r.B = Inc8Bit(_r.B); cycles = 5; break; // INR B
                case 0x0C: _r.C = Inc8Bit(_r.C); cycles = 5; break; // INR C
                case 0x14: _r.D = Inc8Bit(_r.D); cycles = 5; break; // INR D
                case 0x1C: _r.E = Inc8Bit(_r.E); cycles = 5; break; // INR E
                case 0x24: _r.H = Inc8Bit(_r.H); cycles = 5; break; // INR H
                case 0x2C: _r.L = Inc8Bit(_r.L); cycles = 5; break; // INR L
                case 0x34: WriteByte(_r.HL, Inc8Bit(ReadByte(_r.HL))); cycles = 10; break; // INR M
                case 0x3C: _r.A = Inc8Bit(_r.A); cycles = 5; break; // INR A

                case 0x05: _r.B = Dec8Bit(_r.B); cycles = 5; break; // DCR B
                case 0x0D: _r.C = Dec8Bit(_r.C); cycles = 5; break; // DCR C
                case 0x15: _r.D = Dec8Bit(_r.D); cycles = 5; break; // DCR D
                case 0x1D: _r.E = Dec8Bit(_r.E); cycles = 5; break; // DCR E
                case 0x25: _r.H = Dec8Bit(_r.H); cycles = 5; break; // DCR H
                case 0x2D: _r.L = Dec8Bit(_r.L); cycles = 5; break; // DCR L
                case 0x35: WriteByte(_r.HL, Dec8Bit(ReadByte(_r.HL))); cycles = 10; break; // DCR M
                case 0x3D: _r.A = Dec8Bit(_r.A); cycles = 5; break; // DCR A

                case 0x03: _r.BC++; cycles = 5; break; // INX B
                case 0x13: _r.DE++; cycles = 5; break; // INX D
                case 0x23: _r.HL++; cycles = 5; break; // INX H
                case 0x33: _sp++; cycles = 5; break; // INX SP

                case 0x0B: _r.BC--; cycles = 5; break; // DCX B
                case 0x1B: _r.DE--; cycles = 5; break; // DCX D
                case 0x2B: _r.HL--; cycles = 5; break; // DCX H
                case 0x3B: _sp--; cycles = 5; break; // DCX SP

                case 0x09: _r.HL = Add16Bit(_r.HL, _r.BC); cycles = 10; break; // DAD B
                case 0x19: _r.HL = Add16Bit(_r.HL, _r.DE); cycles = 10; break; // DAD D
                case 0x29: _r.HL = Add16Bit(_r.HL, _r.HL); cycles = 10; break; // DAD H
                case 0x39: _r.HL = Add16Bit(_r.HL, _sp); cycles = 10; break; // DAD SP

                case 0x27: DecimalAdjustAccumulator(); cycles = 4; break; // DAA

                #endregion

                #region logical group

                case 0xA0: (_r.A, _r.F) = And8Bit(_r.A, _r.B); cycles = 5; break; // ANA B
                case 0xA1: (_r.A, _r.F) = And8Bit(_r.A, _r.C); cycles = 5; break; // ANA C
                case 0xA2: (_r.A, _r.F) = And8Bit(_r.A, _r.D); cycles = 5; break; // ANA D
                case 0xA3: (_r.A, _r.F) = And8Bit(_r.A, _r.E); cycles = 5; break; // ANA E
                case 0xA4: (_r.A, _r.F) = And8Bit(_r.A, _r.H); cycles = 5; break; // ANA H
                case 0xA5: (_r.A, _r.F) = And8Bit(_r.A, _r.L); cycles = 5; break; // ANA L
                case 0xA6: (_r.A, _r.F) = And8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // ANA M
                case 0xA7: (_r.A, _r.F) = And8Bit(_r.A, _r.A); cycles = 5; break; // ANA A

                case 0xE6: (_r.A, _r.F) = And8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // ANI d8

                case 0xB0: (_r.A, _r.F) = Or8Bit(_r.A, _r.B); cycles = 5; break; // ORA B
                case 0xB1: (_r.A, _r.F) = Or8Bit(_r.A, _r.C); cycles = 5; break; // ORA C
                case 0xB2: (_r.A, _r.F) = Or8Bit(_r.A, _r.D); cycles = 5; break; // ORA D
                case 0xB3: (_r.A, _r.F) = Or8Bit(_r.A, _r.E); cycles = 5; break; // ORA E
                case 0xB4: (_r.A, _r.F) = Or8Bit(_r.A, _r.H); cycles = 5; break; // ORA H
                case 0xB5: (_r.A, _r.F) = Or8Bit(_r.A, _r.L); cycles = 5; break; // ORA L
                case 0xB6: (_r.A, _r.F) = Or8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // ORA M
                case 0xB7: (_r.A, _r.F) = Or8Bit(_r.A, _r.A); cycles = 5; break; // ORA A

                case 0xF6: (_r.A, _r.F) = Or8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // ORI d8

                case 0xA8: (_r.A, _r.F) = Xor8Bit(_r.A, _r.B); cycles = 5; break; // XRA B
                case 0xA9: (_r.A, _r.F) = Xor8Bit(_r.A, _r.C); cycles = 5; break; // XRA C
                case 0xAA: (_r.A, _r.F) = Xor8Bit(_r.A, _r.D); cycles = 5; break; // XRA D
                case 0xAB: (_r.A, _r.F) = Xor8Bit(_r.A, _r.E); cycles = 5; break; // XRA E
                case 0xAC: (_r.A, _r.F) = Xor8Bit(_r.A, _r.H); cycles = 5; break; // XRA H
                case 0xAD: (_r.A, _r.F) = Xor8Bit(_r.A, _r.L); cycles = 5; break; // XRA L
                case 0xAE: (_r.A, _r.F) = Xor8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // XRA M
                case 0xAF: (_r.A, _r.F) = Xor8Bit(_r.A, _r.A); cycles = 5; break; // XRA A

                case 0xEE: (_r.A, _r.F) = Xor8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // XRI d8

                case 0xB8: _r.F = Compare8Bit(_r.A, _r.B); cycles = 5; break; // CMP B
                case 0xB9: _r.F = Compare8Bit(_r.A, _r.C); cycles = 5; break; // CMP C
                case 0xBA: _r.F = Compare8Bit(_r.A, _r.D); cycles = 5; break; // CMP D
                case 0xBB: _r.F = Compare8Bit(_r.A, _r.E); cycles = 5; break; // CMP E
                case 0xBC: _r.F = Compare8Bit(_r.A, _r.H); cycles = 5; break; // CMP H
                case 0xBD: _r.F = Compare8Bit(_r.A, _r.L); cycles = 5; break; // CMP L
                case 0xBE: _r.F = Compare8Bit(_r.A, ReadByte(_r.HL)); cycles = 7; break; // CMP M
                case 0xBF: _r.F = Compare8Bit(_r.A, _r.A); cycles = 5; break; // CMP A

                case 0xFE: _r.F = Compare8Bit(_r.A, ReadByte(_pc++)); cycles = 7; break; // CPI d8

                case 0x07: _r.A = RotateLeft(_r.A); cycles = 4; break; // RLC
                case 0x0F: _r.A = RotateRight(_r.A); cycles = 4; break; // RRC
                case 0x17: _r.A = RotateLeftThroughCarry(_r.A); cycles = 4; break; // RAL
                case 0x1F: _r.A = RotateRightThroughCarry(_r.A); cycles = 4; break; // RAR

                case 0x2F: _r.A = (byte)~_r.A; cycles = 4; break; // CMA
                case 0x3F: _r.F ^= C; cycles = 4; break; // CMC
                case 0x37: _r.F[C] = true; cycles = 4; break; // STC

                #endregion

                #region branch group

                case 0xC3: JumpImmediate(); cycles = 10; break; // JMP a16

                case 0xDA: JumpImmediate(_r.F[C]); cycles = 10; break; // JC a16
                case 0xD2: JumpImmediate(!_r.F[C]); cycles = 10; break; // JNC a16
                case 0xCA: JumpImmediate(_r.F[Z]); cycles = 10; break; // JZ a16
                case 0xC2: JumpImmediate(!_r.F[Z]); cycles = 10; break; // JNZ a16
                case 0xF2: JumpImmediate(!_r.F[S]); cycles = 10; break; // JP a16
                case 0xFA: JumpImmediate(_r.F[S]); cycles = 10; break; // JM a16
                case 0xEA: JumpImmediate(_r.F[P]); cycles = 10; break; // JPE a16
                case 0xE2: JumpImmediate(!_r.F[P]); cycles = 10; break; // JPO a16

                case 0xCD: CallImmediate(); cycles = 17; break; // CALL a16

                case 0xDC: cycles = CallImmediate(_r.F[C]); break; // CC a16
                case 0xD4: cycles = CallImmediate(!_r.F[C]); break; // CNC a16
                case 0xCC: cycles = CallImmediate(_r.F[Z]); break; // CZ a16
                case 0xC4: cycles = CallImmediate(!_r.F[Z]); break; // CNZ a16
                case 0xF4: cycles = CallImmediate(!_r.F[S]); break; // CP a16
                case 0xFC: cycles = CallImmediate(_r.F[S]); break; // CM a16
                case 0xEC: cycles = CallImmediate(_r.F[P]); break; // CPE a16
                case 0xE4: cycles = CallImmediate(!_r.F[P]); break; // CPO a16

                case 0xC9: Return(); cycles = 10; break; // RET

                case 0xD8: cycles = Return(_r.F[C]); break; // RC a16
                case 0xD0: cycles = Return(!_r.F[C]); break; // RNC a16
                case 0xC8: cycles = Return(_r.F[Z]); break; // RZ
                case 0xC0: cycles = Return(!_r.F[Z]); break; // RNZ
                case 0xF0: cycles = Return(!_r.F[S]); break; // RP
                case 0xF8: cycles = Return(_r.F[S]); break; // RM
                case 0xE8: cycles = Return(_r.F[P]); break; // RPE
                case 0xE0: cycles = Return(!_r.F[P]); break; // RPO

                case 0xE9: _pc = _r.HL; cycles = 5; break; // PCHL

                case 0xC7: Reset(0x00); cycles = 11; break; // RST 0
                case 0xCF: Reset(0x08); cycles = 11; break; // RST 1
                case 0xD7: Reset(0x10); cycles = 11; break; // RST 2
                case 0xDF: Reset(0x18); cycles = 11; break; // RST 3
                case 0xE7: Reset(0x20); cycles = 11; break; // RST 4
                case 0xEF: Reset(0x28); cycles = 11; break; // RST 5
                case 0xF7: Reset(0x30); cycles = 11; break; // RST 6
                case 0xFF: Reset(0x38); cycles = 11; break; // RST 7

                #endregion

                case 0x00: cycles = 4; break; // NOP
                case 0x08: cycles = 4; break; // *NOP
                case 0x10: cycles = 4; break; // *NOP
                case 0x18: cycles = 4; break; // *NOP
                case 0x20: cycles = 4; break; // *NOP
                case 0x28: cycles = 4; break; // *NOP
                case 0x30: cycles = 4; break; // *NOP
                case 0x38: cycles = 4; break; // *NOP
                case 0x76: _halted = true; cycles = 7; break; // HLT
                case 0xC1: _r.BC = PopWord(); cycles = 10; break; // POP B
                case 0xC5: PushWord(_r.BC); cycles = 11; break; // PUSH B

                case 0xCB: goto default;

                case 0xD1: _r.DE = PopWord(); cycles = 10; break; // POP D
                case 0xD3: goto default;
                case 0xD5: PushWord(_r.DE); cycles = 11; break; // PUSH D
                case 0xD9: goto default;
                case 0xDB: goto default;
                case 0xDD: goto default;

                case 0xE1: _r.HL = PopWord(); cycles = 10; break; // POP H

                case 0xE5: PushWord(_r.HL); cycles = 11; break; // PUSH H

                case 0xED: goto default;

                case 0xF1: _r.PSW = PopWord(); cycles = 10; break; // POP PSW
                case 0xF3: _interruptEnabled = false; cycles = 4; break; // DI
                case 0xF5: PushWord(_r.PSW); cycles = 11; break; // PUSH PSW

                case 0xF9: _sp = _r.HL; cycles = 5; break; // SPHL
                case 0xFB: _interruptEnabled = true; cycles = 4; break; // EI
                case 0xFD: goto default;


                default: throw new NotImplementedException($"Unimplemented opcode: 0x{opcode:X2} at address 0x{_pc - 1:X4}");
            }

            return cycles;
        }

        #region arithmetic group handlers

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

            flags ^= C;

            return (result, flags);
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

        private ushort Add16Bit(ushort a, ushort b)
        {
            byte hi, lo;
            StatusFlags flags;

            (lo, flags) = Add8Bit((byte)(a >> 0), (byte)(b >> 0));

            (hi, flags) = Add8Bit((byte)(a >> 8), (byte)(b >> 8), flags[C]);

            _r.F[C] = flags[C];

            ushort result = (ushort)(hi << 8 | lo);

            return result;
        }

        private void DecimalAdjustAccumulator()
        {
            bool carry = false;
            byte correctionFactor = 0;

            if ((_r.A & 0x0f) > 0x09 || _r.F[H])
            {
                correctionFactor |= 0x06;
            }

            if ((_r.A) > 0x99 || _r.F[C])
            {
                correctionFactor |= 0x60;
                carry = true;
            }

            (_r.A, _r.F) = Add8Bit(_r.A, correctionFactor);

            _r.F[C] = carry;
        }

        #endregion

        #region logical group handlers

        private static (byte, StatusFlags) And8Bit(byte a, byte b)
        {
            int result = a & b;

            StatusFlags flags = FlagsSZP[result] | ((a | b) & H);

            return ((byte)result, flags);
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

        private byte RotateLeft(byte b)
        {
            // grab bit 7
            bool bit7 = ((b & 0x80) == 0x80);

            // set carry flag if old bit 7 was set
            _r.F[C] = bit7;

            // shift left and place old bit 7 in LSB
            byte result = (byte)((b << 1) | (bit7 ? 1 : 0));

            return result;
        }

        private byte RotateLeftThroughCarry(byte b)
        {
            bool oldCarry = _r.F[C];

            // set carry if top bit is set
            _r.F[C] = (b & 0x80) == 0x80;

            // shift left and place old carry in LSB
            byte result = (byte)((b << 1) | (oldCarry ? 1 : 0));

            return result;
        }

        private byte RotateRight(byte b)
        {
            // grab bit 0
            bool bit0 = (b & 0x01) == 0x01;

            // set carry flag if old bit 0 was set
            _r.F[C] = bit0;

            // shift right and place old bit 0 in MSB
            byte result = (byte)((b >> 1) | (bit0 ? 0x80 : 0));

            return result;
        }

        private byte RotateRightThroughCarry(byte b)
        {
            bool oldCarry = _r.F[C];

            // set carry if bottom bit is set
            _r.F[C] = ((b & 0x01) == 0x01);

            // shift left and place old carry in MSB
            byte result = (byte)((b >> 1) | (oldCarry ? 0x80 : 0));

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

        private int CallImmediate(bool condition)
        {
            ushort address = ReadWord(_pc);
            _pc += 2;

            if (condition)
            {
                PushWord(_pc);
                _pc = address;
                return 17;
            }

            return 11;
        }

        private void Return()
        {
            _pc = PopWord();
        }

        private int Return(bool condition)
        {
            if (condition)
            {
                _pc = PopWord();
                return 11;
            }

            return 5;
        }

        private void Reset(byte resetAddress)
        {
            PushWord(_pc);

            _pc = resetAddress;
        }

        #endregion
    }
}
