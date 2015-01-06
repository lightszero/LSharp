
//  Microsoft (R) .NET Framework IL Disassembler.  Version 3.5.30729.1



// Classes defined in this module:
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Class Class1                         (public) (auto) (ansi) 
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly t01
{
  .custom instance void [mscorlib]System.Runtime.Versioning.TargetFrameworkAttribute::.ctor(string) = ( 01 00 1A 2E 4E 45 54 46 72 61 6D 65 77 6F 72 6B   // ....NETFramework
                                                                                                        2C 56 65 72 73 69 6F 6E 3D 76 34 2E 30 01 00 54   // ,Version=v4.0..T
                                                                                                        0E 14 46 72 61 6D 65 77 6F 72 6B 44 69 73 70 6C   // ..FrameworkDispl
                                                                                                        61 79 4E 61 6D 65 10 2E 4E 45 54 20 46 72 61 6D   // ayName..NET Fram
                                                                                                        65 77 6F 72 6B 20 34 )                            // ework 4
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 03 74 30 31 00 00 )                         // ...t01..
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 03 74 30 31 00 00 )                         // ...t01..
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                  20 32 30 31 35 00 00 )                            //  2015..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 61 64 34 38 66 65 36 33 2D 31 62 61 31   // ..$ad48fe63-1ba1
                                                                                                  2D 34 39 38 62 2D 38 64 31 64 2D 31 36 39 62 64   // -498b-8d1d-169bd
                                                                                                  35 66 62 61 38 32 34 00 00 )                      // 5fba824..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..

  // --- 下列自定义属性会自动添加，不要取消注释 -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module t01.dll
// MVID: {1891C572-B2A2-4292-A630-154763815231}
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x01090000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit t01.Class1
       extends [mscorlib]System.Object
{
  .method private hidebysig instance void 
          Do001() cil managed
  {
    // 代码大小       32 (0x20)
    .maxstack  2
    .locals init ([0] int32 i,
             [1] bool CS$4$0000)
    .line 11,11 : 9,10 ''
//000011:         {
    IL_0000:  nop
    .line 12,12 : 17,25 ''
//000012:             for(int i=0;i<10;i++)
    IL_0001:  ldc.i4.0
    IL_0002:  stloc.0
    IL_0003:  br.s       IL_0016

    .line 13,13 : 13,14 ''
//000013:             {
    IL_0005:  nop
    .line 14,14 : 17,48 ''
//000014:                 Console.WriteLine("hi there.");
    IL_0006:  ldstr      "hi there."
    IL_000b:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_0010:  nop
    .line 15,15 : 13,14 ''
//000015:             }
    IL_0011:  nop
    .line 12,12 : 30,33 ''
//000012:             for(int i=0;i<10;i++)
    IL_0012:  ldloc.0
    IL_0013:  ldc.i4.1
    IL_0014:  add
    IL_0015:  stloc.0
    .line 12,12 : 25,29 ''
    IL_0016:  ldloc.0
    IL_0017:  ldc.i4.s   10
    IL_0019:  clt
    IL_001b:  stloc.1
    IL_001c:  ldloc.1
    IL_001d:  brtrue.s   IL_0005

    .line 16,16 : 9,10 ''
//000013:             {
//000014:                 Console.WriteLine("hi there.");
//000015:             }
//000016:         }
    IL_001f:  ret
  } // end of method Class1::Do001

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // 代码大小       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method Class1::.ctor

} // end of class t01.Class1


// =============================================================

// *********** 反汇编完成 ***********************
// 警告: 创建了 Win32 资源文件 E:\github\LSharp\t01_l.res
