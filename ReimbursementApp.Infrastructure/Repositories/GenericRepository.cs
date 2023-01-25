using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class GenericRepository<T>: IGenericRepository<T> where T: class
{
    private readonly ReimburseContext _context;

    public GenericRepository(ReimburseContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? Get(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public T Update(T entity)
    { 
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
        return entity;
    }
}