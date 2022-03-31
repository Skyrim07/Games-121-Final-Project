using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounBlock: GridItem
{
    public List<BabaObject> myBlock;

    // Start is called before the first frame update
    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }

    public void RefreshTypes()
    {
        foreach(BabaObject baba in myBlock)
        {
            baba.RefreshType();
        }
    }
}

