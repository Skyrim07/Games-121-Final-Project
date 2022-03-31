using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerbBlock: GridItem
{
    public ObjectType myType;

    // Start is called before the first frame update
    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }
}

