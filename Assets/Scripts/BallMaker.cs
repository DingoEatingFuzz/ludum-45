using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : MonoBehaviour
{
    public GameObject template;
    private Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        this.cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(1)) {
           var mouse = Input.mousePosition;
           mouse.z = cam.nearClipPlane;
           var mouseWorld = cam.ScreenToWorldPoint(mouse);
           mouseWorld.z = -1;
           var ball = Instantiate(this.template, mouseWorld, new Quaternion());
           ball.tag = "dynamic";
       }
    }
}
