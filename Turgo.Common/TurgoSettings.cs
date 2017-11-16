using Sramek.FX;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public class TurgoSettings : SettingsBase<TurgoSettings>
    {
        public TurgoSettings()
        {
            BaseClassConfiguration = new ClassConfiguration();
            Model = new TurgoModel();
        }

        public ClassConfiguration BaseClassConfiguration { get; set; }
        public TurgoModel Model { get; set; }
    }
}