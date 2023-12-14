using COS.Models;
using System.ComponentModel;
using System.Collections.Generic;

public  class CartDetails :Order
{
    public int Foodid{get;set;}
    public int Quantity{get;set;}
    public string? EmployeeId{get;set;}
    public int ItemId{get;set;}
    public string? Itemimage{get;set;}
    public string? Itemname{get;set;}
    public int Itemprice{get;set;}
     
}