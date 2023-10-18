using Newtonsoft.Json;
using SqlSugar;
using WebMenu.Models.DTOClass;

namespace WebMenu.Models.api
{
    public class ProprietorModel
    {
        private static ProprietorModel mProprietor;
        /// <summary>
        /// 取得 ProprietorModel 物件
        /// </summary>
        public static ProprietorModel proprietor
        {

            get
            {
                if (mProprietor == null)
                {
                    mProprietor = new ProprietorModel();
                }
                return mProprietor;
            }
        }

        public List<Order> SelectUndoOrder(IConfiguration config)
        {
            var order_list = new List<Order>();
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //連線字串
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
                //begin tran
                db.BeginTran();
                //取得菜單表頭，並排序(正序)
                var list_h = db.Queryable<Menulisth>().Where(item => item.OrderDone == false).OrderBy(menu => menu.Createtime, OrderByType.Asc).ToList();
                var list_b = db.Queryable<Menulistb, Menulisth>((body, head) => new JoinQueryInfos(JoinType.Left, head.Uid == body.H_uid))
                    .Where((body, head) => head.Createtime >= DateTime.Today)
                    .OrderBy((body, head) => head.Createtime, OrderByType.Asc)
                    .Select<Menulistb>().ToList();
                //將表頭加入到表身資料寫入
                foreach (var head in list_h)
                {

                    var order = new Order();
                    //加入表頭
                    order.head = head;
                    //加入表身
                    foreach (var body in list_b)
                    {
                        if (body.H_uid == head.Uid)
                        {
                            order.bodys.Add(body);
                        }
                    }
                    order_list.Add(order);

                }

                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();//rollback
                throw;
            }
            return order_list;
        }

        public List<Order> SelectDoneOrder(IConfiguration config)
        {
            var order_list = new List<Order>();
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //連線字串
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
                //begin tran
                db.BeginTran();
                //取得菜單表頭，並排序(正序)
                var list_h = db.Queryable<Menulisth>().Where(head => head.OrderDone == true && head.Createtime >= DateTime.Today).OrderBy(menu => menu.Createtime, OrderByType.Asc).ToList();
                var list_b = db.Queryable<Menulistb, Menulisth>((body, head) => new JoinQueryInfos(JoinType.Left, head.Uid == body.H_uid))
                    .Where((body, head) => head.OrderDone == true && head.Createtime >= DateTime.Today)
                    .OrderBy((body, head) => head.Createtime, OrderByType.Asc)
                    .Select<Menulistb>().ToList();
                //將表頭加入到表身資料寫入
                foreach (var head in list_h)
                {

                    var order = new Order();
                    //加入表頭
                    order.head = head;
                    //加入表身
                    foreach (var body in list_b)
                    {
                        if (body.H_uid == head.Uid)
                        {
                            order.bodys.Add(body);
                        }
                    }
                    order_list.Add(order);

                }

                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();//rollback
                throw;
            }
            return order_list;
        }

        public bool UpdateOrder(IConfiguration config, string orders_string)
        {
            var successful = false;
            var orders = JsonConvert.DeserializeObject<List<Order>>(orders_string);
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //連線字串
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
                //begin tran
                db.BeginTran();
                //取得菜單表頭，並排序(正序)
                foreach (var order in orders)
                {
                    //更新表頭中的訂單狀態
                    var result_h = db.Updateable(order.head).UpdateColumns(head => new { head.OrderDone }).ExecuteCommand();
                    //更新表身中的餐點狀態
                    var result_b = db.Updateable(order.bodys).UpdateColumns(body => new { body.ItemDone }).ExecuteCommand();
                }
                db.CommitTran();
                successful = true;
            }
            catch
            {
                successful = false;
                db.RollbackTran();//rollback
                throw;
            }
            return successful;
        }

        public List<Order> SelectOrder(IConfiguration config)
        {
            var order_list = new List<Order>();
            //連線設定
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //連線字串
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
                //begin tran
                db.BeginTran();
                //取得菜單表頭，並排序(正序)
                var list_h = db.Queryable<Menulisth>().Where(head => head.Createtime >= DateTime.Today).OrderBy(head => head.Createtime, OrderByType.Asc).ToList();
                var list_b = db.Queryable<Menulistb, Menulisth>((body, head) => new JoinQueryInfos(JoinType.Left, head.Uid == body.H_uid))
                    .Where((body, head) => head.Createtime >= DateTime.Today)
                    .OrderBy((body, head) => head.Createtime, OrderByType.Asc)
                    .Select<Menulistb>().ToList();
                //將表頭加入到表身資料寫入
                foreach (var head in list_h)
                {
                    head.Formnum = head.Formnum.Replace(" ", "").Trim();
                    var order = new Order();
                    //加入表頭
                    order.head = head;
                    //加入表身
                    foreach (var body in list_b)
                    {
                        if (body.H_uid == head.Uid)
                        {
                            body.Item = body.Item.Replace(" ", "").Trim();
                            order.bodys.Add(body);
                        }
                    }
                    order_list.Add(order);

                }

                db.CommitTran();
            }
            catch
            {
                db.RollbackTran();//rollback
                throw;
            }
            return order_list;
        }

        public List<Order> DataCompare(List<Order> db_orders, string data)
        {
            var client_orders = JsonConvert.DeserializeObject<List<Order>>(data);
            var order_Enumable = from db in db_orders
                                 where !(from client in client_orders
                                         select client.head.Uid)
                                         .Contains(db.head.Uid)
                                 select db;
            var orders = order_Enumable.ToList();
            return orders;
        }

    }

    

}
