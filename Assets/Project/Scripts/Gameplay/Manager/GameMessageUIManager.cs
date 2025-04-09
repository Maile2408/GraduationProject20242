using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageUIManager : MonoBehaviour
{
    public static GameMessageUIManager Instance { get; private set; }

    [SerializeField] private GameMessageUI gameMessageUI;

    private Queue<(string message, GameMessageType type)> messageQueue = new();
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameMessageUIManager!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowMessage(string message, GameMessageType type)
    {
        messageQueue.Enqueue((message, type));
        TryShowNext();
    }

    private void TryShowNext()
    {
        if (isShowing || messageQueue.Count == 0) return;

        var (msg, type) = messageQueue.Dequeue();
        StartCoroutine(ShowRoutine(msg, type));
    }

    private IEnumerator ShowRoutine(string msg, GameMessageType type)
    {
        isShowing = true;
        yield return gameMessageUI.ShowMessage(msg, type);
        isShowing = false;
        TryShowNext();
    }
}
