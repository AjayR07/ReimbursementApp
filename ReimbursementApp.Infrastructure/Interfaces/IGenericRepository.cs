namespace ReimbursementApp.Infrastructure.Interfaces;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAll();

    Task<T?> Get(int id);

    Task<T> Add(T entity);

    Task<T> Update(T entity);
    
    Task Delete(T entity);
}