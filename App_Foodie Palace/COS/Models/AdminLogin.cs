using COS.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public  class AdminLogin
{
    [Key]
    [Required(ErrorMessage ="Username is required")]
    public string? Username{get;set;}
    [Required(ErrorMessage ="Password is required")]
    public string? Password{get;set;}
}