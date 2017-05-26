using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDotNet.IPC
{
    public class REngineRemoteRepository : DynamicProxy<REngine>
    {
      


        public REngineRemote GetEngine()
        {
            REngine m = (REngine)this.GetTransparentProxy();

            this.BeforeExecute += REngineRemoteRepository_BeforeExecute;
            
            Console.WriteLine(m);
            Console.WriteLine(m.GetType());
            return m as REngineRemote;
            
        }

        private void REngineRemoteRepository_BeforeExecute(object sender, MethodCallEventArgs e)
        {
           Console.WriteLine("Before !!!");
           return;
            
        }

        public REngineRemoteRepository(REngine decorated) : base(decorated)
        {
        }
    }

    public class REngineRemote : REngine
    {
        public REngineRemote(string id, string dll) : base(id, dll)
        {
        }

        public  override string ToString()
        {
            return base.ToString();
        }
    }

   
}
