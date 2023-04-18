namespace apirest
{
    public class Account 
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password {get; set;} = string.Empty;
        public required string  Role { get; set; } = string.Empty;

    }
    
}