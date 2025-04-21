using System.ComponentModel.DataAnnotations;

namespace TinderAPI.Models.Entitys;

public class Hobby
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    
    public ICollection<Profile> Profiles { get; set; }
}