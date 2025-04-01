using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Register";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}