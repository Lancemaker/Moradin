using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {
    //define delegates and events;
   
    public float speed = 5;
    // Use this for initialization
    void Start() {
        
    }
    //trigger events
    //subscribe stuff
   

    // Update is called once per frame
    void Update () {
       float ty = Input.GetAxis("LevelY");
       float tx = Input.GetAxis("Horizontal") * speed;
       float tz = Input.GetAxis("Vertical") * speed;        
       transform.Translate(tx, 0, 0);
       transform.Translate(0, 0, tz, Space.World);
       transform.Translate(0, ty, 0, Space.World);        
    }
}
