namespace TheatricalPlayersRefactoringKata
{
    public record Performance
    {
        private string _playID;
        private int _audience;

        public string PlayID { get => _playID; set => _playID = value; }
        public int Audience { get => _audience; set => _audience = value; }
        public Play Play { get; set; }
        public int Amoumt { get; set; }
        public int VolumeCredits { get; set; }

        public Performance(string playID, int audience)
        {
            this._playID = playID;
            this._audience = audience;
        }

    }
}
