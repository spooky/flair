using System;
using System.Collections.Generic;
using System.Text;
using IronRuby;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using SassAndCoffee.Core;
using SassAndCoffee.Ruby.Sass;

namespace Flair.Transformations
{
    public abstract class SassTransformerBase : ITransform
    {
        private readonly dynamic sassCompiler;
        private readonly dynamic option;

        public abstract string DisplayText { get; }

        public SassTransformerBase(string syntaxOption)
        {
            var pal = new ResourceRedirectionPlatformAdaptationLayer();
            var srs = new ScriptRuntimeSetup
            {
                HostType = typeof(SassCompilerScriptHost),
                HostArguments = new List<object> { pal },
            };
            srs.AddRubySetup();
            var runtime = Ruby.CreateRuntime(srs);
            var engine = runtime.GetRubyEngine();

            // NB: 'R:\{345ED29D-C275-4C64-8372-65B06E54F5A7}' is a garbage path that the PAL override will
            // detect and attempt to find via an embedded Resource file
            engine.SetSearchPaths(new List<string>
                                      {
                                          @"R:\{345ED29D-C275-4C64-8372-65B06E54F5A7}\lib\ironruby",
                                          @"R:\{345ED29D-C275-4C64-8372-65B06E54F5A7}\lib\ruby\1.9.1"
                                      });

            var source = engine.CreateScriptSourceFromString(Utility.ResourceAsString("lib.sass_in_one.rb", typeof(SassCompiler)), SourceCodeKind.File);
            var scope = engine.CreateScope();
            source.Execute(scope);

            sassCompiler = scope.Engine.Runtime.Globals.GetVariable("Sass");
            option = engine.Execute(syntaxOption);
        }

        public virtual string Transform(string text)
        {
            try
            {
                return (string)sassCompiler.compile(text, option);
            }
            catch (Exception ex)
            {
                // Provide more information for SassSyntaxErrors
                if (ex.Message == "Sass::SyntaxError")
                {
                    dynamic error = ex;
                    var sb = new StringBuilder();
                    sb.AppendFormat("{0}\n\n", error.to_s());
                    //sb.AppendFormat("Backtrace:\n{0}\n\n", error.sass_backtrace_str(pathInfo.FullName) ?? "");
                    //sb.AppendFormat("FileName: {0}\n\n", error.sass_filename() ?? pathInfo.FullName);
                    sb.AppendFormat("MixIn: {0}\n\n", error.sass_mixin() ?? "");
                    sb.AppendFormat("Line Number: {0}\n\n", error.sass_line() ?? "");
                    sb.AppendFormat("Sass Template:\n{0}\n\n", error.sass_template ?? "");
                    throw new Exception(sb.ToString(), ex);
                }

                throw;
            }
        }
    }
}