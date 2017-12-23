namespace Turgo.Common
{
    public interface IChangeable
    {
        bool IsChanged { get; set; }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
