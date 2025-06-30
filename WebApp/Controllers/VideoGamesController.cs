using Application.VideoGames.Commands;
using Application.VideoGames.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/video-games")]
public class VideoGamesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideoGamesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: /api/video-games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllVideoGamesQuery.Result>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<GetAllVideoGamesQuery.Result> response = await _mediator.Send(new GetAllVideoGamesQuery(), cancellationToken);
        return Ok(response);
    }

    // GET: /api/video-games/{id}
    [HttpGet("{videoGameId}")]
    public async Task<ActionResult<GetVideoGameDetailByIdQuery.Result>> GetById(int videoGameId, CancellationToken cancellationToken)
    {
        GetVideoGameDetailByIdQuery.Result? result = await _mediator.Send(new GetVideoGameDetailByIdQuery { VideoGameId = videoGameId }, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    // POST: /api/video-games
    [HttpPost]
    public async Task<ActionResult<AddVideoGameCommand.Result>> Create([FromBody] AddVideoGameCommand command, CancellationToken cancellationToken)
    {
        AddVideoGameCommand.Result result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { videoGameId = result.VideoGameId }, result);
    }

    // DELETE: /api/video-games/{id}
    [HttpDelete("{videoGameId}")]
    public async Task<ActionResult<DeleteVideoGameCommand.Result>> Delete(int videoGameId, CancellationToken cancellationToken)
    {
        DeleteVideoGameCommand.Result result = await _mediator.Send(new DeleteVideoGameCommand { VideoGameId = videoGameId }, cancellationToken);
        return Ok(result);
    }
    
    // PATCH: /api/video-games/{id}
    [HttpPatch("{videoGameId}")]
    public async Task<ActionResult<UpdateVideoGameCommand.Result>> Update(int videoGameId, [FromBody] UpdateVideoGameRequest request, CancellationToken cancellationToken)
    {
        UpdateVideoGameCommand.Result result = await _mediator.Send(new UpdateVideoGameCommand
        {
            VideoGameId = videoGameId,
            Title = request.Title,
            Genre = request.Genre,
            ReleaseDate = request.ReleaseDate
        }, cancellationToken);

        return Ok(result);
    }

    // Request model for PATCH
    public sealed class UpdateVideoGameRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}
