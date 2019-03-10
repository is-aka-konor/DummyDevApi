namespace DummyDevApi.DataLayer
{
    public interface IUnitOfWork
    {
        IObjectRepository GetRepository(string key);
        void Save();
    }
}
