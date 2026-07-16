using Xunit;

namespace Trainee_Tracker.E2ETests;


//Code-Owner: Julia Sandner
/// <summary>
/// This class links the collection to the MentorTestFixture so that only one fixture is created, 
/// which is shared by all test classes in the collection.
/// </summary>
[CollectionDefinition("Mentor Tests")]
public class MentorTestCollection : ICollectionFixture<MentorTestFixture>
{
}