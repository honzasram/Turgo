namespace Sramek.FX
{
    public class SettingsBase<T>
    {
        private const string cDefaultPath = "Config\\Config.xml";

        public static T I { get; private set; }
        

        public static void Load(string aPath = cDefaultPath)
        {
            I = XMLConfig.Load<T>(aPath);
        }

        public static void Save(T aSetting, string aPath = cDefaultPath)
        {
            XMLConfig.Save(aPath, aSetting);
        }
    }
}
