using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace RDotNet.IPC
{
    public class DynamicProxy<T> : RealProxy, IRemotingTypeInfo
    {
        private readonly T _decorated;
        private Predicate<MethodInfo> _filter;

        public DynamicProxy(T decorated)
            : base(typeof(T))
        {
            _decorated = decorated;
            _filter = m => true;
        }

        public Predicate<MethodInfo> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                    _filter = m => true;
                else
                    _filter = value;
            }
        }

        public bool CanCastTo(Type fromType, object o)
        {
            return true;
        }

        public string TypeName { get; set; }

        public event EventHandler<MethodCallEventArgs> BeforeExecute;
        public event EventHandler<MethodCallEventArgs> AfterExecute;
        public event EventHandler<MethodCallEventArgs> ErrorExecuting;

        private void OnBeforeExecute(MethodCallEventArgs methodCall)
        {
            if (BeforeExecute != null)
            {
                var methodInfo = methodCall.MethodBase as MethodInfo;
                if (_filter(methodInfo))
                    BeforeExecute(this, methodCall);
            }
        }

        private void OnAfterExecute(MethodCallEventArgs methodCall)
        {
            if (AfterExecute != null)
            {
                var methodInfo = methodCall.MethodBase as MethodInfo;
                if (_filter(methodInfo))
                    AfterExecute(this, methodCall);
            }
        }

        private void OnErrorExecuting(MethodCallEventArgs methodCall)
        {
            if (ErrorExecuting != null)
            {
                var methodInfo = methodCall.MethodBase as MethodInfo;
                if (_filter(methodInfo))
                    ErrorExecuting(this, methodCall);
            }
        }


        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;

            OnBeforeExecute(new MethodCallEventArgs(methodCall));

            try
            {
                object result;

                Console.WriteLine("Call "+ methodInfo.Name);
                if (methodInfo.Name == "ToString")
                {
                    var firstOrDefault = _decorated.GetType().GetMethods()
                        .FirstOrDefault(i => i.Name == methodInfo.Name);
                    result = firstOrDefault.Invoke(_decorated, new object[0]);
                }
                else
                {
                    result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                }


                OnAfterExecute(new MethodCallEventArgs(methodCall));

                return new ReturnMessage(result, null, 0,
                    methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                OnErrorExecuting(new MethodCallEventArgs(methodCall));
                return new ReturnMessage(e, methodCall);
            }
        }
    }
}