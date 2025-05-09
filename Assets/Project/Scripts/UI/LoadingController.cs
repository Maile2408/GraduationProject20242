using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Loading";

    public enum LoadType
    {
        ProfileToHome,
        GameDataToGamePlay,
        BackToHome
    }

    private static LoadType currentType;

    [SerializeField] private Transform loadingIcon;
    [SerializeField] private float rotationSpeed = 180f;

    private Tween spinnerTween;

    public static void Show(LoadType type)
    {
        currentType = type;
        ScreenManager.Load<LoadingController>(NAME);
    }

    private void OnEnable()
    {
        if (loadingIcon != null)
        {
            spinnerTween = loadingIcon
                .DORotate(new Vector3(0, 0, -360), 360f / rotationSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }
    }

    private void OnDisable()
    {
        spinnerTween?.Kill();
    }

    void Start()
    {
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        switch (currentType)
        {
            case LoadType.ProfileToHome:
                yield return HomeLoader.Instance.LoadProfileData();
                ScreenManager.Load<HomeController>(HomeController.NAME);
                break;

            case LoadType.GameDataToGamePlay:
                ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
                break;

            case LoadType.BackToHome:
                ScreenManager.Load<HomeController>(HomeController.NAME);
                break;
        }
    }
    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}