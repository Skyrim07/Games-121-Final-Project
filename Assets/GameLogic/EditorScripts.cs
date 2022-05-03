using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorScripts : MonoBehaviour
{
    private LogicGrid lm;
    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isPlaying)
        {
            lm = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
            lm.Init();
            //lm.MarkLimits();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            lm = GameObject.FindGameObjectWithTag("GridManager").GetComponent<LogicGrid>();
            lm.RefreshLogic(true);

            if(lm.markers.Count > lm.gridLength * lm.gridLength + 1)
            {
                Debug.Log("clearing excess grid-markers");
                lm.Init();
            }
            GameObject[] verbs = GameObject.FindGameObjectsWithTag("Verb");

            foreach (GameObject verb in verbs)
            {
                verb.GetComponent<VerbBlock>().Init();
            }

            GameObject[] babas = GameObject.FindGameObjectsWithTag("Baba");

            foreach (GameObject baba in babas)
            {
                baba.GetComponent<BabaObject>().Init();
            }
        }
        
    }
}
