using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace GoodStore
{
    class Repository : IDisposable
    {
        readonly string conStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() +
            "\\AppData\\GoodStore.mdf;Integrated Security=True;Connect Timeout=30";

        SqlConnection _con;

        public Repository()
        {
            _con = new SqlConnection(conStr);
        }

        public void Dispose()
        {
            _con.Dispose();
        }

        public void AddGoods(Goods goods)
        {
            try
            {
                string commandString = @"INSERT INTO Goods (Name, Unit, UnitPrice, Quantity) 
                                    VALUES(@Name, @Unit, @UnitPrice, @Quantity)";

                var command = new SqlCommand(commandString, _con);
                command.Parameters.AddWithValue("@Name", goods.Name);
                command.Parameters.AddWithValue("@Unit", goods.Unit);
                command.Parameters.AddWithValue("@UnitPrice", goods.UnitPrice);
                command.Parameters.AddWithValue("@Quantity", goods.Quantity);
                _con.Open();
                command.ExecuteNonQuery();
                _con.Close();
            }
            catch (Exception)
            {
                _con.Close();
                throw;
            }
        }

        public List<Goods> GetGoods()
        {
            var goods = new List<Goods>();

            try
            {
                string commandString = "SELECT * FROM Goods";

                SqlCommand command = new SqlCommand(commandString, _con);
                _con.Open();

                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        goods.Add(new Goods
                        {
                            GoodsId = (int)reader["GoodsId"],
                            Name = (string)reader["Name"],
                            Unit = (string)reader["Unit"],
                            UnitPrice = (double)reader["UnitPrice"],
                            Quantity = (int)reader["Quantity"]
                        });
                    }
                }
                _con.Close();
            }
            catch (Exception)
            {
                _con.Close();
                throw;
            }

            return goods;
        }

        public void AddBatch(Batch batch, BatchContent batchContent)
        {
            try
            {
                string batchCommandString = @"INSERT INTO Batch (OperationType, Date)
                                            VALUES (@OperationType, @Date)";

                var batchCommand = new SqlCommand(batchCommandString, _con);
                batchCommand.Parameters.AddWithValue("@OperationType", batch.OperationType);
                batchCommand.Parameters.AddWithValue("@Date", batch.Date);
                _con.Open();
                batchCommand.ExecuteNonQuery();

                string batchContentCommandString =
                    @"INSERT INTO BatchContent (GoodsId, BatchId, Quantity) VALUES (@GoodsId, @BatchId, @Quantity)";

                var batchContentCommand = new SqlCommand(batchContentCommandString, _con);
                batchContentCommand.Parameters.AddWithValue("@GoodsId", batchContent.GoodsId);
                batchContentCommand.Parameters.AddWithValue("@BatchId", batchContent.BatchId);
                batchContentCommand.Parameters.AddWithValue("@Quantity", batchContent.Quantity);
                batchContentCommand.ExecuteNonQuery();

                _con.Close();
            }
            catch (Exception)
            {
                _con.Close();
                throw;
            }
        }
    }

    class Goods
    {
        public int GoodsId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    class Batch
    {
        public int BatchId { get; set; }
        public string OperationType { get; set; }
        public DateTime Date { get; set; }
    }

    class BatchContent
    {
        public int GoodsId { get; set; }
        public int BatchId { get; set; }
        public int Quantity { get; set; }
    }
}