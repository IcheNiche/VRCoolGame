using NetBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkChildOfControllerGrabAttach : VRTK_ChildOfControllerGrabAttach {

    public PhotonView[] ownAdditionalPhotonviews;

    private int grabOwner;

    private VRTK_InteractableObject interactableObject;
    private Rigidbody rigidBody;

    //private VRTK_InteractableObject interactableObject;
    private NetworkReference networkReference;

    private bool wasKinimatic = false;

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
        //propKey = PROP_KEY_ID + networkReference.parentHandleId + "$" + (networkReference.pathFromParent != null ? networkReference.pathFromParent : "") + "$";

    }


    public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        if (base.StartGrab(grabbingObject, givenGrabbedObject, givenControllerAttachPoint))
        {
            HandleGrab();
            return true;
        }
        return false;
    }

    /// <summary>
    /// The StopGrab method ends the grab of the current object and cleans up the state.
    /// </summary>
    /// <param name="applyGrabbingObjectVelocity">If true will apply the current velocity of the grabbing object to the grabbed object on release.</param>
    public override void StopGrab(bool applyGrabbingObjectVelocity)
    {
        base.StopGrab(applyGrabbingObjectVelocity);
        HandleUngrab();
    }


    void OnEnable()
    {
        if (networkReference.IsPhotonView)
        {
            SetState(networkReference.GetPhotonView().ownerId);
        }

        PhotonNetwork.OnEventCall += this.OnUpdateData;
    }

    void OnDisable()
    {

        PhotonNetwork.OnEventCall -= this.OnUpdateData;
    }



    private void HandleGrab()
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

        UpdateDataToAllClients(true);
    }

    private void HandleUngrab()
    {
        SetState(0);

        UpdateDataToAllClients(false);
    }

    void UpdateDataToAllClients(bool grabbed)
    {
        Hashtable content = new Hashtable();

        content.Add("grabbed", grabbed);
        content.Add("grabOwner", grabOwner);

        content.Add("position", transform.position);
        content.Add("rotation", transform.rotation);

        if (rigidBody != null)
        {
            content.Add("velocity", rigidBody.velocity);
            content.Add("angularVelocity", rigidBody.angularVelocity);
        }

        PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.UpdateObjectRoomData, content, true, null);
    }


    void OnUpdateData(byte eventCode, object eventContent, int senderId)
    {
        if (eventCode != (byte)RaiseEventCodes.UpdateObjectRoomData) { return; }

        Hashtable content = (Hashtable)eventContent;

        interactableObject.isKinematic = (bool)content["grabbed"];
        SetState((int)content["grabOwner"]);

        transform.position = (Vector3)content["position"];
        transform.rotation = (Quaternion)content["rotation"];

        if (rigidBody != null)
        {
            rigidBody.velocity = (Vector3)content["velocity"];
            rigidBody.angularVelocity = (Vector3)content["angularVelocity"];
        }

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
            wasKinimatic = interactableObject.isKinematic;
            interactableObject.isKinematic = true;
        }

    }

}
