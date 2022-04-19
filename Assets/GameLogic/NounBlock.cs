using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounBlock: GridItem
{
    public List<BabaObject> myBlock;
    public BlockAppearance defaultBlock;
    // Start is called before the first frame update
    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
        RefreshTypes();
    }

    public void RefreshTypes()
    {
        foreach (BabaObject baba in myBlock)
        {
            baba.RefreshType(ObjectType.None, true);
        }

        myBlock.Clear();
        foreach (Point p in gridMaster.grid)
        {
            foreach (GameObject g in p.localObjects)
            {
                if(g.TryGetComponent<BabaObject>(out BabaObject baba))
                {
                    if (baba.defaultBlock.Equals(defaultBlock))
                    {
                        myBlock.Add(baba);
                        baba.AssignAppearance(defaultBlock);
                    }
                }
            }
        }

        foreach (BabaObject baba in myBlock)
        {
            baba.RefreshType(ObjectType.None, false);
        }
    }
}

