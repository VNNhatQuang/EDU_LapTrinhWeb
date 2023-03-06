using _19T1021198.DataLayers;
using _19T1021198.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace _19T1021198.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL , IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Order data, IEnumerable<OrderDetail> details)
        {
            int orderID = 0;
            
            //Thêm Order
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Orders(CustomerID, OrderTime, EmployeeID, AcceptTime, ShipperID, ShippedTime, FinishedTime, Status)
                                    VALUES(@CustomerID, @OrderTime, @EmployeeID, @AcceptTime, @ShipperID, @ShippedTime, @FinishedTime, @Status);
                                    SELECT SCOPE_IDENTITY()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@CustomerID", data.CustomerID);
                cmd.Parameters.AddWithValue("@OrderTime", data.OrderTime);
                cmd.Parameters.AddWithValue("@EmployeeID", data.EmployeeID);
                if (data.AcceptTime == null)
                    cmd.Parameters.AddWithValue("@AcceptTime", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@AcceptTime", data.AcceptTime);
                if (data.ShipperID == null)
                    cmd.Parameters.AddWithValue("@ShipperID", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ShipperID", data.ShipperID);
                if (data.ShippedTime == null)
                    cmd.Parameters.AddWithValue("@ShippedTime", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@ShippedTime", data.ShippedTime);
                if (data.FinishedTime == null)
                    cmd.Parameters.AddWithValue("@FinishedTime", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@FinishedTime", data.FinishedTime);
                cmd.Parameters.AddWithValue("@Status", data.Status);


                orderID = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            int result = 0;

            if (orderID > 0)
            {
                //Thêm DetailOrders
                
                foreach (var dt in details)
                {
                    using (SqlConnection cn = OpenConnection())
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = @"INSERT INTO OrderDetails(OrderID, ProductID, Quantity, SalePrice)
                                            VALUES(@OrderID, @ProductID, @Quantity, @SalePrice)
                                            SELECT SCOPE_IDENTITY()";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cn;
                        cmd.Parameters.AddWithValue("@OrderID", orderID);
                        cmd.Parameters.AddWithValue("@ProductID", dt.ProductID);
                        cmd.Parameters.AddWithValue("@Quantity", dt.Quantity);
                        cmd.Parameters.AddWithValue("@SalePrice", dt.SalePrice);
                        result = Convert.ToInt32(cmd.ExecuteScalar());
                        cn.Close();
                    }
                }
            }


            return result;
        }

        public int Count(int status = -99, string searchValue = "")
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int orderID)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteDetail(int orderID, int productID)
        {
            throw new System.NotImplementedException();
        }

        public Order Get(int orderID)
        {
            Order data = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Orders WHERE OrderID = @OrderID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new Order()
                    {
                        OrderID = Convert.ToInt32(dbReader["OrderID"]),
                        CustomerID = Convert.ToInt32(dbReader["CustomerID"]),
                        OrderTime = Convert.ToDateTime(dbReader["OrderTime"]),
                        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                        AcceptTime = Convert.ToDateTime(dbReader["AcceptTime"]),
                        ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                        ShippedTime = Convert.ToDateTime(dbReader["ShippedTime"]),
                        FinishedTime = Convert.ToDateTime(dbReader["FinishedTime"]),
                        Status = Convert.ToInt32(dbReader["Status"]),
                        
                    };
                }
                cn.Close();
            }
            return data;
        }

        public OrderDetail GetDetail(int orderID, int productID)
        {
            throw new System.NotImplementedException();
        }

        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, string searchValue = "")
        {
            throw new System.NotImplementedException();
        }

        public IList<OrderDetail> ListDetails(int orderID)
        {
            throw new System.NotImplementedException();
        }

        public int SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Order data)
        {
            throw new System.NotImplementedException();
        }
    }
}