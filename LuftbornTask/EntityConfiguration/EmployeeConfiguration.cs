namespace LuftbornTask;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);
        builder.Property(c => c.Id).HasValueGenerator<GuidValueGenerator>();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Mobile).HasMaxLength(20);
        builder.Property(e => e.Telephone).HasMaxLength(20);
    }
}