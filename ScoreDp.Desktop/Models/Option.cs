namespace ScoreDp.Desktop.Models;

public class Option
{
    public int UserId { get; set; }
    public int SongId { get; set; }
    public int Flip { get; set; }      // 0/1
    public int LeftArr { get; set; }   // 0=정배 1=미러 2=랜덤 3=R-랜덤 4=슈퍼랜덤
    public int RightArr { get; set; }
    public User? User { get; set; }
    public Song? Song { get; set; }
}
