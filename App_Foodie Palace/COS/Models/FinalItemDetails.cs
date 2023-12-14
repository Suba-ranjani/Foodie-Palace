using COS.Models;
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

public  class FinalItemDetails 
{
    [Key]
    public int id{get;set;}
    //public string? EmployeeId{get;set;}
    public int ItemName{get;set;}
    public string? ItemPrice{get;set;}
    public string? ItemQuantity{get;set;}
     
}