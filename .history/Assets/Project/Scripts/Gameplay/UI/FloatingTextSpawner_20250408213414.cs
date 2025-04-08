using UnityEngine;

public class FloatingTextSpawner
{
    public static void Show(string text, Vector3 worldPos)
    {
        GameObject fx = PoolManager.Instance.Spawn(PoolPrefabPath.FX("FX_CoinText"));
        if (fx == null) return;

        fx.transform.SetParent(GameObject.Find("Canvas").transform, false);
        fx.transform.position = worldPos;

        var effect = fx.GetComponent<FloatingTextEffect>();
        effect?.PlayText(text);
    }
}
