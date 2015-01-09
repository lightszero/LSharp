using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    /// <summary>
    /// 堆栈帧
    /// 一个堆栈帧，包含一个计算栈，一个临时变量槽，一个参数槽
    /// 模拟虚拟机上的堆栈帧
    /// </summary>
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
        public object Return()
        {
            if (this.stackCalc.Count == 0) return null;
            else return stackCalc.Pop();
        }
        //流程控制
        public void Call(ThreadContext context, Mono.Cecil.MethodReference method)
        {


            object[] _pp = null;
            object _this = null;

            if (method.HasParameters)
            {
                _pp = new object[method.Parameters.Count];
                for (int i = 0; i < _pp.Length; i++)
                {
                    _pp[_pp.Length - 1 - i] = stackCalc.Pop();
                }
            }
            if (method.HasThis)
            {

                _this = stackCalc.Pop();
            }


            var typesys = context.environment.GetType(method.DeclaringType.FullName, method.Module);
            if (typesys == null)
                throw new Exception("type can't find:" + method.DeclaringType.FullName);
            if (typesys.FullName.Contains("System.Runtime.CompilerServices.RuntimeHelpers") && method.FullName.Contains("InitializeArray"))
            {
                _pos = _pos.Next;
                return;
            }
            MethodParamList list = new MethodParamList(context.environment, method);

            if (_this is RefObj && method.Name != ".ctor")
            {
                _this = (_this as RefObj).Get();
            }
            string methodname = method.Name;
            IMethod _method = null;
            if (method.IsGenericInstance)
            {
                Mono.Cecil.GenericInstanceMethod gmethod = method as Mono.Cecil.GenericInstanceMethod;
                MethodParamList _plist = new MethodParamList(context.environment, gmethod);
                _method = typesys.GetMethodT(methodname, _plist, list);
            }
            else
            {
                _method = typesys.GetMethod(methodname, list);
            }

            if (_method == null)
                throw new Exception("type can't find:" + method.FullName);

            object returnvar = _method.Invoke(context, _this, _pp);
            bool breturn = method.ReturnType.FullName != "System.Void";
            if (breturn)
            {
                stackCalc.Push(returnvar);
            }
            else if (_this is RefObj && method.Name == ".ctor")
            {
                (_this as RefObj).Set(returnvar);
            }
            _pos = _pos.Next;
        }
        //栈操作
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
        //流程控制
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
        public void Leave(Mono.Cecil.Cil.Instruction pos)
        {
            stackCalc.Clear();
            _pos = pos;
        }
        public void Brtrue(Mono.Cecil.Cil.Instruction pos)
        {
            decimal b = Convert.ToDecimal(stackCalc.Pop());
            //bool b = (bool)stackCalc.Pop();
            if (b > 0)
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
            decimal b = Convert.ToDecimal(stackCalc.Pop());
            if (b <= 0)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        //条件跳转
        public void Beq(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 == num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bne(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 != num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bne_Un(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 != num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bge(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 >= num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bge_Un(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 >= num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bgt(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 > num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Bgt_Un(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 > num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Ble(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 <= num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Ble_Un(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 <= num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Blt(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 < num2;
            if (!b)
            {
                _pos = pos;
            }
            else
            {
                _pos = _pos.Next;
            }
        }
        public void Blt_Un(Mono.Cecil.Cil.Instruction pos)
        {
            object n2 = stackCalc.Pop();
            object n1 = stackCalc.Pop();
            decimal num1 = Convert.ToDecimal(n1);
            decimal num2 = Convert.ToDecimal(n2);
            bool b = num1 < num2;
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
        public enum RefType
        {
            loc,//本地变量槽
            arg,//参数槽
            field//成员变量
        }
        public class RefObj
        {
            public StackFrame frame;
            public int pos;
            public RefType type;
            //public ICLRType _clrtype;
            public IField _field;
            public object _this;
            public RefObj(StackFrame frame, int pos, RefType type)
            {
                this.frame = frame;
                this.pos = pos;
                this.type = type;
            }
            public RefObj(IField field, object _this)
            {
                this.type = RefType.field;
                //this._clrtype = type;
                this._field = field;
                this._this = _this;
            }
            public void Set(object obj)
            {
                if (type == RefType.arg)
                {
                    frame._params[pos] = obj;
                }
                else if (type == RefType.loc)
                {
                    while (frame.slotVar.Count <= pos)
                    {
                        frame.slotVar.Add(null);
                    }
                    frame.slotVar[pos] = obj;
                }
                else if (type == RefType.field)
                {
                    _field.Set(_this, obj);
                }

            }
            public object Get()
            {
                if (type == RefType.arg)
                {
                    return frame._params[pos];
                }
                else if (type == RefType.loc)
                {
                    while (frame.slotVar.Count <= pos)
                    {
                        frame.slotVar.Add(null);
                    }
                    return frame.slotVar[pos];
                }
                else if (type == RefType.field)
                {
                    return _field.Get(_this);
                }
                return null;
            }

        }
        //拿出变量槽的引用
        public void Ldloca(int pos)
        {
            stackCalc.Push(new RefObj(this, pos, RefType.loc));
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
            stackCalc.Push(new RefObj(this, pos, RefType.arg));
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
            decimal outd = num1 + num2;
            if (n1 is sbyte)
            {
                stackCalc.Push((sbyte)outd);
            }
            else if (n1 is byte)
            {
                stackCalc.Push((byte)outd);
            }
            else if (n1 is Int16)
            {
                stackCalc.Push((int)outd);
            }
            else if (n1 is UInt16)
            {
                stackCalc.Push((UInt16)outd);
            }
            else if (n1 is int)
            {
                stackCalc.Push((int)outd);
            }
            else if (n1 is uint)
            {
                stackCalc.Push((int)outd);
            }
            else if (n1 is float)
            {
                stackCalc.Push((float)outd);
            }
            else if (n1 is double)
            {
                stackCalc.Push((double)outd);
            }
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

            stackCalc.Push((Int32)num1);
            _pos = _pos.Next;
        }
        public void Conv_U()
        {

            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt32)num1);
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

        ////数组
        public void NewArr(ThreadContext context, Mono.Cecil.TypeReference type)
        {
            string typename = type.FullName + "[]";
            var _type = context.environment.GetType(typename, type.Module);
            MethodParamList tlist = MethodParamList.OneParam_Int;
            var m = _type.GetMethod(".ctor", tlist);
            var array = m.Invoke(context, null, new object[] { stackCalc.Pop() });
            stackCalc.Push(array);
            _pos = _pos.Next;
        }
        public void LdLen()
        {
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldelema(object obj)
        {
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldelem_I1()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_U1()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }

        public void Ldelem_I2()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_U2()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_I4()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_U4()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }

        public void Ldelem_I8()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_I()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_R4()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_R8()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_Ref()
        {
            int index = (int)stackCalc.Pop();
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_Any(object obj)
        {
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stelem_I()
        {
            var value = (Int32)stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Int32[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_I1()
        {
            var value = (sbyte)stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as sbyte[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_I2()
        {
            var value = Convert.ToDecimal(stackCalc.Pop());
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop();
            if (array is char[])
            {
                (array as char[])[index] = (char)value;
            }
            else if (array is Int16[])
            {
                (array as Int16[])[index] = (Int16)value;
            }

            _pos = _pos.Next;
        }
        public void Stelem_I4()
        {
            var value = Convert.ToDecimal(stackCalc.Pop());
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Int32[];
            array[index] = (Int32)value;
            _pos = _pos.Next;
        }
        public void Stelem_I8()
        {
            var value = (Int64)stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Int64[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_R4()
        {
            var value = (float)stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as float[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_R8()
        {
            var value = (double)stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as double[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_Ref()
        {
            var value = stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Object[];
            array[index] = value;
            _pos = _pos.Next;
        }

        public void Stelem_Any(object obj)
        {
            var value = stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Object[];
            array[index] = value;
            _pos = _pos.Next;
        }

        //寻址类
        public void NewObj(ThreadContext context, Mono.Cecil.MethodDefinition def)
        {
            _pos = _pos.Next;
        }
        public void NewObj(ThreadContext context, Mono.Cecil.MethodReference method)
        {
            object[] _pp = null;
            if (method.Parameters.Count > 0)
            {
                _pp = new object[method.Parameters.Count];
                for (int i = 0; i < _pp.Length; i++)
                {
                    _pp[_pp.Length - 1 - i] = stackCalc.Pop();
                }
            }
            var typesys = context.environment.GetType(method.DeclaringType.FullName, method.Module);

            MethodParamList list = new MethodParamList(context.environment, method);

            object returnvar = typesys.GetMethod(method.Name, list).Invoke(context, null, _pp);

            stackCalc.Push(returnvar);




            _pos = _pos.Next;

        }
        public void Ldfld(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var obj = stackCalc.Pop();

            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);
            if (obj is RefObj)
            {
                obj = (obj as RefObj).Get();
            }
            var value = ff.Get(obj);
            stackCalc.Push(value);
            //System.Type t =obj.GetType();
            _pos = _pos.Next;
        }
        public void Ldflda(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var obj = stackCalc.Pop();

            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);

            stackCalc.Push(new RefObj(ff, obj));

            _pos = _pos.Next;
        }
        public void Ldsfld(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);
            var value = ff.Get(null);
            stackCalc.Push(value);
            //System.Type t =obj.GetType();
            _pos = _pos.Next;
        }
        public void Ldsflda(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);

            stackCalc.Push(new RefObj(ff, null));

            _pos = _pos.Next;
        }
        public void Stfld(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var value = stackCalc.Pop();
            var obj = stackCalc.Pop();
            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);
            if (obj is RefObj)
            {
                obj = (obj as RefObj).Get();
            }
            ff.Set(obj, value);
            _pos = _pos.Next;
        }
        public void Stsfld(ThreadContext context, Mono.Cecil.FieldReference field)
        {
            var value = stackCalc.Pop();
            //var obj = stackCalc.Pop();
            var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            var ff = type.GetField(field.Name);
            ff.Set(null, value);
            _pos = _pos.Next;
        }
        public void Constrained(ThreadContext context, Mono.Cecil.TypeReference obj)
        {

            _pos = _pos.Next;
        }
        public void Isinst(ThreadContext context, Mono.Cecil.TypeReference obj)
        {
            var value = stackCalc.Pop();
            var _type = context.environment.GetType(obj.FullName, obj.Module);
            if (_type.IsInst(value))
                stackCalc.Push(value);
            else
                stackCalc.Push(null);
            _pos = _pos.Next;
        }
        public void Ldtoken(ThreadContext context, Mono.Cecil.FieldDefinition obj)
        {
            string fname=obj.FullName;
            string tfname = obj.FieldType.FullName;
            var _type = context.environment.GetType(obj.DeclaringType.FullName, obj.Module);
            var field = _type.GetField(obj.Name);
            stackCalc.Push(field);
            _pos = _pos.Next;
        }

        public void Conv_Ovf_I1()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((sbyte)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U1()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((byte)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I2()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int16)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U2()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt16)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I4()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int32)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U4()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt32)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I8()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int64)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U8()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt64)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((Int32)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U()
        {
            decimal num1 = Convert.ToDecimal(stackCalc.Pop());

            stackCalc.Push((UInt32)num1);
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I1_Un()
        {
            throw new NotImplementedException();
        }

        public void Conv_Ovf_U1_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_I2_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_U2_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_I4_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_U4_Un()
        {
            throw new NotImplementedException();
        }

        public void Conv_Ovf_I8_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_U8_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_I_Un()
        {
            throw new NotImplementedException();
        }
        public void Conv_Ovf_U_Un()
        {
            throw new NotImplementedException();
        }
    }
}
