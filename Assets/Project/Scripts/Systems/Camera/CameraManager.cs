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
                Debug.LogError("Cameramanager has not been created!");
            }
            return _instance;
        }
    }

    [Header("Camera Objects")]
    [SerializeField] private UnityEngine.GameObject thirdPersonCam;
    [SerializeField] private UnityEngine.GameObject firstPersonCam;
    [SerializeField] private UnityEngine.GameObject godModeCam;

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
        thirdPersonCam?.SetActive(false);
        firstPersonCam?.SetActive(false);
        godModeCam?.SetActive(false);

        Camera newActiveCamera = null;

        if (cameraType == GameCameraType.thirdPerson)
        {
            thirdPersonCam?.SetActive(true);
            newActiveCamera = thirdPersonCam.GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            GameMessage.Info("Third Person Camera");
        }
        else if (cameraType == GameCameraType.firstPerson)
        {
            firstPersonCam?.SetActive(true);
            newActiveCamera = firstPersonCam.GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            GameMessage.Info("First Person Camera");
        }
        else if (cameraType == GameCameraType.godMode)
        {
            godModeCam?.SetActive(true);
            newActiveCamera = godModeCam.GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameMessage.Info("God Mode Camera");
        }

        foreach (var cam in Camera.allCameras)
            cam.tag = "Untagged";

        if (newActiveCamera != null)
            newActiveCamera.tag = "MainCamera";

        currentCamera = cameraType;
    }

}
