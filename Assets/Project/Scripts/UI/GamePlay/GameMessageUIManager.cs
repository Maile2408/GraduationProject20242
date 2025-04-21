using System.Collections.Generic;
using UnityEngine;

public class GameMessageUIManager : MonoBehaviour
{
    public static GameMessageUIManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private RectTransform container;
    [SerializeField] private int maxMessages = 3;
    [SerializeField] private float spacingY = 80f;

    private readonly List<UnityEngine.GameObject> activeMessages = new();

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

            if (oldest != null)
            {
                if (PoolManager.Instance.HasSpawned(oldest))
                {
                    PoolManager.Instance.Despawn(oldest);
                }
                else
                {
                    Destroy(oldest); 
                }
            }

            activeMessages.RemoveAt(activeMessages.Count - 1);
        }

        UnityEngine.GameObject msgObj = PoolManager.Instance.Spawn(PoolPrefabPath.UI("GameMessageUI"), container);

        if (msgObj == null)
        {
            Debug.LogWarning("GameMessageUIManager: Spawn returned null");
            return;
        }

        msgObj.transform.SetAsFirstSibling();

        var msgUI = msgObj.GetComponent<GameMessageUI>();
        if (msgUI == null)
        {
            Debug.LogError("GameMessageUI component missing on spawned GameMessageUI prefab!");
            return;
        }

        AudioManager.Instance.PlayMessageShow();
        
        msgUI.ShowMessage(message, type);
        activeMessages.Insert(0, msgObj);

        for (int i = 0; i < activeMessages.Count; i++)
        {
            if (activeMessages[i] == null) continue;
            var rt = activeMessages[i].GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * spacingY);
        }
    }

}
