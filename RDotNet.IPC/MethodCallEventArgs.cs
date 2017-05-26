using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace RDotNet.IPC
{
    public class MethodCallEventArgs : EventArgs, IMethodCallMessage
    {
        private readonly IMethodCallMessage _msg;

        public MethodCallEventArgs(IMethodCallMessage msg)
        {
            _msg = msg;
        }

        public IDictionary Properties => _msg.Properties;

        public string GetArgName(int index)
        {
            return _msg.GetArgName(index);
        }

        public object GetArg(int argNum)
        {
            return _msg.GetArg(argNum);
        }

        public string Uri => _msg.Uri;
        public string MethodName => _msg.MethodName;
        public string TypeName => _msg.TypeName;
        public object MethodSignature => _msg.MethodSignature;
        public int ArgCount => _msg.ArgCount;
        public object[] Args => _msg.Args;
        public bool HasVarArgs => _msg.HasVarArgs;

        public LogicalCallContext LogicalCallContext => _msg.LogicalCallContext;

        public MethodBase MethodBase => _msg.MethodBase;

        public string GetInArgName(int index)
        {
            return _msg.GetInArgName(index);
        }

        public object GetInArg(int argNum)
        {
            return _msg.GetInArgName(argNum);
        }

        public int InArgCount => _msg.InArgCount;
        public object[] InArgs => _msg.InArgs;
    }
}