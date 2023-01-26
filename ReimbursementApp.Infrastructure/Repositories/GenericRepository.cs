using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class GenericRepository<T>: IGenericRepository<T> where T: class
{
    private readonly DbSet<T> _context;
    private readonly ReimburseContext _dbContext;

    public GenericRepository(ReimburseContext context)
    {
        _dbContext = context;
        _context = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.ToListAsync();
    }

    public async Task<T?> Get(int id)
    {
        return await _context.FindAsync(id);
    }

    public async Task<T> Add(T entity)
    {
        var result = _context.Add(entity);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> Update(T entity)
    { 
        var result = _context.Update(entity);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task Delete(T entity)
    {
        var result = _context.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}