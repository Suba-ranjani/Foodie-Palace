using System;
using System.Data;
using System.Data.SqlClient;
using COS.Models;
using System.IO;
using System.Globalization;
namespace COS.Models
{
  public class Database
  {
    protected static string connect="Server=LAPTOP-2P4VF0LP\\SQLEXPRESS;Database=registration;Trusted_Connection=True;TrustServerCertificate=True;";
    public int checkData(PasswordChange details)
    {
      int flag=0;
      using(SqlConnection connection = new SqlConnection())
      {  
        connection.ConnectionString=connect;
        connection.Open();            
        SqlCommand selectCommand=new SqlCommand("spGetEmpCount" , connection);
        {              
          selectCommand.CommandType = CommandType.StoredProcedure;
          selectCommand.Parameters.Add(new SqlParameter("@Userid",details.EmployeeId));
          selectCommand.Parameters.Add(new SqlParameter("@SecurityKey",details.OldPassword));        
          var countId = selectCommand.ExecuteScalar();     
          if(Convert.ToInt32(countId)==1)
          {
            SqlCommand updateCommand=new SqlCommand("spUpdateLoginDetails",connection);
            { 
              updateCommand.CommandType = CommandType.StoredProcedure;
              updateCommand.Parameters.Add(new SqlParameter("@employeeid", details.EmployeeId));
              updateCommand.Parameters.Add(new SqlParameter("@newpassword", details.NewPassword));
              updateCommand.ExecuteNonQuery();
              flag=1;  
            }
          }
          else
          {
            LogError("Error in Entering of Id and Password durring password change");
          }   
        }
          return flag;
      }
                 
                 
    }

  
    public static List<Menu> getMenuItems (int foodid)
    {
      using(SqlConnection connection = new SqlConnection())
      { 
        List<Menu> item=new List<Menu>();
        try
        { 
          connection.ConnectionString=connect;
          connection.Open();
          if(foodid==0)
          {
            SqlCommand selectCommand = new SqlCommand("spGetItemDetails", connection)
            {
            CommandType = CommandType.StoredProcedure
            };
            SqlDataReader Item =selectCommand.ExecuteReader();
            while(Item.Read())
            {  
            Menu itemDetails=new Menu();
            itemDetails.Food_id=Convert.ToInt32(Item["Food_id"]);
            itemDetails.Item_id=Convert.ToInt32(Item["Item_id"]);
            itemDetails.Item_name=Convert.ToString(Item["Item_name"]);
            itemDetails.Item_price=Convert.ToDouble(Item["Item_price"]);
            itemDetails.Item_image=Convert.ToString(Item["Item_image"]);
            item.Add(itemDetails);
            }
          }                     
          else
          {
            SqlCommand selectCommand = new SqlCommand("spGetIdDetails", connection);
            {
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.Parameters.Add(new SqlParameter("@foodid",foodid));
            }
            SqlDataReader items =selectCommand.ExecuteReader();
            while(items.Read())
            {  
            Menu itemDetails=new Menu();
            itemDetails.Food_id=Convert.ToInt32(items["Food_id"]);
            itemDetails.Item_id=Convert.ToInt32(items["Item_id"]);
            itemDetails.Item_name=Convert.ToString(items["Item_name"]);
            itemDetails.Item_price=Convert.ToDouble(items["Item_price"]);
            itemDetails.Item_image=Convert.ToString(items["Item_image"]);
            item.Add(itemDetails);      
            }
          }
        }
        catch(Exception error)
        {
          Console.WriteLine(error);
        }             
        return item;                               
                                           
      }
    }
    public int postCartDetails(string employeeid,int id,string name,int price,int foodid,int quantity)
    {
      int flag=0;
      using(SqlConnection connection = new SqlConnection())
      {
        connection.ConnectionString=connect;
        try
        {
          connection.Open();            
          SqlCommand getCount=new SqlCommand("spCountCartDetails" , connection);
          {         
          getCount.CommandType = CommandType.StoredProcedure;
          getCount.Parameters.Add(new SqlParameter("@employeeid",employeeid));
          getCount.Parameters.Add(new SqlParameter("@itemid",id));
          getCount.Parameters.Add(new SqlParameter("@foodid",foodid));           
          var count = getCount.ExecuteScalar();
          Console.WriteLine(Convert.ToInt16(count));
          if(Convert.ToInt16(count)==0)
          {
            SqlCommand insertCommand=new SqlCommand("spInsertCartDetails",connection);
            { 
            insertCommand.CommandType = CommandType.StoredProcedure;
            insertCommand.Parameters.Add(new SqlParameter("@employeeid", employeeid));
            insertCommand.Parameters.Add(new SqlParameter("@id", id));
            insertCommand.Parameters.Add(new SqlParameter("@name" , name));
            insertCommand.Parameters.Add(new SqlParameter("@price" ,price));
            insertCommand.Parameters.Add(new SqlParameter("@foodid" ,foodid));
            insertCommand.Parameters.Add(new SqlParameter("@quantity" ,quantity));
            insertCommand.ExecuteNonQuery();
            flag=1;
            return flag;
            }                        
          }
          }
        }
        catch(Exception connectionError)
        {
          Console.WriteLine(connectionError);
        }
        return flag;
      }        
    }
    public static  List<CartDetails> getCartDetails(string employeeid)
    {     
      using (SqlConnection connection =new SqlConnection())
      {
        List<CartDetails> itemDetails =new List<CartDetails>();
        try
        {
          connection.ConnectionString= connect;
          connection.Open();
          SqlCommand selectCommand = new SqlCommand("spGetCartDetails", connection);
          {
          selectCommand.CommandType = CommandType.StoredProcedure;
          selectCommand.Parameters.Add(new SqlParameter("@employeeid",employeeid));
          }
          SqlDataReader getData = selectCommand.ExecuteReader();
          while(getData.Read())
          {
            CartDetails data=new CartDetails ();
            data.Quantity=Convert.ToInt16(getData["Quantity"]);
            data.ItemId=Convert.ToInt32(getData["Item_id"]);
            data.Itemname=Convert.ToString(getData["Item_name"]);
            data.Itemprice=Convert.ToUInt16(getData["Item_price"]);
            data.Foodid=Convert.ToInt16(getData["Food_id"]);
            data.EmployeeId=employeeid;
            itemDetails.Add(data);
          }
        }
        catch(Exception connectionerror)
        {
          LogError(connectionerror.ToString());
        }
        return itemDetails;
      }         
    }
    public static void removeItems(int id,string employeeid)
    {
      using(SqlConnection connection=new SqlConnection())
      {
        try
        {
          connection.ConnectionString= connect;
          connection.Open();
          SqlCommand deleteCommand=new SqlCommand("spDeleteCartDetails",connection);
          { 
            Console.WriteLine(1);
            Console.WriteLine(id);
            Console.WriteLine(employeeid);                      
            deleteCommand.CommandType = CommandType.StoredProcedure;
            deleteCommand.Parameters.Add(new SqlParameter("@employeeid",employeeid));
            deleteCommand.Parameters.Add(new SqlParameter("@Itemid",id ));
            deleteCommand.ExecuteNonQuery();  
          }
        }
        catch(Exception connectionError)
        {
          Console.WriteLine(connectionError);
        }
      }
    }
    public static void deleteItems(int id,string userid)
    {
      using(SqlConnection connection=new SqlConnection())
      {
        try
        {
          connection.ConnectionString= connect;
          connection.Open();        
          SqlCommand deleteCommand=new SqlCommand("spDeleteCartDetails",connection);
          {   
          deleteCommand.CommandType = CommandType.StoredProcedure;
          deleteCommand.Parameters.Add(new SqlParameter("@employeeid",userid ));
          deleteCommand.Parameters.Add(new SqlParameter("@Itemid",id ));
          deleteCommand.ExecuteNonQuery();  
          }          
        }
        catch(Exception connectionError)
        {
          Console.WriteLine(connectionError);
        }
      }
    }
    public static void updateCartDetails(int quantity,string userId,int foodId,int itemId,string itemName,int itemPrice,string sign)
    {
      Console.WriteLine(quantity);
      if(sign=="+")
      {
        quantity+=1;
      }
      else if(sign=="-")
      {
        quantity-=1;
        if(quantity<0)
        {
          quantity=0;
        }          
      }
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString= connect;
        connection.Open();
        SqlCommand updateCommand=new SqlCommand("spUpdateCartDetails",connection);
        {    
          updateCommand.CommandType = CommandType.StoredProcedure;
          updateCommand.Parameters.Add(new SqlParameter("@employeeid",userId));
          updateCommand.Parameters.Add(new SqlParameter("@quantity", quantity));
          updateCommand.Parameters.Add(new SqlParameter("@itemid", itemId));
          updateCommand.Parameters.Add(new SqlParameter("@foodid", foodId));
          updateCommand.ExecuteNonQuery();                               
        }
        
      }           
    }

