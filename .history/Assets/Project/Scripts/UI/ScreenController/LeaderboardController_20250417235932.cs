using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Leaderboard";

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}