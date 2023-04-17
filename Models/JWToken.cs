namespace WebApiFarm
{
    public class Token
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
}

    
}