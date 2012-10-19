
using System;
using dotless.Core;
using dotless.Core.Loggers;
using dotless.Core.Parser;

namespace Flair.Transformations
{
    public class LessToCss : ITransform
    {
        public string DisplayText
        {
            get { return "Less\t➜\tCss"; }
        }

        public string Transform(string source)
        {
            var engine = new LessEngine(new Parser(), new WeirdLogger(LogLevel.Warn), false, false);
            return engine.TransformToCss(source, null);
        }
    }

    // to get some error reporting on the screen
    public class WeirdLogger : Logger
    {
        public WeirdLogger(LogLevel level) : base(level)
        {
        }

        protected override void Log(string message)
        {
            throw new Exception(message);
        }
    }
}
