using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Main";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}