using UnityEngine;

public class CameraManager : SaiBehaviour
{
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CameraManager chưa được khởi tạo!");
            }
            return _instance;
        }
    }

    [Header("Camera Objects")]
    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private GameObject firstPersonCam;
    [SerializeField] private GameObject godModeCam;

    [Header("Current Camera Mode")]
    [SerializeField] private GameCameraType currentCamera = GameCameraType.godMode;

    protected override void Awake()
    {
        base.Awake();
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    protected override void Start()
    {
        base.Start();
        SwitchToCamera(currentCamera);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            ToggleCameraMode();
        }
    }

    public void ToggleCameraMode()
    {
        switch (currentCamera)
        {
            case GameCameraType.thirdPerson:
                SwitchToCamera(GameCameraType.firstPerson);
                break;
            case GameCameraType.firstPerson:
                SwitchToCamera(GameCameraType.godMode);
                break;
            case GameCameraType.godMode:
                SwitchToCamera(GameCameraType.thirdPerson);
                break;
        }
    }

    public void SwitchToCamera(GameCameraType cameraType)
    {
        // Tắt tất cả camera
        thirdPersonCam?.SetActive(false);
        firstPersonCam?.SetActive(false);
        godModeCam?.SetActive(false);

        // Bật camera được chọn
        if (cameraType == GameCameraType.thirdPerson) thirdPersonCam?.SetActive(true);
        if (cameraType == GameCameraType.firstPerson) firstPersonCam?.SetActive(true);
        if (cameraType == GameCameraType.godMode) godModeCam?.SetActive(true);

        // Cập nhật trạng thái camera hiện tại
        currentCamera = cameraType;
    }
}
