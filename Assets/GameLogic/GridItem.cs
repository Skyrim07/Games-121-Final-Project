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

                if (gridMaster.grid[ourGridIndex].localObjects.Contains(gameObject))
                {
                    gridMaster.grid[ourGridIndex].localObjects.Remove(gameObject);
                }
              
                ourGridIndex = Moves[Moves.Count - 1].index;

                transform.position = gridMaster.grid[ourGridIndex].pos;
                gridMaster.grid[ourGridIndex].localObjects.Add(gameObject);

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
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        if ((ourGridIndex + moveIndex) > 0 && (ourGridIndex + moveIndex) < gridMaster.grid.Length)
        {
            if(!gridMaster.grid[ourGridIndex + moveIndex].Equals(null))
            {
                foreach (var item in gridMaster.grid[ourGridIndex].localObjects)
                {
                    if (item.TryGetComponent<BabaObject>(out BabaObject bbj))
                    {
                        if (bbj.myTypes.Contains(ObjectType.Player))
                        {
                            print("Player move");
                            foreach (var item2 in gridMaster.grid[ourGridIndex + moveIndex].localObjects)
                            {
                                if (item2.TryGetComponent<BabaObject>(out BabaObject bbj2))
                                {
                                    if (bbj2.myTypes.Contains(ObjectType.Win))
                                    {
                                        print("Win!");
                                    }
                                }
                            }
                        }
                    }
                }
                if (gridMaster.grid[ourGridIndex].localObjects.Contains(gameObject))
                {
                    gridMaster.grid[ourGridIndex].localObjects.Remove(gameObject);
                }
                gridMaster.grid[ourGridIndex + moveIndex].localObjects.Add(gameObject);
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
                if (gridMaster.grid[ourGridIndex + moveIndex].localObjects.Count > 0)
                {
                    List<GridItem> move = new List<GridItem>();
                    foreach(GameObject obj in gridMaster.grid[ourGridIndex + moveIndex].localObjects)
                    {
                        if (obj.GetComponent<GridItem>().locked)
                        {
                            return false;
                        }
                        if (obj.GetComponent<GridItem>().pushable)
                        {
                            move.Add(obj.GetComponent<GridItem>());
                        }
                        else if (obj.GetComponent<GridItem>().TryGetComponent<BabaObject>(out BabaObject babao))
                        {
                            if (babao.myTypes.Contains(ObjectType.Player))
                            {
                                if (obj.GetComponent<GridItem>().MoveIndex(moveIndex, false, depth - 1))
                                {
                                    // Not sure if we need to do something here
                                }
                                else
                                {
                                    return false;
                                }
                            }
 
                        }
                    }
                    if (move.Count > 0)
                    {
                        if (move[0].MoveIndex(moveIndex, doIt, depth - 1))
                        {
                            foreach (GridItem item in move)
                            {
                                if (!item.Equals(move[0]) && doIt)
                                {
                                    item.DoMove(moveIndex);
                                }
                            }
                            if (doIt)
                            {
                                DoMove(moveIndex);
                            }
                            return true;
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
