namespace Sramek.FX
{
    public abstract class StaticInstance<T>
        where T : new()
    {
        public static T I { get; protected set; } = new T();
    }
}
