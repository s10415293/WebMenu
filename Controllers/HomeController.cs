using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using System.Diagnostics;
using WebMenu.Models;
using WebMenu.Models.api;
using WebMenu.Models.DTOClass;

namespace WebMenu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Customer()
        {
            var customer = CustomerModel.customer;
            var menu_list = customer.SelectMenu(_config);
            string jsonData = Common.Obj2JsonString(menu_list);
            ViewBag.menuData = jsonData;
            return View();
        }

        public IActionResult Proprietor()
        {     
                
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult CreateOrder(string data, string phone)
        {
            var customer = CustomerModel.customer;
            customer.CreateOrder(data, phone, out var order_body, out var order_head);
            customer.insertOrder(_config, order_body, order_head);

            return Ok();
        }

        [HttpPost]
        public IActionResult GetUndoOrder()
        {
            var proprietor = ProprietorModel.proprietor;
            var list = proprietor.SelectUndoOrder(_config);
            //自動轉成json格式
            return Json(list);
        }

        [HttpPost]
        public IActionResult GetDoneOrder()
        {
            var proprietor = ProprietorModel.proprietor;
            var list = proprietor.SelectDoneOrder(_config);
            //自動轉成json格式
            return Json(list);
        }



        [HttpPost]
        public IActionResult ExchangeData(string data)
        {
            Console.WriteLine("data:" + data);

            var proprietor = ProprietorModel.proprietor;
            //更新資料
            bool successful = proprietor.UpdateOrder(_config, data);
            //取得今天的訂單清單
            var orders = proprietor.SelectOrder(_config);
            //比對出新的資料
            var new_orders = proprietor.DataCompare(orders, data);
            //自動轉成json格式
            return Json(new_orders);
        }



        private IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
    }
}