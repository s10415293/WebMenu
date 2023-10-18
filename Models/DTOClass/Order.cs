using SqlSugar;

namespace WebMenu.Models.DTOClass
{
    public class Order
    {
        public Menulisth head { get; set; }
        public List<Menulistb> bodys { get; set; } = new List<Menulistb>();
    }

    
}
