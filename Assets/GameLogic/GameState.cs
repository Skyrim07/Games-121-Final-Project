using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class GameState {
    /*
     *  This class has been deprecated
     *  
     *  
     *  
//saved data
public Vector3 playerPos;

public int playerIndex;
public Sprite playerSprite;
public List<Vector3> blockPositions;

public static GameState GetCurrentState()
{
GameState gameStateToSave = new GameState();
SavedElement[] elementsToSaveOnScene = GameObject.FindObjectsOfType<SavedElement>();
gameStateToSave.blockPositions = new List<Vector3>();
foreach(SavedElement element in elementsToSaveOnScene)
{
gameStateToSave.playerIndex = element.GetComponent<GridItem>().ourGridIndex;
gameStateToSave.playerPos = element.transform.position;

if (element.type == SavedElement.Type.Player)
{


    gameStateToSave.playerSprite = element.transform.GetComponent<SpriteRenderer>().sprite;
} else if(element.type == SavedElement.Type.Block)
{
    gameStateToSave.blockPositions.Add(element.transform.position);
}

}
return gameStateToSave; 
    }

    public void LoadGameState()
    {
        SavedElement[] elementsToLoadOnScene = GameObject.FindObjectsOfType<SavedElement>();
        List<Vector3> remainingBlockPosition = new List<Vector3>(blockPositions);
        foreach(SavedElement elementToLoad in elementsToLoadOnScene)
        {
            elementToLoad.GetComponent<GridItem>().ourGridIndex = playerIndex;
            elementToLoad.transform.position = playerPos;

            /*
            if(elementToLoad.type == SavedElement.Type.Player)
            {
            
                elementToLoad.GetComponent<SpriteRenderer>().sprite = playerSprite;
                //elementToLoad.GetComponent<BabaObject>().UndoSpriteIndex();
            }
            else if(elementToLoad.type == SavedElement.Type.Block)
            {
                elementToLoad.transform.position = remainingBlockPosition[0];
                remainingBlockPosition.RemoveAt(0);
            }

        }
    }
                */
}
