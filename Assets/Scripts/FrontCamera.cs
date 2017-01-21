using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCamera : MonoBehaviour {

    public CameraMove camera;
    private float originalspeed;

	// Use this for initialization
	void Start () {
        originalspeed = camera.speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.speed = originalspeed * 3;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.speed = originalspeed;
        }
    }
}
