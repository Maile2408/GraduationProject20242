using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopupController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ConfirmationPopup";

    public static Action OnYesCallback;
    public static Action OnNoCallback;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;

    private void OnClickYes()
    {
        OnYesCallback?.Invoke();
        ScreenManager.Close();
    }

    private void OnClickNo()
    {
        OnNoCallback?.Invoke();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}