namespace NetVRTK {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;
    using NetBase;
    using System;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    [RequireComponent(typeof(VRTK_InteractableObject))]
    public class NetworkGrabManager : NetworkBehaviour {

        public Rigidbody rigidBody;

        public PhotonView[] ownAdditionalPhotonviews;
        
        private int grabOwner;

        private VRTK_InteractableObject interactableObject;
        private NetworkReference networkReference;
        
        private bool wasKinimatic = false;

        public int currentGrabOwner {
            get {
                return grabOwner;
            }
        }

        void Awake() {
            interactableObject = GetComponent<VRTK_InteractableObject>();
            networkReference = NetworkReference.FromObject(this.gameObject);
            propKey = PROP_KEY_ID + networkReference.parentHandleId + "$" + (networkReference.pathFromParent != null ? networkReference.pathFromParent : "") + "$";
            var dummy = PropertyEventHandler.Instance;
        }

        void OnEnable() {
            interactableObject.InteractableObjectGrabbed += HandleGrab;
            interactableObject.InteractableObjectUngrabbed += HandleUngrab;
            if (networkReference.IsPhotonView) {
                InitState(networkReference.GetPhotonView().ownerId);
            }

            PhotonNetwork.OnEventCall += this.OnUpdateClientObjectRoomData;
        }

        void OnDisable() {
            interactableObject.InteractableObjectGrabbed -= HandleGrab;
            interactableObject.InteractableObjectUngrabbed -= HandleUngrab;

            PhotonNetwork.OnEventCall -= this.OnUpdateClientObjectRoomData;
        }



        private void HandleGrab(object sender, InteractableObjectEventArgs e) {
            if (networkReference.IsPhotonView) {
                networkReference.GetPhotonView().TransferOwnership(PhotonNetwork.player);
            }
            foreach (PhotonView pv in ownAdditionalPhotonviews) {
                pv.TransferOwnership(PhotonNetwork.player);
            }
            InitState(PhotonNetwork.player.ID);
            SendState();

            UpdateObjectRoomDataToAllClients(true);
        }

        private void HandleUngrab(object sender, InteractableObjectEventArgs e) {
            InitState(0);
            SendState();

            UpdateObjectRoomDataToAllClients(false);
        }

        void UpdateObjectRoomDataToAllClients(bool grabbed)
        {
            Hashtable content = new Hashtable();

            content.Add("grabbed", grabbed);

            content.Add("position", transform.position);
            content.Add("rotation", transform.rotation);

            if (rigidBody != null)
            {
                content.Add("velocity", rigidBody.velocity);
                content.Add("angularVelocity", rigidBody.angularVelocity);
            }

            PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.UpdateObjectRoomData, content, true, null);
        }


        void OnUpdateClientObjectRoomData(byte eventCode, object eventContent, int senderId)
        {
            if (eventCode != (byte)RaiseEventCodes.UpdateObjectRoomData) { return; }

            Hashtable content = (Hashtable)eventContent;

            interactableObject.isKinematic = (bool)content["grabbed"];

            transform.position = (Vector3)content["position"];
            transform.rotation = (Quaternion)content["rotation"];

            if (rigidBody != null)
            {
                rigidBody.velocity = (Vector3)content["velocity"];
                rigidBody.angularVelocity = (Vector3)content["angularVelocity"];
            }

        }

        private void InitState(int ownerId) {
            grabOwner = ownerId;
            interactableObject.isGrabbable = (grabOwner == 0);

            if (ownerId == PhotonNetwork.player.ID) { return; }

            if (grabOwner == 0)
            {
                interactableObject.isKinematic = wasKinimatic;
            }
            else
            {
                wasKinimatic = interactableObject.isKinematic;
                interactableObject.isKinematic = true;
            }
            
        }

        //
        // Syncing states
        //

        private string propKey;

        public const string PROP_KEY_ID = "ngm$";

        protected override string PropKey {
            get {
                return propKey;
            }
        }

        private void SendState() {
            Hashtable content = new Hashtable();
            content.Add("go", grabOwner);
            SetProperties(content);
        }

        protected override void RecvState(Hashtable content) {
            InitState((int)content["go"]);
        }
    }
}
