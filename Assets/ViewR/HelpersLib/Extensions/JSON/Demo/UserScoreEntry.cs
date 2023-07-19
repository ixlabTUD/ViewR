namespace ViewR.HelpersLib.Extensions.JSON.Demo
{
    [System.Serializable]
    public class UserScoreEntry {
        public string name;
        public int score;

        public UserScoreEntry (string name, int score) {
            this.name = name;
            this.score = score;
        }
    }
}