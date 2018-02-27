﻿using NetBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkChildOfControllerGrabAttach : VRTK_ChildOfControllerGrabAttach
{

    public PhotonView[] ownAdditionalPhotonviews;

    private int grabOwner;

    private VRTK_InteractableObject interactableObject;
    private Rigidbody rigidBody;

    private NetworkReference networkReference;

    public int currentGrabOwner
    {
        get
        {
            return grabOwner;
        }
    }

    protected override void Initialise()
    {
        base.Initialise();

        interactableObject = GetComponent<VRTK_InteractableObject>();
        rigidBody = GetComponent<Rigidbody>();

        networkReference = NetworkReference.FromObject(this.gameObject);

    }

    public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        if (base.StartGrab(grabbingObject, givenGrabbedObject, givenControllerAttachPoint))
        {
            HandleGrab(grabbingObject, givenGrabbedObject, givenControllerAttachPoint);
            return true;
        }
        return false;
    }

    public override void StopGrab(bool applyGrabbingObjectVelocity)
    {
        base.StopGrab(applyGrabbingObjectVelocity);
        HandleUngrab();
    }


    void OnEnable()
    {
        if (networkReference.IsPhotonView)
        {
            SetState(0);
        }

    }

    private void HandleGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        if (networkReference.IsPhotonView)
        {
            networkReference.GetPhotonView().TransferOwnership(PhotonNetwork.player);
        }
        foreach (PhotonView pv in ownAdditionalPhotonviews)
        {
            pv.TransferOwnership(PhotonNetwork.player);
        }
        SetState(PhotonNetwork.player.ID);

        UpdateDataToAllClients(true, givenControllerAttachPoint);
    }

    private void HandleUngrab()
    {
        SetState(0);

        UpdateDataToAllClients(false, null);
    }

    void UpdateDataToAllClients(bool grabbed, Rigidbody givenControllerAttachPoint)
    {

        PhotonView.Get(this).RPC("UpdateData", PhotonTargets.OthersBuffered,
            grabbed,
            grabOwner,
            transform.position,
            transform.rotation,
            grabbed ? Vector3.zero : rigidBody.velocity,
            grabbed ? Vector3.zero : rigidBody.angularVelocity);

    }

    [PunRPC]
    void UpdateData(bool grabbed, int grabOwner, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {

        interactableObject.isKinematic = grabbed;
        SetState(grabOwner);

        if (rigidBody != null)
        {
            rigidBody.velocity = velocity;
            rigidBody.angularVelocity = angularVelocity;
        }

        transform.position = position;
        transform.rotation = rotation;

    }


    private void SetState(int ownerId)
    {
        grabOwner = ownerId;
        //interactableObject.isGrabbable = (grabOwner == 0);

        if (ownerId == PhotonNetwork.player.ID) { return; }

        if (grabOwner == 0)
        {
            interactableObject.isKinematic = false;
        }
        else
        {
            interactableObject.isKinematic = true;
        }

    }

}
