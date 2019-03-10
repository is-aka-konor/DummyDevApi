using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DummyDevApi.Classes
{
    public class JsonDbEngine
    {
        private const string _dbFileName = "db.json";
        private Dictionary<string, List<object>> _dictionary;
        public JsonDbEngine()
        {
            using (StreamReader r = new StreamReader(_dbFileName))
            {
                string json = r.ReadToEnd();
                this._dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(json);
            }
        }

        public Dictionary<string, List<object>> GetDb()
        {
            return this._dictionary;
        }

        public Task SaveDb(Dictionary<string, List<object>> data)
        {
            // TODO make multithread save writing to the file
            var jsonData = JsonConvert.SerializeObject(data);
            return File.WriteAllTextAsync(_dbFileName, jsonData);
        }
    }
}
