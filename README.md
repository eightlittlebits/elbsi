# elbsi - [![Build status](https://ci.appveyor.com/api/projects/status/g74hgca5424in1j8?svg=true)](https://ci.appveyor.com/project/eightlittlebits/elbsi)

## what

A Space Invaders emulator in C#

<p align="center">
  <img src="https://eightlittlebits.github.io/elbsi/images/titlescreen.png" title="Title Screen" alt="Space Invaders Title Screen" />
  <img src="https://eightlittlebits.github.io/elbsi/images/abduction.png" title="Where's he going with that Y?" alt="Space invader stealing the Y on the title screen" />
</p>
<p align="center">
  <img src="https://eightlittlebits.github.io/elbsi/images/ingame.png" title="In game" alt="Space Invaders in game screenshot" />
  <img src="https://eightlittlebits.github.io/elbsi/images/death.png" title="DEATH!" alt="Player ship being destroyed" />
</p>

## why

Aren't there already many space invaders emulators out there? Yup. Are they 
better than this one? Probably

I didn't really intend on creating an emulator for space invaders, I was 
helping someone in the /r/emudev slack (now the [/r/emudev discord](https://discord.gg/dkmJAes))
with questions that they had about the Intel 8080.

That turned into implementing parts of the i8080, which then turned into this.

## test results

### CPUTEST.COM - DIAGNOSTICS II V1.2 - CPU TEST

	DIAGNOSTICS II V1.2 - CPU TEST
	COPYRIGHT (C) 1981 - SUPERSOFT ASSOCIATES

	ABCDEFGHIJKLMNOPQRSTUVWXYZ
	CPU IS 8080/8085
	BEGIN TIMING TEST
	END TIMING TEST
	CPU TESTS OK

### TEST.COM - MICROCOSM ASSOCIATES 8080/8085 CPU DIAGNOSTIC VERSION 1.0  (C) 1980

	MICROCOSM ASSOCIATES 8080/8085 CPU DIAGNOSTIC VERSION 1.0  (C) 1980

	CPU IS OPERATIONAL

### 8080PRE.COM

	8080 Preliminary tests complete

### 8080EXM.COM - 8080 instruction exerciser

	8080 instruction exerciser
	dad <b,d,h,sp>................  PASS! crc is:14474ba6
	aluop nn......................  PASS! crc is:9e922f9e
	aluop <b,c,d,e,h,l,m,a>.......  PASS! crc is:cf762c86
	<daa,cma,stc,cmc>.............  PASS! crc is:bb3f030c
	<inr,dcr> a...................  PASS! crc is:adb6460e
	<inr,dcr> b...................  PASS! crc is:83ed1345
	<inx,dcx> b...................  PASS! crc is:f79287cd
	<inr,dcr> c...................  PASS! crc is:e5f6721b
	<inr,dcr> d...................  PASS! crc is:15b5579a
	<inx,dcx> d...................  PASS! crc is:7f4e2501
	<inr,dcr> e...................  PASS! crc is:cf2ab396
	<inr,dcr> h...................  PASS! crc is:12b2952c
	<inx,dcx> h...................  PASS! crc is:9f2b23c0
	<inr,dcr> l...................  PASS! crc is:ff57d356
	<inr,dcr> m...................  PASS! crc is:92e963bd
	<inx,dcx> sp..................  PASS! crc is:d5702fab
	lhld nnnn.....................  PASS! crc is:a9c3d5cb
	shld nnnn.....................  PASS! crc is:e8864f26
	lxi <b,d,h,sp>,nnnn...........  PASS! crc is:fcf46e12
	ldax <b,d>....................  PASS! crc is:2b821d5f
	mvi <b,c,d,e,h,l,m,a>,nn......  PASS! crc is:eaa72044
	mov <bcdehla>,<bcdehla>.......  PASS! crc is:10b58cee
	sta nnnn / lda nnnn...........  PASS! crc is:ed57af72
	<rlc,rrc,ral,rar>.............  PASS! crc is:e0d89235
	stax <b,d>....................  PASS! crc is:2b0471e9
	Tests complete

## resources

* [emulator 101](http://emulator101.com/)
* [Intel 8080 Assembly Language Programming Manual](http://altairclone.com/downloads/manuals/8080%20Programmers%20Manual.pdf)

## license

MIT License

Copyright (c) 2018-2019 David Parrott

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
