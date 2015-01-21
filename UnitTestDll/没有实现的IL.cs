using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestDll
{
    public class 没有实现的IL
    {
        public static void UnitTest_Break()
        {
            //release模式下运行会崩
            //System.Diagnostics.Debugger.Break();
            //那是因为release模式下不可调用
            //你永远也不可能手工操作到IL的 break指令。
            //调试器断点可以产生，L#不是CLR在执行，没有调试器
        }

        public static void UnitTest_位操作()
        {
            //针对IL的 and or ....
            int a = 1;
            int b = 1;
            int c = 1;
            c = a << b;
            c = a >> b;
            c = 0 | a;
            c = 1 & c;
            c = 1 ^ a;
            c = c % 1;
            c = ~a;

            c |= a;
            c &= a;
            c ^= a;
            c %= a;
        }

        //并不会支持Marshal这种功能，
        //也不会支持任何的unsafe操作
        //public static void UnitTest_sizeof()
        //{
        //    //针对IL的 sizeof?

        //    var a = new CCCCC();
        //    int b = 1;
        //    int c = 1;
        //    //c = sizeof(SSSSSS);
        //    var t = a.GetType();
        //    b = System.Runtime.InteropServices.Marshal.SizeOf(a);
        //    c = System.Runtime.InteropServices.Marshal.SizeOf(a);
        //}

        public static void UnitTest_各种溢出()
        {
            /*
             Conv.Ovf.I 	将位于计算堆栈顶部的有符号值转换为有符号 native int，并在溢出时引发 OverflowException。
Conv.Ovf.I.Un 	将位于计算堆栈顶部的无符号值转换为有符号 native int，并在溢出时引发 OverflowException。
Conv.Ovf.I1 	将位于计算堆栈顶部的有符号值转换为有符号 int8 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I1.Un 	将位于计算堆栈顶部的无符号值转换为有符号 int8 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I2 	将位于计算堆栈顶部的有符号值转换为有符号 int16 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I2.Un 	将位于计算堆栈顶部的无符号值转换为有符号 int16 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I4 	将位于计算堆栈顶部的有符号值转换为有符号 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I4.Un 	将位于计算堆栈顶部的无符号值转换为有符号 int32，并在溢出时引发 OverflowException。
Conv.Ovf.I8 	将位于计算堆栈顶部的有符号值转换为有符号 int64，并在溢出时引发 OverflowException。
Conv.Ovf.I8.Un 	将位于计算堆栈顶部的无符号值转换为有符号 int64，并在溢出时引发 OverflowException。
Conv.Ovf.U 	将位于计算堆栈顶部的有符号值转换为 unsigned native int，并在溢出时引发 OverflowException。
Conv.Ovf.U.Un 	将位于计算堆栈顶部的无符号值转换为 unsigned native int，并在溢出时引发 OverflowException。
Conv.Ovf.U1 	将位于计算堆栈顶部的有符号值转换为 unsigned int8 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.U1.Un 	将位于计算堆栈顶部的无符号值转换为 unsigned int8 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.U2 	将位于计算堆栈顶部的有符号值转换为 unsigned int16 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.U2.Un 	将位于计算堆栈顶部的无符号值转换为 unsigned int16 并将其扩展为 int32，并在溢出时引发 OverflowException。
Conv.Ovf.U4 	将位于计算堆栈顶部的有符号值转换为 unsigned int32，并在溢出时引发 OverflowException。
Conv.Ovf.U4.Un 	将位于计算堆栈顶部的无符号值转换为 unsigned int32，并在溢出时引发 OverflowException。
Conv.Ovf.U8 	将位于计算堆栈顶部的有符号值转换为 unsigned int64，并在溢出时引发 OverflowException。
Conv.Ovf.U8.Un 	将位于计算堆栈顶部的无符号值转换为 unsigned int64，并在溢出时引发 OverflowException。
Conv.R.Un 	将位于计算堆栈顶部的无符号整数值转换为 float32。
Conv.R4 	将位于计算堆栈顶部的值转换为 float32。
Conv.R8 	将位于计算堆栈顶部的值转换为 float64。
Conv.U 	将位于计算堆栈顶部的值转换为 unsigned native int，然后将其扩展为 native int。
Conv.U1 	将位于计算堆栈顶部的值转换为 unsigned int8，然后将其扩展为 int32。
Conv.U2 	将位于计算堆栈顶部的值转换为 unsigned int16，然后将其扩展为 int32。
Conv.U4 	将位于计算堆栈顶部的值转换为 unsigned int32，然后将其扩展为 int32。
Conv.U8 	将位于计算堆栈顶部的值转换为 unsigned int64，然后将其扩展为 int64。
             */
            //unchecked
            //{
            //    long i = 2147483647 + 1;
            //    void* vp = &i;
            //    int* ip;
            //    ip = (int*)vp;
            //    int c = *ip;

            //}


        }
        class CCCCC
        {
            int xx;
        }
        struct SSSSSS
        {
            int xx;
        }
    }
}
