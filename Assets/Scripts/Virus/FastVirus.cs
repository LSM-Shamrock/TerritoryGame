using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastVirus : Virus
{
    private float boost;
    private float BoostAmount => Mathf.Lerp(1f, 4f, Mathf.Clamp01(boost));
    public override float MoveSpeed => base.MoveSpeed * BoostAmount;

    protected override void FlipDir()
    {
        boost = 1f;
        base.FlipDir();
    }

    protected override void Update()
    {
        base.Update();
        boost -= Time.deltaTime;
        boost = Mathf.Max(boost, 0f);
    }
}
