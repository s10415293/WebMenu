using Newtonsoft.Json;

namespace WebMenu.Models.api
{
    public class Common
    {
        private Common() { }
        public static string Obj2JsonString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
    
}
