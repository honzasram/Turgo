using System.Reflection;
using System.Xml.Serialization;
using log4net;

namespace Sramek.FX
{
    public class XMLConfig
    {
        private static readonly ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static T Load<T>(string aPath)
        {
            try
            {
                using (var lReader = new System.IO.StreamReader(aPath))
                {
                    var lSerializer = new XmlSerializer(typeof(T));
                    return (T)lSerializer.Deserialize(lReader);
                }
            }
            catch (System.Exception e)
            {
                mLog.Error(e);
                return default(T);
            }
        }

        public static void Save<T>(string aPath, T aConfig)
        {
            try
            {
                using (var lWriter = new System.IO.StreamWriter(aPath))
                {
                    var lSerializer = new XmlSerializer(typeof(T));
                    lSerializer.Serialize(lWriter, aConfig);
                }
            }
            catch (System.Exception e)
            {
                mLog.Error(e);
            }
        }
    }
}