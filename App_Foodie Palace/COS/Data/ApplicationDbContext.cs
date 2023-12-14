using Microsoft.EntityFrameworkCore;
using COS.Models;

public class ApplicationDbContext:DbContext
{
public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){}
public DbSet<Logindetails> Logindetails{get;set;}
public DbSet<AdminLogin> Admindetails{get;set;}
}