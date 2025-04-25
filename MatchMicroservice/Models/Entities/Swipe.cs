namespace MatchMicroservice.Models.Entities;

public class Swipe
{
    public Guid UserId1 { get; set; }
    public Guid UserId2 { get; set; }
    public bool? DecisionId1 { get; set; }
    public bool? DecisionId2 { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}