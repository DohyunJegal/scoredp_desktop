namespace ScoreDp.Desktop.Models;

public class Song
{
    public int Id { get; set; }
    public string? ZasaId { get; set; }
    public string Title { get; set; } = null!;
    public string TitleNormalized { get; set; } = null!;
    public int Level { get; set; }
    public string Chart { get; set; } = null!;
    public double? UnofficialLevel { get; set; }
    public int? VersionId { get; set; }
    public GameVersion? Version { get; set; }
}
