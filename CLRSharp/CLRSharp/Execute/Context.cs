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
        public CLRSharp_Environment environment
        {
            get;
            private set;
        }
        public int DebugLevel
        {
            get;
            private set;
        }
        public ThreadContext(CLRSharp_Environment env)
        {
            this.environment = env;
            DebugLevel = 0;
        }
        public ThreadContext(CLRSharp_Environment env, int DebugLevel)
        {
            this.environment = env;
            this.DebugLevel = DebugLevel;
        }
        Stack<StackFrame> stacks = new Stack<StackFrame>();
        public object ExecuteFunc(Mono.Cecil.MethodDefinition func, object _this, object[] _params)
        {
            StackFrame stack = new StackFrame();
            stacks.Push(stack);
            stack.SetParams(_params);
            RunCode(stack, func.Body.Instructions);
            return stacks.Pop().Return();

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
                        if (code.Operand is Mono.Cecil.MethodDefinition)
                            stack.NewObj(this, code.Operand as Mono.Cecil.MethodDefinition);
                        else
                            stack.NewObj(this, code.Operand as Mono.Cecil.MethodReference);
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
                        stack.Constrained(code.Operand);
                        break;
                    default:
                        throw new Exception("未实现的OpCode:" + code.OpCode.Code);
                }
            }

        }
    }
}
