namespace WebApiFarm
{
    
    public class Product
    {
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty; 
    public required string Description { get; set; } = string.Empty;
        public required string Weight {get; set;} = string.Empty;
        public  required decimal Price { get; set; }
    public required bool Active {get; set;}
    public  DateTime UploadDate {get; set;}
    
    }

}