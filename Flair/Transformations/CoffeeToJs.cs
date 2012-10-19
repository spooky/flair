using CoffeeSharp;

namespace Flair.Transformations
{
    public class CoffeeToJs : ITransform
    {
        private readonly CoffeeScriptEngine engine;

        public string DisplayText
        {
            get { return "Coffee\t➜\tJs"; }
        }

        public CoffeeToJs()
        {
            engine = new CoffeeScriptEngine();
        }

        public string Transform(string source)
        {
            return engine.Compile(source);
        }
    }
}