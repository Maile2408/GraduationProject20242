using UnityEngine;
using DG.Tweening;
using System.Collections;

public class LoadingController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Loading";

    [SerializeField] Transform loadingIcon;
    [SerializeField] float rotationSpeed = 180f;

    Tween spinnerTween;

    private void OnEnable()
    {
        if (loadingIcon == null) return;

        spinnerTween = loadingIcon
            .DORotate(new Vector3(0, 0, -360), 360f / rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        spinnerTween?.Kill();
    }

    private void Start()
    {
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        switch (LoadingRequest.loadStage)
        {
            case LoadingRequest.LoadStage.LoadProfileToHome:
                yield return HomeLoader.Instance.LoadProfileData();
                break;

            case LoadingRequest.LoadStage.LoadGameDataToGameplay:
                yield return GameLoader.Instance.LoadAllGameData();
                break;

            case LoadingRequest.LoadStage.ReturnHomeFromGameplay:
                yield return new WaitForSeconds(0.1f);
                break;
        }

        yield return new WaitForSeconds(0.1f);

        switch (LoadingRequest.targetScene)
        {
            case HomeController.NAME:
                ScreenManager.Load<HomeController>(HomeController.NAME);
                break;

            case GamePlayController.NAME:
                ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
                break;
        }
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
