using SassAndCoffee.Core;
using SassAndCoffee.JavaScript;
using SassAndCoffee.JavaScript.CoffeeScript;

namespace Flair.Transformations
{
    public class CoffeeToJs : ITransform
    {
        private readonly IInstanceProvider<IJavaScriptRuntime> runtimeProvider;
        private readonly IInstanceProvider<IJavaScriptCompiler> compilerProvider;

        public string DisplayText
        {
            get { return "Coffee\t➜\tJs"; }
        }

        public CoffeeToJs()
        {
            runtimeProvider = new InstanceProvider<IJavaScriptRuntime>(() => (IJavaScriptRuntime)new IEJavaScriptRuntime());
            compilerProvider = CreateCompilerProvider(runtimeProvider);
        }

        public string Transform(string source)
        {
            // bare = true skips the function safety wrapper
            const bool bare = false;
            var args = new[] { (object)(bare ? 1 : 0) };

            using (var compiler = compilerProvider.GetInstance())
            {
                return compiler.Compile(source, args);
            }
        }

        private static IInstanceProvider<IJavaScriptCompiler> CreateCompilerProvider(IInstanceProvider<IJavaScriptRuntime> jsRuntimeProvider)
        {
            return new InstanceProvider<IJavaScriptCompiler>(() => (IJavaScriptCompiler)new CoffeeScriptCompiler(jsRuntimeProvider));
        }
    }
}