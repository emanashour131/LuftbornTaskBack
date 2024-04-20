namespace LuftbornTask;

public interface IEmployeeRepository : IDisposable
{
    Task Add(Employee entity);
    Task Add(IEnumerable<Employee> entities);

    Task<IEnumerable<Employee>> Get();
    Task<Employee> Get(Guid id);

    Task Update(Employee entity);
    Task Update(List<Employee> entities);

    Task Remove(Guid id);
    Task Remove(Employee entity);
    Task Remove(IEnumerable<Employee> entities);

    Task<IDbContextTransaction> GetTransaction();
}