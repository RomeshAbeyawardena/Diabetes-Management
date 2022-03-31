using DiabetesManagement.Shared.Extensions;
using NUnit.Framework;

namespace DiabetesManagement.Shared.Tests
{
    public class StringExtensionTests
    {
        [Test]
        public void Test()
        {
            var expectedValue = "The blue Moon";
            var caseSignature = expectedValue.GetCaseSignature();
            Assert.AreEqual("AgQEBAQEBAQEAgQEBA==", caseSignature);
            Assert.AreEqual(expectedValue, caseSignature.ProcessCaseSignature("the blue moon"));
        }
    }
}
