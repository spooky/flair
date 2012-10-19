namespace Flair.Transformations
{
    public class SassToCss : SassTransformerBase
    {
        public override string DisplayText
        {
            get { return "Sass\t➜\tCss"; }
        }

        public SassToCss() : base("{:cache => false, :syntax => :sass}")
        {
        }
    }
}
