using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RotateSphereTestScript : MonoBehaviour {

    private void Awake()
    {
        //Time.fixedDeltaTime = Time.
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.Rotate(transform.up, 5 * Time.deltaTime);

        //Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();

        //playArea.position += playArea.right * 1 * Time.deltaTime;
        

    }
    
}
