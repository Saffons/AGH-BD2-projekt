using System;
using System.Globalization;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true, MaxByteSize = 3000, ValidationMethodName = "ValidateAccNumber")]
public struct AccNumber : INullable, IBinarySerialize
{
    private string _country;
    private int _number;
    private string _currency;
    private double _balance;
    private bool m_Null;

    public AccNumber(string country, int number, string currency)
    {
        _country = country;
        _number = number;
        _currency = currency;
        _balance = 0;
        m_Null = false;
    }

    public override string ToString()
    {
        if (this.IsNull)
            return "NULL";
        else
        {
            return _country + "." + _number + "." + _currency;
        }
    }

    public bool IsNull
    {
        get
        {
            return m_Null;
        }
    }

    public static AccNumber Null
    {
        get
        {
            AccNumber acc = new AccNumber();
            acc.m_Null = true;
            return acc;
        }
    }

    public static AccNumber Parse(SqlString s)
    {
        string value = s.Value;
        if (s.IsNull || value.Trim() == "")
        {
            return Null;
        }

        AccNumber acc = new AccNumber();
        string[] vals = s.Value.Split(".".ToCharArray());

        if (vals.Length != 3)
        {
            throw new ArgumentException("Niepoprawne wartosci. Podaj trzy wartosci, 2literowy kod kraju, numer konta i 3literowy kod waluty po kropce");
        }

        acc._country = vals[0];
        acc._number = int.Parse(vals[1]);
        acc._currency = vals[2];

        if (!acc.ValidateAccNumber())
        {
            throw new ArgumentException("Niepoprawne konto.");
        }

        return acc;
    }

    public string Country
    {
        get
        {
            return _country;
        }

        set
        {
            _country = value;
        }
    }

    public int Number
    {
        get
        {
            return _number;
        }

        set
        {
            _number = value;
        }
    }

    public string Currency
    {
        get
        {
            return _currency;
        }

        set
        {
            _currency = value;
        }
    }

    public double Balance
    {
        get
        {
            return _balance;
        }

        set
        {
            _balance = value;
        }
    }

    public void Deposit(double val)
    {
        _balance += val;
    }

    public void Withdraw(double val)
    {
        _balance -= val;
    }

    public static bool isEqual(AccNumber acc1, AccNumber acc2)
    {
        return acc1.Country == acc2.Country && acc1.Number == acc2.Number;
    }

    private bool ValidateAccNumber()
    {
        if (string.IsNullOrEmpty(ToString()))
        {
            return false;
        }

        try
        {
            RegionInfo info = new RegionInfo(_country);
        }
        catch (ArgumentException argEx)
        {
            Console.WriteLine(argEx.Message);
            throw new ArgumentException("Podaj poprawny kod kraju, 2-literowy, np pl!");
        }

        if (_number.ToString().Length != 6)
        {
            throw new ArgumentException("Numer konta jest liczba calkowita o dlugosci 6 cyfr!");
        }

        return true;
    }

    public void Write(System.IO.BinaryWriter w)
    {
        string acc = _country + "." + _number + "." + _currency;
        w.Write(acc);
    }

    public void Read(System.IO.BinaryReader r)
    {
        string acc = r.ReadString();
        string[] parameters = acc.Split(".".ToCharArray());
        _country = parameters[0];
        _number = int.Parse(parameters[1]);
        _currency = parameters[2];
    }
}
