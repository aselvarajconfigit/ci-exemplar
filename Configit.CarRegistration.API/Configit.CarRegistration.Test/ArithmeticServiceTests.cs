using Configit.CarRegistration.API.BusinessLogic;
using NUnit.Framework;

namespace Configit.CarRegistration.Tests {
  public class ArithmeticServiceTests {

    private ArithmeticService _arithmeticService;

    [SetUp]
    public void Setup() {
      _arithmeticService = new ArithmeticService();
    }

    [Test]
    public void AddNumbersCorrect() {
      var x = 1;
      var y = 2;

      var result = _arithmeticService.Add( x, y );
      
      Assert.AreEqual( x+y, result );
    }
  }
}