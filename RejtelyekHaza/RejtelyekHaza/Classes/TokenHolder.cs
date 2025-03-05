using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Org.BouncyCastle.Asn1.Icao;
using RejtelyekHaza.DTOs;
using RejtelyekHaza.Models;

namespace RejtelyekHaza.Classes;


public class TokenHolder
{
    public static List<CustomToken> tokens = new List<CustomToken>();
    public LoggedInUserDTO master = new LoggedInUserDTO();
        
    public static System.Timers.Timer? aTimer;

    public bool AutoDecrease { get; private set; }
    public bool SingleUseToken { get; private set; }
    int interval;
    public int Interval
    {
        get { return interval; }
        set
        {
            interval = value;
            if (interval == 0)
            {
                aTimer?.Close();
            }
            else
            {
                SetTimer(Interval);
            }
        }
    }

    public void SetTimer(int interval)
    {
        aTimer = new System.Timers.Timer(interval);
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    public void OnTimedEvent(Object? source, ElapsedEventArgs e)
    {
        DecreaseExpirationTime();
    }
    
    public string GetMasterToken() 
    {
        return tokens[0].LoggedInUser.Token;
    }
    
    public void DecreaseExpirationTime()
    {
        if (tokens.Count > 0)
        {
            for (int i = tokens.Count - 1; i > 0; i--)
            {
                if (tokens[i].RemainingTime > 0)
                {
                    tokens[i].DecreaseRemainingTime();
                }
                else
                {
                    tokens.Remove(tokens[i]);
                }
            }
        }
    }

    public LoggedInUserDTO GenerateToken(int expirationTime, User loggedInUser)
    {
        LoggedInUserDTO newLoggedInUser = new LoggedInUserDTO();
        newLoggedInUser.Id=loggedInUser.Id;
        newLoggedInUser.Name=loggedInUser.Name;
        newLoggedInUser.Email=loggedInUser.Email;
        newLoggedInUser.UserName=loggedInUser.UserName;
        newLoggedInUser.Permission=loggedInUser.Permission;
        newLoggedInUser.PhoneNumber=loggedInUser.PhoneNumber;   
        tokens.Add(new CustomToken(expirationTime, newLoggedInUser));
        return newLoggedInUser;   
    }

    public TokenHolder(bool singleUseToken, int interval)
    {
        SingleUseToken = singleUseToken;
        Interval = interval;        
        master.Id = Guid.NewGuid().ToString();
        master.UserName = "master";
        master.Permission = 9;
        master.Name = "";
        master.Email = "";
        master.PhoneNumber = "";
        tokens.Add(new CustomToken(0,master));
    }

    public TokenHolder(Guid masterToken, bool singleUseToken, int interval)
    {
        SingleUseToken = singleUseToken;
        Interval = interval;
        master.Id = Guid.NewGuid().ToString();
        master.UserName = "master";
        master.Permission = 9;
        master.Name = "";
        master.Email = "";
        master.PhoneNumber = "";
        tokens.Add(new CustomToken(masterToken,0,master));        
    }

    public CustomToken CheckTokenValidity(string token)
    {
        int index = -1;
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].LoggedInUser.Token.ToString() == token)
            {
                index = i; break;
            }
        }
        if (index != -1)
        {
            tokens[index].ResetRemainingTime();
            if (SingleUseToken == true && index!=0) { tokens[index].LoggedInUser.Token = Guid.NewGuid().ToString(); }
            return tokens[index];
        }
        else
        {
            return new CustomToken(new Guid(),-1,new LoggedInUserDTO());
        }
    }

    ~TokenHolder()
    {
        aTimer?.Close();
    }

}
