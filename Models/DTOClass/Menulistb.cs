using SqlSugar;

namespace WebMenu.Models.DTOClass
{
    public class Menulistb
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Uid { get; set; }
        public long H_uid { get; set; }
        public string Item { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public bool ItemDone { get; set; }
    }
}
