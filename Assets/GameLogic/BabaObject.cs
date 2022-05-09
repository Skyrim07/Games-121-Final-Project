using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public class BabaObject : GridItem
{
    // This class handles most object behaviors and appearance stuff


    //General variables

    [HideInInspector]
    public ObjectType babaType; //Current object behavior


    public BlockAppearance defaultBlock; // What kind of block we are by default

    [HideInInspector]
    public BlockAppearance currentApp; // What we currently look like due to in-game hijinks

    [HideInInspector] public SpriteRenderer myren; // Reference to renderer

    // References to things we might look like
    private References refr;
    private Sprite Baba;
    private Sprite Rock;
    private Sprite Wall;
    private Sprite Flag;
    private Sprite Death;

    private Animator anim;
    private Vector3 oScale;


    public bool dead;
    public int diemove;

    private void Awake()
    {
        babaType = ObjectType.None;
    }

    public void Init()
    {
        myren = GetComponent<SpriteRenderer>();
        AssignAppearance(defaultBlock);
    }
    public override void Start()
    {
        base.Start();
        myren = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oScale = transform.localScale;
        AssignAppearance(defaultBlock); // Set up our sprite
    }
    public void LateUpdate()
    {
        PlayerUpdate(); // Check for player movement
    }

    public override void DoMove(int moveIndex)
    {
        if (currentApp == BlockAppearance.Baba)
        {
            GameObject fx = Instantiate(References.instance.particleBaba, transform.position, Quaternion.identity);
            Vector2 dir;
            if (moveIndex == 1)
            {
                dir = Vector2.left;
            }
            else if (moveIndex == -1)
            {
                dir = Vector2.right;
            }
            else if (moveIndex > 0)
            {
                dir = Vector2.down;
            }
            else
            {
                dir = Vector2.up;
            }
            CommonUtils.StartProcedure(SKCurve.LinearIn, 1f, (f) =>
            {
                fx.transform.Translate(dir * Time.deltaTime * 1.5f);
            });
            CommonUtils.InvokeAction(2f, () =>
            {
                Destroy(fx);
            });
            base.DoMove(moveIndex);
            PlayerMoveAnim(moveIndex);
            if (moveIndex == -1) //left
            {
                transform.localScale = new Vector3(-oScale.x, oScale.y, oScale.z);
            }
            else if (moveIndex == 1)
            {
                transform.localScale = oScale;
            }
        }
        else
        {
            base.DoMove(moveIndex);
        }
    }

    private void PlayerMoveAnim(int moveIndex)
    {

        if (Mathf.Abs(moveIndex) == 1)
        {
            anim.Play("BabaWalk", 0, 0);
        }
        else
        {
            if (moveIndex > 0)
            {
                anim.Play("BabaWalkUp", 0, 0);
            }
            else
            {
                anim.Play("BabaWalkDown", 0, 0);
            }
        }
    }
    public void UpdateTypeLogic()
    {
        // This update our block behaviors every time our related logic changes
        if (mark)
        {
            return;
        }
            
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

        if (appOverride && !dead) // appOverride just lets us know if we're reseting to default
        {
            AssignAppearance(defaultBlock); //Reset our appearance
        }

    }

    private void PlayerUpdate()
    {
        if (babaType == ObjectType.Player && !dead)
        {
            // Matches Player Input

            // MoveIndex() is kept on the GridItem class, which we inherit from
            // The first param is our movement index, kind of our direction
            // The second param is letting the function know we want to actually move the block
            // as opposed to just calculating whether or not we can
            // The third param lets the function know the depth of recursion (see GridItem.cs for more)

            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveIndex(gridMaster.gridLength, true, 15); 
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveIndex(-1, true, 15);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                MoveIndex(-gridMaster.gridLength, true, 15);
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                MoveIndex(1, true, 15);
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
        refr = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<References>();
        // This just sets our sprite to whatever we want
        currentApp = input;
        myren.enabled = true; 
        switch (input)
        {
            case BlockAppearance.None:
                myren.enabled = false;
                break;
            case BlockAppearance.Baba:
                myren.sprite = refr.Baba;
                break;
            case BlockAppearance.Rock:
                myren.sprite = refr.Rock;
                break;
            case BlockAppearance.Wall:
                myren.sprite = refr.Wall;
                break;
            case BlockAppearance.Flag:
                myren.sprite = refr.Flag;
                break;
            case BlockAppearance.Death:
                myren.sprite = refr.Death;
                break;
            case BlockAppearance.Bush:
                myren.sprite = refr.Bush;
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
    Bush,

}

