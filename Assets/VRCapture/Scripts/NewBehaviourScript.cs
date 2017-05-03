using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey("i"))
        {
            VRCapture.VRCapture.Instance.StartCapture();
        }
        else if (Input.GetKey("o"))
        {
            VRCapture.VRCapture.Instance.StopCapture();
        }
    }
}
