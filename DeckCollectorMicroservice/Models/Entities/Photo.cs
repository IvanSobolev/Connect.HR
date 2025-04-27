using System.ComponentModel.DataAnnotations;

namespace DeckCollectorMicroservice.Models.Entities;

public class Photo
{
    public long PhotoId { get; set; }
    public Guid ProfileId { get; set; }
    [Url]
    public string PhotoUrl { get; set; }
    public bool IsMain { get; set; }
    
    public Profile Profile { get; set; }
}