using UnityEngine;

public class WorkerMovement : SaiBehaviour
{
    public WorkerCtrl workerCtrl;
    [SerializeField] protected Transform target;
    [SerializeField] protected float moveLimit = 0.7f;
    public bool isMoving = false;
    public bool isWorking = false;
    public WorkingType workingType = WorkingType.chopping;
    public MovingType movingType = MovingType.walking;

    protected float targetDistance = 0f;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (workerCtrl == null) workerCtrl = GetComponent<WorkerCtrl>();
    }

    protected override void FixedUpdate()
    {
        this.HandleMovement();
        this.UpdateAnimator();
    }

    public virtual void SetTarget(Transform trans)
    {
        this.target = trans;

        if(this.target == null)
        {
            this.workerCtrl.navMeshAgent.enabled = false;
        }
        else
        {
            this.workerCtrl.navMeshAgent.enabled = true;
            this.IsClose2Target();
        }
    }

    public virtual void StartWorking(WorkingType workType)
    {
        this.isWorking = true;
        this.workingType = workType;

        this.isMoving = false;
        workerCtrl.navMeshAgent.enabled = false;

        workerCtrl.tools.UpdateTool(MovingType.walking, workType);
    }

    public virtual void StopWorking()
    {
        this.isWorking = false;
        workerCtrl.tools.ClearTool();
    }

    public virtual bool IsClose2Target()
    {
        if (this.target == null) return false;

        Vector3 targetPos = this.target.position;
        targetPos.y = transform.position.y;
        this.targetDistance = Vector3.Distance(transform.position, targetPos);

        return this.targetDistance < moveLimit;
    }

    protected virtual void HandleMovement()
    {
        if (!this.isMoving || this.target == null)
        {
            this.isMoving = false;
            return;
        }

        if (this.IsClose2Target())
        {
            this.isMoving = false;
            workerCtrl.navMeshAgent.ResetPath();
        }
        else
        {
            workerCtrl.navMeshAgent.SetDestination(target.position);
        }
    }

    protected virtual void UpdateAnimator()
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
