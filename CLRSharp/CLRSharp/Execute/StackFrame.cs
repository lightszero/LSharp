using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public class RefFunc
    {
        public IMethod _method;
        public object _this;
        public RefFunc(IMethod _method, object _this)
        {
            this._method = _method;
            this._this = _this;
        }
    }
    /// <summary>
    /// 堆栈帧
    /// 一个堆栈帧，包含一个计算栈，一个临时变量槽，一个参数槽
    /// 模拟虚拟机上的堆栈帧
    /// </summary>
    class StackFrame
    {
        public string Name
        {
            get;
            private set;
        }
        public bool IsStatic
        {
            get;
            private set;
        }
        public StackFrame(string name, bool isStatic)
        {
            this.Name = name;
            this.IsStatic = IsStatic;
        }
        public Mono.Cecil.Cil.Instruction _pos = null;

        Stack<object> stackCalc = new Stack<object>();
        List<object> slotVar = new List<object>();
        object[] _params = null;
        public void SetParams(object[] _p)
        {
            _params = _p;
        }
        CodeBody _body = null;
        public void Init(CodeBody body)
        {
            _body = body;
            if (body.typelistForLoc != null)
            {
                for (int i = 0; i < body.typelistForLoc.Count; i++)
                {
                    ICLRType t = _body.typelistForLoc[i];
                    slotVar.Add(ValueOnStack.Make(t));
                }
            }
        }
        public object Return()
        {
            if (this.stackCalc.Count == 0) return null;
            else return stackCalc.Pop();
        }
        //流程控制
        public void Call(ThreadContext context, IMethod _clrmethod)
        {

            if (_clrmethod == null)//不想被执行的函数
            {
                _pos = _pos.Next;
                return;
            }

            object[] _pp = null;
            object _this = null;

            if (_clrmethod.ParamList != null)
            {
                _pp = new object[_clrmethod.ParamList.Count];
                for (int i = 0; i < _pp.Length; i++)
                {
                    var pp = stackCalc.Pop();
                    if (pp is IBox)
                    {
                        pp = (pp as IBox).BoxDefine();
                    }
                    _pp[_pp.Length - 1 - i] = pp;
                }
            } 


            //if (method.HasThis)
            if (!_clrmethod.isStatic)
            {
                _this = stackCalc.Pop();
            }
            if (_clrmethod.DeclaringType.FullName.Contains("System.Runtime.CompilerServices.RuntimeHelpers") && _clrmethod.Name.Contains("InitializeArray"))
            {
                _pos = _pos.Next;
                return;
            }
            if (_clrmethod.DeclaringType.FullName.Contains("System.Type") && _clrmethod.Name.Contains("GetTypeFromHandle"))
            {
                stackCalc.Push(_pp[0]);
                _pos = _pos.Next;
                return;
            }
            if (_this is RefObj && _clrmethod.Name != ".ctor")
            {
                _this = (_this as RefObj).Get();

            }
            if (_this is IBox)
            {
                _this = (_this as IBox).BoxDefine();
            }
            object returnvar = _clrmethod.Invoke(context, _this, _pp);

            // bool breturn = false;
            if (_clrmethod.ReturnType != null && _clrmethod.ReturnType.FullName != "System.Void")
            {
                if ((returnvar is IBox) == false)
                {
                    var type = ValueOnStack.Make(_clrmethod.ReturnType);
                    if (type != null)
                    {
                        type.SetDirect(returnvar);
                        returnvar = type;
                    }
                }
                stackCalc.Push(returnvar);
            }

            else if (_this is RefObj && _clrmethod.Name == ".ctor")
            {
                (_this as RefObj).Set(returnvar);
            }
            _pos = _pos.Next;
            return;

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
            object obj = stackCalc.Pop();
            IBox box = obj as IBox;
            if (box != null)
                stackCalc.Push(box.BoxDefine());
            else
                stackCalc.Push(obj);
            _pos = _pos.Next;
        }
        public void Unbox()
        {
            object obj = stackCalc.Pop();
            var box = ValueOnStack.Make(obj.GetType());
            if (box != null)
            {
                stackCalc.Push(box);
            }
            else
            {
                stackCalc.Push(obj);
            }
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
            object obj = stackCalc.Pop();
            bool b = false;
            if (obj != null)
            {
                if (obj is BoxInt32)
                {
                    BoxInt32 box = obj as BoxInt32;
                    b = box.value > 0;
                }
                else if (obj is BoxInt64)
                {
                    BoxInt64 box = obj as BoxInt64;
                    b = box.value > 0;
                }
                else if (obj.GetType().IsClass)
                {
                    b = true;
                }
                else if (obj is bool)
                {
                    b = (bool)obj;
                }
                else
                {
                    b = Convert.ToDecimal(obj) > 0;
                }
            }
            //decimal b = Convert.ToDecimal(stackCalc.Pop());
            //bool b = (bool)stackCalc.Pop();
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
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_eq(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bne(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_ne(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bne_Un(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_ne_Un(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bge(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_ge(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bge_Un(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_ge_Un(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bgt(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_gt(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Bgt_Un(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_gt_Un(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Ble(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_le(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Ble_Un(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_le_Un(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Blt(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_lt(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        public void Blt_Un(Mono.Cecil.Cil.Instruction pos)
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            if (n1.logic_lt_Un(n2))
            {
                _pos = _pos.Next;
            }
            else
            {
                _pos = pos;
            }
        }
        //加载常量
        public void Ldc_I4(int v)//int32
        {
            BoxInt32 box = new BoxInt32(NumberType.INT32);
            box.value = v;
            stackCalc.Push(box);
            _pos = _pos.Next;

        }

        public void Ldc_I8(Int64 v)//int64
        {
            BoxInt64 box = new BoxInt64(NumberType.INT64);
            box.value = v;
            stackCalc.Push(box);
            _pos = _pos.Next;
        }
        public void Ldc_R4(float v)
        {
            BoxDouble box = new BoxDouble(NumberType.FLOAT);
            box.value = v;
            stackCalc.Push(box);
            _pos = _pos.Next;
        }
        public void Ldc_R8(double v)
        {
            BoxDouble box = new BoxDouble(NumberType.DOUBLE);
            box.value = v;
            stackCalc.Push(box);
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
            IBox box = slotVar[pos] as IBox;
            if (box == null)
            {
                slotVar[pos] = v;
            }
            else
            {
                if (v is IBox)
                    box.Set(v as IBox);
                else
                    box.SetDirect(v);
            }
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
            object p = null;
            if (_params != null)
                p = _params[pos];
            stackCalc.Push(p);
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
            var obj2 = stackCalc.Pop();
            var obj1 = stackCalc.Pop();
            IBox n2 = obj2 as IBox;
            IBox n1 = obj1 as IBox;
            bool beq = false;
            if (n1 == null || n2 == null)
            //if (obj1 == null || obj2 == null)
            {
                if (obj1 != null)
                    beq = obj1.Equals(obj2);
                else
                    beq = (obj1 == obj2);
            }
            else
            {
                beq = n1.logic_eq(n2);
            }



            stackCalc.Push(ValueOnStack.MakeBool(beq));
            _pos = _pos.Next;
        }
        public void Cgt()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;


            stackCalc.Push(ValueOnStack.MakeBool(n1.logic_gt(n2)));
            _pos = _pos.Next;
        }
        public void Cgt_Un()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;

            stackCalc.Push(ValueOnStack.MakeBool(n1.logic_gt_Un(n2)));
            _pos = _pos.Next;
        }
        public void Clt()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            stackCalc.Push(ValueOnStack.MakeBool(n1.logic_lt(n2)));
            _pos = _pos.Next;
        }
        public void Clt_Un()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            stackCalc.Push(ValueOnStack.MakeBool(n1.logic_lt_Un(n2)));
            _pos = _pos.Next;
        }
        public void Ckfinite()
        {
            object n1 = stackCalc.Pop();
            if (n1 is float)
            {
                float v = (float)n1;
                stackCalc.Push(float.IsInfinity(v) || float.IsNaN(v) ? 1 : 0);
            }
            else
            {
                double v = (double)n1;
                stackCalc.Push(double.IsInfinity(v) || double.IsNaN(v) ? 1 : 0);
            }
            _pos = _pos.Next;
        }
        //算术操作
        public void Add()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1.Add(n2);
            stackCalc.Push(n1);
            _pos = _pos.Next;
        }
        public void Sub()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1.Sub(n2);
            stackCalc.Push(n1);
            _pos = _pos.Next;
        }
        public void Mul()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1.Mul(n2);
            stackCalc.Push(n1);
            _pos = _pos.Next;
        }
        public void Div()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1.Div(n2);
            stackCalc.Push(n1);
            _pos = _pos.Next;
        }
        public void Div_Un()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1.Div(n2);//!!! _un
            stackCalc.Push(n1);
            _pos = _pos.Next;
        }
        public void Rem()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1 = n1.Mod_New(n2);
            stackCalc.Push(n1);
            _pos = _pos.Next; ;
        }
        public void Rem_Un()
        {
            IBox n2 = stackCalc.Pop() as IBox;
            IBox n1 = stackCalc.Pop() as IBox;
            n1 = n1.Mod_New(n2);//!!!_un
            stackCalc.Push(n1);
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

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.SBYTE));
            }
            else
            {
                stackCalc.Push((sbyte)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_U1()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.BYTE));
            }
            else
            {
                stackCalc.Push((byte)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_I2()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT16));
            }
            else
            {
                stackCalc.Push((Int16)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_U2()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT16));
            }
            else
            {
                stackCalc.Push((UInt16)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_I4()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT32));
            }
            else
            {
                stackCalc.Push((Int32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_U4()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT32));
            }
            else
            {
                stackCalc.Push((UInt32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_I8()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT64));
            }
            else
            {
                stackCalc.Push((Int64)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_U8()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT64));
            }
            else
            {
                stackCalc.Push((UInt64)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_I()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT32));
            }
            else
            {
                stackCalc.Push((Int32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_U()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT32));
            }
            else
            {
                stackCalc.Push((UInt32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_R4()
        {


            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.FLOAT));
            }
            else
            {
                stackCalc.Push((float)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_R8()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.DOUBLE));
            }
            else
            {
                stackCalc.Push((double)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_R_Un()
        {

            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.FLOAT));
            }
            else
            {
                stackCalc.Push((float)num1);
            }
            _pos = _pos.Next;
        }

        ////数组
        public void NewArr(ThreadContext context, IMethod newForArray)
        {
            //string typename = type.FullName + "[]";
            //var _type = context.environment.GetType(typename, type.Module);
            //MethodParamList tlist = MethodParamList.MakeList_OneParam_Int(context.environment);
            //var m = _type.GetMethod(".ctor", tlist);
            var objv = stackCalc.Pop();
            if (objv is IBox) objv = (objv as IBox).BoxDefine();
            var array = newForArray.Invoke(context, null, new object[] { objv });
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
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_U1()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }

        public void Ldelem_I2()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_U2()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_I4()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
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
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_I()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_R4()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_R8()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_Ref()
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Array array = stackCalc.Pop() as Array;
            stackCalc.Push(array.GetValue(index));
            _pos = _pos.Next;
        }
        public void Ldelem_Any(object obj)
        {
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is IBox))
            {
                index = (indexobj as IBox).ToInt();
            }
            else
            {
                index = (int)indexobj;
            }
            Object[] array = stackCalc.Pop() as Object[];
            stackCalc.Push(array[index]);
            _pos = _pos.Next;
        }
        public void Stelem_I()
        {
            var obj = stackCalc.Pop();
            int value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToInt();
            }
            else
            {
                value = (Int32)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as Int32[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_I1()
        {
            var obj = stackCalc.Pop();
            int value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToInt();
            }
            else
            {
                value = (sbyte)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as sbyte[];
            array[index] = (sbyte)value;
            _pos = _pos.Next;
        }
        public void Stelem_I2()
        {
            var obj = stackCalc.Pop();
            int value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToInt();
            }
            else
            {
                value = (Int16)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
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
            var obj = stackCalc.Pop();
            int value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToInt();
            }
            else
            {
                value = (Int32)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as Int32[];
            array[index] = (Int32)value;
            _pos = _pos.Next;
        }
        public void Stelem_I8()
        {
            var obj = stackCalc.Pop();
            long value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToInt64();
            }
            else
            {
                value = (Int64)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as Int64[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_R4()
        {
            var obj = stackCalc.Pop();
            float value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToFloat();
            }
            else
            {
                value = (float)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as float[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_R8()
        {
            var obj = stackCalc.Pop();
            double value = 0;
            if (obj is IBox)
            {
                value = (obj as IBox).ToDouble();
            }
            else
            {
                value = (double)obj;
            }
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as double[];
            array[index] = value;
            _pos = _pos.Next;
        }
        public void Stelem_Ref()
        {
            var value = stackCalc.Pop();
            var indexobj = stackCalc.Pop();
            int index = 0;
            if ((indexobj is BoxInt32))
            {
                index = (indexobj as BoxInt32).value;
            }
            else if ((indexobj is BoxInt64))
            {
                index = (int)(indexobj as BoxInt64).value;
            }
            else
            {
                index = (int)indexobj;
            }
            var array = stackCalc.Pop() as Object[];

            array[index] = value;
            _pos = _pos.Next;
        }

        public void Stelem_Any()
        {
            var value = stackCalc.Pop();
            var index = (int)stackCalc.Pop();
            var array = stackCalc.Pop() as Object[];
            array[index] = value;
            _pos = _pos.Next;
        }

        //寻址类
        public void NewObj(ThreadContext context, IMethod _clrmethod)
        {
            //MethodParamList list = new MethodParamList(context.environment, method);
            object[] _pp = null;
            if (_clrmethod.ParamList != null && _clrmethod.ParamList.Count > 0)
            {
                _pp = new object[_clrmethod.ParamList.Count];
                for (int i = 0; i < _pp.Length; i++)
                {
                    var obj = stackCalc.Pop();
                    if (obj is IBox)
                    {
                        obj = (obj as IBox).BoxDefine();
                    }
                    _pp[_pp.Length - 1 - i] = obj;
                }
            }
            //var typesys = context.environment.GetType(method.DeclaringType.FullName, method.Module);
            object returnvar = _clrmethod.Invoke(context, null, _pp);

            stackCalc.Push(returnvar);

            _pos = _pos.Next;
        }
        //public void NewObj(ThreadContext context, Mono.Cecil.MethodReference method)
        //{
        //    object[] _pp = null;
        //    if (method.Parameters.Count > 0)
        //    {
        //        _pp = new object[method.Parameters.Count];
        //        for (int i = 0; i < _pp.Length; i++)
        //        {
        //            _pp[_pp.Length - 1 - i] = stackCalc.Pop();
        //        }
        //    }
        //    var typesys = context.environment.GetType(method.DeclaringType.FullName, method.Module);

        //    MethodParamList list = new MethodParamList(context.environment, method);

        //    object returnvar = typesys.GetMethod(method.Name, list).Invoke(context, null, _pp);

        //    stackCalc.Push(returnvar);




        //    _pos = _pos.Next;

        //}
        public void Ldfld(ThreadContext context, IField field)
        {
            var obj = stackCalc.Pop();

            //var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //ar ff = type.GetField(field.Name);
            if (obj is RefObj)
            {
                obj = (obj as RefObj).Get();
            }
            var value = field.Get(obj);
            IBox box = ValueOnStack.Make(field.FieldType);
            if (box != null)
            {
                box.SetDirect(value);
                value = box;
            }
            stackCalc.Push(value);
            //System.Type t =obj.GetType();
            _pos = _pos.Next;
        }
        public void Ldflda(ThreadContext context, IField field)
        {
            var obj = stackCalc.Pop();

            // var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //var ff = type.GetField(field.Name);

            stackCalc.Push(new RefObj(field, obj));

            _pos = _pos.Next;
        }
        public void Ldsfld(ThreadContext context, IField field)
        {
            //var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //var ff = type.GetField(field.Name);
            var value = field.Get(null);
            IBox box = ValueOnStack.Make(field.FieldType);
            if (box != null)
            {
                box.SetDirect(value);
                value = box;
            }
            stackCalc.Push(value);
            //System.Type t =obj.GetType();
            _pos = _pos.Next;
        }
        public void Ldsflda(ThreadContext context, IField field)
        {
            //var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //var ff = type.GetField(field.Name);

            stackCalc.Push(new RefObj(field, null));

            _pos = _pos.Next;
        }
        public void Stfld(ThreadContext context, IField field)
        {
            var value = stackCalc.Pop();

            var obj = stackCalc.Pop();
            //var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //var ff = type.GetField(field.Name);
            if (obj is RefObj)
            {
                var _this = (obj as RefObj).Get();
                if (_this == null && !field.isStatic)
                {
                    (obj as RefObj).Set(field.DeclaringType.InitObj());
                }
                obj = (obj as RefObj).Get();
            }
            if (value is IBox)
            {
                value = (value as IBox).BoxDefine();
            }
            field.Set(obj, value);
            _pos = _pos.Next;
        }
        public void Stsfld(ThreadContext context, IField field)
        {
            var value = stackCalc.Pop();
            //var obj = stackCalc.Pop();

            if (value is IBox)
            {
                value = (value as IBox).BoxDefine();
            }
            //var type = context.environment.GetType(field.DeclaringType.FullName, field.Module);
            //var ff = type.GetField(field.Name);
            field.Set(null, value);

            _pos = _pos.Next;
        }
        public void Constrained(ThreadContext context, ICLRType obj)
        {

            _pos = _pos.Next;
        }
        public void Isinst(ThreadContext context, ICLRType _type)
        {
            var value = stackCalc.Pop();
            //var _type = context.environment.GetType(obj.FullName, obj.Module);
            if (_type.IsInst(value))
                stackCalc.Push(value);
            else
                stackCalc.Push(null);
            _pos = _pos.Next;
        }
        public void Ldtoken(ThreadContext context, object token)
        {
            //string fname = obj.FullName;
            //string tfname = obj.FieldType.FullName;
            //var _type = context.environment.GetType(obj.DeclaringType.FullName, obj.Module);
            //var field = _type.GetField(obj.Name);
            stackCalc.Push(token);
            _pos = _pos.Next;
        }

        public void Conv_Ovf_I1()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.SBYTE));
            }
            else
            {
                stackCalc.Push((sbyte)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U1()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.BYTE));
            }
            else
            {
                stackCalc.Push((byte)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I2()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT16));
            }
            else
            {
                stackCalc.Push((Int16)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U2()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT16));
            }
            else
            {
                stackCalc.Push((Int16)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I4()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT32));
            }
            else
            {
                stackCalc.Push((Int32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U4()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT32));
            }
            else
            {
                stackCalc.Push((UInt32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I8()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT64));
            }
            else
            {
                stackCalc.Push((Int64)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U8()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT64));
            }
            else
            {
                stackCalc.Push((Int64)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_I()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.INT32));
            }
            else
            {
                stackCalc.Push((Int32)num1);
            }
            _pos = _pos.Next;
        }
        public void Conv_Ovf_U()
        {
            object num1 = stackCalc.Pop();
            IBox b = num1 as IBox;
            if (b != null)
            {
                stackCalc.Push(ValueOnStack.Convert(b, NumberType.UINT32));
            }
            else
            {
                stackCalc.Push((UInt32)num1);
            }
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

        public void Ldftn(ThreadContext context, IMethod method)
        {
            stackCalc.Push(new RefFunc(method, null));
            //throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldvirtftn(ThreadContext context, IMethod method)
        {
            object _this = stackCalc.Pop();
            stackCalc.Push(new RefFunc(method, _this));

            _pos = _pos.Next;
        }
        public void Ldarga(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Calli(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }


        public void Break(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Starg_S(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldnull()
        {
            stackCalc.Push(null);
            _pos = _pos.Next;
        }
        public void Jmp(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }

        public void Switch(ThreadContext context, Mono.Cecil.Cil.Instruction[] poss)
        {
            var indexobj = stackCalc.Pop();
            uint pos = 0;
            if (indexobj is IBox)
            {
                pos = (indexobj as IBox).ToUint();
            }
            else
            {
                pos = (uint)(int)indexobj;
            }
            if (pos >= poss.Length)
            {
                _pos = _pos.Next;

            }
            else
            {
                _pos = poss[pos];
            }
        }
        public void Ldind_I1(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_U1(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_I2(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_U2(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_I4(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_U4(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_I8(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_I(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_R4(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_R8(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldind_Ref(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_Ref(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_I1(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_I2(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_I4(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_I8(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_R4(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_R8(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void And(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Or(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Xor(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Shl(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Shr(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Shr_Un(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Not(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Cpobj(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Ldobj(ThreadContext context, object obj)
        {
            stackCalc.Push(obj);
            //Type t = obj.GetType();
            //throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Castclass(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Throw(ThreadContext context, object obj)
        {
            Exception exc = stackCalc.Pop() as Exception;
            throw exc;
            //_pos = _pos.Next;
        }
        public void Stobj(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Refanyval(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Mkrefany(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }

        public void Add_Ovf(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Add_Ovf_Un(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Mul_Ovf(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Mul_Ovf_Un(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Sub_Ovf(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Sub_Ovf_Un(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Endfinally(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Stind_I(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Arglist(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }

        public void Starg(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Localloc(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Endfilter(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Unaligned(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Volatile(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Tail(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Initobj(ThreadContext context, ICLRType _type)
        {
            RefObj _this = stackCalc.Pop() as RefObj;

            //var typesys = context.environment.GetType(method.DeclaringType.FullName, method.Module);
            var _object = _type.InitObj();

            _this.Set(_object);

            _pos = _pos.Next;
        }
        public void Cpblk(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Initblk(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void No(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Rethrow(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Sizeof(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Refanytype(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
        public void Readonly(ThreadContext context, object obj)
        {
            Type t = obj.GetType();
            throw new NotImplementedException();
            _pos = _pos.Next;
        }
    }
}
