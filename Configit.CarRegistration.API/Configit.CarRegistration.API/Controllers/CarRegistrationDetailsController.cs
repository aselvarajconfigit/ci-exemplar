using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Configit.CarRegistration.API.Data;
using Configit.CarRegistration.API.Models;

namespace Configit.CarRegistration.API.Controllers {

  [Route( "api/[controller]" )]
  [ApiController]
  public class CarRegistrationDetailsController: ControllerBase {
    private readonly CarRegistrationDbContext _context;

    public CarRegistrationDetailsController( CarRegistrationDbContext context ) {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarRegistrationDetails>>> GetCarRegistrations() {
      return await _context.CarRegistrations.ToListAsync();
    }

    [HttpGet( "{id}" )]
    public async Task<ActionResult<CarRegistrationDetails>> GetCarRegistrationDetails( int id ) {
      var carRegistrationDetails = await _context.CarRegistrations.FindAsync( id );

      if ( carRegistrationDetails == null ) {
        return NotFound();
      }

      return carRegistrationDetails;
    }

    [HttpPut( "{id}" )]
    public async Task<IActionResult> PutCarRegistrationDetails( int id, CarRegistrationDetails carRegistrationDetails ) {
      if ( id != carRegistrationDetails.Id ) {
        return BadRequest();
      }

      _context.Entry( carRegistrationDetails ).State = EntityState.Modified;

      try {
        await _context.SaveChangesAsync();
      }
      catch ( DbUpdateConcurrencyException ) {
        if ( !CarRegistrationDetailsExists( id ) ) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CarRegistrationDetails>> PostCarRegistrationDetails( CarRegistrationDetails carRegistrationDetails ) {
      _context.CarRegistrations.Add( carRegistrationDetails );
      await _context.SaveChangesAsync();

      return CreatedAtAction( "GetCarRegistrationDetails", new { id = carRegistrationDetails.Id }, carRegistrationDetails );
    }

    // DELETE: api/CarRegistrationDetails/5
    [HttpDelete( "{id}" )]
    public async Task<ActionResult<CarRegistrationDetails>> DeleteCarRegistrationDetails( int id ) {
      var carRegistrationDetails = await _context.CarRegistrations.FindAsync( id );
      if ( carRegistrationDetails == null ) {
        return NotFound();
      }

      _context.CarRegistrations.Remove( carRegistrationDetails );
      await _context.SaveChangesAsync();

      return carRegistrationDetails;
    }

    private bool CarRegistrationDetailsExists( int id ) {
      return _context.CarRegistrations.Any( e => e.Id == id );
    }
  }
}
