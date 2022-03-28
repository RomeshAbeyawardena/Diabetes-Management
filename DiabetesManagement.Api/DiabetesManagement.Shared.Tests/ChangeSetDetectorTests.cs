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
            var source = new Models.Inventory { Modified = System.DateTimeOffset.Now };
            var destination = new Models.Inventory { };
            var changes = defaultChangeSetDetector!.DetectChanges(source, destination);

            Assert.AreEqual(1, changes.ChangedProperties.Count);
            destination = (Models.Inventory)changes.CommitChanges(source);

            Assert.True(changes.HasChanges);
            Assert.AreEqual(source.Modified, destination.Modified);
        }

        [Test] public void Commit()
        {
            var source = new Models.Inventory { Modified = System.DateTimeOffset.Now };
            var destination = new Models.Inventory { };
            var changes = defaultChangeSetDetector!.DetectChanges(source, destination);
            destination = (Models.Inventory)changes.CommitChanges(source);

            Assert.True(changes.HasChanges);
            Assert.AreEqual(source.Modified, destination.Modified);
        }
    }
}
