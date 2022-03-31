using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point
{
    public Vector2 pos;
    public GameObject obj;
    
    public Point(Vector2 location, GameObject gobj)
    {
        pos = location;
        obj = gobj;
    }
}
public class LogicGrid : MonoBehaviour
{
    public int gridSpacing;
    public Vector2 gridOffset;

    public int gridLength;
    public Point[] grid;

    // Just a visual queue, no functionality
    public GameObject marker;

    void Awake()
    {
        grid = new Point[gridLength * gridLength];

        BuildGrid();
    }

    public int NearestPoint(Vector2 pos) //Gives the nearest grid point for any position
    {
        int y;
        int x;


        y = Mathf.RoundToInt(pos.y - gridOffset.y / gridSpacing);
        x = Mathf.RoundToInt(pos.x - gridOffset.x / gridSpacing);

        if (y > gridLength)
        {
            y = gridLength;
        }
        if (x > gridLength)
        {
            x = gridLength;
        }
        if (x < 0)
        {
            x = 0;
        }
        if (y < 0)
        {
            y = 0;
        }

        return (y * gridLength + x);
    }
    public void Register(int index, GameObject obj)
    {
        grid[index].obj = obj;
    }
    void BuildGrid()
    {
        gridOffset = new Vector2(-gridLength / 2, -gridLength / 2);

        for(int y = 0; y < gridLength; y++)
        {
            for(int x = 0; x < gridLength; x++)
            {
                Instantiate(marker, new Vector2(x * gridSpacing, y * gridSpacing) + gridOffset, transform.rotation, transform); 
                grid[y * gridLength + x] = new Point(new Vector2(x * gridSpacing, y * gridSpacing) + gridOffset, null);
            }
        }
    }

    public void RefreshLogic()
    {
        // I know, this sucks ass
        GameObject[] nouns = GameObject.FindGameObjectsWithTag("Noun");
        foreach(GameObject noun in nouns)
        {
            noun.GetComponent<NounBlock>().RefreshTypes();
        }

        GameObject[] ises = GameObject.FindGameObjectsWithTag("Is");
        foreach (GameObject block in ises)
        {
            block.GetComponent<IsBlock>().CheckActive();
        }
    }
}
