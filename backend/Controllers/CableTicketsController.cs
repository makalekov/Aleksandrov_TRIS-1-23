using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/cable-tickets")]
public class CableTicketsController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly ILogger<CableTicketsController> _logger;

    public CableTicketsController(ITicketService service, ILogger<CableTicketsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private int GetUserId()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var value) && int.TryParse(value, out var id))
            return id;
        return 1; // По умолчанию Диспетчер
    }

    [HttpGet]
    public async Task<ActionResult<List<TicketResponse>>> GetAll([FromQuery] int? assigneeUserId, CancellationToken cancellationToken)
    {
        var tickets = await _service.GetAll(assigneeUserId, cancellationToken);
        return Ok(tickets.Select(MapToResponse).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var ticket = await _service.GetById(id, cancellationToken);
        if (ticket == null) return Problem(statusCode: 404, title: "Заявка не найдена");
        return Ok(MapToResponse(ticket));
    }

    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create([FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var ticket = await _service.Create(request, GetUserId(), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, MapToResponse(ticket));
        }
        catch (TicketNumberConflictException ex)
        {
            _logger.LogWarning("CreateTicket number conflict; siteId={SiteId}; error={ErrorType}", request.SiteId, ex.GetType().Name);
            return Problem(statusCode: 409, title: "Не удалось сформировать уникальный номер заявки");
        }
        catch (Exception ex)
        {
            _logger.LogError("CreateTicket failed; siteId={SiteId}; error={ErrorType}", request.SiteId, ex.GetType().Name);
            return Problem(
                statusCode: 500,
                title: "Не удалось обработать запрос",
                detail: "Повторите запрос позже.");
        }
    }

    [HttpPatch("{id}/assignee")]
    public async Task<ActionResult<TicketResponse>> Assign(int id, [FromBody] AssignTicketRequest request, CancellationToken cancellationToken)
    {
        var (ticket, error) = await _service.Assign(id, request.AssigneeUserId, cancellationToken);
        if (ticket == null)
        {
            var statusCode = error == "Заявка не найдена" ? 404 : 409;
            return Problem(statusCode: statusCode, title: error);
        }
        return Ok(MapToResponse(ticket));
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TicketResponse>> ChangeStatus(int id, [FromBody] ChangeStatusRequest request, CancellationToken cancellationToken)
    {
        var (ticket, error) = await _service.ChangeStatus(id, request.Status, GetUserId(), cancellationToken);
        if (ticket == null)
        {
            if (error == "Заявка не найдена") return Problem(statusCode: 404, title: error);
            return Problem(statusCode: 409, title: error);
        }
        return Ok(MapToResponse(ticket));
    }

    private static TicketResponse MapToResponse(backend.Models.Ticket t) => new()
    {
        Id = t.Id, Number = t.Number, Title = t.Title, SiteId = t.SiteId,
        Description = t.Description, Status = t.Status,
        AssigneeUserId = t.AssigneeUserId, CreatedByUserId = t.CreatedByUserId
    };
}
