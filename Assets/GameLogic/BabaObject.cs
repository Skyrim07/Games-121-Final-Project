using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : MonoBehaviour
{

    //General
    public List<ObjectType> myTypes;

    //Obstacle
    private Collider2D myCol;
    private SpriteRenderer ren;

    private void Start()
    {
        myCol = GetComponent<Collider2D>();
        ren = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        ObstacleUpdate();
        PlayerUpdate();
        NoneUpdate();
    }
    private void PlayerUpdate()
    {
        if (myTypes.Contains(ObjectType.Player))
        {
            // Matches Player Input
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
                ren.enabled = false;
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

