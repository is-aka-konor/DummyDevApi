using System;
using System.Collections.Generic;
using DummyDevApi.Classes;

namespace DummyDevApi.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<string, List<object>> _dataSource;
        private JsonDbEngine _engine;

        public UnitOfWork()
        {
            this._engine = new JsonDbEngine();
            this._dataSource = this._engine.GetDb();
        }

        public IObjectRepository GetRepository(string key)
        {
            if(!this._dataSource.ContainsKey(key))
            {
                throw new Exception($"There is no data in the source for the endpoint: {key}");
            }
            return new ObjectRepository(this._dataSource[key]);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
