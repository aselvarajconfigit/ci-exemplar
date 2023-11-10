using Microsoft.EntityFrameworkCore;
using Configit.CarRegistration.API.Models;

namespace Configit.CarRegistration.API.Data {
  public class CarRegistrationDbContext: DbContext {
    public CarRegistrationDbContext( DbContextOptions<CarRegistrationDbContext> options )
      : base( options ) {

    }

    public DbSet<CarRegistrationDetails> CarRegistrations { get; set; }
  }
}
