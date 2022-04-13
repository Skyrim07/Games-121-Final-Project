using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : GridItem
{
    public ObjectType myType;

    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }
}
