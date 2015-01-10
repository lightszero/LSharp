using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    /// <summary>
    /// 线程上下文
    /// 一个线程上下文表示一次调用，直到结束
    /// </summary>
    public class ThreadContext
    {
        public ICLRSharp_Environment environment
        {
            get;
            private set;
        }
        public int DebugLevel
        {
            get;
            private set;
        }
        public ThreadContext(ICLRSharp_Environment env)
        {
            this.environment = env;
            DebugLevel = 0;
        }
        public ThreadContext(ICLRSharp_Environment env, int DebugLevel)
        {
            this.environment = env;
            this.DebugLevel = DebugLevel;
        }
        Stack<StackFrame> stacks = new Stack<StackFrame>();
        public object ExecuteFunc(Mono.Cecil.MethodDefinition func, object _this, object[] _params)
        {
            if(this.DebugLevel>=9)
            {
                environment.logger.Log("<Call>::" + func.ToString());

            }
            StackFrame stack = new StackFrame();
            stacks.Push(stack);
            if (func.Name == ".ctor")
            {
                //CLRSharp_Instance pthis = new CLRSharp_Instance(GetType(func.ReturnType) as Type_Common_CLRSharp);
                //StackFrame.RefObj pthis = new StackFrame.RefObj(stack, 0, StackFrame.RefType.arg);
                stack.SetParams(new object[] { _this });
                RunCode(stack, func.Body.Instructions);
                if (this.DebugLevel >= 9)
                {
                    environment.logger.Log("<CallEnd>");

                }
                var ret = stacks.Pop().Return();
                return _this;
            }
            else
            {
                object[] pp = null;
                if(!func.IsStatic)
                {
                    pp =new object[_params.Length+1];
                    pp[0] = _this;
                    _params.CopyTo(pp, 1);
                }
                else
                {
                    pp = _params;
                }
                stack.SetParams(pp);
                RunCode(stack, func.Body.Instructions);
                var ret=stacks.Pop().Return();
                if (this.DebugLevel >= 9)
                {
                    environment.logger.Log("<CallEnd>");

                }
                return ret;
            }

     
        }

        ICLRType GetType(object token)
        {

            Mono.Cecil.ModuleDefinition module = null;
            string typename = null;
            if (token is Mono.Cecil.TypeDefinition)
            {
                Mono.Cecil.TypeDefinition _def = (token as Mono.Cecil.TypeDefinition);
                module = _def.Module;
                typename = _def.FullName;
            }
            else if (token is Mono.Cecil.TypeReference)
            {
                Mono.Cecil.TypeReference _ref = (token as Mono.Cecil.TypeReference);
                module = _ref.Module;
                typename = _ref.FullName;
            }
            else
            {
                throw new NotImplementedException();
            }
            return environment.GetType(typename, module);
        }
        IMethod GetMethod(object token)
        {
            Mono.Cecil.ModuleDefinition module = null;
            string methodname = null;
            string typename = null;
            MethodParamList genlist = null;
            MethodParamList list = null;
            if (token is Mono.Cecil.MethodReference)
            {
                Mono.Cecil.MethodReference _ref = (token as Mono.Cecil.MethodReference);
                module = _ref.Module;
                methodname = _ref.Name;
                typename = _ref.DeclaringType.FullName;
                list = new MethodParamList(environment, _ref);
                if (_ref.IsGenericInstance)
                {
                    Mono.Cecil.GenericInstanceMethod gmethod = _ref as Mono.Cecil.GenericInstanceMethod;
                    genlist = new MethodParamList(environment, gmethod);

                }
            }
            else if (token is Mono.Cecil.MethodDefinition)
            {
                Mono.Cecil.MethodDefinition _def = token as Mono.Cecil.MethodDefinition;
                module = _def.Module;
                methodname = _def.Name;
                typename = _def.DeclaringType.FullName;
                list = new MethodParamList(environment, _def);
                if (_def.IsGenericInstance)
                {
                    throw new NotImplementedException();
                    //Mono.Cecil.GenericInstanceMethod gmethod = _def as Mono.Cecil.GenericInstanceMethod;
                    //genlist = new MethodParamList(environment, gmethod);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            var typesys = environment.GetType(typename, module);
            if (typesys == null)
                throw new Exception("type can't find:" + typename);


            IMethod _method = null;
            if (genlist != null)
            {
                _method = typesys.GetMethodT(methodname, genlist, list);
            }
            else
            {
                _method = typesys.GetMethod(methodname, list);
            }

            return _method;
        }
        void RunCode(StackFrame stack, Mono.Collections.Generic.Collection<Mono.Cecil.Cil.Instruction> codes)
        {
            stack._pos = codes[0];
            while (true)
            {
                var code = stack._pos;
                if (DebugLevel >= 9)
                {
                    environment.logger.Log(code.ToString());
                }
                switch (code.OpCode.Code)
                {

                    ///////////
                    //流程控制

                    case Code.Nop:
                        stack.Nop();
                        break;
                    case Code.Ret:
                        stack.Ret();
                        return;
                    //流程控制之goto
                    case Code.Br:
                        stack.Br(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Br_S:
                        stack.Br(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Brtrue:
                        stack.Brtrue(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Brtrue_S:
                        stack.Brtrue(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Brfalse:
                        stack.Brfalse(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Brfalse_S:
                        stack.Brfalse(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Leave:
                        stack.Leave(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Leave_S:
                        stack.Ret();
                        stack.Leave(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    //比较流程控制
                    case Code.Beq:
                        stack.Beq(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Beq_S:
                        stack.Beq(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bne_Un:
                        stack.Bne_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bne_Un_S:
                        stack.Bne_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bge:
                        stack.Bge(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bge_S:
                        stack.Bge(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bge_Un:
                        stack.Bge_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bge_Un_S:
                        stack.Bge_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bgt:
                        stack.Bgt(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bgt_S:
                        stack.Bgt(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bgt_Un:
                        stack.Bgt_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Bgt_Un_S:
                        stack.Bge_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Ble:
                        stack.Ble(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Ble_S:
                        stack.Ble(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Ble_Un:
                        stack.Ble_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Ble_Un_S:
                        stack.Ble_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Blt:
                        stack.Blt(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Blt_S:
                        stack.Blt(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Blt_Un:
                        stack.Blt_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    case Code.Blt_Un_S:
                        stack.Ble_Un(code.Operand as Mono.Cecil.Cil.Instruction);
                        break;
                    //逻辑计算
                    case Code.Ceq:
                        stack.Ceq();
                        break;
                    case Code.Cgt:
                        stack.Cgt();
                        break;
                    case Code.Cgt_Un:
                        stack.Cgt_Un();
                        break;
                    case Code.Clt:
                        stack.Clt();
                        break;
                    case Code.Clt_Un:
                        stack.Clt_Un();
                        break;
                    case Code.Ckfinite:
                        stack.Ckfinite();
                        break;
                    //常量加载
                    case Code.Ldc_I4:
                        stack.Ldc_I4(code.Operand);
                        break;
                    case Code.Ldc_I4_S:
                        stack.Ldc_I4_S(code.Operand);
                        break;
                    case Code.Ldc_I4_M1:
                        stack.Ldc_I4(-1);
                        break;
                    case Code.Ldc_I4_0:
                        stack.Ldc_I4(0);
                        break;
                    case Code.Ldc_I4_1:
                        stack.Ldc_I4(1);
                        break;
                    case Code.Ldc_I4_2:
                        stack.Ldc_I4(2);
                        break;
                    case Code.Ldc_I4_3:
                        stack.Ldc_I4(3);
                        break;
                    case Code.Ldc_I4_4:
                        stack.Ldc_I4(4);
                        break;
                    case Code.Ldc_I4_5:
                        stack.Ldc_I4(5);
                        break;
                    case Code.Ldc_I4_6:
                        stack.Ldc_I4(6);
                        break;
                    case Code.Ldc_I4_7:
                        stack.Ldc_I4(7);
                        break;
                    case Code.Ldc_I4_8:
                        stack.Ldc_I4(8);
                        break;
                    case Code.Ldc_I8:
                        stack.Ldc_I8(code.Operand);
                        break;
                    case Code.Ldc_R4:
                        stack.Ldc_R4(code.Operand);
                        break;
                    case Code.Ldc_R8:
                        stack.Ldc_R8(code.Operand);
                        break;

                    //定义为临时变量
                    case Code.Stloc:
                        stack.Stloc((int)code.Operand);
                        break;
                    case Code.Stloc_S:
                        stack.Stloc(((VariableDefinition)code.Operand).Index);
                        break;
                    case Code.Stloc_0:
                        stack.Stloc(0);
                        break;
                    case Code.Stloc_1:
                        stack.Stloc(1);
                        break;
                    case Code.Stloc_2:
                        stack.Stloc(2);
                        break;
                    case Code.Stloc_3:
                        stack.Stloc(3);
                        break;
                    //从临时变量加载
                    case Code.Ldloc:
                        stack.Ldloc((int)code.Operand);
                        break;
                    case Code.Ldloc_S:
                        stack.Ldloc(((VariableDefinition)code.Operand).Index);
                        break;
                    case Code.Ldloc_0:
                        stack.Ldloc(0);
                        break;
                    case Code.Ldloc_1:
                        stack.Ldloc(1);
                        break;
                    case Code.Ldloc_2:
                        stack.Ldloc(2);
                        break;
                    case Code.Ldloc_3:
                        stack.Ldloc(3);
                        break;
                    case Code.Ldloca:
                        stack.Ldloca(((VariableDefinition)code.Operand).Index);
                        break;
                    case Code.Ldloca_S:
                        stack.Ldloca(((VariableDefinition)code.Operand).Index);
                        break;
                    //加载字符串
                    case Code.Ldstr:
                        stack.Ldstr(code.Operand as string);
                        break;
                    //呼叫函数
                    case Code.Call:
                        stack.Call(this, GetMethod(code.Operand));
                        break;
                    case Code.Callvirt:
                        stack.Call(this, GetMethod(code.Operand));
                        break;
                    //算术指令
                    case Code.Add:
                        stack.Add();
                        break;
                    case Code.Sub:
                        stack.Sub();
                        break;
                    case Code.Mul:
                        stack.Mul();
                        break;
                    case Code.Div:
                        stack.Div();
                        break;
                    case Code.Div_Un:
                        stack.Div_Un();
                        break;
                    case Code.Rem:
                        stack.Rem();
                        break;
                    case Code.Rem_Un:
                        stack.Rem_Un();
                        break;
                    case Code.Neg:
                        stack.Neg();
                        break;

                    //装箱
                    case Code.Box:
                        stack.Box();
                        break;
                    case Code.Unbox:
                        stack.Unbox();
                        break;
                    case Code.Unbox_Any:
                        stack.Unbox_Any();
                        break;

                    //加载参数
                    case Code.Ldarg:
                        stack.Ldarg((int)code.Operand);
                        break;
                    case Code.Ldarg_S:
                        stack.Ldarg((byte)code.Operand);
                        break;
                    case Code.Ldarg_0:
                        stack.Ldarg(0);
                        break;
                    case Code.Ldarg_1:
                        stack.Ldarg(1);
                        break;
                    case Code.Ldarg_2:
                        stack.Ldarg(2);
                        break;
                    case Code.Ldarg_3:
                        stack.Ldarg(3);
                        break;
                    //转换
                    case Code.Conv_I1:
                        stack.Conv_I1();
                        break;
                    case Code.Conv_U1:
                        stack.Conv_U1();
                        break;
                    case Code.Conv_I2:
                        stack.Conv_I2();
                        break;
                    case Code.Conv_U2:
                        stack.Conv_U2();
                        break;
                    case Code.Conv_I4:
                        stack.Conv_I4();
                        break;
                    case Code.Conv_U4:
                        stack.Conv_U4();
                        break;
                    case Code.Conv_I8:
                        stack.Conv_I8();
                        break;
                    case Code.Conv_U8:
                        stack.Conv_U8();
                        break;
                    case Code.Conv_I:
                        stack.Conv_I();
                        break;
                    case Code.Conv_U:
                        stack.Conv_U();
                        break;
                    case Code.Conv_R4:
                        stack.Conv_R4();
                        break;
                    case Code.Conv_R8:
                        stack.Conv_R8();
                        break;
                    case Code.Conv_R_Un:
                        stack.Conv_R_Un();
                        break;
                    case Code.Conv_Ovf_I1:
                        stack.Conv_Ovf_I1();
                        break;
                    case Code.Conv_Ovf_U1:
                        stack.Conv_Ovf_U1();
                        break;
                    case Code.Conv_Ovf_I2:
                        stack.Conv_Ovf_I2();
                        break;
                    case Code.Conv_Ovf_U2:
                        stack.Conv_Ovf_U2();
                        break;
                    case Code.Conv_Ovf_I4:
                        stack.Conv_Ovf_I4();
                        break;
                    case Code.Conv_Ovf_U4:
                        stack.Conv_Ovf_U4();
                        break;

                    case Code.Conv_Ovf_I8:
                        stack.Conv_Ovf_I8();
                        break;
                    case Code.Conv_Ovf_U8:
                        stack.Conv_Ovf_U8();
                        break;
                    case Code.Conv_Ovf_I:
                        stack.Conv_Ovf_I();
                        break;
                    case Code.Conv_Ovf_U:
                        stack.Conv_Ovf_U();
                        break;
                    case Code.Conv_Ovf_I1_Un:
                        stack.Conv_Ovf_I1_Un();
                        break;

                    case Code.Conv_Ovf_U1_Un:
                        stack.Conv_Ovf_U1_Un();
                        break;
                    case Code.Conv_Ovf_I2_Un:
                        stack.Conv_Ovf_I2_Un();
                        break;
                    case Code.Conv_Ovf_U2_Un:
                        stack.Conv_Ovf_U2_Un();
                        break;
                    case Code.Conv_Ovf_I4_Un:
                        stack.Conv_Ovf_I4_Un();
                        break;
                    case Code.Conv_Ovf_U4_Un:
                        stack.Conv_Ovf_U4_Un();
                        break;

                    case Code.Conv_Ovf_I8_Un:
                        stack.Conv_Ovf_I8_Un();
                        break;
                    case Code.Conv_Ovf_U8_Un:
                        stack.Conv_Ovf_U8_Un();
                        break;
                    case Code.Conv_Ovf_I_Un:
                        stack.Conv_Ovf_I_Un();
                        break;
                    case Code.Conv_Ovf_U_Un:
                        stack.Conv_Ovf_U_Un();
                        break;
                    //数组
                    case Code.Newarr:
                        stack.NewArr(this, (Mono.Cecil.TypeReference)code.Operand);
                        break;
                    case Code.Ldlen:
                        stack.LdLen();
                        break;
                    case Code.Ldelema:
                        stack.Ldelema((Mono.Cecil.TypeReference)code.Operand);
                        break;
                    case Code.Ldelem_I1:
                        stack.Ldelem_I1();
                        break;
                    case Code.Ldelem_U1:
                        stack.Ldelem_U1();
                        break;
                    case Code.Ldelem_I2:
                        stack.Ldelem_I2();
                        break;
                    case Code.Ldelem_U2:
                        stack.Ldelem_U2();
                        break;
                    case Code.Ldelem_I4:
                        stack.Ldelem_I4();
                        break;
                    case Code.Ldelem_U4:
                        stack.Ldelem_U4();
                        break;
                    case Code.Ldelem_I8:
                        stack.Ldelem_I8();
                        break;
                    case Code.Ldelem_I:
                        stack.Ldelem_I();
                        break;
                    case Code.Ldelem_R4:
                        stack.Ldelem_R4();
                        break;
                    case Code.Ldelem_R8:
                        stack.Ldelem_R8();
                        break;
                    case Code.Ldelem_Ref:
                        stack.Ldelem_Ref();
                        break;
                    case Code.Ldelem_Any:
                        stack.Ldelem_Any((Mono.Cecil.TypeReference)code.Operand);
                        break;

                    case Code.Stelem_I:
                        stack.Stelem_I();
                        break;
                    case Code.Stelem_I1:
                        stack.Stelem_I1();
                        break;
                    case Code.Stelem_I2:
                        stack.Stelem_I2();
                        break;
                    case Code.Stelem_I4:
                        stack.Stelem_I4();
                        break;
                    case Code.Stelem_I8:
                        stack.Stelem_I8();
                        break;
                    case Code.Stelem_R4:
                        stack.Stelem_R4();
                        break;
                    case Code.Stelem_R8:
                        stack.Stelem_R8();
                        break;
                    case Code.Stelem_Ref:
                        stack.Stelem_Ref();
                        break;
                    case Code.Stelem_Any:
                        stack.Stelem_Any((Mono.Cecil.TypeReference)code.Operand);
                        break;

                    case Code.Newobj:
                        stack.NewObj(this, GetMethod(code.Operand));
                        break;

                    case Code.Dup:
                        stack.Dup();
                        break;
                    case Code.Pop:
                        stack.Pop();
                        break;

                    case Code.Ldfld:
                        stack.Ldfld(this, code.Operand as Mono.Cecil.FieldReference);
                        break;
                    case Code.Ldflda:
                        stack.Ldflda(this, code.Operand as Mono.Cecil.FieldReference);
                        break;
                    case Code.Ldsfld:
                        stack.Ldsfld(this, code.Operand as Mono.Cecil.FieldReference);
                        break;
                    case Code.Ldsflda:
                        stack.Ldsflda(this, code.Operand as Mono.Cecil.FieldReference);
                        break;
                    case Code.Stfld:
                        stack.Stfld(this, code.Operand as Mono.Cecil.FieldReference);
                        break;
                    case Code.Stsfld:
                        stack.Stsfld(this, code.Operand as Mono.Cecil.FieldReference);
                        break;


                    case Code.Constrained:
                        stack.Constrained(this, code.Operand as Mono.Cecil.TypeReference);
                        break;

                    case Code.Isinst:
                        stack.Isinst(this, code.Operand as Mono.Cecil.TypeReference);
                        break;
                    case Code.Ldtoken:
                        stack.Ldtoken(this, code.Operand as Mono.Cecil.FieldDefinition);
                        break;

                    case Code.Ldftn:
                        stack.Ldftn(this, GetMethod(code.Operand));
                        break;
                    case Code.Ldvirtftn:
                        stack.Ldvirtftn(this, GetMethod(code.Operand));
                        break;
                    case Code.Ldarga:
                        stack.Ldarga(this, code.Operand);
                        break;
                    case Code.Ldarga_S:
                        stack.Ldarga(this, code.Operand);
                        break;
                    case Code.Calli:
                        stack.Calli(this, code.Operand);
                        break;
                    ///下面是还没有处理的指令
                    case Code.Break:
                        stack.Break(this, code.Operand);
                        break;
                    case Code.Starg_S:
                        stack.Starg_S(this, code.Operand);
                        break;
                    case Code.Ldnull:
                        stack.Ldnull(this, code.Operand);
                        break;
                    case Code.Jmp:
                        stack.Jmp(this, code.Operand);
                        break;
                    case Code.Switch:
                        stack.Switch(this, code.Operand);
                        break;
                    case Code.Ldind_I1:
                        stack.Ldind_I1(this, code.Operand);
                        break;
                    case Code.Ldind_U1:
                        stack.Ldind_U1(this, code.Operand);
                        break;
                    case Code.Ldind_I2:
                        stack.Ldind_I2(this, code.Operand);
                        break;
                    case Code.Ldind_U2:
                        stack.Ldind_U2(this, code.Operand);
                        break;
                    case Code.Ldind_I4:
                        stack.Ldind_I4(this, code.Operand);
                        break;
                    case Code.Ldind_U4:
                        stack.Ldind_U4(this, code.Operand);
                        break;
                    case Code.Ldind_I8:
                        stack.Ldind_I8(this, code.Operand);
                        break;
                    case Code.Ldind_I:
                        stack.Ldind_I(this, code.Operand);
                        break;
                    case Code.Ldind_R4:
                        stack.Ldind_R4(this, code.Operand);
                        break;
                    case Code.Ldind_R8:
                        stack.Ldind_R8(this, code.Operand);
                        break;
                    case Code.Ldind_Ref:
                        stack.Ldind_Ref(this, code.Operand);
                        break;
                    case Code.Stind_Ref:
                        stack.Stind_Ref(this, code.Operand);
                        break;
                    case Code.Stind_I1:
                        stack.Stind_I1(this, code.Operand);
                        break;
                    case Code.Stind_I2:
                        stack.Stind_I2(this, code.Operand);
                        break;
                    case Code.Stind_I4:
                        stack.Stind_I4(this, code.Operand);
                        break;
                    case Code.Stind_I8:
                        stack.Stind_I8(this, code.Operand);
                        break;
                    case Code.Stind_R4:
                        stack.Stind_R4(this, code.Operand);
                        break;
                    case Code.Stind_R8:
                        stack.Stind_R8(this, code.Operand);
                        break;
                    case Code.And:
                        stack.And(this, code.Operand);
                        break;
                    case Code.Or:
                        stack.Or(this, code.Operand);
                        break;
                    case Code.Xor:
                        stack.Xor(this, code.Operand);
                        break;
                    case Code.Shl:
                        stack.Shl(this, code.Operand);
                        break;
                    case Code.Shr:
                        stack.Shr(this, code.Operand);
                        break;
                    case Code.Shr_Un:
                        stack.Shr_Un(this, code.Operand);
                        break;
                    case Code.Not:
                        stack.Not(this, code.Operand);
                        break;
                    case Code.Cpobj:
                        stack.Cpobj(this, code.Operand);
                        break;
                    case Code.Ldobj:
                        stack.Ldobj(this, code.Operand);
                        break;
                    case Code.Castclass:
                        stack.Castclass(this, code.Operand);
                        break;
                    case Code.Throw:
                        stack.Throw(this, code.Operand);
                        break;
                    case Code.Stobj:
                        stack.Stobj(this, code.Operand);
                        break;
                    case Code.Refanyval:
                        stack.Refanyval(this, code.Operand);
                        break;
                    case Code.Mkrefany:
                        stack.Mkrefany(this, code.Operand);
                        break;

                    case Code.Add_Ovf:
                        stack.Add_Ovf(this, code.Operand);
                        break;
                    case Code.Add_Ovf_Un:
                        stack.Add_Ovf_Un(this, code.Operand);
                        break;
                    case Code.Mul_Ovf:
                        stack.Mul_Ovf(this, code.Operand);
                        break;
                    case Code.Mul_Ovf_Un:
                        stack.Mul_Ovf_Un(this, code.Operand);
                        break;
                    case Code.Sub_Ovf:
                        stack.Sub_Ovf(this, code.Operand);
                        break;
                    case Code.Sub_Ovf_Un:
                        stack.Sub_Ovf_Un(this, code.Operand);
                        break;
                    case Code.Endfinally:
                        stack.Endfinally(this, code.Operand);
                        break;
                    case Code.Stind_I:
                        stack.Stind_I(this, code.Operand);
                        break;
                    case Code.Arglist:
                        stack.Arglist(this, code.Operand);
                        break;

                    case Code.Starg:
                        stack.Starg(this, code.Operand);
                        break;
                    case Code.Localloc:
                        stack.Localloc(this, code.Operand);
                        break;
                    case Code.Endfilter:
                        stack.Endfilter(this, code.Operand);
                        break;
                    case Code.Unaligned:
                        stack.Unaligned(this, code.Operand);
                        break;
                    case Code.Volatile:
                        stack.Volatile(this, code.Operand);
                        break;
                    case Code.Tail:
                        stack.Tail(this, code.Operand);
                        break;
                    case Code.Initobj:
                        stack.Initobj(this, code.Operand);
                        break;
                    case Code.Cpblk:
                        stack.Cpblk(this, code.Operand);
                        break;
                    case Code.Initblk:
                        stack.Initblk(this, code.Operand);
                        break;
                    case Code.No:
                        stack.No(this, code.Operand);
                        break;
                    case Code.Rethrow:
                        stack.Rethrow(this, code.Operand);
                        break;
                    case Code.Sizeof:
                        stack.Sizeof(this, code.Operand);
                        break;
                    case Code.Refanytype:
                        stack.Refanytype(this, code.Operand);
                        break;
                    case Code.Readonly:
                        stack.Readonly(this, code.Operand);
                        break;
                    default:
                        throw new Exception("未实现的OpCode:" + code.OpCode.Code);
                }
            }

        }
    }
}
