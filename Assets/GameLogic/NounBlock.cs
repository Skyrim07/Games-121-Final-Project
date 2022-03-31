using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounBlock: MonoBehaviour
{
    public List<BabaObject> myBlock;
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
