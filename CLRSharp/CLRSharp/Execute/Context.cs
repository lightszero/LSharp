using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    public class Context
    {
        public CLRSharp_Environment environment
        {
            get;
            private set;
        }
        public Context(CLRSharp_Environment env)
        {
            this.environment = env;
        }
        Stack<StackFrame> stacks = new Stack<StackFrame>();
        public object ExecuteFunc(Mono.Cecil.MethodDefinition func, object _this, object[] _params)
        {
            StackFrame stack = new StackFrame();
            stacks.Push(stack);
            stack.SetParams( _params);
            RunCode(stack, func.Body.Instructions);

            return null;
        }
        public object Call(Mono.Cecil.MethodReference method, object[] _params)
        {
            string name = method.DeclaringType.FullName;
            var typesys = this.environment.GetType(name);
            if (typesys == null)
            {
                System.Type t = System.Type.GetType(name);
                typesys = new Type_Common(t);
            }
            TypeList list = new TypeList();
            foreach (var p in method.Parameters)
            {
                list.Add(new Type_Common(System.Type.GetType(p.ParameterType.FullName)));
            }

            return typesys.GetMethod(method.Name, list).Invoke(this, null, _params);

        }
        void RunCode(StackFrame stack, Mono.Collections.Generic.Collection<Mono.Cecil.Cil.Instruction> codes)
        {
            stack._pos = codes[0];
            while (true)
            {
                var code = stack._pos;
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
                        stack.Ldloca((int)code.Operand);
                        break;
                    case Code.Ldloca_S:
                        stack.Ldloca((sbyte)code.Operand);
                        break;
                    //加载字符串
                    case Code.Ldstr:
                        stack.Ldstr(code.Operand as string);
                        break;
                    //呼叫函数
                    case Code.Call:
                        stack.Call(this, code.Operand as Mono.Cecil.MethodReference);
                        break;
                    case Code.Callvirt:
                        stack.Call(this, code.Operand as Mono.Cecil.MethodReference);
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
                        stack.Ldarg(2);
                        break;
                    case Code.Ldarg_2:
                        stack.Ldarg(2);
                        break;
                    case Code.Ldarg_3:
                        stack.Ldarg(3);
                        break;
                    default:
                        throw new Exception("未实现的OpCode:" + code.OpCode.Code);
                }
            }

        }
    }
}
