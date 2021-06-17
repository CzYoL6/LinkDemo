using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLink : MonoBehaviour
{
    public alink[] links;
    void Start()
    {
        foreach(alink x in links){
            FindObjectOfType<Player>().LinkTwoNodes(x.to, x.start);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
