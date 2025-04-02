using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Settings";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}