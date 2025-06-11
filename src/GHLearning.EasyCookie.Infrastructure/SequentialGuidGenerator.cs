using GHLearning.EasyCookie.SharedKernel;

namespace GHLearning.EasyCookie.Infrastructure;
internal class SequentialGuidGenerator : ISequentialGuidGenerator
{
	public Guid NewId() => SequentialGuid.SequentialGuidGenerator.Instance.NewGuid();
}
