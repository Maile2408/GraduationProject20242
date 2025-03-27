using UnityEngine;

public class WorkerMovement : SaiBehaviour
{
    public WorkerCtrl workerCtrl;
    [SerializeField] protected Transform target;
    [SerializeField] protected float moveLimit = 0.7f;

    public bool isMoving { get; private set; }
    public bool isWorking { get; private set; }

    public WorkingType workingType { get; private set; } = WorkingType.chopping;
    public MovingType movingType { get; private set; } = MovingType.walking;

    protected float targetDistance = 0f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (workerCtrl == null) workerCtrl = GetComponent<WorkerCtrl>();
    }

    protected override void FixedUpdate()
    {
        this.Moving();
        this.UpdateAnim();
    }

    public virtual void SetTarget(Transform trans)
    {
        this.target = trans;

        if (this.target == null)
        {
            this.workerCtrl.navMeshAgent.enabled = false;
        }
        else
        {
            this.workerCtrl.navMeshAgent.enabled = true;
            this.IsClose2Target();
        }
    }

    public virtual bool IsClose2Target()
    {
        if (this.target == null) return false;

        Vector3 targetPos = this.target.position;
        targetPos.y = transform.position.y;
        this.targetDistance = Vector3.Distance(transform.position, targetPos);

        return this.targetDistance < this.moveLimit;
    }

    protected virtual void Moving()
    {
        if (this.target == null || this.IsClose2Target())
        {
            this.isMoving = false;
            return;
        }

        this.isMoving = true;
        this.workerCtrl.navMeshAgent.SetDestination(this.target.position);
    }

    public void SetWorkingType(bool working, WorkingType workingType)
    {
        this.isWorking = working;
        this.workingType = workingType;
    }

    public void SetMovingType(bool moving, MovingType movingType)
    {
        this.isMoving = moving;
        this.movingType = movingType;
    }

    public virtual void UpdateAnim()
    {
        var animator = workerCtrl.animator;

        animator.SetBool("isMoving", this.isMoving);
        animator.SetBool("isWorking", this.isWorking);
        animator.SetFloat("workingType", (float)this.workingType);
        animator.SetFloat("movingType", (float)this.movingType);
    }

    public virtual Transform GetTarget() => this.target;
    public virtual float GetTargetDistance() => this.targetDistance;
}
