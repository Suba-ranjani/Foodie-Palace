using Microsoft.AspNetCore.Mvc;
using FeedbackApi.Data;
using FeedbackApi.Models;
using System.Collections.Generic;
namespace FeedbackApi.Controllers;


[ApiController]
[Route("[controller]")]
public class FeedbackController : ControllerBase
{
     private readonly ApplicationDbContext _database;
    public FeedbackController(ApplicationDbContext database)
    {
         
        _database=database;
        
    }
    
    [HttpPost]
    public IActionResult GetFeedback(Customerfeedback feedback)
    {
        Console.WriteLine(feedback.userfeedback);
        _database.UserFeedback.Add(feedback);
        _database.SaveChanges();
        return Ok(feedback);
    }
    [HttpGet]
    public IActionResult PostFeedback()
    {
        IEnumerable<Customerfeedback> data =_database.UserFeedback.ToList();
        return Ok(data);
    }
}