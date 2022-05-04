using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    // This struct stores information about a move for undo-purposes

    public int movenum; // The move's order in the game (first, second... last)
    public int index; // Where this square was on that move

    // Constructor
    public Move(int currentmove, int currentindex)
    {
        movenum = currentmove;
        index = currentindex;
    }

}

//This is the class that handles all movement on the grid.
public class GridItem : MonoBehaviour
{
    [HideInInspector] public LogicGrid gridMaster;
    [HideInInspector] public LevelManager levelManager;

    public int ourGridIndex; // Our current position on the grid

    public bool pushable;    // Written to by BabaObject for movement calculations
    public bool locked;      // Written to by BabaObject for movement calculations

    [HideInInspector] public bool isLogicBlock; // This is used to detect when we physically change the logic

    public List<Move> Moves = new List<Move>(); //Stores this block's move history, used in loading

    public GameManager gamemanager;

    [HideInInspector]
    public bool mark;

    public virtual void Start()
    {
        //References
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        gridMaster = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();

        // Local Grid Setup stuff
        ourGridIndex = gridMaster.NearestPoint(transform.position); // Snaps position to grid
        gridMaster.Register(ourGridIndex, gameObject); // Tells the LogicGrid to put us on the grid
        transform.position = gridMaster.grid[ourGridIndex].pos; // Adjusts our position
        Save();

        if(GameObject.FindGameObjectWithTag("LevelManager") != null)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }
            
    }

    public void Save()
    {
        // Saving stores this block's current position in the list
        Moves.Add(new Move(gamemanager.currentMove, ourGridIndex));
    }

    
    public virtual void LateUpdate() //Late to let GameManager class deincrement current save
    {

    }

    public void Load() 
    {
        // Loading is handled here, seperately for each block on the grid
        // We want to: 
        // A. Check if we CAN load
        // B. Check if we need to load
        // C. Adjust movelist and localObjects to prevent ghost data
        // D. Adjust our position and all the data relying on it (index, grid, localobjs)
        // E. Refresh logic if we're a logic block

        // Check if we CAN load
        if (Moves.Count > 1 && !mark)
        {
            // Check if we need to load
            if (gamemanager.currentMove == Moves[Moves.Count - 1].movenum)
            {
                // Adjust movelist and localObjects to prevent ghost data
                Moves.RemoveAt(Moves.Count - 1);
                if (gridMaster.grid[ourGridIndex].localObjects.Contains(gameObject))
                {
                    gridMaster.grid[ourGridIndex].localObjects.Remove(gameObject);
                }

                // Adjust our position and all the data relying on it (index, grid, localobjs)
                ourGridIndex = Moves[Moves.Count - 1].index;
                transform.position = gridMaster.grid[ourGridIndex].pos;
                gridMaster.grid[ourGridIndex].localObjects.Add(gameObject);

                // Refresh logic if we're a logic block
                if (isLogicBlock)
                {
                    StartCoroutine(WaitRefresh());
                }
            }

        }


    }

    private IEnumerator WaitRefresh()
    {
        // This is to make sure all logic stuff is handled after movement
        yield return new WaitForEndOfFrame();
        gridMaster.RefreshLogic(false);
        yield break;
    }

    // Coroutine Wrapper function
    // Lets us call MarkDead() from outside this instance
    public void QueueReset()
    {
        StartCoroutine(MarkDead());
    }

    //This coroutine ensures that we don't modify
    // the p.localObjects while we still need them in DoMove()
    IEnumerator MarkDead()
    {
        yield return new WaitForFixedUpdate(); //Delays action one frame
        Die();
        yield break;
    }

    // This resets every block to the starting position
    public void Die()
    {
        // Prevents redundant loading by only resetting if it has moved
        if(Moves.Count > 1)
        {
            gamemanager.currentMove = 0;
            Move firstMove = Moves[0]; // Gets us a reference to the start position
            Moves.Clear(); // Clears local move history
            Moves.Add(firstMove); // Gives the move history the start position
            Moves.Add(firstMove); // Gives the load function data to unload
            Load();
        }
    }

    public virtual void DoMove(int moveIndex)
    {
        // This function handles all the actual movement on the grid

        gamemanager.pmove = true; // Lets the GameManager class know it needs to save this tick

        // Makes sure we know where we are
        ourGridIndex = gridMaster.NearestPoint(transform.position);

        // Check to make sure we don't go out of bounds
        if ((ourGridIndex + moveIndex) > 0 && (ourGridIndex + moveIndex) < gridMaster.grid.Length && !mark)
        {
            // Possibly redundant check to make sure our destination is valid
            if (!gridMaster.grid[ourGridIndex + moveIndex].Equals(null))
            {
                // Grid points can have multiple inhabitands, so we're going to loop through all of them
                foreach (var item in gridMaster.grid[ourGridIndex].localObjects)
                {
                    // Check if the item is a BabaObject (like a rock, or a wall, NOT a logic block)
                    if (item.TryGetComponent<BabaObject>(out BabaObject bbj))
                    {
                        // Check to see if we're player controlled
                        if (bbj.babaType == ObjectType.Player)
                        {
                            // Loop through all the objects on our destination square
                            foreach (var item2 in gridMaster.grid[ourGridIndex + moveIndex].localObjects)
                            {
                                // Check if its a Baba Object
                                if (item2.TryGetComponent<BabaObject>(out BabaObject bbj2))
                                {
                                    // Are we dead?
                                    if (bbj2.babaType == ObjectType.Killer)
                                    {
                                        // We're dead, reset the level to the start position
                                        gamemanager.currentMove = 0;

                                        // Tell every grid item on every grid point to reset
                                        foreach (Point p in gridMaster.grid)
                                        {
                                            foreach (GameObject g in p.localObjects)
                                            {
                                                g.GetComponent<GridItem>().QueueReset();
                                            }
                                        }
                                     
                                        return; // Don't need to execute the move anymore

                                    }
                                    // Did we win?
                                    if (bbj2.babaType == ObjectType.Win)
                                    {
                                        Debug.Log("Win");
                                        levelManager.LoadNextLevel();
                                    }
                                }
                            }
                        }
                    }
                }

                // Okay, it's time to move

                // Remove ourselves from the current point
                if (gridMaster.grid[ourGridIndex].localObjects.Contains(gameObject))
                {
                    gridMaster.grid[ourGridIndex].localObjects.Remove(gameObject);
                }

                // Add ourselves to the next point
                gridMaster.grid[ourGridIndex + moveIndex].localObjects.Add(gameObject);
                ourGridIndex += moveIndex; // Update position index
                transform.position = gridMaster.grid[ourGridIndex].pos; // Move

                // Save this move
                Save();

                // Check to see if the logic has changed
                if (isLogicBlock)
                {
                    // Let the game know that we need to refresh the logic next frame
                    StartCoroutine(WaitRefresh());
                }
            }
        }
    }

    // Check if a given move is valid.
    public bool MoveIndex(int moveIndex, bool doIt, int depth)
    {
        // bool doIt tells the function to actually execute the move
        // if it's false, the function is simply calculating the validity, not performing the move

        // In some cases, we can't be sure whether or not a move is valid with the info we have
        // An example would be when a player is pushing a long line 
        // of pushable blocks with an unpushable block on the end
        // In order to see if the move is valid, we need check every
        // block in the line and see if it's pushable.

        // We do this by recursively calling this function down the line
        // checking the validity of each block.

        // In order to stop the search going on infinitely, we have a depth value
        // Each time we call the function within itself, we lower the depth
        // If the depth ever hits 0, we stop our search
        // Our default depth is 9. If we want to be able to push more blocks,
        // we would up this value. 


        // Hopefully redundant safety measure
        ourGridIndex = gridMaster.NearestPoint(transform.position);

        // Depth check
        if (depth < 1)
        {
            return false;
        }
        // Check that our target square isn't ourself
        if (moveIndex != 0)
        {
            // Check that our target is in bounds
            if ((ourGridIndex + moveIndex > 0) && (ourGridIndex + moveIndex < gridMaster.gridLength * gridMaster.gridLength))
            {
                // Does our target square already have something on it?
                if (gridMaster.grid[ourGridIndex + moveIndex].localObjects.Count > 0)
                {
                    // Create a list to store all the pushable items on that square
                    List<GridItem> move = new List<GridItem>();

                    // For each item on the target square:
                    foreach (GameObject obj in gridMaster.grid[ourGridIndex + moveIndex].localObjects)
                    {
                        // If the item is unpushable, we can't move onto this square, and return false
                        if (obj.GetComponent<GridItem>().locked)
                        {
                            return false;
                        }
                        // If it's pushable, we add it to the list
                        if (obj.GetComponent<GridItem>().pushable)
                        {
                            move.Add(obj.GetComponent<GridItem>());
                        }

                        // Is it a player?
                        if (obj.TryGetComponent<BabaObject>(out BabaObject baba))
                        {
                            if(baba.babaType == ObjectType.Player)
                            {
                                if(baba.MoveIndex(moveIndex, false, depth - 1))
                                {
                                    // It's a player, and it can move, so we ignore it here
                                    // It will move itself later
                                }
                                else
                                {
                                    // It's a player and it can't move
                                    return false;
                                }
                            }
                          
                        }
                    }
                
                    // If we have items to evaluate
                    if (move.Count > 0)
                    {
                        // Can our pushable item move over for us?
                        if (move[0].MoveIndex(moveIndex, doIt, depth - 1))
                        {
                            // If it can, move all those pushable items we found
                            foreach (GridItem item in move)
                            {
                                if (!item.Equals(move[0]) && doIt)
                                {
                                    item.DoMove(moveIndex);
                                }
                            }
                            // Then move ourselves
                            if (doIt)
                            {
                                DoMove(moveIndex);
                            }
                            return true;
                        }
                    } 
                    else 
                    {
                        // We didn't find anything blocking 
                        //us or needing to be pushed out of the way

                        if (doIt) // Move us
                        {
                            DoMove(moveIndex);
                        }
                        return true;
                    }

                }
                else // There was nothing on our target square
                {
                    if (doIt) // Move us
                    {
                        DoMove(moveIndex);
                    }
                    return true;

                }
            }

        }
        return false; // Otherwise, we can't move there, so return false.
    }


}