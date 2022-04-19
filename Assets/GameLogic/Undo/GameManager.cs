using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static List<GameState> savedStates = new List<GameState>();
    public int currentMove;

    public bool pmove = false;


    void Update()
    {
        if (pmove == true)
        {
            currentMove++;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentMove -= 1;
        }
        if (currentMove < 0)
        {
            currentMove = 0;
        }
        pmove = false;
    }
}
