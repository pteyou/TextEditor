using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Configuration;

namespace PythonExecutor
{
    public sealed class TextGenerationCaller : PythonCaller
    {
        protected override string GetPipelineTaskName() => "text-generation";

        protected override string GetModelPath(Conf configuration) => configuration.TextGenerationModelPath;

        protected override string GetBaseScriptPath(Conf configuration) => configuration.BaseScript;
    }
}
