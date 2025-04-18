using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopupController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ConfirmationPopup";

    public static Action OnYesCallback;
    public static Action OnNoCallback;
    public static string Message = "Are you sure?";

    [SerializeField] private TMP_Text messageText;

    private void OnEnable()
    {
        if (messageText != null)
            messageText.text = Message;
    }

    public void OnClickYes()
    {
        OnYesCallback?.Invoke();
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void OnClickNo()
    {
        OnNoCallback?.Invoke();
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }
   
    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}