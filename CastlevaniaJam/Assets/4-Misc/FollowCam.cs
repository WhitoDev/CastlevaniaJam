using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {    
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.position = new Vector3(Camera.main.transform.position.x, this.transform.position.y, this.transform.position.z);	
	}
}
