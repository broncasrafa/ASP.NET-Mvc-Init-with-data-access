namespace DataAccess.Providers.ADO.Interfaces
{
    public interface IUnitOfWork
    {
        void Dispose();
        void SaveChanges();
    }
}
