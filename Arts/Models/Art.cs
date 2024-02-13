                                                 
using Arts.Models;

namespace Arts
{


    public class Art
    {
        public  Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public DateTime EndTime  { get; set; }
        public string? Image { get; set; }
        public string? status { get; set; } = "open";
    }

}
