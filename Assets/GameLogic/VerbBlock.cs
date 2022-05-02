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
    private References refr;
    private SpriteRenderer myren;

    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }
    public void Init()
    {
        refr = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<References>();
        isLogicBlock = true;
        pushable = true;
        myren = GetComponent<SpriteRenderer>();
        AssignAppearance();
    }

    public void AssignAppearance()
    {
        // This just sets our sprite to whatever we want
        myren.enabled = true;
        switch (myType)
        {
            case ObjectType.None:
                myren.enabled = false;
                break;
            case ObjectType.Player:
                myren.sprite = refr.vPlayer;
                break;
            case ObjectType.Pushable:
                myren.sprite = refr.vPushable;
                break;
            case ObjectType.Obstacle:
                myren.sprite = refr.vObstacle;
                break;
            case ObjectType.Win:
                myren.sprite = refr.vWin;
                break;
            case ObjectType.Killer:
                myren.sprite = refr.vKill;
                break;

        }
    }

}

