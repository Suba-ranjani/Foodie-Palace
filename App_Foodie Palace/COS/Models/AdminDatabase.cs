using System;
using System.Data;
using System.Data.SqlClient;
using COS.Models;
namespace COS.Models
{
public  class AdminDatabase:Database
{ 
  public void updateItemdetails(ItemDetails details)
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString=connect;
      connection.Open();
      SqlCommand insertCommand=new SqlCommand("spInsertItemDetails",connection);
      { 
          
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.Parameters.Add(new SqlParameter("@foodid", details.Foodid));
        insertCommand.Parameters.Add(new SqlParameter("@itemid", details.Itemid));
        insertCommand.Parameters.Add(new SqlParameter("@itemname" , details.Itemname));
        insertCommand.Parameters.Add(new SqlParameter("@itemimage" ,"/images/"+details.Itemimage));
        insertCommand.Parameters.Add(new SqlParameter("@itemprice" ,details.Itemprice));
        insertCommand.ExecuteNonQuery();
      }
     }
    }
  public void updateItem(ItemDetails details)
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
                            
      SqlCommand updateCommand=new SqlCommand("spUpdateItemDetails",connection);
      { 
          
        updateCommand.CommandType = CommandType.StoredProcedure;
        updateCommand.Parameters.Add(new SqlParameter("@itemprice",details.Itemprice));
        updateCommand.Parameters.Add(new SqlParameter("@foodid", details.Foodid));
        updateCommand.Parameters.Add(new SqlParameter("@itemid", details.Itemid));
        updateCommand.ExecuteNonQuery();
                                      
      }
                     
        }
  }
  public void deleteItemdetails(ItemdetailsRemove details)
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString=connect;
      connection.Open();
      SqlCommand deleteCommand=new SqlCommand("spDeleteItemDetails",connection);
      { 
          
        deleteCommand.CommandType = CommandType.StoredProcedure;
        deleteCommand.Parameters.Add(new SqlParameter("@foodid", details.foodid));
        deleteCommand.Parameters.Add(new SqlParameter("@itemid", details.itemid));
        deleteCommand.ExecuteNonQuery();             
      }
           
    }
  }
  public int addUserDetails(Logindetails details)
  {
    int flag=0;
    if(details.Userid==""&&details.SecurityKey=="")
    {
      flag=1;
    }
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand insertCommand=new SqlCommand("spInsertNewLoginDetails",connection);
      { 
          
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.Parameters.Add(new SqlParameter("@userid", details.Userid));
        insertCommand.Parameters.Add(new SqlParameter("@securitykey", details.SecurityKey));
        insertCommand.ExecuteNonQuery();
      } 
    }
      return flag;
  }
  public void removeUserDetails(Logindetails details)
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString=connect;
      connection.Open();
      SqlCommand insertCommand=new SqlCommand("spDeleteLoginDetails",connection);
      { 
          
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.Parameters.Add(new SqlParameter("@userid", details.Userid));
        insertCommand.Parameters.Add(new SqlParameter("@securitykey", details.SecurityKey));
        insertCommand.ExecuteNonQuery();
      } 
    }
  }
  public static List<Feedback> getFeedback()
  {
    string? user;
    List<Feedback> data = new List<Feedback> ();
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand selectCommand = new SqlCommand("spGetFbDetails", connection);
      {
        selectCommand.CommandType = CommandType.StoredProcedure;
      }
      SqlDataReader getData = selectCommand.ExecuteReader();
      while(getData.Read())
      {
        Feedback feedback =new Feedback();
        feedback.userfeedback=Convert.ToString(getData["feedback"]);
        feedback.ratings=Convert.ToInt16(getData["ratings"]);
        feedback.employeeid=Convert.ToString(getData["employeeid"]);
        data.Add(feedback);
      } 
    }
      return data;
  }
  public static void updateUserStatus()
  {
    double totalprice=0;
    string? id="";
    string? time="";
    string? status="paid";
    List<OrderDetails> datas =new List<OrderDetails>();
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand selectCommand = new SqlCommand("spGetUserDetails", connection);
      {
        selectCommand.CommandType = CommandType.StoredProcedure;
      }
      SqlDataReader reader=selectCommand.ExecuteReader();
      while(reader.Read())
      {
        OrderDetails details = new OrderDetails();
        details.EmployeeId=Convert.ToString(reader["EmployeeId"]);
        id=Convert.ToString(reader["EmployeeId"]);
        details.Itemname=Convert.ToString(reader["Item_name"]);
        details.Totalprice=Convert.ToDouble(reader["Item_price"]);
        totalprice=totalprice+ details.Totalprice;
        time=Convert.ToString(reader["time"]);
        datas.Add(details);
      }
      connection.Close();
      foreach(var item in datas)
      {
        Console.WriteLine(item.Totalprice);
        totalprice=totalprice+item.Totalprice;
                
      }
            //  Console.WriteLine(id);
            //    Console.WriteLine(totalprice);
            //    Console.WriteLine(status);
            //    Console.WriteLine(time);
             
      connection.Open();
      SqlCommand insertCommand = new SqlCommand("INSERT INTO Userstatus VALUES('"+id+"','"+totalprice+"','"+status+"','"+time+"')",connection);
      insertCommand.ExecuteNonQuery();  
      connection.Close();

    }
      
  }
  public static IEnumerable<Userstatus> getUserStatus()
  {
    List<Userstatus> data = new List<Userstatus>();
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand selectCommand = new SqlCommand("spGetUserStatus", connection);
      {
        selectCommand.CommandType = CommandType.StoredProcedure;
      }
      SqlDataReader reader=selectCommand.ExecuteReader();
      while(reader.Read())
      {
        Userstatus detail = new Userstatus();
        detail.EmployeeId=Convert.ToString(reader["EmployeeId"]);
        detail.status=Convert.ToString(reader["Status"]);
        detail.time=Convert.ToString(reader["Time"]);
        detail.TotalPrice=Convert.ToDouble(reader["Totalprice"]);
        data.Add(detail);
      }
    }
      return  data;
  }
  public static List<Likedlist> getMostLikedItems()
  {
    List<Likedlist> lists =new List<Likedlist>();
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand selectCommand = new SqlCommand("spSelectItemDetails", connection);
      {
        selectCommand.CommandType = CommandType.StoredProcedure;
      }
      SqlDataReader reader = selectCommand.ExecuteReader();
      while(reader.Read())
      {
        Likedlist data =new Likedlist();
                
                // data.itemname =Convert.ToString(reader["item_details.Item_name"]);
// Console.WriteLine(cnt++);
        data.itemname =reader.GetString(0);
        data.itemimage =reader.GetString(1);
        Console.WriteLine(data.itemimage);
        data.itemprice =Convert.ToDouble( reader.GetDecimal(2));
                 
        Console.WriteLine(data.itemname);
        Console.WriteLine(data.itemprice);
        lists.Add(data);
      }
           
    }
      return lists;
  }
  public static void getCount()
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand insertCommand=new SqlCommand("spInsertCountDetails",connection);
      { 
          
        insertCommand.CommandType = CommandType.StoredProcedure;
        insertCommand.ExecuteNonQuery();
      }
    }
  }
  public static void deleteCount()
  {
    using(SqlConnection connection=new SqlConnection())
    {
      connection.ConnectionString= connect;
      connection.Open();
      SqlCommand deleteCommand=new SqlCommand("spCountDetails",connection);
      { 
        deleteCommand.CommandType = CommandType.StoredProcedure;
        deleteCommand.ExecuteNonQuery();  
      }
    }
  }
    
}

}