namespace AuthMicroservice.Models.Dtos;

public class GrpcOptions (string address)
{
    public string Address { get; set; } = address;
}