    public static void UpdateOrderDetails(string employeeid)
    {
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString= connect;
        connection.Open();
        SqlCommand insertCommand=new SqlCommand("spInsertOrderDetails",connection);
        {      
          insertCommand.CommandType = CommandType.StoredProcedure;
          insertCommand.Parameters.Add(new SqlParameter("@employeeid", employeeid));
          insertCommand.ExecuteNonQuery();  
        }                 
      }
    }

    public static void deleteCartDetails(string employeeid)
    {       
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString=connect;
        connection.Open();
        SqlCommand deleteCommand=new SqlCommand("spDeleteCartEmpDetails",connection);
        {     
          deleteCommand.CommandType = CommandType.StoredProcedure;
          deleteCommand.Parameters.Add(new SqlParameter("@employeeid",employeeid ));
          deleteCommand.ExecuteNonQuery();  
        }
            
      }
    }

    public static void UpdateUserDetails(string employeeid)
    {
      DateTime time =DateTime.Now;
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString= connect;
        connection.Open();
        SqlCommand insertCommand=new SqlCommand("spInsertUserDetails",connection);
        {      
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.Parameters.Add(new SqlParameter("@employeeid", employeeid));
        insertCommand.ExecuteNonQuery();  
        }
        SqlCommand updateCommand=new SqlCommand("spUpdateUserDetails",connection);
        {   
        updateCommand.CommandType = CommandType.StoredProcedure;
        updateCommand.Parameters.Add(new SqlParameter("@employeeid",employeeid));
        updateCommand.Parameters.Add(new SqlParameter("@time", time));              
        updateCommand.ExecuteNonQuery();                                      
        }            
      }
    }
    
    public static void addFeedback( Feedback feedbacks,string user)
    {
      Console.WriteLine(feedbacks.ratings);
      using(SqlConnection connection=new SqlConnection())
        {
          connection.ConnectionString= connect;
          connection.Open();
          SqlCommand insertCommand=new SqlCommand("spInsertFbDetails",connection);
          {   
          insertCommand.CommandType = CommandType.StoredProcedure;
          insertCommand.Parameters.Add(new SqlParameter("@feedback",feedbacks.userfeedback ));
          insertCommand.Parameters.Add(new SqlParameter("@rating",feedbacks.ratings ));
          insertCommand.Parameters.Add(new SqlParameter("@employeeid",user ));
          insertCommand.ExecuteNonQuery();  
          }
             
        }
    }
    public static List<OrderDetails> getOrderDetails(string employeeid)
    {
      List<OrderDetails> Item= new List<OrderDetails>();
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString= connect;
        connection.Open();       
        SqlCommand selectCommand = new SqlCommand("spGetOrderdetails", connection);
        {
        selectCommand.CommandType = CommandType.StoredProcedure;
        selectCommand.Parameters.Add(new SqlParameter("@employeeid",employeeid));
        }
        SqlDataReader items =selectCommand.ExecuteReader();
        while(items.Read())
        {  
        OrderDetails orders=new OrderDetails();
        orders.EmployeeId=Convert.ToString(items["EmployeeId"]);
        orders.Itemname=Convert.ToString(items["Item_name"]);
        orders.Totalprice=Convert.ToDouble(items["Item_price"]);
        Item.Add(orders);
        }
      }
        return Item;
    }

    public static void deleteOrderdetails(string id)
    {
      using(SqlConnection connection=new SqlConnection())
      {
        connection.ConnectionString= connect;
        connection.Open();
        SqlCommand deleteCommand=new SqlCommand("spDeleteOrderDetails",connection);
        {       
        deleteCommand.CommandType = CommandType.StoredProcedure;
        deleteCommand.Parameters.Add(new SqlParameter("@employeeid",id ));
        deleteCommand.ExecuteNonQuery();  
        }             
      }
    }

    public static void LogError(string message)
    {
      try
      {
        string path =@"C:\Users\Ranjani\Desktop\Foodie Palace\App_Foodie Palace\COS\wwwroot\LogError"+DateTime.Today.ToString("dd-MM-YY")+".txt";
        if (!File.Exists(path))
        {
          File.Create(path).Dispose();
        }
        using (StreamWriter w = File.AppendText(path))
        {
          w.WriteLine("\r\nLog Entry : ");
          w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
          string err = "Error in: " + path.ToString() +
                          ". Error Message:" + message;
          w.WriteLine(err);
          w.WriteLine("__________________________");
          w.Flush();
          w.Close();
        }
      }
      catch
      {
        throw;
      }
    }
  }
}
   