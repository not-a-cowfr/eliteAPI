using EliteAPI.Features.Leaderboards.Models;

namespace EliteAPI.Features.Leaderboards.Definitions;

public class PestMiteLeaderboard : IMemberLeaderboardDefinition {
	public LeaderboardInfo Info { get; } = new() {
		Title = "Mite Kills",
		ShortTitle = "Mite",
		Slug = "mite",
		Category = "Pests",
		IntervalType = [LeaderboardType.Current],
		ScoreDataType = LeaderboardScoreDataType.Long
	};

	public IConvertible? GetScoreFromMember(EliteAPI.Models.Entities.Hypixel.ProfileMember member) {
		var pest = member.Farming.Pests.Mite;
		
		if (pest == 0) return null;
		return pest;
	}
}