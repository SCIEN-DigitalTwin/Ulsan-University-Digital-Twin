using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public SlecteData data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(object a in data.prc_hist_tb_list)
        {
            //Debug.Log(a.ToString());
        }
        
    }
}
