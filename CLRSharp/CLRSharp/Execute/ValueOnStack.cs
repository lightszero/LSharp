using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    //栈上值类型，拆箱，装箱转换非常频繁,需要处理一下。
    //
    public class ValueOnStack
    {
        ////valuetype
        //        public NumberType TypeOnDef;
        //        public NumberOnStack TypeOnStack;
        //        public IBox box;
        public static IBox Make(ICLRType type)
        {
            return Make(type.TypeForSystem);
         
        }
        public static IBox Make(System.Type type)
        {
            NumberType code = Math.GetTypeCode(type);
            switch (code)
            {
                case NumberType.BOOL:
                case NumberType.SBYTE:
                case NumberType.BYTE:
                case NumberType.CHAR:
                case NumberType.INT16:
                case NumberType.UINT16:
                case NumberType.INT32:
                case NumberType.UINT32:
                    return new BoxInt32(code);
                case NumberType.INT64:
                case NumberType.UINT64:
                    return new BoxInt64(code);
                case NumberType.FLOAT:
                case NumberType.DOUBLE:
                    return new BoxDouble(code);
                default:
                    return null;
            }

        }
        public static IBox Convert(IBox box, NumberType type)
        {
            switch (type)
            {
                case NumberType.BOOL:
                case NumberType.SBYTE:
                case NumberType.BYTE:
                case NumberType.CHAR:
                case NumberType.INT16:
                case NumberType.UINT16:
                case NumberType.INT32:
                case NumberType.UINT32:
                    {
                        if (box is BoxInt32) return box;
                        BoxInt32 v32= new BoxInt32(type);
                        BoxInt64 b64 = box as BoxInt64;
                        BoxDouble bdb = box as BoxDouble;
                        if (b64 != null)
                            v32.value = (int)b64.value;
                        else
                            v32.value = (int)bdb.value;
                        return v32;
                    }
                case NumberType.INT64:
                case NumberType.UINT64:
                    {
                        if (box is BoxInt64) return box;
                        BoxInt64 v64 = new BoxInt64(type);
                        BoxInt32 b32 = box as BoxInt32;
                        BoxDouble bdb = box as BoxDouble;
                        if (b32 != null)
                            v64.value = b32.value;
                        else
                            v64.value = (Int64)bdb.value;
                        return v64;
                    }
                case NumberType.FLOAT:
                case NumberType.DOUBLE:
                    {
                        if (box is BoxDouble) return box;
                        BoxDouble vdb = new BoxDouble(type);
                        BoxInt32 b32 = box as BoxInt32;
                        BoxInt64 b64 = box as BoxInt64;
                        if (b32 != null)
                            vdb.value = b32.value;
                        else
                            vdb.value = b64.value;
                        return vdb;
                    }
                default:
                    return null;
            }
        }
    }
    public enum NumberType
    {
        IsNotNumber = 0,
        SBYTE = 1,
        BYTE = 2,
        INT16 = 3,
        UINT16 = 4,
        INT32 = 5,
        UINT32 = 6,
        INT64 = 7,
        UINT64 = 8,
        FLOAT = 9,
        DOUBLE = 10,
        INTPTR = 11,
        UINTPTR = 12,
        DECIMAL = 13,
        CHAR = 14,
        BOOL = 15,
    };
    public enum NumberOnStack
    {
        Int32,
        Int64,
        Double,
    }
    public interface IBox
    {
        object BoxStack();
        object BoxDefine();

        void Add(IBox right);
        void Sub(IBox right);
        void Mul(IBox right);
        void Div(IBox right);
        void Mod(IBox right);
        void SetDirect(object value);
        void Set(IBox value);

        NumberType type
        {
            get;
        }
        NumberOnStack typeStack
        {
            get;
        }
    }
    public class BoxInt32 : IBox
    {
        public BoxInt32(NumberType type)
        {
            this.type = type;
        }
        public NumberType type
        {
            get;
            private set;
        }
        public NumberOnStack typeStack
        {
            get
            {
                return NumberOnStack.Int32;
            }
        }
        public Int32 value;
        public object BoxStack()
        {
            return value;
        }

        public object BoxDefine()
        {
            switch (type)
            {
                case NumberType.BOOL:
                    return (value > 0);
                case NumberType.SBYTE:
                    return (sbyte)value;
                case NumberType.BYTE:
                    return (byte)value;
                case NumberType.CHAR:
                    return (char)value;
                case NumberType.INT16:
                    return (Int16)value;
                case NumberType.UINT16:
                    return (UInt16)value;
                case NumberType.INT32:
                    return (Int32)value;
                case NumberType.UINT32:
                    return (UInt32)value;
                case NumberType.INT64:
                    return (Int64)value;
                case NumberType.UINT64:
                    return (UInt64)value;
                case NumberType.FLOAT:
                    return (float)value;
                case NumberType.DOUBLE:
                    return (double)value;
                default:
                    return null;
            }

        }

        public void Set(IBox value)
        {

            this.value = (value as BoxInt32).value;
        }
        public void SetDirect(object value)
        {
            this.value = (int)value;
        }
        public void Add(IBox right)
        {
            this.value += (right as BoxInt32).value;
        }

        public void Sub(IBox right)
        {
            this.value -= (right as BoxInt32).value;
        }

        public void Mul(IBox right)
        {
            this.value *= (right as BoxInt32).value;
        }

        public void Div(IBox right)
        {
            this.value /= (right as BoxInt32).value;
        }
        public void Mod(IBox right)
        {
            this.value %= (right as BoxInt32).value;
        }
    }
    public class BoxInt64 : IBox
    {
        public BoxInt64(NumberType type)
        {
            this.type = type;
        }
        public NumberType type
        {
            get;
            private set;
        }
        public NumberOnStack typeStack
        {
            get
            {
                return NumberOnStack.Int64;
            }
        }
        public Int64 value;
        public object BoxStack()
        {
            return value;
        }

        public object BoxDefine()
        {
            switch (type)
            {
                case NumberType.BOOL:
                    return (value > 0);
                case NumberType.SBYTE:
                    return (sbyte)value;
                case NumberType.BYTE:
                    return (byte)value;
                case NumberType.CHAR:
                    return (char)value;
                case NumberType.INT16:
                    return (Int16)value;
                case NumberType.UINT16:
                    return (UInt16)value;
                case NumberType.INT32:
                    return (Int32)value;
                case NumberType.UINT32:
                    return (UInt32)value;
                case NumberType.INT64:
                    return (Int64)value;
                case NumberType.UINT64:
                    return (UInt64)value;
                case NumberType.FLOAT:
                    return (float)value;
                case NumberType.DOUBLE:
                    return (double)value;
                default:
                    return null;
            }

        }
        public void Set(IBox value)
        {
            this.value = (value as BoxInt64).value;
        }
        public void SetDirect(object value)
        {
            this.value = (Int64)value;
        }
        public void Add(IBox right)
        {
            this.value += (right as BoxInt64).value;
        }

        public void Sub(IBox right)
        {
            this.value -= (right as BoxInt64).value;
        }

        public void Mul(IBox right)
        {
            this.value *= (right as BoxInt64).value;
        }

        public void Div(IBox right)
        {
            this.value /= (right as BoxInt64).value;
        }
        public void Mod(IBox right)
        {
            this.value %= (right as BoxInt64).value;
        }
    }
    public class BoxDouble : IBox
    {
        public BoxDouble(NumberType type)
        {
            this.type = type;
        }
        public NumberType type
        {
            get;
            private set;
        }
        public NumberOnStack typeStack
        {
            get
            {
                return NumberOnStack.Double;
            }
        }
        public double value;
        public object BoxStack()
        {
            return value;
        }

        public object BoxDefine()
        {
            switch (type)
            {
                case NumberType.BOOL:
                    return (value > 0);
                case NumberType.SBYTE:
                    return (sbyte)value;
                case NumberType.BYTE:
                    return (byte)value;
                case NumberType.CHAR:
                    return (char)value;
                case NumberType.INT16:
                    return (Int16)value;
                case NumberType.UINT16:
                    return (UInt16)value;
                case NumberType.INT32:
                    return (Int32)value;
                case NumberType.UINT32:
                    return (UInt32)value;
                case NumberType.INT64:
                    return (Int64)value;
                case NumberType.UINT64:
                    return (UInt64)value;
                case NumberType.FLOAT:
                    return (float)value;
                case NumberType.DOUBLE:
                    return (double)value;
                default:
                    return null;
            }

        }

        public void Set(IBox value)
        {
            this.value = (value as BoxDouble).value;
        }
        public void SetDirect(object value)
        {

            this.value = (double)Convert.ToDecimal(value);
        }
        public void Add(IBox right)
        {
            this.value += (right as BoxDouble).value;
        }

        public void Sub(IBox right)
        {
            this.value -= (right as BoxDouble).value;
        }

        public void Mul(IBox right)
        {
            this.value *= (right as BoxDouble).value;
        }

        public void Div(IBox right)
        {
            this.value /= (right as BoxDouble).value;
        }
        public void Mod(IBox right)
        {
            this.value %= (right as BoxDouble).value;
        }
    }

}
