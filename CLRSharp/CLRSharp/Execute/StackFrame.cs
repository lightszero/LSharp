using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    class StackFrame
    {
        public Mono.Cecil.Cil.Instruction _pos = null;

        Stack<object> stackCalc = new Stack<object>();
        List<object> slotVar = new List<object>();
        object[] _params = null;
        public void SetParams(object[] _p)
        {
            _params = _p;
        }
        //流程控制
        public void Call(Context context, Mono.Cecil.MethodReference method)
        {

            object[] _pp = new object[stackCalc.Count];
            for (int i = 0; i < _pp.Length; i++)
            {
                _pp[_pp.Length - 1 - i] = stackCalc.Pop();
            }
            object returnvar = context.Call(method, _pp);
            bool breturn = method.ReturnType.FullName != "System.Void";
            if (breturn)
            {
                stackCalc.Push(returnvar);
            }
            _pos = _pos.Next;
        }
        public void Nop()
        {
            _pos = _pos.Next;
        }
        public void Dup()
        {
            stackCalc.Push(stackCalc.Peek());
            _pos = _pos.Next;
        }
        public void Pop()
        {
            stackCalc.Pop();
            _pos = _pos.Next;
        }
        public void Ret()
        {
            _pos = _pos.Next;
        }
        public void Box()
        {
            _pos = _pos.Next;
        }
        public void Unbox()
        {
            _pos = _pos.Next;
        }
        public void Unbox_Any()
        {
            _pos = _pos.Next;
        }
        public void Br(Mono.Cecil.Cil.Instruction pos)
        {
            _pos = pos;
        }
        public void Brtrue(Mono.Cecil.Cil.Instruction pos)
        {
            bool b = (bool)stackCalc.Pop();
            if (b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Brfalse(Mono.Cecil.Cil.Instruction pos)
        {
            bool b = (bool)stackCalc.Pop();
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        //加载常量
        public void Ldc_I4(object v)//int32
        {
            stackCalc.Push(v);
            _pos = _pos.Next;

        }
        public void Ldc_I4_S(object v)//int8
        {
            stackCalc.Push(v);
            _pos = _pos.Next;
        }
        public void Ldc_I8(object v)//int64
        {
            stackCalc.Push(v);
            _pos = _pos.Next;
        }
        public void Ldc_R4(object v)
        {
            stackCalc.Push(v);
            _pos = _pos.Next;
        }
        public void Ldc_R8(object v)
        {
            stackCalc.Push(v);
            _pos = _pos.Next;
        }
        //放进变量槽
        public void Stloc(int pos)
        {
            object v = stackCalc.Pop();
            while (slotVar.Count <= pos)
            {
                slotVar.Add(null);
            }
            slotVar[pos] = v;
            _pos = _pos.Next;
        }
        //拿出变量槽
        public void Ldloc(int pos)
        {
            stackCalc.Push(slotVar[pos]);
            _pos = _pos.Next;
        }
        class RefObj
        {
            public RefObj(int pos)
            {

            }

        }
        //拿出变量槽的引用
        public void Ldloca(int pos)
        {
            stackCalc.Push(new RefObj(pos));
            _pos = _pos.Next;
        }

        public void Ldstr(string text)
        {
            stackCalc.Push(text);
            _pos = _pos.Next;
        }

        //加载参数(还得处理static，il静态非静态不一样，成员参数0是this)
        public void Ldarg(int pos)
        {
            stackCalc.Push(_params[pos]);
            _pos = _pos.Next;
        }
        public void Ldarga(int pos)
        {
            stackCalc.Push(new RefObj(pos));
            _pos = _pos.Next;
        }
        //逻辑计算

        public void Ceq()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            stackCalc.Push(n1.Equals(n2));
            _pos = _pos.Next;
        }
        public void Cgt()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 > num2);
            _pos = _pos.Next;
        }
        public void Cgt_Un()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 > num2);
            _pos = _pos.Next;
        }
        public void Clt()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();


            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);


            stackCalc.Push(num1 < num2);
            _pos = _pos.Next;
        }
        public void Clt_Un()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 < num2);
            _pos = _pos.Next;
        }
        public void Ckfinite()
        {
            object n1 = stackCalc.Pop();
            if (n1 is float)
            {
                float v = (float)n1;
                stackCalc.Push(float.IsInfinity(v) || float.IsNaN(v));
            }
            else
            {
                double v = (double)n1;
                stackCalc.Push(double.IsInfinity(v) || double.IsNaN(v));
            }
            _pos = _pos.Next;
        }
        //算术操作
        public void Add()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 + num2);
            _pos = _pos.Next;
        }
        public void Sub()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 - num2);
            _pos = _pos.Next;
        }
        public void Mul()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 * num2);
            _pos = _pos.Next;
        }
        public void Div()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 / num2);
            _pos = _pos.Next;
        }
        public void Div_Un()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 / num2);
            _pos = _pos.Next;
        }
        public void Rem()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 % num2);
            _pos = _pos.Next;
        }
        public void Rem_Un()
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            stackCalc.Push(num1 % num2);
            _pos = _pos.Next;
        }
        public void Neg()
        {

            object n1 = stackCalc.Pop();
            if (n1 is int)
            {
                stackCalc.Push(~(int)n1);
            }
            else if (n1 is Int64)
            {
                stackCalc.Push(~(Int64)n1);
            }
            else
            {
                stackCalc.Push(n1);
            }

            _pos = _pos.Next;
        }
        //转换
        public void Conv_I1()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((byte)num1);
            _pos = _pos.Next;
        }
        public void Conv_U1()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((sbyte)num1);
            _pos = _pos.Next;
        }
        public void Conv_I2()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int16)num1);
            _pos = _pos.Next;
        }
        public void Conv_U2()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt16)num1);
            _pos = _pos.Next;
        }
        public void Conv_I4()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int32)num1);
            _pos = _pos.Next;
        }
        public void Conv_U4()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt32)num1);
            _pos = _pos.Next;
        }
        public void Conv_I8()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int64)num1);
            _pos = _pos.Next;
        }
        public void Conv_U8()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt64)num1);
            _pos = _pos.Next;
        }
        public void Conv_I()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int64)num1);
            _pos = _pos.Next;
        }
        public void Conv_U()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt64)num1);
            _pos = _pos.Next;
        }
        public void Conv_R4()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((float)num1);
            _pos = _pos.Next;
        }
        public void Conv_R8()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((double)num1);
            _pos = _pos.Next;
        }
        public void Conv_R_Un()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());
            stackCalc.Push((float)num1);
            _pos = _pos.Next;
        }



    }
}
