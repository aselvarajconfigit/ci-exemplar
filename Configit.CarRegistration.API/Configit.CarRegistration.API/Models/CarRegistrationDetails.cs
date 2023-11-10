using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Configit.CarRegistration.API.Models {
  public class CarRegistrationDetails {
    [Key]
    public int Id { get; set; }

    [Column( TypeName = "NVARCHAR(10)" )]
    public string LicensePlate { get; set; }

    [Column( TypeName = "NVARCHAR(100)" )]
    public string Make { get; set; }

    [Column( TypeName = "NVARCHAR(100)" )]
    public string Model { get; set; }

    [Column( TypeName = "NVARCHAR(100)" )]
    public string Colour { get; set; }

    [Column( TypeName = "FLOAT" )]
    public decimal EngineCapacity { get; set; }

    [Column( TypeName = "SMALLINT" )]
    public int Horsepower { get; set; }

    [Column( TypeName = "DATETIME" )]
    public DateTime RegisteredAt { get; set; }

  }
}
