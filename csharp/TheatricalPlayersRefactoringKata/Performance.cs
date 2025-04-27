namespace TheatricalPlayersRefactoringKata
{
    public record Performance
    {
        public string PlayID { get; set; }

        public int Audience { get; set; }

        public Performance(string playID, int audience)
        {
            this.PlayID = playID;
            this.Audience = audience;
        }

    }
}
