using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class IsBlock : GridItem
{
    public bool logicActive;

    // Start is called before the first frame update
    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
        CheckActive();
    }

    private void Update()
    {
        CheckActive();
    }

    ObjectType OurVerb(GameObject verb)
    {
        //Find and return what kind of verb we're using
        
        return verb.GetComponent<VerbBlock>().myType;
    }

    List<BabaObject> OurNoun(GameObject noun)
    {
        return noun.GetComponent<NounBlock>().myBlock;
    }
    public void CheckActive() //We could call this every time a LogicObject moves (noun, verb, is, and)
    {
        logicActive = false; // Reset from previous call
        
        // Stay in bounds
        if(ourGridIndex < 1 || ourGridIndex >= gridMaster.gridLength * gridMaster.gridLength)
        {
            return;
        }

        //If we have a noun above us or too our left, we're active
        GameObject left = null;
        GameObject right = null;

        if (gridMaster.grid[ourGridIndex - 1].localObjects.Count > 0)
        {
            left = gridMaster.grid[ourGridIndex - 1].localObjects[0];
            if (!left.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex - 1].localObjects.Count > 1)
            {
                left = gridMaster.grid[ourGridIndex - 1].localObjects[1];
            }
        }
        if(gridMaster.grid[ourGridIndex + 1].localObjects.Count > 0)
        {
            right = gridMaster.grid[ourGridIndex + 1].localObjects[0];
            if (!right.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex + 1].localObjects.Count > 1)
            {
                right = gridMaster.grid[ourGridIndex + 1].localObjects[1];
            }
        }
        
        if (left != null)
        {
            if (left.CompareTag("Noun"))
            {
                if (right != null)
                {
                    if (right.CompareTag("Verb"))
                    {
                        logicActive = true;
                        ApplyLogic(left, right);
                    }
                }
            }
        }

        //Check up/down logic
        //Check to make sure we're not at the end of the array
        if((ourGridIndex < (gridMaster.gridLength * gridMaster.gridLength) - gridMaster.gridLength) && ourGridIndex > gridMaster.gridLength)
        {


            GameObject up = null;
            GameObject down = null;

            if (gridMaster.grid[ourGridIndex + gridMaster.gridLength].localObjects.Count > 0)
            {
                up = gridMaster.grid[ourGridIndex + gridMaster.gridLength].localObjects[0];
                if (!up.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex + gridMaster.gridLength].localObjects.Count > 1)
                {
                    up = gridMaster.grid[ourGridIndex + gridMaster.gridLength].localObjects[1];
                }
            }
            if (gridMaster.grid[ourGridIndex - gridMaster.gridLength].localObjects.Count > 0)
            {
                down = gridMaster.grid[ourGridIndex - gridMaster.gridLength].localObjects[0];
                if (!down.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex - gridMaster.gridLength].localObjects.Count > 1)
                {
                    down = gridMaster.grid[ourGridIndex - gridMaster.gridLength].localObjects[1];
                }
            }

            //GameObject up = gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj;
            //GameObject down = gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj;

            if (up != null)
            {
                if (up.CompareTag("Noun"))
                {
                    if (down != null)
                    {
                        if (down.CompareTag("Verb"))
                        {
                           
                            logicActive = true;
                            ApplyLogic(up, down); 
                        }
                    }

                }
            }
        }
        // If we didn't return, we didn't apply logic, so this code runs
    }

    void ApplyLogic(GameObject noun, GameObject verb)
    {
        foreach (BabaObject baba in OurNoun(noun)) //Iterates through every noun (wall, for instance)
        {
            baba.RefreshType(OurVerb(verb)); //Only allows for one type per block currently
        } 
    }
}
