public static class LoadingRequest
{
    public static string targetScene;
    public static LoadStage loadStage;

    public enum LoadStage
    {
        LoadProfileToHome,
        LoadGameDataToGameplay,
        ReturnHomeFromGameplay
    }
}
