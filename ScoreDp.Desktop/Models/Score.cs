namespace ScoreDp.Desktop.Models;

public class Score
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SongId { get; set; }
    public int ClearType { get; set; } // 0=NO PLAY 1=FAILED 2=CLEAR 3=HARD 4=EX_HARD 5=FC
    public int ScoreValue { get; set; }
    public string DjLevel { get; set; } = "---";
    public DateTime UpdatedAt { get; set; }
    public User? User { get; set; }
    public Song? Song { get; set; }
}
