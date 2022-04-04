using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : GridItem
{

    //General
    public List<ObjectType> myTypes = new List<ObjectType>();

    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        ObstacleUpdate();
        PlayerUpdate();
        NoneUpdate();
        PushUpdate();
    }
    public void RefreshType()
    {
        myTypes.Clear();
        myTypes.Add(ObjectType.None);
    }
    private void PlayerUpdate()
    {
        if (myTypes.Contains(ObjectType.Player))
        {
            // Matches Player Input
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveIndex(gridMaster.gridLength, true);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveIndex(-1, true);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                MoveIndex(-gridMaster.gridLength, true);
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                MoveIndex(1, true);
            }
            GameManager.SaveGameState();
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

    private void ObstacleUpdate()
    {
        if (myTypes.Contains(ObjectType.Obstacle))
        {
            pushable = false;
            locked = true;
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

