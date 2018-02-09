using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class StraightLocomotion : MonoBehaviour {

    public float speed = 5f;

    private Player player;

    void Start()
    {

        player = Player.instance;

        if (player == null)
        {
            Debug.LogError("LocomotionScript: No Player instance found in map.");
            Destroy(this.gameObject);
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Hand hand in player.hands)
        {
            SteamVR_Controller.Device controller = hand.controller;

            if (controller != null)
            {

                if (controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    Vector2 axis0 = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

                    Vector3 direction = (hand.transform.right * axis0.x + hand.transform.forward * axis0.y) * speed * Time.deltaTime;

                    direction.y = 0;

                    player.gameObject.transform.position += direction;

                }
            }
        }
    }
}
