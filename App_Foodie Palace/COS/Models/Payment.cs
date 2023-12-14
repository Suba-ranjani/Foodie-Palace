using COS.Models;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

public  class Payment 
{
    
    [Required]
    public long  Cardnumber{get;set;}
    [Required]
    public string Expirydate{get;set;}
    [Required]
    public long cvv{get;set;}
    [Required]
    public string? Name{get;set;}

  // public DateTime logintime{get;set;} = DateTime.Now;
     
}