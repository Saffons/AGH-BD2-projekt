using System;
using System.Globalization;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true, ValidationMethodName = "ValidateAccNumber")]
public struct AccNumber : INullable
{
    private string _country;
    private int _number;
    private bool m_Null;

    public AccNumber(string country, int number)
    {
        _country = country;
        _number = number;
        m_Null = false;
    }

    public override string ToString()
    {
        if (this.IsNull)
            return "NULL";
        else
        {
            return  "ACC" + _country +  + _number ;
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
        string[] vals = s.Value.Split(",".ToCharArray());

        if (vals.Length != 2)
        {
            throw new ArgumentException("Niepoprawne wartosci. Podaj dwie wartosci, kod kraju i numer konta po przecinku");
        }

        vals[0] = vals[0].Trim();
        vals[1] = vals[1].Trim();
        acc._country = vals[0].Substring(1, vals[0].Length - 1).Trim();
        //acc._country = int.Parse(vals[0].Substring(1, vals[0].Length - 1).Trim(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
        acc._number = int.Parse(vals[1].Trim(), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);

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
            return this._country;
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
            return this._number;
        }

        set
        {
            _number = value;
        }
    }

    public static bool isEquals(AccNumber acc1, AccNumber acc2)
    {
        return acc1.Country == acc2.Country && acc1.Number == acc2.Number;
    }

    private bool ValidateAccNumber()
    {
        if (String.IsNullOrEmpty(this.ToString()))
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
            throw new ArgumentException("Podaj poprawny kod kraju, 2-literowy, np PL!");
        }

        if (_number.ToString().Length != 6 )
        {
            throw new ArgumentException("Numer konta jest liczba calkowita o dlugosci 6 cyfr!");
        }

        return true;
    }
}