using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkCT : MonoBehaviour
{
    public int sparkid;

    public void checkSparks()
    {
        Debug.Log(sparkid + "  " + Static.sparkid);
        
        if(sparkid <= Static.sparkid - Static.MaxSparks)
        {
            Destroy(this.gameObject);
        }
        
    }

    public void destroySparks()
    {
        Destroy(this.gameObject);
    }
}
