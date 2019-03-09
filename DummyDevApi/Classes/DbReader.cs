using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DummyDevApi.Classes
{
    public class DbReader
    {
        private Dictionary<string, List<object>> _dictionary;
        public DbReader()
        {
            var file = "db.json";
            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                this._dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(json);
            }
        }

        public Dictionary<string, List<object>> GetDb()
        {
            return this._dictionary;
        }
    }
}
