using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationPopupController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ConfirmationPopup";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}