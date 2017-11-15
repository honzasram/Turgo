using Sramek.FX.WPF;

namespace Turgo
{
    public class TurgoLoc : Loc<TurgoLoc>
    {
        [Default("Selected for round")]
        public string SelectedForRound { get; set; }

        [Default("Name")]
        public string Name { get; set; }

        [Default("Points")]
        public string Points { get; set; }
    
        [Default("Players")]
        public string Players { get; set; }

        public TurgoLoc()
        {
            Load();
        }
    }
}