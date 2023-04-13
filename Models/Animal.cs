namespace WebApiFarm
{

    public class Animal
     {
        public int Id {get; set;}
        public required string Name {get; set;} = string.Empty;
        public required string Chicken_Coop {get; set;} = string.Empty;
        public required string TypeOfAnimal {get; set;} = string.Empty;
    }
    
}