
using Microsoft.EntityFrameworkCore;
using Data.DataModels;
namespace Data;

// Primary constructor to initialize
public class ClubEventsContext(DbContextOptions<ClubEventsContext> options) : DbContext(options)
{

   public DbSet<Event> Events { get; set; }
   public DbSet<EventOption> EventOptions { get; set; }
    
    // Our two dbSets, just table per class
}