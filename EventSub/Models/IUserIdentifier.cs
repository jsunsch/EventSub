namespace EventSub.Models
{
    // TODO: How to identify a user? Do children have an e-mail adress? Unilogi? Hvad gør man i andre lande?
    public interface IUserIdentifier
    {
        string Email { get; set; }
        string Name { get; set; }
        string LastName { get; set; }
    }
}