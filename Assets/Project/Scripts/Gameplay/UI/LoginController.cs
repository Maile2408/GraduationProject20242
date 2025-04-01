using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Login";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}