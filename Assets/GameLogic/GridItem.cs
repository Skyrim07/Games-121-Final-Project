using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public int movenum;
    public int index;

    public Move(int currentmove, int currentindex)
    {
        movenum = currentmove;
        index = currentindex;
    }

}
public class GridItem : MonoBehaviour
{
    [HideInInspector] public LogicGrid gridMaster;

    public int ourGridIndex;

    public bool pushable;
    public bool locked;

    [HideInInspector] public bool isLogicBlock;

    public List<Move> Moves = new List<Move>();
    public GameManager gamemanager;
    // Start is called before the first frame update
    public virtual void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gridMaster = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        gridMaster.Register(ourGridIndex, gameObject);
        transform.position = gridMaster.grid[ourGridIndex].pos;
        Save();
    }

    public void Save()
    {
        Moves.Add(new Move(gamemanager.currentMove, ourGridIndex));
    }
    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Load();
        }
    }
    public void Load()
    {
        if(Moves.Count > 1)
        {
            if (gamemanager.currentMove == Moves[Moves.Count - 1].movenum)
            {
                Moves.RemoveAt(Moves.Count - 1);

                if (gridMaster.grid[ourGridIndex].obj!= null && gridMaster.grid[ourGridIndex].obj.Equals(gameObject))
                {
                    gridMaster.grid[ourGridIndex].obj = null;
                }
              
                ourGridIndex = Moves[Moves.Count - 1].index;

                transform.position = gridMaster.grid[ourGridIndex].pos;
                gridMaster.grid[ourGridIndex].obj = gameObject;

                if (isLogicBlock)
                {
                    gridMaster.RefreshLogic();
                }
            }
            
        }
        

    }
    void DoMove(int moveIndex)
    {
        gamemanager.pmove = true;

        if((ourGridIndex + moveIndex) > 0 && (ourGridIndex + moveIndex) < gridMaster.grid.Length)
        {
            if(!gridMaster.grid[ourGridIndex + moveIndex].Equals(null))
            {
                if (gridMaster.grid[ourGridIndex].obj!=null && gridMaster.grid[ourGridIndex].obj.Equals(gameObject))
                {
                    gridMaster.grid[ourGridIndex].obj = null;
                }
                gridMaster.grid[ourGridIndex + moveIndex].obj = gameObject;
                ourGridIndex += moveIndex;
                transform.position = gridMaster.grid[ourGridIndex].pos;

                Save();

                if (isLogicBlock)
                {
                    gridMaster.RefreshLogic();
                }
            }
        }
    }

    public bool MoveIndex(int moveIndex, bool doIt, int depth)
    {
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        //gridMaster.grid[ourGridIndex].obj = gameObject;

        if(depth < 1)
        {
            return false;
        }
        if (moveIndex != 0)
        {
            if ((ourGridIndex + moveIndex > 0) && (ourGridIndex + moveIndex < gridMaster.gridLength * gridMaster.gridLength))
            {
                if (gridMaster.grid[ourGridIndex + moveIndex].obj != null)
                {
                    GridItem gridItem = gridMaster.grid[ourGridIndex + moveIndex].obj.GetComponent<GridItem>();
                    if (gridItem.pushable)
                    {
                        if (gridItem.MoveIndex(moveIndex, doIt, depth - 1))
                        {
                            if (doIt)
                            {
                                DoMove(moveIndex);
                                GoalBlock goal = gridItem.GetComponent<GoalBlock>();
                                if (goal)
                                {
                                    print("Win");
                                }
                            }
                            return true;
                        }
                    }
                    else if (!gridItem.locked)
                    {
                        if (doIt)
                        {
                            DoMove(moveIndex);
                        }
                        return true;
                    }
                    else if (gridMaster.grid[ourGridIndex + moveIndex].obj.TryGetComponent<BabaObject>(out BabaObject baba))
                    {
                        if (baba.Equals(this))
                        {
                            Debug.Log("baba is me");
                            gridMaster.grid[ourGridIndex + moveIndex].obj = null;
                            return true;
                        }
                        if (baba.myTypes.Contains(ObjectType.Player))
                        {
                            if (baba.MoveIndex(moveIndex, false, depth - 1))
                            {
                                if (doIt)
                                {
                                    DoMove(moveIndex);
                                }
                                return true;
                            }
                        }

                    }

                }
                else
                {
                    if (doIt)
                    {
                        DoMove(moveIndex);
                    }
                    return true;

                }
            }

        }
        return false;
    }

    
}
