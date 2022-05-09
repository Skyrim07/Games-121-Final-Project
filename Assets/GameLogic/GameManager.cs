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

    private LogicGrid lg;

    private void Start()
    {
        LoadGridItems();
        lg = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
        LevelManager.instance.WinSound();
    }
    public static void LoadGridItems()
    {
        gridItems = GameObject.FindObjectsOfType<GridItem>();
    }
    
    private static void UndoGridItems()
    {
        LevelManager.instance.UndoSound();
        GameManager me = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        LogicGrid lg = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
        foreach (GridItem item in gridItems)
        {
            if (item.TryGetComponent(out BabaObject baba))
            {
                if (baba.dead && baba.diemove == me.currentMove)
                {
                    baba.dead = false;
                    baba.diemove = 0;
                }
            }
            item.Load();
           
        }
        lg.RefreshLogic(false);
    }

    void Update()
    {
        if (pmove == true) // Did someone move?
        {
            currentMove++;

            //Play movement sound
            if(LevelManager.instance != null)
            {
                LevelManager.instance.WalkingSound(); //Hopefully works
            }
            else
            {
                print("no instance");
            }
          

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.instance.ResetSound();
            // We're dead, reset the level to the start position
            currentMove = 0;

            // Tell every grid item on every grid point to reset
            foreach (GridItem g in gridItems)
            {
                if(g.TryGetComponent(out BabaObject baba))
                {
                    baba.dead = false;
                    baba.diemove = 0;
                }
                g.QueueReset();
            }
            StartCoroutine(RefreshDelay());
        }

        IEnumerator RefreshDelay()
        {
            yield return new WaitForSeconds(0.1f);
            lg.RefreshLogic(false);
            yield break;
        }
        if (currentMove < 0)
        {
            currentMove = 0;
        }
        pmove = false; 
    }
}
