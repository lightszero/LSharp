using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    class Math
    {
        static Dictionary<Type, NumberType> typecode = null;
        public static NumberType GetTypeCode(Type type)
        {
            if (typecode == null)
            {
                typecode = new Dictionary<Type, NumberType>();
                //typecode[null] = 0;
                typecode[typeof(sbyte)] = NumberType.SBYTE;
                typecode[typeof(byte)] = NumberType.BYTE;
                typecode[typeof(Int16)] = NumberType.INT16;
                typecode[typeof(UInt16)] = NumberType.UINT16;
                typecode[typeof(Int32)] = NumberType.INT32;
                typecode[typeof(UInt32)] = NumberType.UINT32;
                typecode[typeof(Int64)] = NumberType.INT64;
                typecode[typeof(UInt64)] = NumberType.UINT64;
                typecode[typeof(float)] = NumberType.FLOAT;
                typecode[typeof(double)] = NumberType.DOUBLE;
                typecode[typeof(IntPtr)] = NumberType.INTPTR;
                typecode[typeof(UIntPtr)] = NumberType.UINTPTR;
                typecode[typeof(decimal)] = NumberType.DECIMAL;
            }
            NumberType t= NumberType.IsNotNumber;
            typecode.TryGetValue(type, out t);
            return t;
        }
        public static int GetNumTypeCode(object obj)
        {

            if (obj == null)
                return (int)NumberType.IsNotNumber;
            else

                return (int)GetTypeCode(obj.GetType());
        }
        public static object Add(object left, object right)
        {
            int lt = GetNumTypeCode(left);
            int rt = GetNumTypeCode(right);
            switch (lt)
            {
                case 1:
                    switch (rt)
                    {
                        case 1: return (sbyte)left + (sbyte)right;
                        case 2: return (sbyte)left + (byte)right;
                        case 3: return (sbyte)left + (Int16)right;
                        case 4: return (sbyte)left + (UInt16)right;
                        case 5: return (sbyte)left + (Int32)right;
                        case 6: return (sbyte)left + (UInt32)right;
                        case 7: return (sbyte)left + (Int64)right;
                        //case 8: return (sbyte)left + (UInt64)right;
                        case 9: return (sbyte)left + (float)right;
                        case 10: return (sbyte)left + (double)right;
                        default: return (byte)left + Convert.ToDecimal(right);
                    }
                case 2:
                    switch (rt)
                    {
                        case 1: return (byte)left + (sbyte)right;
                        case 2: return (byte)left + (byte)right;
                        case 3: return (byte)left + (Int16)right;
                        case 4: return (byte)left + (UInt16)right;
                        case 5: return (byte)left + (Int32)right;
                        case 6: return (byte)left + (UInt32)right;
                        case 7: return (byte)left + (Int64)right;
                        case 8: return (byte)left + (UInt64)right;
                        case 9: return (byte)left + (float)right;
                        case 10: return (byte)left + (double)right;
                        default: return (byte)left + Convert.ToDecimal(right);
                    }
                case 3:
                    switch (rt)
                    {
                        case 1: return (Int16)left + (sbyte)right;
                        case 2: return (Int16)left + (byte)right;
                        case 3: return (Int16)left + (Int16)right;
                        case 4: return (Int16)left + (UInt16)right;
                        case 5: return (Int16)left + (Int32)right;
                        case 6: return (Int16)left + (UInt32)right;
                        case 7: return (Int16)left + (Int64)right;
                        //case 8: return (Int16)left + (UInt64)right;
                        case 9: return (Int16)left + (float)right;
                        case 10: return (Int16)left + (double)right;
                        default: return (Int16)left + Convert.ToDecimal(right);
                    }
                case 4:
                    switch (rt)
                    {
                        case 1: return (UInt16)left + (sbyte)right;
                        case 2: return (UInt16)left + (byte)right;
                        case 3: return (UInt16)left + (Int16)right;
                        case 4: return (UInt16)left + (UInt16)right;
                        case 5: return (UInt16)left + (Int32)right;
                        case 6: return (UInt16)left + (UInt32)right;
                        case 7: return (UInt16)left + (Int64)right;
                        case 8: return (UInt16)left + (UInt64)right;
                        case 9: return (UInt16)left + (float)right;
                        case 10: return (UInt16)left + (double)right;
                        default: return (UInt16)left + Convert.ToDecimal(right);
                    }
                case 5:
                    switch (rt)
                    {
                        case 1: return (Int32)left + (sbyte)right;
                        case 2: return (Int32)left + (byte)right;
                        case 3: return (Int32)left + (Int16)right;
                        case 4: return (Int32)left + (UInt16)right;
                        case 5: return (Int32)left + (Int32)right;
                        case 6: return (Int32)left + (UInt32)right;
                        case 7: return (Int32)left + (Int64)right;
                        //case 8: return (Int32)left + (UInt64)right;
                        case 9: return (Int32)left + (float)right;
                        case 10: return (Int32)left + (double)right;
                        default: return (Int32)left + Convert.ToDecimal(right);
                    }
                case 6:
                    switch (rt)
                    {
                        case 1: return (UInt32)left + (sbyte)right;
                        case 2: return (UInt32)left + (byte)right;
                        case 3: return (UInt32)left + (Int16)right;
                        case 4: return (UInt32)left + (UInt16)right;
                        case 5: return (UInt32)left + (Int32)right;
                        case 6: return (UInt32)left + (UInt32)right;
                        case 7: return (UInt32)left + (Int64)right;
                        case 8: return (UInt32)left + (UInt64)right;
                        case 9: return (UInt32)left + (float)right;
                        case 10: return (UInt32)left + (double)right;
                        default: return (UInt32)left + Convert.ToDecimal(right);
                    }
                case 7:
                    switch (rt)
                    {
                        case 1: return (Int64)left + (sbyte)right;
                        case 2: return (Int64)left + (byte)right;
                        case 3: return (Int64)left + (Int16)right;
                        case 4: return (Int64)left + (UInt16)right;
                        case 5: return (Int64)left + (Int32)right;
                        case 6: return (Int64)left + (UInt32)right;
                        case 7: return (Int64)left + (Int64)right;
                        //case 8: return (Int64)left + (UInt64)right;
                        case 9: return (Int64)left + (float)right;
                        case 10: return (Int64)left + (double)right;
                        default: return (Int64)left + Convert.ToDecimal(right);
                    }
                case 8:
                    switch (rt)
                    {
                        //case 1: return (UInt64)left + (sbyte)right;
                        case 2: return (UInt64)left + (byte)right;
                        //case 3: return (UInt64)left + (Int16)right;
                        case 4: return (UInt64)left + (UInt16)right;
                        //case 5: return (UInt64)left + (Int32)right;
                        case 6: return (UInt64)left + (UInt32)right;
                        //case 7: return (UInt64)left + (Int64)right;
                        case 8: return (UInt64)left + (UInt64)right;
                        case 9: return (UInt64)left + (float)right;
                        case 10: return (UInt64)left + (double)right;
                        default: return (UInt64)left + Convert.ToDecimal(right);
                    }
                case 9:
                    switch (rt)
                    {
                        case 1: return (float)left + (sbyte)right;
                        case 2: return (float)left + (byte)right;
                        case 3: return (float)left + (Int16)right;
                        case 4: return (float)left + (UInt16)right;
                        case 5: return (float)left + (Int32)right;
                        case 6: return (float)left + (UInt32)right;
                        case 7: return (float)left + (Int64)right;
                        case 8: return (float)left + (UInt64)right;
                        case 9: return (float)left + (float)right;
                        case 10: return (float)left + (double)right;
                        default: return Convert.ToDecimal(left) + Convert.ToDecimal(right);
                    }
                case 10:
                    switch (rt)
                    {
                        case 1: return (double)left + (sbyte)right;
                        case 2: return (double)left + (byte)right;
                        case 3: return (double)left + (Int16)right;
                        case 4: return (double)left + (UInt16)right;
                        case 5: return (double)left + (Int32)right;
                        case 6: return (double)left + (UInt32)right;
                        case 7: return (double)left + (Int64)right;
                        case 8: return (double)left + (UInt64)right;
                        case 9: return (double)left + (float)right;
                        case 10: return (double)left + (double)right;
                        default: return Convert.ToDecimal(left) + Convert.ToDecimal(right);
                    }
                default:
                    decimal num1 = Convert.ToDecimal(left);
                    decimal num2 = Convert.ToDecimal(right);
                    return num1 + num2;
            }


        }
        public static object Sub(object left, object right)
        {
            int lt = GetNumTypeCode(left);
            int rt = GetNumTypeCode(right);
            switch (lt)
            {
                case 1:
                    switch (rt)
                    {
                        case 1: return (sbyte)left - (sbyte)right;
                        case 2: return (sbyte)left - (byte)right;
                        case 3: return (sbyte)left - (Int16)right;
                        case 4: return (sbyte)left - (UInt16)right;
                        case 5: return (sbyte)left - (Int32)right;
                        case 6: return (sbyte)left - (UInt32)right;
                        case 7: return (sbyte)left - (Int64)right;
                        //case 8: return (sbyte)left - (UInt64)right;
                        case 9: return (sbyte)left - (float)right;
                        case 10: return (sbyte)left - (double)right;
                        default: return (byte)left - Convert.ToDecimal(right);
                    }
                case 2:
                    switch (rt)
                    {
                        case 1: return (byte)left - (sbyte)right;
                        case 2: return (byte)left - (byte)right;
                        case 3: return (byte)left - (Int16)right;
                        case 4: return (byte)left - (UInt16)right;
                        case 5: return (byte)left - (Int32)right;
                        case 6: return (byte)left - (UInt32)right;
                        case 7: return (byte)left - (Int64)right;
                        case 8: return (byte)left - (UInt64)right;
                        case 9: return (byte)left - (float)right;
                        case 10: return (byte)left - (double)right;
                        default: return (byte)left - Convert.ToDecimal(right);
                    }
                case 3:
                    switch (rt)
                    {
                        case 1: return (Int16)left - (sbyte)right;
                        case 2: return (Int16)left - (byte)right;
                        case 3: return (Int16)left - (Int16)right;
                        case 4: return (Int16)left - (UInt16)right;
                        case 5: return (Int16)left - (Int32)right;
                        case 6: return (Int16)left - (UInt32)right;
                        case 7: return (Int16)left - (Int64)right;
                        //case 8: return (Int16)left - (UInt64)right;
                        case 9: return (Int16)left - (float)right;
                        case 10: return (Int16)left - (double)right;
                        default: return (Int16)left - Convert.ToDecimal(right);
                    }
                case 4:
                    switch (rt)
                    {
                        case 1: return (UInt16)left - (sbyte)right;
                        case 2: return (UInt16)left - (byte)right;
                        case 3: return (UInt16)left - (Int16)right;
                        case 4: return (UInt16)left - (UInt16)right;
                        case 5: return (UInt16)left - (Int32)right;
                        case 6: return (UInt16)left - (UInt32)right;
                        case 7: return (UInt16)left - (Int64)right;
                        case 8: return (UInt16)left - (UInt64)right;
                        case 9: return (UInt16)left - (float)right;
                        case 10: return (UInt16)left - (double)right;
                        default: return (UInt16)left - Convert.ToDecimal(right);
                    }
                case 5:
                    switch (rt)
                    {
                        case 1: return (Int32)left - (sbyte)right;
                        case 2: return (Int32)left - (byte)right;
                        case 3: return (Int32)left - (Int16)right;
                        case 4: return (Int32)left - (UInt16)right;
                        case 5: return (Int32)left - (Int32)right;
                        case 6: return (Int32)left - (UInt32)right;
                        case 7: return (Int32)left - (Int64)right;
                        //case 8: return (Int32)left - (UInt64)right;
                        case 9: return (Int32)left - (float)right;
                        case 10: return (Int32)left - (double)right;
                        default: return (Int32)left - Convert.ToDecimal(right);
                    }
                case 6:
                    switch (rt)
                    {
                        case 1: return (UInt32)left - (sbyte)right;
                        case 2: return (UInt32)left - (byte)right;
                        case 3: return (UInt32)left - (Int16)right;
                        case 4: return (UInt32)left - (UInt16)right;
                        case 5: return (UInt32)left - (Int32)right;
                        case 6: return (UInt32)left - (UInt32)right;
                        case 7: return (UInt32)left - (Int64)right;
                        case 8: return (UInt32)left - (UInt64)right;
                        case 9: return (UInt32)left - (float)right;
                        case 10: return (UInt32)left - (double)right;
                        default: return (UInt32)left - Convert.ToDecimal(right);
                    }
                case 7:
                    switch (rt)
                    {
                        case 1: return (Int64)left - (sbyte)right;
                        case 2: return (Int64)left - (byte)right;
                        case 3: return (Int64)left - (Int16)right;
                        case 4: return (Int64)left - (UInt16)right;
                        case 5: return (Int64)left - (Int32)right;
                        case 6: return (Int64)left - (UInt32)right;
                        case 7: return (Int64)left - (Int64)right;
                        //case 8: return (Int64)left - (UInt64)right;
                        case 9: return (Int64)left - (float)right;
                        case 10: return (Int64)left - (double)right;
                        default: return (Int64)left - Convert.ToDecimal(right);
                    }
                case 8:
                    switch (rt)
                    {
                        //case 1: return (UInt64)left - (sbyte)right;
                        case 2: return (UInt64)left - (byte)right;
                        //case 3: return (UInt64)left - (Int16)right;
                        case 4: return (UInt64)left - (UInt16)right;
                        //case 5: return (UInt64)left - (Int32)right;
                        case 6: return (UInt64)left - (UInt32)right;
                        //case 7: return (UInt64)left - (Int64)right;
                        case 8: return (UInt64)left - (UInt64)right;
                        case 9: return (UInt64)left - (float)right;
                        case 10: return (UInt64)left - (double)right;
                        default: return (UInt64)left - Convert.ToDecimal(right);
                    }
                case 9:
                    switch (rt)
                    {
                        case 1: return (float)left - (sbyte)right;
                        case 2: return (float)left - (byte)right;
                        case 3: return (float)left - (Int16)right;
                        case 4: return (float)left - (UInt16)right;
                        case 5: return (float)left - (Int32)right;
                        case 6: return (float)left - (UInt32)right;
                        case 7: return (float)left - (Int64)right;
                        case 8: return (float)left - (UInt64)right;
                        case 9: return (float)left - (float)right;
                        case 10: return (float)left - (double)right;
                        default: return Convert.ToDecimal(left) - Convert.ToDecimal(right);
                    }
                case 10:
                    switch (rt)
                    {
                        case 1: return (double)left - (sbyte)right;
                        case 2: return (double)left - (byte)right;
                        case 3: return (double)left - (Int16)right;
                        case 4: return (double)left - (UInt16)right;
                        case 5: return (double)left - (Int32)right;
                        case 6: return (double)left - (UInt32)right;
                        case 7: return (double)left - (Int64)right;
                        case 8: return (double)left - (UInt64)right;
                        case 9: return (double)left - (float)right;
                        case 10: return (double)left - (double)right;
                        default: return Convert.ToDecimal(left) - Convert.ToDecimal(right);
                    }
                default:
                    decimal num1 = Convert.ToDecimal(left);
                    decimal num2 = Convert.ToDecimal(right);
                    return num1 - num2;
            }


        }
        public static object Mul(object left, object right)
        {
            int lt = GetNumTypeCode(left);
            int rt = GetNumTypeCode(right);
            switch (lt)
            {
                case 1:
                    switch (rt)
                    {
                        case 1: return (sbyte)left * (sbyte)right;
                        case 2: return (sbyte)left * (byte)right;
                        case 3: return (sbyte)left * (Int16)right;
                        case 4: return (sbyte)left * (UInt16)right;
                        case 5: return (sbyte)left * (Int32)right;
                        case 6: return (sbyte)left * (UInt32)right;
                        case 7: return (sbyte)left * (Int64)right;
                        //case 8: return (sbyte)left * (UInt64)right;
                        case 9: return (sbyte)left * (float)right;
                        case 10: return (sbyte)left * (double)right;
                        default: return (byte)left * Convert.ToDecimal(right);
                    }
                case 2:
                    switch (rt)
                    {
                        case 1: return (byte)left * (sbyte)right;
                        case 2: return (byte)left * (byte)right;
                        case 3: return (byte)left * (Int16)right;
                        case 4: return (byte)left * (UInt16)right;
                        case 5: return (byte)left * (Int32)right;
                        case 6: return (byte)left * (UInt32)right;
                        case 7: return (byte)left * (Int64)right;
                        case 8: return (byte)left * (UInt64)right;
                        case 9: return (byte)left * (float)right;
                        case 10: return (byte)left * (double)right;
                        default: return (byte)left * Convert.ToDecimal(right);
                    }
                case 3:
                    switch (rt)
                    {
                        case 1: return (Int16)left * (sbyte)right;
                        case 2: return (Int16)left * (byte)right;
                        case 3: return (Int16)left * (Int16)right;
                        case 4: return (Int16)left * (UInt16)right;
                        case 5: return (Int16)left * (Int32)right;
                        case 6: return (Int16)left * (UInt32)right;
                        case 7: return (Int16)left * (Int64)right;
                        //case 8: return (Int16)left * (UInt64)right;
                        case 9: return (Int16)left * (float)right;
                        case 10: return (Int16)left * (double)right;
                        default: return (Int16)left * Convert.ToDecimal(right);
                    }
                case 4:
                    switch (rt)
                    {
                        case 1: return (UInt16)left * (sbyte)right;
                        case 2: return (UInt16)left * (byte)right;
                        case 3: return (UInt16)left * (Int16)right;
                        case 4: return (UInt16)left * (UInt16)right;
                        case 5: return (UInt16)left * (Int32)right;
                        case 6: return (UInt16)left * (UInt32)right;
                        case 7: return (UInt16)left * (Int64)right;
                        case 8: return (UInt16)left * (UInt64)right;
                        case 9: return (UInt16)left * (float)right;
                        case 10: return (UInt16)left * (double)right;
                        default: return (UInt16)left * Convert.ToDecimal(right);
                    }
                case 5:
                    switch (rt)
                    {
                        case 1: return (Int32)left * (sbyte)right;
                        case 2: return (Int32)left * (byte)right;
                        case 3: return (Int32)left * (Int16)right;
                        case 4: return (Int32)left * (UInt16)right;
                        case 5: return (Int32)left * (Int32)right;
                        case 6: return (Int32)left * (UInt32)right;
                        case 7: return (Int32)left * (Int64)right;
                        //case 8: return (Int32)left * (UInt64)right;
                        case 9: return (Int32)left * (float)right;
                        case 10: return (Int32)left * (double)right;
                        default: return (Int32)left * Convert.ToDecimal(right);
                    }
                case 6:
                    switch (rt)
                    {
                        case 1: return (UInt32)left * (sbyte)right;
                        case 2: return (UInt32)left * (byte)right;
                        case 3: return (UInt32)left * (Int16)right;
                        case 4: return (UInt32)left * (UInt16)right;
                        case 5: return (UInt32)left * (Int32)right;
                        case 6: return (UInt32)left * (UInt32)right;
                        case 7: return (UInt32)left * (Int64)right;
                        case 8: return (UInt32)left * (UInt64)right;
                        case 9: return (UInt32)left * (float)right;
                        case 10: return (UInt32)left * (double)right;
                        default: return (UInt32)left * Convert.ToDecimal(right);
                    }
                case 7:
                    switch (rt)
                    {
                        case 1: return (Int64)left * (sbyte)right;
                        case 2: return (Int64)left * (byte)right;
                        case 3: return (Int64)left * (Int16)right;
                        case 4: return (Int64)left * (UInt16)right;
                        case 5: return (Int64)left * (Int32)right;
                        case 6: return (Int64)left * (UInt32)right;
                        case 7: return (Int64)left * (Int64)right;
                        //case 8: return (Int64)left * (UInt64)right;
                        case 9: return (Int64)left * (float)right;
                        case 10: return (Int64)left * (double)right;
                        default: return (Int64)left * Convert.ToDecimal(right);
                    }
                case 8:
                    switch (rt)
                    {
                        //case 1: return (UInt64)left * (sbyte)right;
                        case 2: return (UInt64)left * (byte)right;
                        //case 3: return (UInt64)left * (Int16)right;
                        case 4: return (UInt64)left * (UInt16)right;
                        //case 5: return (UInt64)left * (Int32)right;
                        case 6: return (UInt64)left * (UInt32)right;
                        //case 7: return (UInt64)left * (Int64)right;
                        case 8: return (UInt64)left * (UInt64)right;
                        case 9: return (UInt64)left * (float)right;
                        case 10: return (UInt64)left * (double)right;
                        default: return (UInt64)left * Convert.ToDecimal(right);
                    }
                case 9:
                    switch (rt)
                    {
                        case 1: return (float)left * (sbyte)right;
                        case 2: return (float)left * (byte)right;
                        case 3: return (float)left * (Int16)right;
                        case 4: return (float)left * (UInt16)right;
                        case 5: return (float)left * (Int32)right;
                        case 6: return (float)left * (UInt32)right;
                        case 7: return (float)left * (Int64)right;
                        case 8: return (float)left * (UInt64)right;
                        case 9: return (float)left * (float)right;
                        case 10: return (float)left * (double)right;
                        default: return Convert.ToDecimal(left) * Convert.ToDecimal(right);
                    }
                case 10:
                    switch (rt)
                    {
                        case 1: return (double)left * (sbyte)right;
                        case 2: return (double)left * (byte)right;
                        case 3: return (double)left * (Int16)right;
                        case 4: return (double)left * (UInt16)right;
                        case 5: return (double)left * (Int32)right;
                        case 6: return (double)left * (UInt32)right;
                        case 7: return (double)left * (Int64)right;
                        case 8: return (double)left * (UInt64)right;
                        case 9: return (double)left * (float)right;
                        case 10: return (double)left * (double)right;
                        default: return Convert.ToDecimal(left) * Convert.ToDecimal(right);
                    }
                default:
                    decimal num1 = Convert.ToDecimal(left);
                    decimal num2 = Convert.ToDecimal(right);
                    return num1 * num2;
            }
        }
        public static object Div(object left, object right)
        {
            int lt = GetNumTypeCode(left);
            int rt = GetNumTypeCode(right);
            switch (lt)
            {
                case 1:
                    switch (rt)
                    {
                        case 1: return (sbyte)left / (sbyte)right;
                        case 2: return (sbyte)left / (byte)right;
                        case 3: return (sbyte)left / (Int16)right;
                        case 4: return (sbyte)left / (UInt16)right;
                        case 5: return (sbyte)left / (Int32)right;
                        case 6: return (sbyte)left / (UInt32)right;
                        case 7: return (sbyte)left / (Int64)right;
                        //case 8: return (sbyte)left / (UInt64)right;
                        case 9: return (sbyte)left / (float)right;
                        case 10: return (sbyte)left / (double)right;
                        default: return (byte)left / Convert.ToDecimal(right);
                    }
                case 2:
                    switch (rt)
                    {
                        case 1: return (byte)left / (sbyte)right;
                        case 2: return (byte)left / (byte)right;
                        case 3: return (byte)left / (Int16)right;
                        case 4: return (byte)left / (UInt16)right;
                        case 5: return (byte)left / (Int32)right;
                        case 6: return (byte)left / (UInt32)right;
                        case 7: return (byte)left / (Int64)right;
                        case 8: return (byte)left / (UInt64)right;
                        case 9: return (byte)left / (float)right;
                        case 10: return (byte)left / (double)right;
                        default: return (byte)left / Convert.ToDecimal(right);
                    }
                case 3:
                    switch (rt)
                    {
                        case 1: return (Int16)left / (sbyte)right;
                        case 2: return (Int16)left / (byte)right;
                        case 3: return (Int16)left / (Int16)right;
                        case 4: return (Int16)left / (UInt16)right;
                        case 5: return (Int16)left / (Int32)right;
                        case 6: return (Int16)left / (UInt32)right;
                        case 7: return (Int16)left / (Int64)right;
                        //case 8: return (Int16)left / (UInt64)right;
                        case 9: return (Int16)left / (float)right;
                        case 10: return (Int16)left / (double)right;
                        default: return (Int16)left / Convert.ToDecimal(right);
                    }
                case 4:
                    switch (rt)
                    {
                        case 1: return (UInt16)left / (sbyte)right;
                        case 2: return (UInt16)left / (byte)right;
                        case 3: return (UInt16)left / (Int16)right;
                        case 4: return (UInt16)left / (UInt16)right;
                        case 5: return (UInt16)left / (Int32)right;
                        case 6: return (UInt16)left / (UInt32)right;
                        case 7: return (UInt16)left / (Int64)right;
                        case 8: return (UInt16)left / (UInt64)right;
                        case 9: return (UInt16)left / (float)right;
                        case 10: return (UInt16)left / (double)right;
                        default: return (UInt16)left / Convert.ToDecimal(right);
                    }
                case 5:
                    switch (rt)
                    {
                        case 1: return (Int32)left / (sbyte)right;
                        case 2: return (Int32)left / (byte)right;
                        case 3: return (Int32)left / (Int16)right;
                        case 4: return (Int32)left / (UInt16)right;
                        case 5: return (Int32)left / (Int32)right;
                        case 6: return (Int32)left / (UInt32)right;
                        case 7: return (Int32)left / (Int64)right;
                        //case 8: return (Int32)left / (UInt64)right;
                        case 9: return (Int32)left / (float)right;
                        case 10: return (Int32)left / (double)right;
                        default: return (Int32)left / Convert.ToDecimal(right);
                    }
                case 6:
                    switch (rt)
                    {
                        case 1: return (UInt32)left / (sbyte)right;
                        case 2: return (UInt32)left / (byte)right;
                        case 3: return (UInt32)left / (Int16)right;
                        case 4: return (UInt32)left / (UInt16)right;
                        case 5: return (UInt32)left / (Int32)right;
                        case 6: return (UInt32)left / (UInt32)right;
                        case 7: return (UInt32)left / (Int64)right;
                        case 8: return (UInt32)left / (UInt64)right;
                        case 9: return (UInt32)left / (float)right;
                        case 10: return (UInt32)left / (double)right;
                        default: return (UInt32)left / Convert.ToDecimal(right);
                    }
                case 7:
                    switch (rt)
                    {
                        case 1: return (Int64)left / (sbyte)right;
                        case 2: return (Int64)left / (byte)right;
                        case 3: return (Int64)left / (Int16)right;
                        case 4: return (Int64)left / (UInt16)right;
                        case 5: return (Int64)left / (Int32)right;
                        case 6: return (Int64)left / (UInt32)right;
                        case 7: return (Int64)left / (Int64)right;
                        //case 8: return (Int64)left / (UInt64)right;
                        case 9: return (Int64)left / (float)right;
                        case 10: return (Int64)left / (double)right;
                        default: return (Int64)left / Convert.ToDecimal(right);
                    }
                case 8:
                    switch (rt)
                    {
                        //case 1: return (UInt64)left / (sbyte)right;
                        case 2: return (UInt64)left / (byte)right;
                        //case 3: return (UInt64)left / (Int16)right;
                        case 4: return (UInt64)left / (UInt16)right;
                        //case 5: return (UInt64)left / (Int32)right;
                        case 6: return (UInt64)left / (UInt32)right;
                        //case 7: return (UInt64)left / (Int64)right;
                        case 8: return (UInt64)left / (UInt64)right;
                        case 9: return (UInt64)left / (float)right;
                        case 10: return (UInt64)left / (double)right;
                        default: return (UInt64)left / Convert.ToDecimal(right);
                    }
                case 9:
                    switch (rt)
                    {
                        case 1: return (float)left / (sbyte)right;
                        case 2: return (float)left / (byte)right;
                        case 3: return (float)left / (Int16)right;
                        case 4: return (float)left / (UInt16)right;
                        case 5: return (float)left / (Int32)right;
                        case 6: return (float)left / (UInt32)right;
                        case 7: return (float)left / (Int64)right;
                        case 8: return (float)left / (UInt64)right;
                        case 9: return (float)left / (float)right;
                        case 10: return (float)left / (double)right;
                        default: return Convert.ToDecimal(left) / Convert.ToDecimal(right);
                    }
                case 10:
                    switch (rt)
                    {
                        case 1: return (double)left / (sbyte)right;
                        case 2: return (double)left / (byte)right;
                        case 3: return (double)left / (Int16)right;
                        case 4: return (double)left / (UInt16)right;
                        case 5: return (double)left / (Int32)right;
                        case 6: return (double)left / (UInt32)right;
                        case 7: return (double)left / (Int64)right;
                        case 8: return (double)left / (UInt64)right;
                        case 9: return (double)left / (float)right;
                        case 10: return (double)left / (double)right;
                        default: return Convert.ToDecimal(left) / Convert.ToDecimal(right);
                    }
                default:
                    decimal num1 = Convert.ToDecimal(left);
                    decimal num2 = Convert.ToDecimal(right);
                    return num1 / num2;
            }




        }

        public static object Rem(object left, object right)
        {
            int lt = GetNumTypeCode(left);
            int rt = GetNumTypeCode(right);
            switch (lt)
            {
                case 1:
                    switch (rt)
                    {
                        case 1: return (sbyte)left % (sbyte)right;
                        case 2: return (sbyte)left % (byte)right;
                        case 3: return (sbyte)left % (Int16)right;
                        case 4: return (sbyte)left % (UInt16)right;
                        case 5: return (sbyte)left % (Int32)right;
                        case 6: return (sbyte)left % (UInt32)right;
                        case 7: return (sbyte)left % (Int64)right;
                        //case 8: return (sbyte)left % (UInt64)right;
                        case 9: return (sbyte)left % (float)right;
                        case 10: return (sbyte)left % (double)right;
                        default: return (byte)left % Convert.ToDecimal(right);
                    }
                case 2:
                    switch (rt)
                    {
                        case 1: return (byte)left % (sbyte)right;
                        case 2: return (byte)left % (byte)right;
                        case 3: return (byte)left % (Int16)right;
                        case 4: return (byte)left % (UInt16)right;
                        case 5: return (byte)left % (Int32)right;
                        case 6: return (byte)left % (UInt32)right;
                        case 7: return (byte)left % (Int64)right;
                        case 8: return (byte)left % (UInt64)right;
                        case 9: return (byte)left % (float)right;
                        case 10: return (byte)left % (double)right;
                        default: return (byte)left % Convert.ToDecimal(right);
                    }
                case 3:
                    switch (rt)
                    {
                        case 1: return (Int16)left % (sbyte)right;
                        case 2: return (Int16)left % (byte)right;
                        case 3: return (Int16)left % (Int16)right;
                        case 4: return (Int16)left % (UInt16)right;
                        case 5: return (Int16)left % (Int32)right;
                        case 6: return (Int16)left % (UInt32)right;
                        case 7: return (Int16)left % (Int64)right;
                        //case 8: return (Int16)left % (UInt64)right;
                        case 9: return (Int16)left % (float)right;
                        case 10: return (Int16)left % (double)right;
                        default: return (Int16)left % Convert.ToDecimal(right);
                    }
                case 4:
                    switch (rt)
                    {
                        case 1: return (UInt16)left % (sbyte)right;
                        case 2: return (UInt16)left % (byte)right;
                        case 3: return (UInt16)left % (Int16)right;
                        case 4: return (UInt16)left % (UInt16)right;
                        case 5: return (UInt16)left % (Int32)right;
                        case 6: return (UInt16)left % (UInt32)right;
                        case 7: return (UInt16)left % (Int64)right;
                        case 8: return (UInt16)left % (UInt64)right;
                        case 9: return (UInt16)left % (float)right;
                        case 10: return (UInt16)left % (double)right;
                        default: return (UInt16)left % Convert.ToDecimal(right);
                    }
                case 5:
                    switch (rt)
                    {
                        case 1: return (Int32)left % (sbyte)right;
                        case 2: return (Int32)left % (byte)right;
                        case 3: return (Int32)left % (Int16)right;
                        case 4: return (Int32)left % (UInt16)right;
                        case 5: return (Int32)left % (Int32)right;
                        case 6: return (Int32)left % (UInt32)right;
                        case 7: return (Int32)left % (Int64)right;
                        //case 8: return (Int32)left % (UInt64)right;
                        case 9: return (Int32)left % (float)right;
                        case 10: return (Int32)left % (double)right;
                        default: return (Int32)left % Convert.ToDecimal(right);
                    }
                case 6:
                    switch (rt)
                    {
                        case 1: return (UInt32)left % (sbyte)right;
                        case 2: return (UInt32)left % (byte)right;
                        case 3: return (UInt32)left % (Int16)right;
                        case 4: return (UInt32)left % (UInt16)right;
                        case 5: return (UInt32)left % (Int32)right;
                        case 6: return (UInt32)left % (UInt32)right;
                        case 7: return (UInt32)left % (Int64)right;
                        case 8: return (UInt32)left % (UInt64)right;
                        case 9: return (UInt32)left % (float)right;
                        case 10: return (UInt32)left % (double)right;
                        default: return (UInt32)left % Convert.ToDecimal(right);
                    }
                case 7:
                    switch (rt)
                    {
                        case 1: return (Int64)left % (sbyte)right;
                        case 2: return (Int64)left % (byte)right;
                        case 3: return (Int64)left % (Int16)right;
                        case 4: return (Int64)left % (UInt16)right;
                        case 5: return (Int64)left % (Int32)right;
                        case 6: return (Int64)left % (UInt32)right;
                        case 7: return (Int64)left % (Int64)right;
                        //case 8: return (Int64)left % (UInt64)right;
                        case 9: return (Int64)left % (float)right;
                        case 10: return (Int64)left % (double)right;
                        default: return (Int64)left % Convert.ToDecimal(right);
                    }
                case 8:
                    switch (rt)
                    {
                        //case 1: return (UInt64)left % (sbyte)right;
                        case 2: return (UInt64)left % (byte)right;
                        //case 3: return (UInt64)left % (Int16)right;
                        case 4: return (UInt64)left % (UInt16)right;
                        //case 5: return (UInt64)left % (Int32)right;
                        case 6: return (UInt64)left % (UInt32)right;
                        //case 7: return (UInt64)left % (Int64)right;
                        case 8: return (UInt64)left % (UInt64)right;
                        case 9: return (UInt64)left % (float)right;
                        case 10: return (UInt64)left % (double)right;
                        default: return (UInt64)left % Convert.ToDecimal(right);
                    }
                case 9:
                    switch (rt)
                    {
                        case 1: return (float)left % (sbyte)right;
                        case 2: return (float)left % (byte)right;
                        case 3: return (float)left % (Int16)right;
                        case 4: return (float)left % (UInt16)right;
                        case 5: return (float)left % (Int32)right;
                        case 6: return (float)left % (UInt32)right;
                        case 7: return (float)left % (Int64)right;
                        case 8: return (float)left % (UInt64)right;
                        case 9: return (float)left % (float)right;
                        case 10: return (float)left % (double)right;
                        default: return Convert.ToDecimal(left) % Convert.ToDecimal(right);
                    }
                case 10:
                    switch (rt)
                    {
                        case 1: return (double)left % (sbyte)right;
                        case 2: return (double)left % (byte)right;
                        case 3: return (double)left % (Int16)right;
                        case 4: return (double)left % (UInt16)right;
                        case 5: return (double)left % (Int32)right;
                        case 6: return (double)left % (UInt32)right;
                        case 7: return (double)left % (Int64)right;
                        case 8: return (double)left % (UInt64)right;
                        case 9: return (double)left % (float)right;
                        case 10: return (double)left % (double)right;
                        default: return Convert.ToDecimal(left) % Convert.ToDecimal(right);
                    }
                default:
                    decimal num1 = Convert.ToDecimal(left);
                    decimal num2 = Convert.ToDecimal(right);
                    return num1 % num2;
            }




        }

    }
}
