namespace ScoreDp.Desktop.Models;

public class User
{
    public int Id { get; set; }
    public string IidxId { get; set; } = null!;
    public string DjName { get; set; } = null!;
    public string? PasswordHash { get; set; }
}
