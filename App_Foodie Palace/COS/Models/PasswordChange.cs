using COS.Models;
 using System.ComponentModel.DataAnnotations.Schema;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel;


public  class PasswordChange
{
   [Required(ErrorMessage ="Username is required")]
    public string? EmployeeId{get;set;}
    [DataType(DataType.Password)]
    [Required(ErrorMessage ="Username is required")]
    public string? OldPassword{get;set;}
    [Required(ErrorMessage ="Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$" ,ErrorMessage="Provide strong password")]
    public string? NewPassword{get;set;}
     [Required(ErrorMessage ="Password is required")]
    [DataType(DataType.Password)]
     [ Compare("NewPassword", ErrorMessage = "Password and Confirmation Password must match.")]
      public string? ConfirmPassword{get;set;}
}