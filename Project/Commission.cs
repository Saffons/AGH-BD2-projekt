using System;
using System.Globalization;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true, ValidationMethodName = "ValidateCommission")]
public struct Commission : INullable
{
    private DateTime _date;
    private double _percent;
    private double _amount;
    private string _currency;
    private bool m_Null;

    public Commission(double percent, double amount, string currency)
    {
        _date = DateTime.Now;
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
            return "Commission: " + _date.ToString() + " " + _percent + "% z " + _amount + " " + _currency;
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
        string[] vals = s.Value.Split(",".ToCharArray());

        if (vals.Length != 3)
        {
            throw new ArgumentException("Podaj po przecinku - % prowizji - kwota - 3-literowy kod waluty");
        }

        vals[0] = vals[0].Trim();
        vals[1] = vals[1].Trim();
        vals[2] = vals[2].Trim();
        com._percent = double.Parse(vals[0].Substring(1, vals[0].Length - 1).Trim());
        com._amount = double.Parse(vals[1].Substring(1, vals[0].Length - 1).Trim());
        com._currency = vals[2].Trim();

        if (!com.ValidateCommission())
        {
            throw new ArgumentException("Niepoprawna prowizja");
        }

        return com;
    }

    public double Percent
    {
        get
        {
            return this._percent;
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
            return this._amount;
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
            return this._currency;
        }

        set
        {
            _currency = value;
        }
    }

    public static bool isEquals(Commission com1, Commission com2)
    {
        return (com1.Currency == com2.Currency && com1.Amount == com2.Amount && com1.Percent == com2.Percent);
    }

    private bool ValidateCommission()
    {
        if (String.IsNullOrEmpty(this.ToString()))
        {
            return false;
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
            throw new ArgumentException("Prowizja musi być w przedziale od 0 do 100(%)");
        }

        if (0 > _amount || _percent > 10000000)
        {
            throw new ArgumentException("Kwota musi być w przedziale od 0 do 10 000 000");
        }

        return true;
    }
}