using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Data.SqlClient;

namespace Test
{
    [TestClass]
    public class TestCardRGB
    {
        static readonly string sqlconnection = @"Data Source=WINSERV01;Initial Catalog=project;Persist Security Info=True;User ID=test;Password=Passw0rd;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        static readonly SqlConnection connection = new SqlConnection(sqlconnection);

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            String sqlcommand = "create table testCardRGB (id INT IDENTITY(1, 1) PRIMARY KEY, rgb dbo.CardRGB);"
                              + "insert into testCardRGB(rgb) values ('222,123,79');"
                              + "insert into testCardRGB(rgb) values ('24,66,123');"
                              + "insert into testCardRGB(rgb) values ('24,66,123');";
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
            string sqlcommand = "DROP TABLE testCardRGB;";
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
            string sqlcommand = "select rgb.ToString() as rgb from testCardRGB where ID = 1;";
            string expected = "222,123,79";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["rgb"].ToString());
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

        [TestMethod]
        public void TestRed()
        {
            string sqlcommand = "select rgb.R as rgb from testCardRGB where ID = 1;";
            string expected = "222";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["rgb"].ToString());
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

        [TestMethod]
        public void TestGreen()
        {
            string sqlcommand = "select rgb.G as rgb from testCardRGB where ID = 2;";
            string expected = "233";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreNotEqual(expected, datareader["rgb"].ToString());
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

        [TestMethod]
        public void TestBlue()
        {
            string sqlcommand = "select rgb.B as rgb from testCardRGB where ID = 2;";
            string expected = "123";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlcommand, connection);
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    Assert.AreEqual(expected, datareader["rgb"].ToString());
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

        [TestMethod]
        public void TestIsEqual()
        {
            string sqlcommand = "select ID, rgb.ToString() as rgb, CardRGB::isEqual('24,66,123', rgb) as isSame from testCardRGB where ID = 2;";
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
