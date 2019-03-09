using System.Collections.Generic;
using System.Linq;
using DummyDevApi.Classes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DummyDevApi.Controllers
{
    [Route("api/{resource}/{id?}")]
    [ApiController]
    public class ApiController : Controller
    {
        private Dictionary<string, List<object>> _db;
        public ActionResult Index(string resource, string id)
        {
            var dbReader = new DbReader();
            this._db = dbReader.GetDb();
            if (!this._db.ContainsKey(resource))
            {
                return Json(NotFound());
            }
            var methodUsed = HttpContext.Request.Method;
            switch(methodUsed.ToUpper()){
                case "POST":
                    return Json("POST METHOD");
                case "DELETE":
                    return Json("DELETE METHOD");
                case "PUT":
                    return Json("PUT METHOD");
                case "GET":
                default: return GetMethod(resource, id);
            }

        }

        /// <summary>
        /// Returns the result of a get method call
        /// </summary>
        /// <returns>The while collection or an element by ID if specified</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="id">Identifier of particular element</param>
        private ActionResult GetMethod(string resource, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(this._db[resource]);
            }
            var item = this._db[resource].FirstOrDefault(el => {
                var tmp = (JObject)el;
                if (tmp.ContainsKey("id"))
                {
                    return tmp.Value<string>("id") == id;
                }
                return false;
            });
            return item != null ? Json(item) : Json(NotFound());
        }
    }
}
