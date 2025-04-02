using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "BuildMenu";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}