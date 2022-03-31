using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    [HideInInspector] public LogicGrid gridMaster;

    public int ourGridIndex;

    public bool pushable;

    [HideInInspector] public bool isLogicBlock;
    // Start is called before the first frame update
    public virtual void Start()
    {
        gridMaster = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        gridMaster.Register(ourGridIndex, gameObject);
        transform.position = gridMaster.grid[ourGridIndex].pos;
    }

    public void MoveRight()
    {
        if(ourGridIndex < gridMaster.gridLength * gridMaster.gridLength)
        {
            if (gridMaster.grid[ourGridIndex + 1].obj != null)
            {
                if(gridMaster.grid[ourGridIndex + 1].obj.GetComponent<GridItem>().pushable)
                {
                    gridMaster.grid[ourGridIndex + 1].obj.GetComponent<GridItem>().MoveRight();
                }
            }
            gridMaster.grid[ourGridIndex].obj = null;
            gridMaster.grid[ourGridIndex + 1].obj = gameObject;
            ourGridIndex++;
            transform.position = gridMaster.grid[ourGridIndex].pos;

            if (isLogicBlock)
            {
                gridMaster.RefreshLogic();
            }
        }
       
    }

    public void MoveLeft()
    {
        if (ourGridIndex > 0)
        {

            if (gridMaster.grid[ourGridIndex - 1].obj != null)
            {
                if (gridMaster.grid[ourGridIndex - 1].obj.GetComponent<GridItem>().pushable)
                {
                    gridMaster.grid[ourGridIndex - 1].obj.GetComponent<GridItem>().MoveLeft();
                }
            }

            gridMaster.grid[ourGridIndex].obj = null;
            gridMaster.grid[ourGridIndex - 1].obj = gameObject;
            ourGridIndex--;
            transform.position = gridMaster.grid[ourGridIndex].pos;

            if (isLogicBlock)
            {
                gridMaster.RefreshLogic();
            }
        }
    }
    public void MoveUp()
    {
        if (ourGridIndex < (gridMaster.gridLength * gridMaster.gridLength) -gridMaster.gridLength)
        {

            if (gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj != null)
            {
                if (gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj.GetComponent<GridItem>().pushable)
                {
                    gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj.GetComponent<GridItem>().MoveUp();
                }
            }
            gridMaster.grid[ourGridIndex].obj = null;
            gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj = gameObject;
            ourGridIndex += gridMaster.gridLength;
            transform.position = gridMaster.grid[ourGridIndex].pos;

            if (isLogicBlock)
            {
                gridMaster.RefreshLogic();
            }
        }
    }
    public void MoveDown()
    {
        if (ourGridIndex > gridMaster.gridLength)
        {

            if (gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj != null)
            {
                if (gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj.GetComponent<GridItem>().pushable)
                {
                    gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj.GetComponent<GridItem>().MoveDown();
                }
            }

            gridMaster.grid[ourGridIndex].obj = null;
            gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj = gameObject;
            ourGridIndex -= gridMaster.gridLength;
            transform.position = gridMaster.grid[ourGridIndex].pos;

            if (isLogicBlock)
            {
                gridMaster.RefreshLogic();
            }
        }
    }
}