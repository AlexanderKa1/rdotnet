using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RDotNet.NativeLibrary;

namespace RDotNet.IPC
{
    public static class Program
    {
        public static void Main()
        {


            SetupPath();
           //  var dll = REngine.GetInstance();
           var dllname=  NativeUtility.GetRLibraryFileName();
            REngine s = new REngineRemote("123", dllname);
            REngineRemoteRepository d = new REngineRemoteRepository(s);
            var engine = d.GetEngine();
            engine.Initialize();
            engine.AutoPrint = true;
            Console.WriteLine(engine.IsRunning);
            Console.WriteLine("stop");
            Console.ReadKey();
        }


        public static void SetupPath(string Rversion = "R-3.3.2")
        {
            var oldPath = System.Environment.GetEnvironmentVariable("PATH");
            var rPath = System.Environment.Is64BitProcess ?
                string.Format(@"C:\Program Files\R\{0}\bin\x64", Rversion) :
                string.Format(@"C:\Program Files\R\{0}\bin\i386", Rversion);

            if (!Directory.Exists(rPath))
                throw new DirectoryNotFoundException(
                    string.Format(" R.dll not found in : {0}", rPath));
            var newPath = string.Format("{0}{1}{2}", rPath,
                System.IO.Path.PathSeparator, oldPath);
            System.Environment.SetEnvironmentVariable("PATH", newPath);
        }
    }
}
