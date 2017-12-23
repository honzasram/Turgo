namespace Sramek.FX
{
    public class SettingsBase<T>
        where T : new()
    {
        private const string cDefaultPath = "Config\\Config.xml";

        public static T I { get; private set; }
        

        public static void Load(string aPath = cDefaultPath)
        {
            I = XMLConfig.Load<T>(aPath);
            if (I == null) I = new T();
        }

        public static void Save(T aSetting, string aPath = cDefaultPath)
        {
            XMLConfig.Save(aPath, aSetting);
        }
    }
}
