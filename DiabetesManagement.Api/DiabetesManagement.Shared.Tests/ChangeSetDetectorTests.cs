using DiabetesManagement.Shared.Defaults;
using NUnit.Framework;

namespace DiabetesManagement.Shared.Tests
{
    public class ChangeSetDetectorTests
    {
        DefaultChangeSetDetector? defaultChangeSetDetector;
        [SetUp]
        public void SetUp()
        {
            defaultChangeSetDetector = new DefaultChangeSetDetector();
        }

        [Test]
        public void DetectChanges()
        {
            var changes = defaultChangeSetDetector!.DetectChanges(new Models.Inventory { Modified = System.DateTimeOffset.Now }, new Models.Inventory { });
        }
    }
}
