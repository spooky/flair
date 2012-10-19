namespace Flair.Transformations
{
    public class ScssToCss : SassTransformerBase
    {
        public override string DisplayText
        {
            get { return "Scss\t➜\tCss"; }
        }

        public ScssToCss() : base("{:cache => false, :syntax => :scss}")
        {
        }
    }
}