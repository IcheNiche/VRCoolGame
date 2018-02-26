using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeftHandMenuController : MonoBehaviour {

    public VRTK_ControllerEvents controllerEvents;
    public GameObject menuCanvas;

    public bool visible = false;

    private void Awake()
    {
        if (controllerEvents == null)
        {
            Debug.LogError("Controller Events must be set");
            return;
        }

        controllerEvents.ButtonTwoPressed += OnButtonTwoPressed;


    }

    void OnButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        visible = !visible;
        SetMenuVisible(visible);
    }

    void SetMenuVisible(bool visible)
    {
        menuCanvas.SetActive(visible);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
