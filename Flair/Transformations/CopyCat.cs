using System.Threading;

namespace Flair.Transformations
{
    /// <summary>
    /// Copies source to target - used to demonstrate how to write a transform
    /// </summary>
    public class CopyCat : ITransform
    {
        public string DisplayText
        {
            get { return "Copy cat"; }
        }

        public string Transform(string source)
        {
            // let'em know we're busy...
            Thread.Sleep(500);
            return source;
        }
    }
}