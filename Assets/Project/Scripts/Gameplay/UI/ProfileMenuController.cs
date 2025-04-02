using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ProfileMenu";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}