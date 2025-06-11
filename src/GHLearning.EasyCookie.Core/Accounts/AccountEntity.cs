namespace GHLearning.EasyCookie.Core.Accounts;

public record AccountEntity
{
	public string Account { get; set; } = default!;
	public string Password { get; set; } = default!;
	public string Name { get; set; } = default!;
}
