namespace NexusDev.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public string displayInfo() => $"{Year} {Make} {Model} - ${Price}";

        /*public string displayInfo()
        {
            return $"{Year} {Make} {Model} - ${Price}";
        }*/
    }
}
