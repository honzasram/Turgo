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

        [Default("Result")]
        public string Result { get; set; }

        [Default("Export")]
        public string Export { get; set; }

        [Default("Total Results")]
        public string TotalResults { get; set; }

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

        [Default("Open")]
        public string Open { get; set; }

        [Default("PDF")]
        public string PDF { get; set; }

        [Default("Count")]
        public string Count { get; set; }

        [Default("New Set")]
        public string NewSet { get; set; }

        [Default("End Game")]
        public string EndGame { get; set; }

        [Default("End Round")]
        public string EndRound { get; set; }

        [Default("Won")]
        public string Won { get; set; }

        [Default("Start")]
        public string Start { get; set; }

        [Default("Court Count")]
        public string CourtCount { get; set; }

        [Default("Select Class")]
        public string SelectClass { get; set; }

        [Default("Finish Class")]
        public string EndClass { get; set; }

        [Default("New Class")]
        public string NewClass { get; set; }

        [Default("Classes")]
        public string Classes { get; set; }

        [Default("User Base")]
        public string UserBase { get; set; }

        [Default("Date")]
        public string Date { get; set; }

        [Default("Round No.")]
        public string RoundNo { get; set; }

        public TurgoLoc()
        {
            Load();
        }
    }
}