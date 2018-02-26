using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMenuController : MonoBehaviour {

    private LeftHandMenuController menuController;

    public GameObject mainMenu;
    
    private void Awake()
    {
        menuController = GetComponent<LeftHandMenuController>();
    }

    public void SwitchToMainMenu()
    {
        menuController.SetCurrentMenu(mainMenu);
    }

    public void SwitchToMenu(string menuObjectName)
    {

        GameObject menu = transform.Find(menuObjectName).gameObject;

        menuController.SetCurrentMenu(menu);

    }
}
