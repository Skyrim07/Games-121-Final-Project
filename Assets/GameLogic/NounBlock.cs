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

    [HideInInspector]
    public BlockAppearance currentApp; //What this noun's blocks should now look like

    [HideInInspector]
    public ObjectType effect; // The current effect this noun applies

    private SpriteRenderer myren;
    private References refr;
    public override void Start()
    {
        base.Start();
        Init();
        RefreshTypes(); // Just to be safe
    }

    public void Init()
    {
        refr = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<References>();
        isLogicBlock = true;
        pushable = true;
        myren = GetComponent<SpriteRenderer>();
        EditorRefresh();
    }

    public void AssignAppearance()
    {
        // This just sets our sprite to whatever we want
        myren.enabled = true;
        switch (defaultBlock)
        {
            case BlockAppearance.None:
                myren.enabled = false;
                break;
            case BlockAppearance.Baba:
                myren.sprite = refr.nBaba;
                break;
            case BlockAppearance.Rock:
                myren.sprite = refr.nRock;
                break;
            case BlockAppearance.Wall:
                myren.sprite = refr.nWall;
                break;
            case BlockAppearance.Flag:
                myren.sprite = refr.nFlag;
                break;
            case BlockAppearance.Death:
                myren.sprite = refr.nSkull;
                break;

        }
    }
    // Reset everything to default
    // But don't touch anything that's initialized at start
    public void EditorRefresh()
    {
        AssignAppearance();
        currentApp = defaultBlock; // Clear our effects
        effect = ObjectType.None; // Clear our effects
    }

    // Reset everything to default
    public void RefreshTypes()
    {
        AssignAppearance();
        currentApp = defaultBlock; // Clear our effects
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

