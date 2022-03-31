using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : GridItem
{

    //General
    public List<ObjectType> myTypes = new List<ObjectType>();

    //Obstacle
    private Collider2D myCol;
    private SpriteRenderer ren;

    public override void Start()
    {
        myCol = GetComponent<Collider2D>();
        ren = GetComponent<SpriteRenderer>();
        base.Start();

    }
    private void Update()
    {
        ObstacleUpdate();
        PlayerUpdate();
        NoneUpdate();
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
                MoveUp();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDown();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
        }
    }   

    private void NoneUpdate()
    {
        if (myTypes.Contains(ObjectType.None))
        {
            if (myCol.enabled)
            {
                myCol.enabled = false;
            }
            if (ren.enabled)
            {
                //ren.enabled = false;
            }
        }
        else
        {
            if (!ren.enabled)
            {
                ren.enabled = true;
            }
        }
    }

    private void ObstacleUpdate()
    {
        if (myTypes.Contains(ObjectType.Obstacle))
        {
            if (!myCol.enabled)
            {
                myCol.enabled = true;
            }
        }
        else
        {
            if (myCol.enabled)
            {
                myCol.enabled = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // OK so I'm not sure how exactly we're handling movement on the grid
        // Maybe we aren't using collisions, who knows

        if (myTypes.Contains(ObjectType.Killer))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                //Kill the player;
            }
        }
        if (myTypes.Contains(ObjectType.Win))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                // Next level
            }
        }
        if (myTypes.Contains(ObjectType.Pushable))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                // Move
            }
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

