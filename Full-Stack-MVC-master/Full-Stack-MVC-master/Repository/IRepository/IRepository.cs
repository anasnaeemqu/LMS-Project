namespace mvcLab.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        T GetById(int id, string? includeProperties = null);
        void Add(T entity);

        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
