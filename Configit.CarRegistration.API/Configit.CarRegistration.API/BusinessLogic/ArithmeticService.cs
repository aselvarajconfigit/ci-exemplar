using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configit.CarRegistration.API.BusinessLogic {
  public sealed class ArithmeticService {

    public int Add( int x, int y ) {
      return x + y;
    }

    public int subtract ( int x, int y ) {
      return x - y;
    }
  }
}
