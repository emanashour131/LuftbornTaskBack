namespace LuftbornTask;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    public EmployeesController(IEmployeeRepository repository) => _repository = repository;

    [HttpPost]
    public virtual async Task Post(Employee employee) => await _repository.Add(employee);

    [HttpGet]
    public virtual async Task<IEnumerable<Employee>> Get() => await _repository.Get();
    [HttpGet("{id}")]
    public virtual async Task<Employee> Get([FromRoute] Guid id) => await _repository.Get(id);

    [HttpPut]
    public async virtual Task Put(Employee employee) => await _repository.Update(employee);

    [HttpDelete("{id}")]
    public async virtual Task Delete(Guid id) => await _repository.Remove(id);
}
