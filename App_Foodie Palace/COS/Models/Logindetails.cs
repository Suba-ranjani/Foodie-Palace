using COS.Models;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

public  class Logindetails 
{
    [Key]
    [Required]
    public string? Userid{get;set;}
    [Required]
    public string? SecurityKey{get;set;}
  // public DateTime logintime{get;set;} = DateTime.Now;
     
}