using UnityEngine;

public class CameraManager : SaiBehaviour
{
    public static CameraManager instance;

    [Header("Camera Objects")]
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public GameObject godModeCam;

    [Header("Current Camera Mode")]
    public CameraType currentCamera = CameraType.godMode;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null) Debug.LogError("Only one CameraManager allowed!");
        instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCameras();
    }

    private void LoadCameras()
    {
        if (this.thirdPersonCam == null) this.thirdPersonCam = GameObject.FindWithTag("ThirdPersonCamera");
        if (this.firstPersonCam == null) this.firstPersonCam = GameObject.FindWithTag("FirstPersonCamera");
        if (this.godModeCam == null) this.godModeCam = GameObject.FindWithTag("GodModeCamera");
    }

    protected override void Start()
    {
        base.Start();
        SetActiveCamera(currentCamera);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Tab)) // Chuyển camera khi nhấn phím Tab
        {
            ToggleCameraMode();
        }
    }

    public void ToggleCameraMode()
    {
        switch (currentCamera)
        {
            case CameraType.thirdPerson:
                SetActiveCamera(CameraType.firstPerson);
                break;
            case CameraType.firstPerson:
                SetActiveCamera(CameraType.godMode);
                break;
            case CameraType.godMode:
                SetActiveCamera(CameraType.thirdPerson);
                break;
        }
    }

    private void SetActiveCamera(CameraType cameraType)
    {
        currentCamera = cameraType;

        thirdPersonCam?.SetActive(cameraType == CameraType.thirdPerson);
        firstPersonCam?.SetActive(cameraType == CameraType.firstPerson);
        godModeCam?.SetActive(cameraType == CameraType.godMode);

        Debug.Log("Switched to: " + cameraType);
    }
}
