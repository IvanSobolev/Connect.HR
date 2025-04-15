namespace TinderAPI.Models.Entitys;

public class Photos
{
    public long PhotoId { get; set; }
    public Guid ProfileId { get; set; }
    public string PhotoUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsMain { get; set; }
}