using SqlSugar;

namespace WebMenu.Models.DTOClass
{
    public class Menulisth
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Uid { get; set; }
        public string Formnum { get; set; }
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public DateTime Createtime { get; set; }

        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public bool OrderDone { get; set; }
    }
}
