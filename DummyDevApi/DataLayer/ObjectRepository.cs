using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DummyDevApi.DataLayer
{
    public class ObjectRepository : IObjectRepository
    {
        private List<object> _data;

        public ObjectRepository(List<object> data)
        {
            this._data = data;
        }

        public virtual List<object> Get(Func<object, bool> filter = null)
        {
            return filter != null 
                ? this._data.Where(filter).ToList()
                : this._data;
        }

        public virtual object GetById(string id)
        {
            var item = this._data.FirstOrDefault(el => {
                var tmp = (JObject)el;
                if (tmp.ContainsKey("id"))
                {
                    return tmp.Value<string>("id") == id;
                }
                return false;
            });
            return item;
        }

        public virtual void Insert(object entity)
        {
            this._data.Add(entity);
        }

        public virtual void Update(string id, object entity)
        {
            this._data.Remove(id);
            this._data.Add(entity);
        }

        public virtual void Delete(string id)
        {
            var itemToDelete = GetById(id);
            this._data.Remove(itemToDelete);
        }
    }
}
