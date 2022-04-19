using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : GridItem
{
    //General
    public List<ObjectType> myTypes = new List<ObjectType>();
    public BlockAppearance defaultBlock;
    public SpriteRenderer myren;

    public Sprite Baba;
    public Sprite Rock;
    public Sprite Wall;
    public Sprite Flag;
    public Sprite Death;

    public override void Start()
    {
        base.Start();
        myren = GetComponent<SpriteRenderer>();
        AssignAppearance(defaultBlock);
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        PlayerUpdate();
    }

    //Alex: Only need to update this when the type changes
    private void UpdateTypeLogic()
    {
        if(myTypes.Count == 0)
        {
            myTypes.Add(ObjectType.None);
        }
        ObstacleUpdate();
        NoneUpdate();
        PushUpdate();
        WinUpdate();
    }

    public void RefreshType(ObjectType ntype, bool appOverride)
    {
        myTypes.Clear();
        myTypes.Add(ntype);
        UpdateTypeLogic();

        if (appOverride)
        {
            AssignAppearance(defaultBlock);
        }

    }
    private void PlayerUpdate()
    {
        if (myTypes.Contains(ObjectType.Player))
        {
            // Matches Player Input
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
        if (myTypes.Contains(ObjectType.None))
        {
            pushable = false;
            locked = false;
        }
    }

    private void PushUpdate()
    {
        if (myTypes.Contains(ObjectType.Pushable))
        {
            pushable = true;
            locked = false;
        }
    }

    private void WinUpdate()
    {
        if (myTypes.Contains(ObjectType.Win))
        {
            pushable = false;
            locked = false;
        }
    }

    private void ObstacleUpdate()
    {
        if (myTypes.Contains(ObjectType.Obstacle))
        {
            pushable = false;
            locked = true;
        }
    }
    public void AssignAppearance(BlockAppearance input)
    {
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
    Player,
    Obstacle,
    Killer,
    Win,
    Pushable,
    None,
}

public enum BlockAppearance
{
    None,
    Baba,
    Wall,
    Death,
    Flag,
    Rock,

}

