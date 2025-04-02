using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Storage";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}