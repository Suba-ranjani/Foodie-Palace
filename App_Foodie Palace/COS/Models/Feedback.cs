using COS.Models;
 using System.ComponentModel.DataAnnotations.Schema;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel;
  using System.Collections.Generic;

public  class Feedback
{
   
    public string? userfeedback{get;set;}
    public string? employeeid { get; set; }
    public int ratings { get; set; }
}