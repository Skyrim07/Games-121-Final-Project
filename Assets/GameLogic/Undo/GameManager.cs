using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static List<GameState> savedStates = new List<GameState>();

    void Start()
    {
        SaveGameState();
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            UndoMove();
        }
        // Matches Player Input
        if (Input.GetKeyDown(KeyCode.W))
        {
            SaveGameState();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveGameState();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGameState();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveGameState();
        }
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
