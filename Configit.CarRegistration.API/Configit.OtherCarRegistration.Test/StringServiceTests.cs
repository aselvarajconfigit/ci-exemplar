using Configit.CarRegistration.API.BusinessLogic;
using NUnit.Framework;

namespace Configit.OtherCarRegistration.Test {
  public class Tests {

    private StringService _stringService;
    [SetUp]
    public void Setup() {
      _stringService = new StringService();
    }

    [Test]
    public void ConcatStringsCorrect() {
      string a = "test";
      string b = "string";
      Assert.AreEqual( a+b, _stringService.ConcatString( a, b ) );
    }
  }
}