using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeftHandMenuController : MonoBehaviour {

    public VRTK_ControllerEvents controllerEvents;
    public GameObject mainMenu;

    private GameObject currentMenu;

    private bool visible = false;

    private void Awake()
    {
        if (controllerEvents == null)
        {
            Debug.LogError("Controller Events must be set");
            return;
        }

        controllerEvents.ButtonTwoPressed += OnButtonTwoPressed;


    }

    void Start()
    {
        SetCurrentMenu(mainMenu);
        SetMenuVisible(visible);
    }

    void OnButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        visible = !visible;
        SetMenuVisible(visible);
    }

    void SetMenuVisible(bool visible)
    {

        if (visible)
        {
            SetCurrentMenu(currentMenu);
        }

        gameObject.SetActive(visible);
    }

    public void SetCurrentMenu(GameObject currentMenu)
    {
        for(int i=0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        this.currentMenu = currentMenu;
        this.currentMenu.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
