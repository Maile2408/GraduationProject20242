using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Home";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}