using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounBlock: GridItem
{
    /// <summary>
    ///
    /// This class is on every Noun block ( like WALL, or BABA)
    ///
    /// 
    /// </summary>
    
    public List<BabaObject> myBlock; // List off this nouns effective blocks (can change through logic)

    public BlockAppearance defaultBlock; // What this noun's blocks normally look like

    public ObjectType effect; // The current effect this noun applies


    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
        RefreshTypes();
    }

    // Reset everything to default
    public void RefreshTypes()
    {
        effect = ObjectType.None; // Clear our effects

        foreach (BabaObject baba in myBlock)
        {
            // Reset all objects under our command
            baba.RefreshType(ObjectType.None, true);
        }
        // Remove all blocks from our command
        myBlock.Clear();


        // Find the blocks that should be under our command
        // Loop through grid
        foreach (Point p in gridMaster.grid)
        {
            // Loop through overlapping objects
            foreach (GameObject g in p.localObjects)
            {
                // Is it a BabaObject? (Like a rock, or a wall, or baba)
                if(g.TryGetComponent<BabaObject>(out BabaObject baba))
                {
                    // Is it one of mine?
                    if (baba.defaultBlock.Equals(defaultBlock))
                    {
                        // Yes, add it to our list
                        myBlock.Add(baba);
                    }
                }
            }
        }
        // For all the new additions, reset them too
        // (probably redundant, but whatever)
        foreach (BabaObject baba in myBlock)
        {
            baba.RefreshType(ObjectType.None, false);
        }
    }
}

