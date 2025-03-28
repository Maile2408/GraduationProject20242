using UnityEngine;

public class GrainLevel : BuildLevel
{
    [SerializeField] protected bool autoGrow = false;
    [SerializeField] protected bool isMaxLevel = false;

    [Header("Growth Timer")]
    [SerializeField] protected float growTimer = 0f;
    [SerializeField] protected float growDelay = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!autoGrow || this.isMaxLevel) return;

        this.growTimer += Time.fixedDeltaTime;
        if (this.growTimer >= this.growDelay)
        {
            this.growTimer = 0f;
            this.ShowNextBuild();
        }

        this.UpdateMaxLevel();
    }

    protected override void ShowNextBuild()
    {
        if (this.currentLevel >= this.levels.Count - 1) return;

        this.currentLevel++;
        this.ShowBuilding();
    }


    protected virtual void UpdateMaxLevel()
    {
        this.isMaxLevel = (this.currentLevel >= this.levels.Count - 1);
    }

    public virtual void StartGrow()
    {
        this.autoGrow = true;
        this.growTimer = 0f;
    }

    public virtual void StopGrow()
    {
        this.autoGrow = false;
    }

    public virtual bool CanHarvest()
    {
        return this.isMaxLevel;
    }

    public virtual void ResetGrowth()
    {
        this.HideAllBuild();
        this.currentLevel = 0;
        this.growTimer = 0f;
        this.isMaxLevel = false;
        this.autoGrow = false;
        this.ShowBuilding();
    }
}
