namespace Flair.Transformations
{
    public interface ITransform
    {
        /// <summary>
        /// String to be displayed in the combo box
        /// </summary>
        string DisplayText { get; }

        string Transform(string source);
    }
}