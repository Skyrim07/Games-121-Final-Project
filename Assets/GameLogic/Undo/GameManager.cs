using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This class I hijacked, it now only stores the current global move counter

    [SerializeField]
    // this has been deprecated
    public static List<GameState> savedStates = new List<GameState>();

    public static GridItem[] gridItems;

    public int currentMove; // Global move counter

    public bool pmove = false;

    private float zTimer = 0, zStartTimer=0, zInterval =0.08f;

    private void Start()
    {
        LoadGridItems();
    }
    public static void LoadGridItems()
    {
        gridItems = GameObject.FindObjectsOfType<GridItem>();
    }
    private static void UndoGridItems()
    {
        foreach(GridItem item in gridItems)
        {
            item.Load();
        }
    }

    void Update()
    {
        if (pmove == true) // Did someone move?
        {
            currentMove++;
        }

        if (Input.GetKeyDown(KeyCode.Z)) // Did we undo?
        {
            zTimer = 0;
            zStartTimer = 0;
            currentMove -= 1;
            if (currentMove < 0)
            {
                currentMove = 0;
            }

            UndoGridItems();
        }

        if (Input.GetKey(KeyCode.Z))
        {
            zStartTimer += Time.deltaTime;
            if (zStartTimer > 0.5f)
            {
                zTimer += Time.deltaTime;
            }
            if(zTimer > zInterval)
            {
                zTimer = 0;
                currentMove -= 1;

                if (currentMove < 0)
                {
                    currentMove = 0;
                }

                UndoGridItems();
            }
        }

        if (currentMove < 0)
        {
            currentMove = 0;
        }
        pmove = false; 
    }
}
