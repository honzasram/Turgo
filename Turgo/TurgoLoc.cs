using Sramek.FX.WPF;

namespace Turgo
{
    public class TurgoLoc : Loc<TurgoLoc>
    {
        [Default("Selected for round")]
        public string SelectedForRound { get; set; }

        [Default("Save")]
        public string Save { get; set; }

        [Default("Load")]
        public string Load { get; set; }

        [Default("New")]
        public string New { get; set; }

        [Default("Game")]
        public string Game { get; set; }

        [Default("Round")]
        public string Round { get; set; }

        [Default("Delete")]
        public string Delete { get; set; }

        [Default("Name")]
        public string Name { get; set; }

        [Default("Surame")]
        public string Surname { get; set; }

        [Default("Points")]
        public string Points { get; set; }
    
        [Default("Players")]
        public string Players { get; set; }

        [Default("Player")]
        public string Player { get; set; }

        [Default("Properties")]
        public string Properties { get; set; }

        [Default("Print")]
        public string Print { get; set; }

        [Default("PDF")]
        public string PDF { get; set; }

        [Default("Count")]
        public string Count { get; set; }

        [Default("New Set")]
        public string NewSet { get; set; }

        public TurgoLoc()
        {
            Load();
        }
    }
}