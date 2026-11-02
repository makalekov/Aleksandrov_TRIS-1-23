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
    public ActionResult<List<TicketResponse>> GetAll([FromQuery] int? assigneeUserId)
    {
        var tickets = _service.GetAll();
        if (assigneeUserId.HasValue)
            tickets = tickets.Where(t => t.AssigneeUserId == assigneeUserId.Value).ToList();
        return Ok(tickets.Select(MapToResponse).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<TicketResponse> GetById(int id)
    {
        var ticket = _service.GetById(id);
        if (ticket == null) return Problem(statusCode: 404, title: "Заявка не найдена");
        return Ok(MapToResponse(ticket));
    }

    [HttpPost]
    public ActionResult<TicketResponse> Create([FromBody] CreateTicketRequest request)
    {
        try
        {
            var ticket = _service.Create(request, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, MapToResponse(ticket));
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
    public ActionResult<TicketResponse> Assign(int id, [FromBody] AssignTicketRequest request)
    {
        var ticket = _service.Assign(id, request.AssigneeUserId);
        if (ticket == null) return Problem(statusCode: 404, title: "Заявка не найдена");
        return Ok(MapToResponse(ticket));
    }

    [HttpPatch("{id}/status")]
    public ActionResult<TicketResponse> ChangeStatus(int id, [FromBody] ChangeStatusRequest request)
    {
        var (ticket, error) = _service.ChangeStatus(id, request.Status, GetUserId());
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
