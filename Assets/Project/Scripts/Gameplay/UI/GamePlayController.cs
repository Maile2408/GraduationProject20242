using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}