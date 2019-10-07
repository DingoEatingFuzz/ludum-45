using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningCross : MonoBehaviour
{

    public bool isclockWise = true;
    public float spinSpeed = 50f;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0,0,isclockWise?-spinSpeed*Time.deltaTime:spinSpeed*Time.deltaTime);
    }
}
