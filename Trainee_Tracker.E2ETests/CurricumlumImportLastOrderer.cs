using Xunit.Abstractions;

namespace Trainee_Tracker.E2ETests;

public class CurricumlumImportLastOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
    {
        var regular = testCollections.Where( c => c.DisplayName != "Curriculum Import Tests");
        var importCollection = testCollections.Where( c=> c.DisplayName == "Curriculum Import Tests");
        return regular.Concat(importCollection);
    }
}