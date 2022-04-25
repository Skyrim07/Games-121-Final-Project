using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This class I hijacked, it now only stores the current global move counter

    [SerializeField]
    // this has been deprecated
    public static List<GameState> savedStates = new List<GameState>();

    public int currentMove; // Global move counter

    public bool pmove = false; 


    void Update()
    {
        if (pmove == true) // Did someone move?
        {
            currentMove++;
        }

        if (Input.GetKeyDown(KeyCode.Z)) // Did we undo?
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
