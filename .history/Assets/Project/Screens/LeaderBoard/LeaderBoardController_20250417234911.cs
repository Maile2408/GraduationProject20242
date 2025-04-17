using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour, IKeyBack
{
    public const string NAME = "LeaderBoard";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}