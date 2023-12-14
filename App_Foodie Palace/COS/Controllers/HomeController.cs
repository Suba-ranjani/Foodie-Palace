using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COS.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;


namespace COS.Controllers;
[Actions]
public class HomeController : Controller
{
    Uri feedbackaddress =new Uri("http://localhost:5221");
    HttpClient client;
    private readonly ILogger<HomeController> _logger;
    private CookieOptions userLastVistedCookie;

    private readonly ApplicationDbContext _database;
    public class InvalidUserException : Exception  
{  
    public InvalidUserException(String message)  
        : base(message)  
    {  
  
    }  
}  
    public HomeController(ApplicationDbContext database,ILogger<HomeController> logger)
    {
        userLastVistedCookie = new CookieOptions();
        userLastVistedCookie.Expires = DateTime.Now.AddDays(60);
        _database=database;
        _logger = logger;
        client =new HttpClient();
        client.BaseAddress=feedbackaddress;
    }
  
    public IActionResult Index()
    {  
        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {   
        return View();
    }
    [HttpPost]
 public IActionResult Login(Logindetails Details)
    { 
       
        string? Id=Details.Userid;
        string? Password =Details.SecurityKey;
        Console.WriteLine(Id);
        Console.WriteLine(Password);

        //var data =_database.Logindetails.Find(Id);
        var data =_database.Logindetails.SingleOrDefault(s=>s.Userid==Details.Userid &&s.SecurityKey==Details.SecurityKey);
        Console.WriteLine(data);
        //Console.WriteLine(data1);
         
        if(Id=="ADMIN900"&&Password=="Admin@2002")
        {
        _logger.LogInformation("Date and Time",DateTime.Now);
        HttpContext.Session.SetString("employeeid", Details.Userid);
        ViewBag.employeeid=HttpContext.Session.GetString("employeeid");
        TempData["showtime"] = Request.Cookies[Details.Userid];
        Response.Cookies.Append(Details.Userid,Convert.ToString(DateTime.Now),userLastVistedCookie);
        TempData["success"]="Logged in successfully";
        return RedirectToAction("AdminChanges","Admin");
            
        }
        if(data==null)
        {
        ViewBag.failure="Username and password doesnot exists";
        return View("Login");
        }
        
        else
        {
        _logger.LogInformation("Logged on {PlaceHolderName:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
        HttpContext.Session.SetString("employeeid", Details.Userid);
        ViewBag.employeeid=HttpContext.Session.GetString("employeeid");
        TempData["showtime"] = Request.Cookies[Details.Userid];
        Response.Cookies.Append(Details.Userid,Convert.ToString(DateTime.Now),userLastVistedCookie);
        TempData["success"]="Logged in successfully";
        return View("Index");
         
        }
    }
    public IActionResult Feedback()
    {
         ViewBag.employeeid=HttpContext.Session.GetString("employeeid");
         if(ViewBag.employeeid==null)
        {
           return RedirectToAction("Login");
        }    
        return View();
    }
    [HttpPost]
    public IActionResult Feedback(Feedback feedBack)
    {  
        int flag=0;
        if(feedBack==null)
        {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        string data =JsonConvert.SerializeObject(feedBack);
        StringContent content =new StringContent(data,Encoding.UTF8,"application/json");
        HttpResponseMessage response =client.PostAsync("/Feedback",content).Result;
        Console.WriteLine(response);
        if(response.IsSuccessStatusCode)
        {
        flag=1;
        return View("Index");
        }}
        Console.WriteLine(flag);
       
     
        return View();
    }
     public IActionResult Password()
    {
        return View();
    }
    [HttpPost]
   public IActionResult Password(PasswordChange password)
    { 
         if (!ModelState.IsValid)
    {
        return View();
    }

        Database access=new Database();
        int result=access.checkData(password);
        Console.WriteLine(result);
        if(result==0)
        throw(new InvalidUserException("Id and Password is not available"));
        else
        return RedirectToAction("Index");
    }
      public IActionResult Menu(int foodid)
    { 
        IEnumerable<Menu> itemDatas = Database.getMenuItems(foodid); 
        return View(itemDatas);
    }
     
    public IActionResult addToCart(string employeeid,int itemid,string itemname,int itemprice,int foodid,int quantity)
    { 
        Database access=new Database();
        int result=access.postCartDetails(employeeid,itemid,itemname,itemprice,foodid,quantity);
            //  Database. getCartDetails(employeeid);
        if(result==0)
        {
        TempData["message"]="Already Exists";
        return RedirectToAction("Order");
        }

        return RedirectToAction("Order");
    }
    
    public IActionResult Cart()
    { 
        string? user=HttpContext.Session.GetString("employeeid");
        Console.WriteLine(user);
        IEnumerable<CartDetails> items=Database.getCartDetails(user);
        return View(items);
    }
    public IActionResult Quantity(int quantity,string userId,int foodId,int itemId,string itemName,int itemPrice,string sign)
    {
        Database.updateCartDetails(quantity,userId,foodId,itemId,itemName,itemPrice,sign);
        return RedirectToAction("cart");
    }
    public IActionResult Remove(int itemid,string employeeid)
    {
        Database.removeItems(itemid,employeeid); 
        return RedirectToAction("cart");
    }
    // public IActionResult Delete(int id,string userid)
    // {
    //     Database.deleteItems(id,userid); 
    //     return RedirectToAction("Order");
    // }
    public IActionResult logout()
    { 
        //  string? user=HttpContext.Session.GetString("employeeid");
        //  Console.WriteLine(user);
         HttpContext.Session.Clear();
        
        
        
       // Database.updateOrderDetails(user);
        return View("Index");
    }
    public IActionResult Order(int Foodid)
    {   
        ViewBag.employeeid=HttpContext.Session.GetString("employeeid");
        if(HttpContext.Session.GetString("employeeid")!=null)
        {
        IEnumerable<Menu> menuItem = Database.getMenuItems(Foodid); 
        return View(menuItem);
          
        }
        else
        {
        return RedirectToAction("Login");
        }
    }
    //  public IActionResult  updateOrder()
    //  {
    //      string? user=HttpContext.Session.GetString("employeeid");
    //      Database.UpdateOrderDetails(user);
    //     return RedirectToAction("Index");
    //  }
    
    public IActionResult PayNow()
    {
        return RedirectToAction("Payment");
    }
    public IActionResult Payment()
    {
        return View();
    }

    [HttpPost]
     public IActionResult Payment(Payment data)
     {
        if(!ModelState.IsValid)
        {
        return View();            
        }
        string? user =Convert.ToString( TempData["employee"]);
        Database.deleteCartDetails(user);
        AdminDatabase.updateUserStatus();
        return View("Thankyou");       
     }
     public IActionResult  Confirm()
     { 
        string? user=HttpContext.Session.GetString("employeeid");
        TempData["employee"]=user;
        IEnumerable<CartDetails> itemDetail=Database.getCartDetails(user);
          
        if(itemDetail.Count()>0)
        {  
        Database.UpdateOrderDetails(user);
        Database.UpdateUserDetails(user);
        ViewBag.user=HttpContext.Session.GetString("employeeid");
        IEnumerable<OrderDetails> items= Database.getOrderDetails(user);            
        Database.deleteOrderdetails(user);
        return View(itemDetail);
        }
        else
        return RedirectToAction("cart");   
    }
     
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
