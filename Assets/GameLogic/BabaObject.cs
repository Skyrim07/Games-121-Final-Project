using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabaObject : MonoBehaviour
{

    //General
    public ObjectType myType;

    //Obstacle
    private Collider2D myCol;

    private void Start()
    {
        //myType = ObjectType.Obstacle;
    }
    private void Update()
    {
        ObstacleUpdate();
        PlayerUpdate();
    }
    private void PlayerUpdate()
    {
        if (myType.Equals(ObjectType.Player))
        {
            // Input
        }
    }

    private void ObstacleUpdate()
    {
        if (myType.Equals(ObjectType.Obstacle))
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

        if (myType.Equals(ObjectType.Killer))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                //Kill the player;
            }
        }
        if (myType.Equals(ObjectType.Win))
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                // Next level
            }
        }
        if (myType.Equals(ObjectType.Pushable))
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
}

