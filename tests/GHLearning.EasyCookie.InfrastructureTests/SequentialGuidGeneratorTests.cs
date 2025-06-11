using GHLearning.EasyCookie.Infrastructure;

namespace GHLearning.EasyCookie.InfrastructureTests;
public class SequentialGuidGeneratorTests
{
	[Fact]
	public void NewId_ShouldReturnSequentialGuid()
	{
		// Arrange
		var generator = new SequentialGuidGenerator();
		var previousGuid = Guid.Empty;
		// Act
		var newGuid = generator.NewId();
		// Assert
		Assert.NotEqual(Guid.Empty, newGuid);
		Assert.True(newGuid.CompareTo(previousGuid) > 0, "New GUID should be greater than the previous one.");
	}
}
