using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Profile";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}