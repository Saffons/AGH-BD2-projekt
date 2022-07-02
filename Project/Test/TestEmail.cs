using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Data.SqlClient;

namespace Test
{
    [TestClass]
    public class TestEmail
    {
        static readonly string sqlconnection = @"Data Source=WINSERV01;Initial Catalog=project;Persist Security Info=True;User ID=test;Password=Passw0rd;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        static readonly SqlConnection connection = new SqlConnection(sqlconnection);

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            String sqlcommand = "create table testMyEmail (id INT IDENTITY(1, 1) PRIMARY KEY, email dbo.MyEmail);"
                              + "insert into testMyEmail(email) values ('adam@malysz.pl');"
                              + "insert into testMyEmail(email) values ('mariusz@marszalek.pl');";
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
            finally { 
                connection.Close(); 
            }
        }

        [ClassCleanup()]
        public static void Clean()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "DROP TABLE testMyEmail;";
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
            string sqlcommand = "select email.ToString() as email from testMyEmail where ID = 1;";
            string expected = "adam@malysz.pl";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["email"].ToString());
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
        public void TestUsername()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "select email.Username as username from testMyEmail where id = 2;";
            string expected = "mariusz";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["username"].ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
            finally { 
                connection.Close();
            }
        }

        [TestMethod]
        public void TestHost()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            string sqlcommand = "select email.Host as host from testMyEmail where id = 1;";
            string expected = "malysz.pl";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["host"].ToString());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
            finally {
                connection.Close();
            }
        }

        [TestMethod]
        public void TestIsEqual()
        {
            string sqlcommand = "select ID, email.ToString() as email, MyEmail::isEqual('adam@malysz.pl', email) as isSame from testMyEmail where ID = 1;";
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
