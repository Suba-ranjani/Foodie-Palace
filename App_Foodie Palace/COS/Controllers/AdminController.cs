using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COS.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace COS.Controllers;
[Actions]
public class AdminController : Controller
{
    Uri feedbackaddress =new Uri("http://localhost:5221");
    HttpClient client;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
        client =new HttpClient();
        client.BaseAddress=feedbackaddress;
    }
    
    public IActionResult AdminChanges()
    {
        ViewBag.time= TempData["showtime"];
        ViewBag.user=HttpContext.Session.GetString("employeeid");        
        return View();
    }
    public IActionResult AddProducts()
    {
        return View();
    }
    public IActionResult RemoveProducts()
    {
        return View();
    }
    public IActionResult UpdateProduct()
    {
        return View();
    }
    public IActionResult UpdateUser()
    {
        return View();
    }
    public IActionResult UserStatus()
    {
          
        IEnumerable<Userstatus> datas = AdminDatabase.getUserStatus();
        return View(datas);
    }
    public IActionResult addUser(Logindetails details)
    {
        AdminDatabase data = new AdminDatabase();
        int result=data.addUserDetails(details);
        if(result==1)
        {
        _logger.LogWarning("Error in adding");   
        return View("UpdateUser");
        }
        else
        {
        _logger.LogWarning("Added successfully");
        return View("AdminChanges");
       
        }
    }
      public IActionResult removeUser(Logindetails details)
    {
        AdminDatabase data = new AdminDatabase();
        data.removeUserDetails(details);
        return View("AdminChanges");
    }
    public IActionResult addItems(ItemDetails details)
    {
        AdminDatabase data = new AdminDatabase();
        data.updateItemdetails(details);
        return View("MenuManagement");
    }
     public IActionResult updateItems(ItemDetails details)
    {
        AdminDatabase data = new AdminDatabase();
        data.updateItem(details);
        return View("MenuManagement");
    }
    public IActionResult deleteItems(ItemdetailsRemove details)
    {
        AdminDatabase data = new AdminDatabase();
        data.deleteItemdetails(details);
        return View("AdminChanges");
    }
    public IActionResult Feedback()
    {
        List<Feedback> feedback=new List<Feedback>();
        HttpResponseMessage response =client.GetAsync("/Feedback").Result;
        if(response.IsSuccessStatusCode)
        {
        string data =response.Content.ReadAsStringAsync().Result;
        feedback=JsonConvert.DeserializeObject<List<Feedback>>(data);        
        return View(feedback);
        }
        return View();
       
    //     int flag=0;
    //     if(feedbacks!=null)
    //     {
    //     client.DefaultRequestHeaders.Accept.Clear();
    //     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //     string data =JsonConvert.DeserializeObject(feedbacks);
    //    StringContent content =new StringContent(data,Encoding.UTF8,"application/json");
    //    HttpResponseMessage response =client.GetAsync("/Feedback",content).Result;
    //    Console.WriteLine(response);
    //    if(response.IsSuccessStatusCode)
    //    {
    //     flag=1;
    //     return RedirectToAction("Feedback");
    //    }}
    //    Console.WriteLine(flag);
       
        //  IEnumerable<Feedback> feedback = AdminDatabase.getFeedback();
    
        
    }
    public IActionResult MostFavourable()
    {
        AdminDatabase.getCount();
        IEnumerable<Likedlist> data =AdminDatabase.getMostLikedItems();
        AdminDatabase.deleteCount();
        return View(data);
    }

    public IActionResult logout()
    { 
        return RedirectToAction("Index","Home");
    }
     public IActionResult MenuManagement()
    {
        int foodid=0;
        IEnumerable<Menu> itemDatas = Database.getMenuItems(foodid);
        return View(itemDatas);
    }
    //  #region  Feedback
    //  public IActionResult FeedbackApi()
    // {
    //      IEnumerable<Feedback> feedback = AdminDatabase.getFeedback();
    
    //     return Json(feedback);
    // }
    // #endregion
   
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}