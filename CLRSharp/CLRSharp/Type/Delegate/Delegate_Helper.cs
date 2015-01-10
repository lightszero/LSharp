//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CLRSharp
//{
//    public class Delegate_Helper
//    {

//        delegate void Action();
//        delegate void Action<T1>(T1 p1);
//        delegate void Action<T1, T2>(T1 p1, T2 p2);
//        delegate void Action<T1, T2, T3>(T1 p1, T2 p2, T3 p3);
//        delegate void Action<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4);
//        delegate TRet RetAction<TRet>();
//        delegate TRet RetAction<TRet, T1>(T1 p1);
//        delegate TRet RetAction<TRet, T1, T2>(T1 p1, T2 p2);
//        delegate TRet RetAction<TRet, T1, T2, T3>(T1 p1, T2 p2, T3 p3);
//        delegate TRet RetAction<TRet, T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4);
//        public static Delegate MakeDelegate(Type deletype, ThreadContext context, CLRSharp_Instance __this,IMethod __method)
//        {
//            var method = deletype.GetMethod("Invoke");
//            if(__method.isStatic)
//            {
//                __this = null;
//            }
//            var pp = method.GetParameters();
//            if (method.ReturnType == typeof(void))
//            {
//                if (pp.Length == 0)
//                {
//                    return CreateDele(deletype, context, __this, __method);
//                }
//                else if (pp.Length == 1)
//                {

//                    var gtype = typeof(Delegate_BindTool<>).MakeGenericType(new Type[] { pp[0].ParameterType });
                  
//                    return gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type;
//                }
//                else if (pp.Length == 2)
//                {
//                    var gtype = typeof(RegHelper_DeleAction<,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType });
//                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);
//                }
//                else if (pp.Length == 3)
//                {
//                    var gtype = typeof(RegHelper_DeleAction<,,>).MakeGenericType(new Type[] { pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
//                    return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);
//                }
//                else
//                {
//                    throw new Exception("还没有支持这么多参数的委托");
//                }
//            }
//            else
//            {
//                Type gtype = null;
//                if (pp.Length == 0)
//                {
//                    gtype = typeof(RegHelper_DeleNonVoidAction<>).MakeGenericType(new Type[] { method.ReturnType });
//                }
//                else if (pp.Length == 1)
//                {
//                    gtype = typeof(RegHelper_DeleNonVoidAction<,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType });
//                }
//                else if (pp.Length == 2)
//                {
//                    gtype = typeof(RegHelper_DeleNonVoidAction<,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType });
//                }
//                else if (pp.Length == 3)
//                {
//                    gtype = typeof(RegHelper_DeleNonVoidAction<,,,>).MakeGenericType(new Type[] { method.ReturnType, pp[0].ParameterType, pp[1].ParameterType, pp[2].ParameterType });
//                }
//                else
//                {
//                    throw new Exception("还没有支持这么多参数的委托");
//                }
//                return (gtype.GetConstructors()[0].Invoke(new object[] { type, keyword }) as RegHelper_Type);

//            }

//        }

//        public static Delegate CreateDele(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action act = () =>
//                {
//                    _method.Invoke(context, _this, null);
//                };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }

//        public static Delegate CreateDele<T1>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action<T1> act = (p1) =>
//            {
//                _method.Invoke(context, _this, new object[] { p1 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateDele<T1, T2>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action<T1, T2> act = (p1, p2) =>
//            {
//                _method.Invoke(context, _this, new object[] { p1, p2 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateDele<T1, T2, T3>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action<T1, T2, T3> act = (p1, p2, p3) =>
//            {
//                _method.Invoke(context, _this, new object[] { p1, p2, p3 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateDele<T1, T2, T3, T4>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action<T1, T2, T3, T4> act = (p1, p2, p3, p4) =>
//            {
//                _method.Invoke(context, _this, new object[] { p1, p2, p3, p4 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateRetDele<TRet>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            RetAction<TRet> act = () =>
//            {
//                return (TRet)_method.Invoke(context, _this, null);
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateRetDele<TRet, T1>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            RetAction<TRet, T1> act = (p1) =>
//            {
//                return (TRet)_method.Invoke(context, _this, new object[] { p1 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateRetDele<TRet, T1, T2>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            RetAction<TRet, T1, T2> act = (p1, p2) =>
//            {
//                return (TRet)_method.Invoke(context, _this, new object[] { p1, p2 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateRetDele<TRet, T1, T2, T3>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            RetAction<TRet, T1, T2, T3> act = (p1, p2, p3) =>
//            {
//                return (TRet)_method.Invoke(context, _this, new object[] { p1, p2, p3 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//        public static Delegate CreateRetDele<TRet, T1, T2, T3, T4>(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            RetAction<TRet, T1, T2, T3, T4> act = (p1, p2, p3, p4) =>
//            {
//                return (TRet)_method.Invoke(context, _this, new object[] { p1, p2, p3, p4 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//    }
//    public class Delegate_BindTool<T1>
//    {
//        public static Delegate CreateDele(Type deletype, ThreadContext context, CLRSharp_Instance _this, IMethod _method)
//        {
//            Action<T1> act = (p1) =>
//            {
//                _method.Invoke(context, _this, new object[] { p1 });
//            };
//            return Delegate.CreateDelegate(deletype, act.Target, act.Method);
//        }
//    }
//}
