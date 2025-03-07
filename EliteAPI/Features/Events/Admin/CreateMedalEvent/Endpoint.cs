using EliteAPI.Authentication;
using EliteAPI.Features.Events.Services;
using EliteAPI.Models.Common;
using EliteAPI.Models.DTOs.Outgoing;
using EliteAPI.Models.Entities.Events;
using EliteAPI.Services.Interfaces;
using FastEndpoints;

namespace EliteAPI.Features.Events.Admin.CreateMedalEvent;

internal sealed class CreateMedalEventRequest : DiscordIdRequest {
	[FromBody]
	public required CreateMedalEventDto Event { get; set; }
}

internal sealed class CreateMedalEventEndpoint(
	IDiscordService discordService,
	IEventService eventService,
	AutoMapper.IMapper mapper
) : Endpoint<CreateMedalEventRequest, EventDetailsDto> {

	public override void Configure() {
		Post("/guild/{DiscordId}/events/medals");
		Options(o => o.WithMetadata(new GuildAdminAuthorizeAttribute()));
		Version(0);

		Summary(s => {
			s.Summary = "Create Weight Event";
		});
	}

	public override async Task HandleAsync(CreateMedalEventRequest request, CancellationToken c) {
		var guild = await discordService.GetGuild(request.DiscordIdUlong);
		if (guild is null) {
			await SendNotFoundAsync(c);
			return;
		}
        
		if (!eventService.CanCreateEvent(guild, out var reason)) {
			ThrowError(reason);
		}

		request.Event.Type = EventType.Medals;
		var result = await eventService.CreateEvent(request.Event, request.DiscordIdUlong);

		if (result.Value is null) {
			ThrowError(result.Result?.ToString() ?? "Failed to create event");
		}

		var mapped = mapper.Map<EventDetailsDto>(result.Value);
		await SendAsync(mapped, cancellation: c);
	}
}

internal sealed class CreateMedalEventRequestValidator : Validator<CreateMedalEventRequest> {
	public CreateMedalEventRequestValidator() {
		Include(new DiscordIdRequestValidator());
	}
}