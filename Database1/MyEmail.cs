using System;
using System.Text;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined,
IsByteOrdered = true, MaxByteSize = 1000, ValidationMethodName = "ValidateMyEmail")]
public struct MyEmail : INullable, IBinarySerialize
{
    private string _user;
    private string _host;
    private bool m_Null;

    public MyEmail(string user, string host)
    {
        MyEmail me = new MyEmail("temp", "temp.temp");
        me.Host = host;
        me.Username = user;
        if (!me.ValidateMyEmail())
        {
            throw new ArgumentException("Invalid user or host values.");
        }

        _user = user;
        _host = host;
        m_Null = false;
    }

    public override string ToString()
    {
        if (this.IsNull)
            return "NULL";
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_user);
            builder.Append("@");
            builder.Append(_host);
            return builder.ToString();
        }
    }

    public bool IsNull
    {
        get
        {
            return m_Null;
        }
    }

    public static MyEmail Null
    {
        get
        {
            MyEmail me = new MyEmail();
            me.m_Null = true;
            return me;
        }
    }

    public static MyEmail Parse(SqlString s)
    {
        string value = s.Value;
        if (s.IsNull || value.Trim() == "")
        {
            return Null;
        }

        if (!ValidateMyEmailString(s))
        {
            throw new ArgumentException("Invalid email value.");
        }

        MyEmail me = new MyEmail();
        string[] email = s.Value.Split("@".ToCharArray());
        me._user = email[0];
        me._host = email[1];

        if (!me.ValidateMyEmail())
        {
            throw new ArgumentException("Invalid user or host values.");
        }

        return me;
    }

    public string Host
    {
        get
        {
            return _host;
        }

        set
        {
            string temp = _host;
            _host = value;
            if (!ValidateMyEmail())
            {
                _host = temp;
                throw new ArgumentException("Invalid host value.");
            }
        }
    }

    public string Username
    {
        get
        {
            return _user;
        }

        set
        {
            _user = value;
        }
    }

    public static bool hasSameUser(MyEmail me1, MyEmail me2)
    {
        return me1.Username.Equals(me2.Username);
    }

    public static bool isEqual(MyEmail me1, MyEmail me2)
    {
        return me1.Host == me2.Host && me1.Username == me2.Username;
    }

    private bool ValidateMyEmail()
    {
        if (string.IsNullOrEmpty(ToString()))
        {
            return false;
        }

        string[] email = ToString().Split("@".ToCharArray());
        if (email[0].Length > 64)
        {
            return false;
        }

        return Regex.IsMatch(this.ToString(),
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase);
    }

    public static bool ValidateMyEmailString(SqlString s)
    {
        if (String.IsNullOrEmpty(s.ToString()))
        {
            return false;
        }

        string[] email = s.Value.Split("@".ToCharArray());
        if (email[0].Length > 64 || email[0].Length > 253)
        {
            return false;
        }

        return Regex.IsMatch(s.ToString(),
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase);
    }

    public void Write(System.IO.BinaryWriter w)
    {
        string email = _user + "@" + _host;
        w.Write(email);
    }

    public void Read(System.IO.BinaryReader r)
    {
        string email = r.ReadString();
        string[] parameters = email.Split("@".ToCharArray());
        _user = parameters[0];
        _host = parameters[1];
    }
}