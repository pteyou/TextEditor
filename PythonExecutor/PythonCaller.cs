using System.Diagnostics;
using System.IO;
using TextEditor.Configuration;

namespace PythonExecutor
{
    public abstract class PythonCaller
    {
        private readonly Conf configuration;
        private readonly string pythonPath;
        protected PythonCaller()
        {
            configuration = Hosting.Instance.Config;
            pythonPath = configuration.PythonExecutablePath;
        }
        public string Launch(string input, out string errors)
        {

            const string activateVenv = "conda activate base";

            var myProcessStartInfo = new ProcessStartInfo(@"powershell.exe");
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardInput = true;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.RedirectStandardError = true;
            myProcessStartInfo.CreateNoWindow = true;
            myProcessStartInfo.Arguments = "";
                Process myProcess = new Process();
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.Start();

            var si = myProcess.StandardInput;
            if (si.BaseStream.CanWrite)
            {
                si.WriteLine(@"C:\Users\pat\anaconda3\shell\condabin\conda-hook.ps1");
                si.WriteLine(activateVenv);
                si.WriteLine($"{pythonPath} {GetBaseScriptPath(configuration)} {GetModelPath(configuration)} {GetPipelineTaskName()} {input}");
                si.Flush();
                si.Close();
                si.Dispose();
            }


            StreamReader myStreamReader = myProcess.StandardOutput;
            string result = myStreamReader.ReadToEnd();

            StreamReader myErrorStreamReader = myProcess.StandardError;
            errors = myErrorStreamReader.ReadToEnd();

            myProcess.WaitForExit();
            myProcess.Close();
            return result;
        }

        protected abstract string GetPipelineTaskName();
        protected abstract string GetModelPath(Conf configuration);
        protected abstract string GetBaseScriptPath(Conf configuration);
    }
}
