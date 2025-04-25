namespace MatchMicroservice.Models.Entities;

public class DecisionDto
{
    public Guid UserA { get; set; }
    public Guid UserB { get; set; }
    public bool? DecisionA { get; set; }
    public bool? DecisionB { get; set; }

    public DecisionDto(Guid a, Guid b, bool decision)
    {
        var isAFirst = a > b;
        UserA = isAFirst ? a : b;
        UserB = isAFirst ? b : a;
        DecisionA = isAFirst ? decision : null;
        DecisionB = isAFirst ? null : decision;
    }
}