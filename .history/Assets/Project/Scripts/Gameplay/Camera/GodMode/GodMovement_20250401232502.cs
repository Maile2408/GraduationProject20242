using UnityEngine;

public class GodMovement : SaiBehaviour
{
    public GodModeCtrl godModeCtrl;
    public float speed = 27f;
    public float speedRotate = 0.1f;
    public bool speedShift = false;
    public float minY = 4f;
    public float maxY = 70f;

    public Vector3 camRotation = Vector3.zero;   
    public Vector3 camMovement = Vector3.zero;   
    public Vector3 camView = new Vector3(45f, 0f, 0f); 

    protected override void Update()
    {
        base.Update();
        this.Moving();
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

    protected virtual void Moving()
    {
        float moveSpeed = this.speed;
        if (this.speedShift) moveSpeed += this.speed * 2;

        Vector3 movement = this.camMovement;
        movement.x *= moveSpeed;
        movement.z *= moveSpeed;
        movement.y *= moveSpeed * 7f;

        transform.Translate(movement * Time.deltaTime, Space.Self);

        Vector3 newPos = transform.position;
        newPos.y = Mathf.Clamp(newPos.y, this.minY, this.maxY);
        transform.position = newPos;

        this.camRotation *= this.speedRotate;
        camView += new Vector3(0, this.camRotation.y, 0);

        camView.x = Mathf.Clamp(camView.x, 20f, 80f); 

        this.godModeCtrl._camera.transform.rotation = Quaternion.Euler(camView);

        this.camMovement = Vector3.zero;
        this.camRotation = Vector3.zero;
    }
}
