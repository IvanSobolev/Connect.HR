namespace TinderAPI.Models.Entitys;

public class Profile
{
    public Guid Id { get; set; }
    public string FirsName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastActive { get; set; }
    public bool IsVerified { get; set; }
    public bool IsMale { get; set; }
    public DateOnly BirthdayDate { get; set; }
    public string? Description { get; set; }
    public float Latitube { get; set; }
    public float Longtube { get; set; }

}