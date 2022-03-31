using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class ISLogic : MonoBehaviour
{
    public LogicGrid gridMaster;
    private int ourGridIndex;


    // Start is called before the first frame update
    void Start()
    {
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        gridMaster.Register(ourGridIndex, this.gameObject);
        transform.position = gridMaster.grid[ourGridIndex].pos;
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
    void CheckActive() //We could call this every time a LogicObject moves (noun, verb, is, and)
    {
      
        // Stay in bounds
        if(ourGridIndex < 1 || ourGridIndex >= gridMaster.gridLength * gridMaster.gridLength)
        {
            return;
        }

        //If we have a noun above us or too our left, we're active
        GameObject left = gridMaster.grid[ourGridIndex - 1].obj;
        GameObject right = gridMaster.grid[ourGridIndex + 1].obj;

        if (left != null)
        {
            if (left.CompareTag("Noun"))
            {
                if (right != null)
                {
                    if (right.CompareTag("Verb"))
                    {
                        ApplyLogic(left, right);
                    }
                }

            }
        }
        //Check to make sure we're not at the end of the array
        else if((ourGridIndex < gridMaster.gridLength * gridMaster.gridLength) && ourGridIndex > gridMaster.gridLength)
        {
            Debug.Log("pasing pt1");
            GameObject up = gridMaster.grid[ourGridIndex + gridMaster.gridLength].obj;
            GameObject down = gridMaster.grid[ourGridIndex - gridMaster.gridLength].obj;

            if (up != null)
            {
                if (up.CompareTag("Noun"))
                {
                    if (down != null)
                    {
                        if (down.CompareTag("Verb"))
                        {
                            ApplyLogic(up, down);
                        }
                    }

                }
            }
        }
    }

    void ApplyLogic(GameObject noun, GameObject verb)
    {
        Debug.Log ("Applying Logic");
        foreach (BabaObject baba in OurNoun(noun)) //Iterates through every noun (wall, for instance)
        {
            if(baba.myTypes.Count > 0)
            {
                //Clears the current property of that group of objects
                baba.myTypes.Clear();
            }

            //Applies the new one
            baba.myTypes.Add(OurVerb(verb));
        }
    }
}
