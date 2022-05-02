using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class IsBlock : GridItem
{
    /// <summary>
    /// 
    /// This class is on each of our IS blocks, and handles our logic
    /// Each IS block checks for Noun and Verb blocks adjacent to it,
    /// And applies the resultant logic statement to the game
    /// 
    /// </summary>

    public bool logicActive; // Is this block on?


    public override void Start()
    {
        isLogicBlock = true;
        pushable = true;
        base.Start();
    }

    ObjectType OurVerb(GameObject verb)
    {
        //Find and return what kind of effect this verb applies
        return verb.GetComponent<VerbBlock>().myType;
    }

    // Get us the list of objects this noun controls
    List<BabaObject> OurNoun(GameObject noun)
    {
        return noun.GetComponent<NounBlock>().myBlock;
    }

    // Check adjacent squares for logic statements
    public void CheckActive() 
    {
        logicActive = false; // Reset from previous call
        
        // Stay in bounds
        if(ourGridIndex < 1 || ourGridIndex >= gridMaster.gridLength * gridMaster.gridLength)
        {
            return;
        }

        //If we have a noun above us or to our left, we're active
        GameObject left = null;
        GameObject right = null;

        // Check left
        if (gridMaster.grid[ourGridIndex - 1].localObjects.Count > 0)
        {
            left = gridMaster.grid[ourGridIndex - 1].localObjects[0]; // Check the first stored obj to our left

            // Is it useful?
            if (!left.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex - 1].localObjects.Count > 1)
            {
                // Nope, try the second stored obj there
                left = gridMaster.grid[ourGridIndex - 1].localObjects[1];
            }
        }
        //Check right
        if(gridMaster.grid[ourGridIndex + 1].localObjects.Count > 0)
        {
            right = gridMaster.grid[ourGridIndex + 1].localObjects[0]; // Check the first stored obj

            //Is it useful?
            if (!right.GetComponent<GridItem>().isLogicBlock && gridMaster.grid[ourGridIndex + 1].localObjects.Count > 1)
            {
                // Nope, try the second
                right = gridMaster.grid[ourGridIndex + 1].localObjects[1];
            }
        }

        if (left != null) // Can't be too safe
        {
            if (left.CompareTag("Noun")) // Is it a noun?
            {
                if (right != null) // Cant be too safe
                {
                    if (right.CompareTag("Verb")) 
                    {
                        // Thats a valid statement!
                        logicActive = true; 
                        ApplyLogic(left, right); // Apply the logic
                    }
                    if (right.CompareTag("Noun"))
                    {
                        // Todo check for logic chains, prevent floating noun statements

                        // That's valid!
                        logicActive = true;
                        ApplyNoun2Noun(left, right); // Apply the logic
                    }
                }
            }
        }

        // Same as above, but for up/down. Only the movement indices are different
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
                        if (down.CompareTag("Noun"))
                        {
                            logicActive = true;
                            ApplyNoun2Noun(up, down);
                        }
                    }
                }
            }
        }
    }

    // Noun to Verb logic
    void ApplyLogic(GameObject noun, GameObject verb)
    {
        foreach (BabaObject baba in OurNoun(noun)) // For every block in our noun
        {
            // Apply the new effect
            baba.RefreshType(OurVerb(verb), false);
        }
        // Let the noun know (for logic chaining purposes)
        noun.GetComponent<NounBlock>().effect = OurVerb(verb);
    }

    // Noun to noun logic
    void ApplyNoun2Noun(GameObject n1, GameObject n2)
    {
        foreach(BabaObject baba in OurNoun(n1)) // For every block in our target-noun
        {

            // Apply the new sprites
            if(OurNoun(n2).Count > 0)
            {
                baba.AssignAppearance(OurNoun(n2)[0].currentApp);
            }
            else
            {
                return;
            }


            // Add objects to the target noun's list
            OurNoun(n2).Add(baba);

            //In the event that this is happening after ApplyLogic(), we need to apply the effects
            baba.babaType = n2.GetComponent<NounBlock>().effect;

            // And make sure that BabaObject is caught up
            baba.UpdateTypeLogic();
        }
        // Mark the effects ( for chaining purposes)
        n1.GetComponent<NounBlock>().effect = n2.GetComponent<NounBlock>().effect;
    }
}
