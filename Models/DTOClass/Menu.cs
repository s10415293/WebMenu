using SqlSugar;

namespace WebMenu.Models.DTOClass
{
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Uid { get; set; }
        public string Item { get; set; }
        public int Price { get; set; }
    }
}
