using Sramek.FX;
using Turgo.Common.Model;

namespace Turgo.Common
{
    public class TurgoSettings : SettingsBase<TurgoSettings>
    {
        public ClassConfiguration BaseClassConfiguration { get; set; }
        public TurgoModel Model { get; set; }
    }
}