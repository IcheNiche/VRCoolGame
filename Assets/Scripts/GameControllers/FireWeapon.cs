using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class FireWeapon : Photon.PunBehaviour
{

    public VRTK_InteractableObject interactableObject;
    public CompleteProject.PlayerShooting playerShooting;

    private bool fired;

    private void Awake()
    {
        interactableObject.InteractableObjectUsed += new InteractableObjectEventHandler(DoFireGun);
        interactableObject.InteractableObjectUnused += new InteractableObjectEventHandler(DontFireGun);
    }

    void DoFireGun(object sender, InteractableObjectEventArgs e)
    {
        fired = true;
        playerShooting.fired = true;
        SendShoot();
    }

    void DontFireGun(object sender, InteractableObjectEventArgs e)
    {
        fired = false;
        playerShooting.fired = false;
        SendShoot();
    }

    void SendShoot()
    {
        photonView.RPC("Shooting", PhotonTargets.AllViaServer, fired);
    }

    [PunRPC]
    void Shooting(bool active)
    {
        fired = active;
        playerShooting.fired = active;
    }
}
