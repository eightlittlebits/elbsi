﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using elbsi_core;
using elbsi_core.CPU;

namespace elbsi_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // roms\CPUTEST.COM roms\TEST.COM roms\8080PRE.COM roms\8080EXM.COM
                foreach (var file in args)
                {
                    RunTest(file);
                }
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void RunTest(string filename)
        {
            var sw = Stopwatch.StartNew();
            ulong instructionCount = 0;

            // load rom 
            byte[] rom = File.ReadAllBytes(filename);

            Bus bus = new Bus();

            bus.Load(0x0100, rom);

            // patch memory location 5 as a ret instruction
            // call 5 is a call into BDOS, intercepted below
            bus.WriteByte(0x0005, 0xC9);

            // patch in ram size
            bus.WriteByte(0x0006, 0x00);
            bus.WriteByte(0x0007, 0x17);

            I8080 cpu = new I8080(bus)
            {
                PC = 0x100
            };

            Console.WriteLine();
            Console.WriteLine("***** Running rom {0} *****", Path.GetFileName(filename));
            Console.WriteLine();

            while (true)
            {
                ushort pc = cpu.PC;

                // trap any HLT instructions
                if (bus.ReadByte(pc) == 0x76)
                {
                    Console.WriteLine("HLT at 0x{0:X4}", pc);
                    Environment.Exit(0);
                }

                // intercept BDOS calls, http://www.seasip.info/Cpm/bdos.html
                if (pc == 0x0005)
                {
                    switch (cpu.R.C)
                    {
                        case 2: // BDOS function 2 (C_WRITE) - Console output
                        {
                            // Entered with C=2, E=ASCII character.
                            Console.Write((char)cpu.R.E);
                        }
                        break;

                        case 9: // BDOS function 9 (C_WRITESTR) - Output string
                        {
                            // Entered with C=9, DE=address of string.
                            // Display a string of ASCII characters, terminated with the $ character.
                            StringBuilder sb = new StringBuilder();
                            char output;

                            ushort i = cpu.R.DE;
                            while ((output = (char)bus.ReadByte(i++)) != '$')
                            {
                                sb.Append(output);
                            }

                            Console.Write(sb.ToString());
                        }
                        break;
                    }
                }

                instructionCount++;
                cpu.ExecuteInstruction();

                // if we jumped to 0 then end this test
                if (cpu.PC == 0x0000)
                {
                    sw.Stop();

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"Jump to 0x0000 from 0x{pc:X4}, {instructionCount} instructions executed, {sw.ElapsedMilliseconds}ms elapsed, {(double)instructionCount / sw.ElapsedMilliseconds} instructions/ms");

                    return;
                }
            }
        }
    }
}
