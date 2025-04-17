using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordEyeToggle : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Image eyeIcon;
    [SerializeField] private Sprite showIcon;
    [SerializeField] private Sprite hideIcon;

    private bool isPasswordVisible = false;

    public void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;

        passwordField.contentType = isPasswordVisible
            ? TMP_InputField.ContentType.Standard
            : TMP_InputField.ContentType.Password;

        passwordField.ForceLabelUpdate();

        eyeIcon.sprite = isPasswordVisible ? hideIcon : showIcon;
    }
}
