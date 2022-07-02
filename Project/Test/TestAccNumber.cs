using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Data.SqlClient;

namespace Test
{
    [TestClass]
    public class TestAccNumber
    {
        static readonly string sqlconnection = @"Data Source=WINSERV01;Initial Catalog=project;Persist Security Info=True;User ID=test;Password=Passw0rd;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        static readonly SqlConnection connection = new SqlConnection(sqlconnection);

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "create table testAccNumber(id INT IDENTITY(1, 1) PRIMARY KEY, acc dbo.AccNumber);"
                              + "insert into testAccNumber(acc) values ('PL.203040.PLN');"
                              + "insert into testAccNumber(acc) values ('DE.304050.EUR');";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        [ClassCleanup()]
        public static void Clean()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "DROP TABLE testAccNumber;";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                datareader.Read();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }
        }

        [TestMethod]
        public void TestToString()
        {
            string sqlcommand = "select acc.ToString() as acc from testAccNumber where ID = 1;";
            string expected = "PL.203040.PLN";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["acc"].ToString());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestCountry()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "select acc.Country as country from testAccNumber where id = 2;";
            string expected = "DE";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["country"].ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestNumber()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "select acc.Number as number from testAccNumber where id = 1;";
            string expected = "203040";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["number"].ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestIsEqual()
        {
            string sqlcommand = "select ID, acc.ToString() as acc, AccNumber::isEqual('PL.203040.PLN', acc) as isSame from testAccNumber where ID = 1;";
            string expected = "True";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["isSame"].ToString());
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Assert.Fail();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
