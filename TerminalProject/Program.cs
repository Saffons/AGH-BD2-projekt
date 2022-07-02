using System;
using System.Globalization;
using System.Threading;
using Microsoft.Data.SqlClient;

//dodawanie usuwanie
//liczenie sredniej dla dep wit ...
//logowanie?
//dodanie konta dla osoby i rozdzielenie z transakcjami

namespace TerminalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Console.Clear();

            string sqlconnection = @"Data Source=WINSERV01;Initial Catalog=project;Persist Security Info=True;User ID=test;Password=Passw0rd;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
            string sqlcommand = "";

            SqlConnection connection = null;

            while (true)
            {
                Console.WriteLine("\n\n");

                int option;

                Console.Write(
"   Baza danych finansowych" +
@"

#       Wybierz opcję:             #
# 1. Wyświetl wszystkie rekordy.   #
# 2. Wyświetl transakcje wg typu   #
# 3. Transakcje wg waluty          #
# 4. Znajdź konto według email.    #
# 5. Dodaj nowe konto.             #
# 6. Dodaj nową transakcję.        #
# 7. Zmień kolor karty.            #
# 8. Wyczyść obie tablice.         #

# 0. Wyjście z aplikacji.          #
                 
Wprowadź opcję: "
            );

                option = -1;
                try
                {
                    option = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }
                catch (FormatException)
                {
                    continue;
                }

                switch (option)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        sqlcommand = "select id, mail.ToString() as mail, acc.ToString() as acc, rgb.ToString() as rgb, bal from Accounts;";

                        connection = new SqlConnection(sqlconnection);

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            Console.WriteLine(string.Format("|{0,2}|{1,22}|{2,15}|{3,11}|{4,10}|", "id", "mail", "acc",
                                    "rgb", "bal"));

                            while (datareader.Read())
                            {
                                Console.Write(string.Format("|{0,2}|{1,22}|{2,15}|{3,11}|{4,10}|", datareader[0].ToString(), datareader[1].ToString(),
                                    datareader[2].ToString(), datareader[3].ToString(), datareader[4].ToString()));
                                Console.Write("\n");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }
                        break;
                    case 2:
                        Console.WriteLine("Podaj typ transakcji - DEP/WIT: ");
                        string typT = Console.ReadLine();
                        sqlcommand = "select id, id_acc, com.ToString() as com from Transactions"
                            + " WHERE com.Type = '" + typT + "';";
                        connection = new SqlConnection(sqlconnection);

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            Console.WriteLine(string.Format("|{0,2}|{1,6}|{2,33}|", "id", "id_acc", "com"));

                            while (datareader.Read())
                            {
                                Console.Write(string.Format("|{0,2}|{1,6}|{2,33}|", datareader[0].ToString(), datareader[1].ToString(), datareader[2].ToString()));
                                Console.Write("\n");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }
                        break;

                    case 3:
                        Console.WriteLine("Podaj walutę transakcji: ");
                        string curren = Console.ReadLine();
                        sqlcommand = "select id, id_acc, com.ToString() as com from Transactions"
                            + " WHERE com.Currency = '" + curren + "';";
                        connection = new SqlConnection(sqlconnection);

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            Console.WriteLine(string.Format("|{0,2}|{1,6}|{2,33}|", "id", "id_acc", "com"));

                            while (datareader.Read())
                            {
                                Console.Write(string.Format("|{0,2}|{1,6}|{2,33}|", datareader[0].ToString(), datareader[1].ToString(), datareader[2].ToString()));
                                Console.Write("\n");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }
                        break;
                    case 4:
                        Console.Write("Wprowadź email: ");
                        string mail = Console.ReadLine();
                        Console.Write("\n");

                        sqlcommand = "select id, mail.ToString() as mail, acc.ToString() as acc, rgb.ToString() as rgb, bal from Accounts where mail.ToString() = '" + mail + "';";
                        connection = new SqlConnection(sqlconnection);

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            while (datareader.Read())
                            {
                                Console.WriteLine("id: " + datareader[0].ToString());
                                Console.WriteLine("mail: " + datareader[1].ToString());
                                Console.WriteLine("acc: " + datareader[2].ToString());
                                Console.WriteLine("rgb: " + datareader[3].ToString());
                                Console.WriteLine("bal: " + datareader[4].ToString());
                                Console.Write("\n");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }
                        break;
                    case 5:
                        Console.WriteLine("Wprowadź email np. user@host.com ");
                        mail = Console.ReadLine();

                        Console.WriteLine("Wprowadź kolor karty np. w formacie - 153,190,227 ");
                        string color = Console.ReadLine();

                        Console.WriteLine("Wprowadź 3literowy kod waluty dużymi literami");
                        string currency = Console.ReadLine();

                        Console.WriteLine("Wprowadź dwuliterowy kod kraju ");
                        string country = Console.ReadLine();

                        Console.WriteLine("Wprowadź 6cyfrowy numer konta");
                        string accountString = Console.ReadLine();
                        try
                        {
                            int account = int.Parse(accountString);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        Console.Write("\n");

                        sqlcommand = "insert into Accounts (mail, rgb, acc, bal) values ('"
                            + mail + "', '" + color + "', '" + country + "." + accountString + "." + currency + "', 0)";
                        Console.WriteLine(sqlcommand);

                        connection = new SqlConnection(sqlconnection);

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }
                        break;

                    case 6:
                        Console.Write("Wprowadź id konta: ");
                        string id = Console.ReadLine();

                        sqlcommand = "SELECT id, acc.Currency as acc FROM Accounts WHERE id = " + id;
                        connection = new SqlConnection(sqlconnection);
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            if (datareader.Read())
                            {
                                string currentCurrency = datareader["acc"].ToString();
                                datareader.Close();
                                Console.WriteLine("Wprowadź typ transakcji DEP/WIT: ");
                                string type = Console.ReadLine();

                                Console.WriteLine("Wprowadź 3literowy kod waluty: ");
                                string curr = Console.ReadLine();

                                if (curr != currentCurrency)
                                {
                                    throw new Exception("To konto jest w innej walucie!");
                                }

                                Console.WriteLine("Wprowadź kwotę: ");
                                string amo = Console.ReadLine();
                                double amount = 0.0;
                                try
                                {
                                    amount = double.Parse(amo.Replace(',', '.'));
                                }
                                catch (FormatException e)
                                {
                                    Console.WriteLine(e.Message);
                                }

                                Console.WriteLine("Wprowadź procent prowizji (bez %) ");
                                string per = Console.ReadLine();
                                double percent = 0.0;
                                try
                                {
                                    percent = double.Parse(per.Replace(',', '.'));
                                }
                                catch (FormatException e)
                                {
                                    Console.WriteLine(e.Message);
                                }

                                sqlcommand = "INSERT INTO Transactions (id_acc, com) VALUES (" + id + " , '" + type + ";" + per + ";" + amo +  ";" + curr + "');";
                                Console.WriteLine(sqlcommand);
                                command = new SqlCommand(sqlcommand, connection);
                                command.ExecuteNonQuery();
                                double sum = 0.0;
                                if (type.ToLower() == "wit")
                                {
                                    sum = Math.Round(amount * (100 + percent) * 0.01, 2);
                                    sqlcommand = "UPDATE Accounts SET bal = bal - " + sum + " WHERE id = " + id + ";";
                                }
                                    
                                else
                                {
                                    sum = Math.Round(amount * (100 - percent) * 0.01, 2);
                                    sqlcommand = "UPDATE Accounts SET bal = bal + " + sum + " WHERE id = " + id + ";";
                                }
                                Console.WriteLine(sqlcommand);
                                command = new SqlCommand(sqlcommand, connection);
                                command.ExecuteNonQuery();
                            }

                            else
                            {
                                Console.WriteLine("Nie ma konta o takim ID!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }

                        break;

                    case 7:
                        Console.Write("Wprowadź id konta: ");
                        id = Console.ReadLine();


                        sqlcommand = "SELECT id FROM Accounts WHERE id = " + id;
                        connection = new SqlConnection(sqlconnection);
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sqlcommand, connection);
                            SqlDataReader datareader = command.ExecuteReader();

                            if (datareader.Read())
                            {
                                datareader.Close();
                                Console.Write("Wprowadź kolor nowej karty (r,g,b): ");
                                string rgb = Console.ReadLine();

                                sqlcommand = "update Accounts set rgb = '" + rgb + "' where id = " + id + ";";
                                command = new SqlCommand(sqlcommand, connection);
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                Console.WriteLine("Nie ma konta o takim ID!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally { connection.Close(); }

                        break;
                    case 8:
                        Console.Write("Czy jesteś tego pewien? y/Y");
                        id = Console.ReadLine();

                        if (id == "y" || id == "Y")
                        {
                            sqlcommand = "TRUNCATE TABLE Accounts; TRUNCATE TABLE Transactions;";
                            connection = new SqlConnection(sqlconnection);

                            try
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand(sqlcommand, connection);
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally { connection.Close(); }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}