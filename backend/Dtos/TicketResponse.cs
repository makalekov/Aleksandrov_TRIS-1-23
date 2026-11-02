namespace backend.Dtos;

public class TicketResponse
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int SiteId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? AssigneeUserId { get; set; }
    public int CreatedByUserId { get; set; }
}