using UnityEngine;

public class Barrier : Skill, ICancellable
{
    public override void UseSkill()
    {
        // Do nothing
    }

    public override float GetCost()
    {
        return 0;
    }

    public void Cancel()
    {
        // Do nothing
    }
}