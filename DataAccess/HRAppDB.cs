using HRApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HRApp.DataAccess
{
    public class HRAppDb : DbContext
{
    public HRAppDb(DbContextOptions<HRAppDb> options) : base(options) {}
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Order> Orders => Set<Order>();
}
}

