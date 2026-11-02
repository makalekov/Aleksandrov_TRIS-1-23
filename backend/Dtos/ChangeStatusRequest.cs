using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class ChangeStatusRequest
{
    [Required]
    [RegularExpression("New|InProgress|Closed|Cancelled", ErrorMessage = "Недопустимый статус")]
    public string Status { get; set; } = string.Empty;
}