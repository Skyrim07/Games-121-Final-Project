using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : GridItem
{
    // This class handles most object behaviors and appearance stuff


    //General variables

    public ObjectType babaType; //Current object behavior
    public BlockAppearance defaultBlock; // What kind of block we are by default
    public BlockAppearance currentApp; // What we currently look like due to in-game hijinks
    [HideInInspector] public SpriteRenderer myren; // Reference to renderer

    // References to things we might look like
    public Sprite Baba;
    public Sprite Rock;
    public Sprite Wall;
    public Sprite Flag;
    public Sprite Death;

    public override void Start()
    {
        base.Start();
        myren = GetComponent<SpriteRenderer>();
        AssignAppearance(defaultBlock); // Set up our sprite
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        PlayerUpdate(); // Check for player movement
    }

    public void UpdateTypeLogic()
    {
        // This update our block behaviors every time our related logic changes
        ObstacleUpdate();
        NoneUpdate();
        PushUpdate();
        WinUpdate();
    }

    public void RefreshType(ObjectType ntype, bool appOverride)
    {
        // This lets other classes update us on what to look like
        // And how to behave

        babaType = ntype; // Grab our new behavior

        UpdateTypeLogic(); // Refresh behavior

        if (appOverride) // appOverride just lets us know if we're reseting to default
        {
            AssignAppearance(defaultBlock); //Reset our appearance
        }

    }

    private void PlayerUpdate()
    {
        if (babaType == ObjectType.Player)
        {
            // Matches Player Input

            // MoveIndex() is kept on the GridItem class, which we inherit from
            // The first param is our movement index, kind of our direction
            // The second param is letting the function know we want to actually move the block
            // as opposed to just calculating whether or not we can
            // The third param lets the function know the depth of recursion (see GridItem.cs for more)

            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveIndex(gridMaster.gridLength, true, 9); 
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveIndex(-1, true, 9);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                MoveIndex(-gridMaster.gridLength, true, 9);
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                MoveIndex(1, true, 9);
            }
        }
    }   
 
    private void NoneUpdate()
    {
        if (babaType == ObjectType.None)
        {
            // If we have no logic statements telling us what to do,
            // Make us intangible
            pushable = false;
            locked = false;
        }
    }

    private void PushUpdate()
    {
        if (babaType == ObjectType.Pushable)
        {
            // Make us pushable
            pushable = true;
            locked = false;
        }
    }

    private void WinUpdate()
    {
        if (babaType == ObjectType.Win)
        {
            // Make us intangible
            pushable = false;
            locked = false;
        }
    }

    private void ObstacleUpdate()
    {
        if (babaType == ObjectType.Obstacle)
        {
            // Make us impassable
            pushable = false;
            locked = true;
        }
    }
    public void AssignAppearance(BlockAppearance input)
    {
        // This just sets our sprite to whatever we want
        myren.enabled = true;
        currentApp = input;
        myren.enabled = true; 
        switch (input)
        {
            case BlockAppearance.None:
                myren.enabled = false;
                break;
            case BlockAppearance.Baba:
                myren.sprite = Baba;
                break;
            case BlockAppearance.Rock:
                myren.sprite = Rock;
                break;
            case BlockAppearance.Wall:
                myren.sprite = Wall;
                break;
            case BlockAppearance.Flag:
                myren.sprite = Flag;
                break;
            case BlockAppearance.Death:
                myren.sprite = Death;
                break;

        }
    }
}

public enum ObjectType
{
    // The different kind of block behaviors
    Player, // YOU
    Obstacle, // STOP
    Killer, // DEATH
    Win, // WIN
    Pushable, // PUSH
    None, // No logic associated
}

public enum BlockAppearance
{
    None, //Unused
    Baba, 
    Wall,
    Death,
    Flag,
    Rock,

}

