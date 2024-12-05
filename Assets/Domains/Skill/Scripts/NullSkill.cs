using UnityEngine;

public class NullSkill : Skill
{
    public override void UseSkill()
    {
        // Do nothing
    }

    public override float GetCost()
    {
        return 0;
    }

    public override float GetCooldownTime()
    {
        return 0;
    }
}