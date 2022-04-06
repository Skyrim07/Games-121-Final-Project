using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static List<GameState> savedStates = new List<GameState>();
    public int currentMove;
    private bool consec = false;
    public bool pmove = false;
    void Start()
    {
        //SaveGameState();
    }

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
 

    public static void SaveGameState()
    {
        savedStates.Add(GameState.GetCurrentState());
    }


    public static void UndoMove()
    {
        if(savedStates.Count <= 1)
        {
            Debug.Log("No moves to undo");
        }
        else
        {
            savedStates[savedStates.Count - 2].LoadGameState();
            savedStates.RemoveAt(savedStates.Count - 1);
        }

    }




}
