using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point
{
    // Points are what make up the grid

    // Each point has a:
    public Vector2 pos; //Position

    public GameObject obj; // (Deprecated) object

    public List<GameObject> localObjects; // List of inhabitant objects

    // This is the constructor
    public Point(Vector2 location, GameObject gobj, List<GameObject> newlist)
    {
        pos = location;
        obj = gobj;
        localObjects = newlist;
    }
}
public class LogicGrid : MonoBehaviour
{
    // This class handles some general grid-construction stuff

    // Scale of grid
    public int gridSpacing;

    // Moves origin of grid
    public Vector2 gridOffset;

    // How many points make up the grid along the X axis
    public int gridLength;

    // The actual grid array
    public Point[] grid;

    // Just a visual queue, no functionality
    public GameObject marker;

    public List<GameObject> markers;

    void Awake()
    {
        if(markers.Count > 0)
        {
            //Editor shenanigans
            foreach(GameObject mark in markers)
            {
                Destroy(mark);
            }
            markers.Clear();
        }

        grid = new Point[gridLength * gridLength];
        BuildGrid(false); // Build the grid for real
    }
    public void Init()
    {
        if (markers.Count > 0)
        {
            foreach (GameObject mark in markers)
            {
                DestroyImmediate(mark);
            }
        }
        grid = new Point[gridLength * gridLength];
        BuildGrid(true); // Only show me the markers, don't make the grid
    }

    private IEnumerator Start()
    {
        // Coroutine due to some timing shenanigans
        // We want this to run after everything else is set up
        yield return new WaitForEndOfFrame();
        //LevelManager.instance.onScreenFaded += MarkLimits;//your function
        RefreshLogic(false);
        yield return new WaitForSeconds(0.6f);
        MarkLimits();
        yield break;
    }

    public void MarkLimits()
    {
        foreach(GameObject mark in markers)
        {
            if(mark == null)
            {
                //Debug.Log("mark is null");
                continue;
            }
            mark.GetComponent<SpriteRenderer>().sortingOrder = 999;
            if (!mark.GetComponent<SpriteRenderer>().isVisible)
            {
                //Debug.Log("mark not spotted");
                if(!mark.TryGetComponent<BabaObject>(out BabaObject baba))
                {
                    baba = mark.AddComponent<BabaObject>();
                }
                baba.babaType = ObjectType.None;
                baba.defaultBlock = BlockAppearance.None;
                baba.locked = true;
                baba.mark = true;
            }
            else
            {
                //Debug.Log("mark spotted");
            }
            mark.GetComponent<SpriteRenderer>().sortingOrder = -100;
        }

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
    // Allows other classes to add themselves onto the grid
    public void Register(int index, GameObject obj)
    {
        grid[index].localObjects.Add(obj);
        grid[index].obj = obj;

        if(grid[index].obj.TryGetComponent(out BabaObject baba))
        {
            grid[index].obj.GetComponent<SpriteRenderer>().sortingOrder = -index / gridLength;
        }
      
    }

    void BuildGrid(bool editor)
    {
        // Sets up the grid to our specifications
        gridOffset = new Vector2(-gridLength / 2, -gridLength / 2);
        markers = new List<GameObject>();
        for(int y = 0; y < gridLength; y++)
        {
            for(int x = 0; x < gridLength; x++)
            {
               
                markers.Add(Instantiate(marker, new Vector2(x * gridSpacing, y * gridSpacing) + gridOffset, transform.rotation, transform));

                if (!editor)
                {
                    grid[y * gridLength + x] = new Point(new Vector2(x * gridSpacing, y * gridSpacing) + gridOffset, null, new List<GameObject>());
                }
               
            }
        }
    }

    // This resets everything back to default, we call this when the logic gets pushed around 
    public void RefreshLogic(bool editor)
    {
        // This is a really slow way of doing this
        GameObject[] nouns = GameObject.FindGameObjectsWithTag("Noun");

        if (editor) //If we're only doing this for the editor
        {
            foreach (GameObject noun in nouns)
            {
                // Reset all our objects
                noun.GetComponent<NounBlock>().Init(); //Same as refresh types, but editor-safe
            }
            return; //Don't bother with checking is
        }
        else
        {

            foreach (GameObject noun in nouns)
            {
                // Reset all our objects
                noun.GetComponent<NounBlock>().RefreshTypes(); //Same as refresh types, but editor-safe
            }
        }

        // This is a really slow way of doing this
        GameObject[] ises = GameObject.FindGameObjectsWithTag("Is");
        foreach (GameObject block in ises)
        {
            // Have our IS blocks generate new logic 
            block.GetComponent<IsBlock>().CheckActive();
        }
    }
}
