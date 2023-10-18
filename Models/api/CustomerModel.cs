using Newtonsoft.Json;
using SqlSugar;
using WebMenu.Models.DTOClass;

namespace WebMenu.Models.api
{
    public class CustomerModel
    {
        private static CustomerModel mCustomer;

        public static CustomerModel customer
        {

            get
            {
                if (mCustomer == null)
                {
                    mCustomer = new CustomerModel();
                }
                return mCustomer;
            }
        }

        public List<Menu> SelectMenu(IConfiguration config)
        {
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //連線字串
                ConnectionString = config.GetValue<string>("WebmenuConnectionString"),
                DbType = DbType.SqlServer,//連線類型
                IsAutoCloseConnection = true //自動關閉連線
            });
            //當執行時，觸發事件
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//查看SQL語法
            };

            var list = db.Queryable<Menu>().ToList();
            foreach (var menu in list)
            {
                menu.Item = menu.Item.Trim();
                Console.WriteLine(menu.Item);
            }
            return list;
        }

        public void CreateOrder(string data, string phone, out Dictionary<string, Menulistb> order_body, out Menulisth order_head)
        {
            order_head = new Menulisth()
            {
                Formnum = phone
            };
            order_body = JsonConvert.DeserializeObject<Dictionary<string, Menulistb>>(data);
        }

        public bool insertOrder(IConfiguration config, Dictionary<string, Menulistb> order_body, Menulisth order_head)
        {
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = config.GetValue<string>("WebmenuConnectionString"),
                DbType = DbType.SqlServer,//連線類型
                IsAutoCloseConnection = true //自動關閉連線
            });
            try
            {

                //當執行時，觸發事件
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);//查看SQL語法
                };
                db.BeginTran();
                //寫入表頭 並 回傳表頭資料
                order_head = db.Insertable(order_head).ExecuteReturnEntity();
                //逐筆將表身資料寫入
                foreach (var keyvalue in order_body)
                {
                    var item = keyvalue.Value;
                    item.H_uid = order_head.Uid;
                    db.Insertable(item).ExecuteCommand();
                }

                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();//rollback
                throw;
            }
            return true;
        }
    }
}
