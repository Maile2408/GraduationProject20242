using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RememberMeManager
{
    private const string KeyCurrentEmail = "RememberedEmail";
    private const string KeyRemember = "RememberMe";
    private const string KeyEmailList = "RememberedEmailList";

    public static void Save(string email, string password)
    {
        PlayerPrefs.SetString(KeyCurrentEmail, email);
        PlayerPrefs.SetString(GetPasswordKey(email), password);
        PlayerPrefs.SetInt(KeyRemember, 1);
        AddEmailToList(email);
        PlayerPrefs.Save();
    }

    public static void Clear()
    {
        string email = GetEmail();
        if (!string.IsNullOrEmpty(email))
            PlayerPrefs.DeleteKey(GetPasswordKey(email));

        PlayerPrefs.DeleteKey(KeyCurrentEmail);
        PlayerPrefs.SetInt(KeyRemember, 0);
        PlayerPrefs.Save();
    }

    public static string GetEmail() => PlayerPrefs.GetString(KeyCurrentEmail, "");

    public static string GetPassword() => GetPasswordOf(GetEmail());

    public static string GetPasswordOf(string email)
    {
        if (string.IsNullOrEmpty(email)) return "";
        return PlayerPrefs.GetString(GetPasswordKey(email), "");
    }

    public static bool IsRemembered() => PlayerPrefs.GetInt(KeyRemember, 0) == 1;

    public static List<string> GetEmailList()
    {
        string raw = PlayerPrefs.GetString(KeyEmailList, "");
        if (string.IsNullOrEmpty(raw)) return new List<string>();
        return raw.Split(',').Distinct().ToList();
    }

    public static void AddEmailToList(string email)
    {
        var list = GetEmailList();
        if (!list.Contains(email))
        {
            list.Add(email);
            PlayerPrefs.SetString(KeyEmailList, string.Join(",", list));
        }
    }

    private static string GetPasswordKey(string email) => $"RememberedPassword_{email}";
}
