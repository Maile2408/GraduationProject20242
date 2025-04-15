using System.Collections.Generic;
using UnityEngine;

public class GameMessageUIManager : MonoBehaviour
{
    public static GameMessageUIManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private RectTransform container;
    [SerializeField] private int maxMessages = 3;
    [SerializeField] private float spacingY = 80f;

    private readonly List<GameObject> activeMessages = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowMessage(string message, GameMessageType type)
    {
        if (activeMessages.Count >= maxMessages)
        {
            var oldest = activeMessages[^1];
            PoolManager.Instance.Despawn(oldest);
            activeMessages.RemoveAt(activeMessages.Count - 1);
        }

        GameObject msgObj = PoolManager.Instance.Spawn(PoolPrefabPath.UI("GameMessageUI"), container);
        msgObj.transform.SetAsFirstSibling();

        GameMessageUI msgUI = msgObj.GetComponent<GameMessageUI>();
        msgUI.ShowMessage(message, type);

        activeMessages.Insert(0, msgObj);

        for (int i = 0; i < activeMessages.Count; i++)
        {
            var rt = activeMessages[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * spacingY);
        }
    }
}
