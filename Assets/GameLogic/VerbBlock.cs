using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerbBlock: GridItem
{
    // This class is on every Verb block (Like PUSH, or STOP)

    // It currently doesn't have much additional logic
    // It stores this ObjectType
    public ObjectType myType;
    // Which lets the IsBlock class know what kind of effect to apply to related blocks

    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }

}

