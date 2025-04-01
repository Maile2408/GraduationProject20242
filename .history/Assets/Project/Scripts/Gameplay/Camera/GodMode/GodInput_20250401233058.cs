using UnityEngine;

public class GodInput : SaiBehaviour
{
    public GodModeCtrl godModeCtrl;

    private bool isRotating = false;
    private Vector3 touchStart = Vector3.zero;
    private Vector3 inputRotation = Vector3.zero;

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
            PCRotation();
        }

        this.godModeCtrl.godMovement.camRotation.y = inputRotation.x;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGodModeCtrl();
    }

    protected virtual void LoadGodModeCtrl()
    {
        if (this.godModeCtrl != null) return;
        this.godModeCtrl = GetComponent<GodModeCtrl>();
        Debug.Log(transform.name + ": LoadGodModeCtrl", gameObject);
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

    protected virtual void PCRotation()
    {
        isRotating = Input.GetKey(KeyCode.Mouse1);
        if (Input.GetKeyDown(KeyCode.Mouse1)) touchStart = Input.mousePosition;

        if (isRotating)
        {
            Vector3 delta = Input.mousePosition - touchStart;
            inputRotation = new Vector3(0, delta.x, 0);
            touchStart = Input.mousePosition;
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

            if (touch.position.x < Screen.width / 2f)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;
                    x = -delta.x * 0.01f;
                    z = -delta.y * 0.01f;
                }
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prev0 = touch0.position - touch0.deltaPosition;
            Vector2 prev1 = touch1.position - touch1.deltaPosition;

            float prevMag = (prev0 - prev1).magnitude;
            float currMag = (touch0.position - touch1.position).magnitude;

            float deltaMag = currMag - prevMag;

            y = -deltaMag * 0.01f; // zoom
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
                {
                    isRotating = true;
                    touchStart = touch.position;
                }

                if (touch.phase == TouchPhase.Moved && isRotating)
                {
                    Vector2 delta = touch.position - (Vector2)touchStart;
                    inputRotation = new Vector3(0, delta.x * 0.2f, 0);
                    touchStart = touch.position;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isRotating = false;
                    inputRotation = Vector3.zero;
                }
            }
        }
    }
    #endregion
}
