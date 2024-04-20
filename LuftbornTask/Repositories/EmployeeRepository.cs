namespace LuftbornTask;

public class EmployeeRepository : IEmployeeRepository
{
    protected DbSet<Employee> dbSet;
    private readonly ApplicationDbContext _context;
    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
        dbSet = _context.Set<Employee>();
    }

    public virtual async Task<IEnumerable<Employee>> Get() => await dbSet.ToListAsync();
    public virtual async Task<Employee> Get(Guid id) => await
                                          dbSet.FirstOrDefaultAsync(e => e.Id == id) ?? Activator.CreateInstance<Employee>();

    public virtual async Task Add(Employee entity)
    {
        ThrowExceptionIfParameterNotSupplied(entity);

        await dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }
    public virtual async Task Add(IEnumerable<Employee> entities)
    {
        ThrowExceptionIfParameterNotSupplied(entities);

        await dbSet.AddRangeAsync(entities);
        await SaveChangesAsync();
    }

    public virtual async Task Update(List<Employee> entities)
    {
        foreach (Employee entity in entities)
            await Update(entity);

        await SaveChangesAsync();
    }
    public virtual async Task Update(Employee entity)
    {
        ThrowExceptionIfParameterNotSupplied(entity);

        await ThrowExceptionIfIfEntityExistsInDatabase(entity);

        await Task.Run(() => dbSet.Update(entity));

        await SaveChangesAsync();
    }
    private async Task ThrowExceptionIfIfEntityExistsInDatabase(Employee entity)
    {
        if (entity.Id == null && entity.Id != Guid.Empty)
            throw new ArgumentNullException($"Id of {typeof(Employee).Name} is null or has an empty GUID");

        Employee? entityFromDb = await Get(entity.Id.Value);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(Employee)} was not found in DB");
    }

    public virtual async Task Remove(Guid id)
    {
        Employee? entityFromDb = await Get(id);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(Employee)} was not found in DB");

        await Task.Run(() => dbSet.Remove(entityFromDb));
        await SaveChangesAsync();
    }
    public virtual async Task Remove(Employee entity)
    {
        if (entity == null || entity.Id == null)
            throw new ArgumentNullException($"{nameof(Employee)} was not provided.");

        Employee? entityFromDb = await Get(entity.Id.Value);
        if (entityFromDb == null)
            throw new ArgumentNullException($"{nameof(Employee)} was not found in DB");

        await Task.Run(() => dbSet.Remove(entity));
        await SaveChangesAsync();
    }
    public virtual async Task Remove(IEnumerable<Employee> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(Employee)} was not provided.");

        await Task.Run(() => dbSet.RemoveRange(entities));
        await SaveChangesAsync();
    }

    private static void ThrowExceptionIfParameterNotSupplied(Employee entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"{nameof(Employee)} was not provided.");
    }
    private static void ThrowExceptionIfParameterNotSupplied(IEnumerable<Employee> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentNullException($"{nameof(Employee)} was not provided.");
    }

    public async Task<IDbContextTransaction> GetTransaction() => await _context.Database.BeginTransactionAsync();

    protected async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
