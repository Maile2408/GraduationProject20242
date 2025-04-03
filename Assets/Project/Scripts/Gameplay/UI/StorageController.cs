using UnityEngine;

public class StorageController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Storage";

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}