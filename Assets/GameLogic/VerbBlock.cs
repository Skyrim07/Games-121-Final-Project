using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerbBlock: MonoBehaviour
{
    public ObjectType myType;
    public LogicGrid gridMaster;
    private int ourGridIndex;


    // Start is called before the first frame update
    void Start()
    {
        ourGridIndex = gridMaster.NearestPoint(transform.position);
        gridMaster.Register(ourGridIndex, this.gameObject);
        transform.position = gridMaster.grid[ourGridIndex].pos;
    }
}

