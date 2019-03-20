using System;
using System.Collections.Generic;
using DummyDevApi.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DummyDevApi.Controllers
{
    [Route("api/{resource}/{id?}")]
    [ApiController]
    public class ApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private Dictionary<string, IObjectRepository> _repos = new Dictionary<string, IObjectRepository>();

        public ApiController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        
        public ActionResult Index(string resource, string id)
        {
            try
            {
                if (!this._repos.ContainsKey(resource))
                {
                    var repo = this._unitOfWork.GetRepository(resource);
                    this._repos.Add(resource, repo);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(NotFound());
            }
            var methodUsed = HttpContext.Request.Method;
            switch(methodUsed.ToUpper()){
                case "DELETE":
                    return ExecuteDeleteMethod(resource, id);
                case "POST":
                    return ExecutePostMethod(resource);
                case "PUT":
                    return ExecutePutMethod(resource, id);
                case "GET": 
                default: return ExecuteGetMethod(resource, id);
            }
        }

        /// <summary>
        /// Returns the result of a get method call
        /// </summary>
        /// <returns>The while collection or an element by ID if specified</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="id">Identifier of particular element</param>
        private ActionResult ExecuteGetMethod(string resource, string id)
        {
            var repo = this._repos[resource];
            if (string.IsNullOrEmpty(id))
            {
                return Json(repo.Get());
            }
            var item = repo.GetById(id);
            return item != null ? Json(item) : Json(NotFound());
        }

        /// <summary>
        /// Executes the post method.
        /// </summary>
        /// <returns>The post method result.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="entity">Entity.</param>
        private ActionResult ExecutePostMethod(string resource)
        {
            var entity = GetBody();
            if (entity == null) return BadRequest();
            
            var id = "";
            if (entity.ContainsKey("id"))
            {
                id = entity.Value<string>("id");
            }
            
            var repo = this._repos[resource];
            if(repo.GetById(id) != null) return Conflict();
            
            repo.Insert(entity);
            this._unitOfWork.Save();
            
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{id}", entity);
        }

        /// <summary>
        /// Executes the put method.
        /// </summary>
        /// <returns>The put method result.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="entity">Entity.</param>
        private ActionResult ExecutePutMethod(string resource, string id)
        {
            var entity = GetBody();
            if (string.IsNullOrEmpty(id) || entity == null)
            {
                return BadRequest();
            }
            
            var repo = this._repos[resource];
            repo.Update(id, entity);
            this._unitOfWork.Save();
            
            return Ok();
        }

        /// <summary>
        /// Executes the delete method.
        /// </summary>
        /// <returns>The delete method result.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="entity">Entity.</param>
        private ActionResult ExecuteDeleteMethod(string resource, string id)
        {
            var repo = this._repos[resource];
            repo.Delete(id);
            this._unitOfWork.Save();

            return NoContent();
        }

        /// <summary>
        /// Extracts body from the request.
        /// </summary>
        /// <returns>The request body as JObject</returns>
        private JObject GetBody()
        {
            using (var reader = new System.IO.StreamReader(Request.Body, System.Text.Encoding.UTF8, true, 1024, true))
            {
                var body = reader.ReadToEndAsync().Result;
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(body);
                return obj;
            }
        }
    }
}
