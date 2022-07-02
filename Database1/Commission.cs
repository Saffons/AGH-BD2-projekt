using System;
using System.Globalization;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true, MaxByteSize = 3000, ValidationMethodName = "ValidateCommission")]
public struct Commission : INullable, IBinarySerialize
{
    private string _type;
    private double _percent;
    private double _amount;
    private string _currency;
    private bool m_Null;

    public Commission(string type, double percent, double amount, string currency)
    {
        _type = type;
        _percent = percent;
        _amount = amount;
        _currency = currency;
        m_Null = false;
    }

    public override string ToString()
    {
        if (this.IsNull)
            return "NULL";
        else
        {
            return _type + ";" + _percent + ";" + _amount + ";" + _currency;
        }
    }

    public bool IsNull
    {
        get
        {
            return m_Null;
        }
    }

    public static Commission Null
    {
        get
        {
            Commission com = new Commission();
            com.m_Null = true;
            return com;
        }
    }

    public static Commission Parse(SqlString s)
    {
        string value = s.Value;
        if (s.IsNull || value.Trim() == "")
        {
            return Null;
        }

        Commission com = new Commission();
        string[] vals = s.Value.Split(";".ToCharArray());
        Console.WriteLine(vals);
        if (vals.Length != 4)
        {
            throw new ArgumentException("Podaj po srednikach rodzaj transakcji (DEP WIT) - % prowizji - kwota - 3-literowy kod waluty");
        }

        com._type = vals[0];
        com._percent = double.Parse(vals[1], CultureInfo.CurrentCulture);
        com._amount = double.Parse(vals[2], CultureInfo.CurrentCulture);
        com._currency = vals[3];

        if (!com.ValidateCommission())
        {
            throw new ArgumentException("Niepoprawna prowizja");
        }

        return com;
    }


    public string Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }
    public double Percent
    {
        get
        {
            return _percent;
        }

        set
        {
            _percent = value;
        }
    }

    public double Amount
    {
        get
        {
            return _amount;
        }

        set
        {
            _amount = value;
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

    public static bool isEqual(Commission com1, Commission com2)
    {
        return (com1.Type == com2.Type && com1.Currency == com2.Currency && com1.Amount == com2.Amount && com1.Percent == com2.Percent);
    }

    public double CalculateCommission()
    {
        return Math.Round(_amount * _percent * 0.01, 2);
    }

    private bool ValidateCommission()
    {
        if (string.IsNullOrEmpty(ToString()))
        {
            return false;
        }

        if (_type != "WIT" && _type != "DEP")
        {
            throw new ArgumentException("Podaj poprawny typ operacji! DEP/WIT");
        }

        string curr = _currency;

        RegionInfo region = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(ct => new RegionInfo(ct.LCID))
                .Where(ri => ri.ISOCurrencySymbol == curr).FirstOrDefault();

        if (region == null)
        {
            throw new ArgumentException("Niepoprawny kod waluty, podaj 3-literowy, np EUR PLN...");
        }

        if (0 > _percent || _percent > 100)
        {
            throw new ArgumentException("Prowizja musi byæ w przedziale od 0 do 100(%)");
        }

        if (0 > _amount || _percent > 10000000)
        {
            throw new ArgumentException("Kwota musi byæ w przedziale od 0 do 10 000 000");
        }

        return true;
    }

    public void Write(System.IO.BinaryWriter w)
    {
        string comm = _type + ";" + _percent + ";" + _amount + ";" + _currency;
        Console.WriteLine(comm);        
        w.Write(comm);
    }

    public void Read(System.IO.BinaryReader r)
    {
        string comm = r.ReadString();
        string[] parameters = comm.Split(";".ToCharArray());
        _type = parameters[0];
        _percent = double.Parse(parameters[1], CultureInfo.CurrentCulture);
        _amount = double.Parse(parameters[2], CultureInfo.CurrentCulture);
        _currency = parameters[3];
        Console.WriteLine(comm);
    }
}
