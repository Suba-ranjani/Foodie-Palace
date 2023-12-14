using Microsoft.EntityFrameworkCore;
using FeedbackApi.Models;
namespace FeedbackApi.Data;
public class ApplicationDbContext:DbContext
{
public  ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){
    
}
public DbSet<Customerfeedback> UserFeedback{get;set;}
}