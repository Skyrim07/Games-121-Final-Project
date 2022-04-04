using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    [HideInInspector] public LogicGrid gridMaster;

    public int ourGridIndex;

    public bool pushable;
    public bool locked;

    [HideInInspector] public bool isLogicBlock;

    public List<int> indicies = new List<int>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        gridMaster = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        gridMaster.Register(ourGridIndex, gameObject);
        transform.position = gridMaster.grid[ourGridIndex].pos;
        Save();
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Load();
        }
    }
    public void Save()
    {
        indicies.Add(ourGridIndex);
    }
    public void Load()
    {
        if(indicies.Count > 1)
        {
            ourGridIndex = indicies[indicies.Count - 2];
            if(indicies.Count > 1)
            {
                if (!indicies[indicies.Count - 1].Equals(null))
                {
                    indicies.RemoveAt(indicies.Count - 1);
                }
            }
            transform.position = gridMaster.grid[ourGridIndex].pos;
            gridMaster.grid[ourGridIndex].obj = gameObject;

            if (isLogicBlock)
            {
                gridMaster.RefreshLogic();
            }
        }
        

    }
    void DoMove(int moveIndex)
    {

        if((ourGridIndex + moveIndex) > 0 && (ourGridIndex + moveIndex) < gridMaster.grid.Length)
        {
          
            if(!gridMaster.grid[ourGridIndex + moveIndex].Equals(null))
            {
                if (gridMaster.grid[ourGridIndex].obj == gameObject)
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

    public bool MoveIndex(int moveIndex, bool doIt)
    {
        if((ourGridIndex + moveIndex > 0) && (ourGridIndex + moveIndex < gridMaster.gridLength * gridMaster.gridLength))
        {
            if (gridMaster.grid[ourGridIndex + moveIndex].obj != null)
            {
                if (gridMaster.grid[ourGridIndex + moveIndex].obj.GetComponent<GridItem>().pushable)
                {
                    if (gridMaster.grid[ourGridIndex + moveIndex].obj.GetComponent<GridItem>().MoveIndex(moveIndex, doIt))
                    {
                        if (doIt)
                        {
                            DoMove(moveIndex);
                        } 
                        return true;
                    }
                }
                else if(!gridMaster.grid[ourGridIndex + moveIndex].obj.GetComponent<GridItem>().locked)
                {
                    if (doIt)
                    {
                        DoMove(moveIndex);
                    }
                    return true;
                }
                else if (gridMaster.grid[ourGridIndex + moveIndex].obj.TryGetComponent<BabaObject>(out BabaObject baba))
                {
                    if (baba.myTypes.Contains(ObjectType.Player))
                    {
                        if (baba.MoveIndex(moveIndex, false))
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
        return false;
    }
}
