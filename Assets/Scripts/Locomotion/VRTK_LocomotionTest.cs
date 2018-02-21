using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTK_LocomotionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        GameObject head = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).gameObject;

    }
}
