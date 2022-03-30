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

    }
    private void Update()
    {
        PlayerUpdate();
    }
    private void PlayerUpdate()
    {
        if (myType.Equals(ObjectType.Player))
        {
            // Input
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // OK so I'm not sure how exactly we're handling movement on the grid
        // Maybe we aren't using collisions, who knows
        // I'm going to wait until we've nailed down the movement to start messing with these

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

