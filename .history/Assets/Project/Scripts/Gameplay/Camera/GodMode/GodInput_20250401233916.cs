using UnityEngine;

public class GodInput : SaiBehaviour
{
    public GodModeCtrl godModeCtrl;

    private Vector3 inputRotation = Vector3.zero;
    private Vector3 mouseReference = Vector3.zero;

    protected override void Update()
    {
        if (Application.isMobilePlatform)
        {
            TouchInputHandle();
            TouchRotation();
        }
        else
        {
            PCInputHandle();
            MouseRotation(); 
        }

        this.godModeCtrl.godMovement.camRotation.y = inputRotation.x;

        //this.ChoosePlace2Build();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGetModeCtrl();
    }

    protected virtual void LoadGetModeCtrl()
    {
        if (this.godModeCtrl != null) return;
        this.godModeCtrl = GetComponent<GodModeCtrl>();
        Debug.Log(transform.name + ": LoadGetModeCtrl", gameObject);
    }

    #region PC Input
    protected virtual void PCInputHandle()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.mouseScrollDelta.y * -1;
        bool leftShift = Input.GetKey(KeyCode.LeftShift);

        this.godModeCtrl.godMovement.camMovement = new Vector3(x, y, z);
        this.godModeCtrl.godMovement.speedShift = leftShift;
    }

    protected virtual void MouseRotation()
    {
        bool isMouseRotating = Input.GetKey(KeyCode.Mouse1);

        if (Input.GetKeyDown(KeyCode.Mouse1))
            mouseReference = Input.mousePosition;

        if (isMouseRotating)
        {
            Vector3 delta = Input.mousePosition - mouseReference;
            inputRotation = new Vector3(0, delta.x, 0);
            mouseReference = Input.mousePosition;
        }
        else
        {
            inputRotation = Vector3.zero;
        }
    }
    #endregion

    #region Mobile Touch Input
    protected virtual void TouchInputHandle()
    {
        float x = 0f, z = 0f, y = 0f;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2f && touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                x = -delta.x * 0.01f;
                z = -delta.y * 0.01f;
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float prevMag = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currMag = (touch0.position - touch1.position).magnitude;

            y = -(currMag - prevMag) * 0.01f;
        }

        this.godModeCtrl.godMovement.camMovement = new Vector3(x, y, z);
    }

    protected virtual void TouchRotation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x >= Screen.width / 2f)
            {
                if (touch.phase == TouchPhase.Began)
                    mouseReference = touch.position;

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.position - (Vector2)mouseReference;
                    inputRotation = new Vector3(0, delta.x * 0.2f, 0);
                    mouseReference = touch.position;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    inputRotation = Vector3.zero;
            }
        }
    }
    #endregion

    /*protected virtual void ChoosePlace2Build()
    {
        if (!BuildManager.instance.isBuilding) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;
#else
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Input.GetTouch(0).position.x < Screen.width / 2f)
                return;
        }
        else return;
#endif

        BuildManager.instance.CurrentBuildPlace();
    }*/
}
