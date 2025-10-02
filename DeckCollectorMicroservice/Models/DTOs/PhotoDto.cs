namespace DeckCollectorMicroservice.Models.DTOs;

public class PhotoDto
{
    public long PhotoId { get; set; }
    public Guid ProfileId { get; set; }
    public string PhotoUrl { get; set; }
    public bool IsMain { get; set; }
}