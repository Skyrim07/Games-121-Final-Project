using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public class Test : MonoBehaviour
{
    SKGridLayer grid;

    private void Start()
    {
        CommonUtils.InvokeAction(2f, () =>
        {
            
        });

        CommonUtils.StartProcedure(SKCurve.QuadraticIn, 1f, (f) =>
        {
           
        });

    }
}
