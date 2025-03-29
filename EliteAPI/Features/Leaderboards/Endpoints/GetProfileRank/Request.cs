using System.ComponentModel;
using EliteAPI.Configuration.Settings;
using EliteAPI.Features.Leaderboards.Services;
using EliteAPI.Models.Common;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace EliteAPI.Features.Leaderboards.Endpoints.GetProfileRank;

public class GetProfileRankRequest : ProfileUuidRequest {
	/// <summary>
	/// Id of leaderboard
	/// </summary>
	public required string Leaderboard { get; set; }
	
	/// <summary>
	/// Include upcoming players
	/// </summary>
	[QueryParam, DefaultValue(false)]
	public bool? IncludeUpcoming { get; set; } = false;
	
	/// <summary>
	/// Amount of upcoming players to include (max 100). Only works with new leaderboard backend
	/// </summary>
	[QueryParam, DefaultValue(0)]
	public int? Upcoming { get; set; } = 0;
	
	/// <summary>
	/// Start at a specified rank for upcoming players
	/// </summary>
	[QueryParam]
	public int? AtRank { get; set; } = -1;
	
	/// <summary>
	/// Use new leaderboard backend (will be default in the future)
	///	</summary>
	[QueryParam, DefaultValue(true)]
	public bool? New { get; set; } = true;
}

internal sealed class GetProfileRankRequestValidator : Validator<GetProfileRankRequest> {
	public GetProfileRankRequestValidator() {
		Include(new ProfileUuidRequestValidator());
		var lbSettings = Resolve<IOptions<ConfigLeaderboardSettings>>();
		var newLbService = Resolve<ILeaderboardRegistrationService>();
		RuleFor(x => x.Leaderboard)
			.NotEmpty()
			.WithMessage("Leaderboard is required")
			.When(x => lbSettings.Value.HasLeaderboard(x.Leaderboard) 
			           || (x.New is true && newLbService.LeaderboardsById.ContainsKey(x.Leaderboard)))
			.WithMessage("Leaderboard does not exist");
		
		RuleFor(x => x.Upcoming)
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(20)
			.WithMessage("Upcoming must be between 0 and 20");
	}
}