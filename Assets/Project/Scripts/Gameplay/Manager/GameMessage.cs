public static class GameMessage
{
    public static void Info(string msg) => GameMessageUIManager.Instance.ShowMessage(msg, GameMessageType.Info);
    public static void Success(string msg) => GameMessageUIManager.Instance.ShowMessage(msg, GameMessageType.Success);
    public static void Warning(string msg) => GameMessageUIManager.Instance.ShowMessage(msg, GameMessageType.Warning);
    public static void Guide(string msg) => GameMessageUIManager.Instance.ShowMessage(msg, GameMessageType.Guide);
}
