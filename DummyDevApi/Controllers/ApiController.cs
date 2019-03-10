using System;
using System.Collections.Generic;
using DummyDevApi.DataLayer;
using Microsoft.AspNetCore.Mvc;

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
                case "POST":
                    return ExecutePostMethod(resource, id);

                case "DELETE":
                    return ExecuteDeleteMethod(resource, id);

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
        private ActionResult ExecutePostMethod(string resource, object entity)
        {
            return Json("POST METHOD");
        }

        /// <summary>
        /// Executes the put method.
        /// </summary>
        /// <returns>The put method result.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="entity">Entity.</param>
        private ActionResult ExecutePutMethod(string resource, object entity)
        {
            return Json("PUT METHOD");
        }

        /// <summary>
        /// Executes the delete method.
        /// </summary>
        /// <returns>The delete method result.</returns>
        /// <param name="resource">Resource.</param>
        /// <param name="entity">Entity.</param>
        private ActionResult ExecuteDeleteMethod(string resource, object entity)
        {
            return Json("DELETE METHOD");
        }
    }
}
