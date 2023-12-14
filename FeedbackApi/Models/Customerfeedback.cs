using System.ComponentModel.DataAnnotations;
namespace FeedbackApi.Models{
public class Customerfeedback
{
    [Key]
    public int Id { get; set; }
    public string? userfeedback{get;set;}
    public string? employeeid { get; set; }
    public int ratings { get; set; }
    
}
}