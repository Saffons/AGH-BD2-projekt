using System;
using System.Globalization;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native,
IsByteOrdered = true, ValidationMethodName = "ValidateCardRGB")]
public struct CardRGB : INullable
{
    private int _r;
    private int _g;
    private int _b;
    private bool m_Null;

    public CardRGB(int r, int g, int b)
    {
        _r = r;
        _g = g;
        _b = b;
        m_Null = false;
    }

    public override string ToString()
    {
        if (this.IsNull)
            return "NULL";
        else
        {
            return  _r + "," + _g + "," + _b;
        }
    }

    public bool IsNull
    {
        get
        {
            return m_Null;
        }
    }

    public static CardRGB Null
    {
        get
        {
            CardRGB rgb = new CardRGB();
            rgb.m_Null = true;
            return rgb;
        }
    }

    public static CardRGB Parse(SqlString s)
    {
        string value = s.Value;
        if (s.IsNull || value.Trim() == "")
        {
            return Null;
        }

        CardRGB rgb = new CardRGB();
        string[] vals = s.Value.Split(",".ToCharArray());

        if (vals.Length != 3)
        {
            throw new ArgumentException("Podaj 3 wartosci, dla red green i blue.");
        }

        rgb._r = int.Parse(vals[0]);
        rgb._g = int.Parse(vals[1]);
        rgb._b = int.Parse(vals[2]);

        if (!rgb.ValidateCardRGB())
        {
            throw new ArgumentException("Invalid input values.");
        }

        return rgb;
    }

    public int R
    {
        get
        {
            return _r;
        }

        set
        {
            _r = value;
        }
    }

    public int G
    {
        get
        {
            return _g;
        }

        set
        {
            _g = value;
        }
    }

    public int B
    {
        get
        {
            return _b;
        }

        set
        {
            _b = value;
        }
    }

    public static CardRGB add(CardRGB rgb1, CardRGB rgb2)
    {
        return new CardRGB(Math.Min(rgb1.R + rgb2.R, 255), Math.Min(rgb1.G + rgb2.G, 255),
        Math.Min(rgb1.B + rgb2.B, 255));
    }

    public static bool isEqual(CardRGB rgb1, CardRGB rgb2)
    {
        return rgb1.R == rgb2.R && rgb1.G == rgb2.G && rgb1.B == rgb2.B;
    }

    private bool ValidateCardRGB()
    {
        if (string.IsNullOrEmpty(ToString()))
        {
            return false;
        }

        if (_r < 0 || _r > 255)
        {
            throw new ArgumentException("Red powinien byc liczba calkowita od 0 do 255");
        }

        if (_g < 0 || _g > 255)
        {
            throw new ArgumentException("Green powinien byc liczba calkowita od 0 do 255");
        }

        if (_b < 0 || _b > 255)
        {
            throw new ArgumentException("Blue powinien byc liczba calkowita od 0 do 255");
        }

        return true;
    }
}