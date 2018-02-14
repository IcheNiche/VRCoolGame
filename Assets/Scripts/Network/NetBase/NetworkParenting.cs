namespace NetBase
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(PhotonView))]
    public class NetworkParenting : Photon.MonoBehaviour
    {

        internal struct State
        {
            internal double timestamp;
            internal int parentId;
            internal string path;
        }

        private State newState;

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            int pid = 0;
            string path = null;

            if (stream.isWriting)
            {

                NetworkReference nref = NetworkReference.FromTransform(transform.parent);
                pid = nref.parentHandleId;
                path = nref.pathFromParent;

                // Send update

                stream.Serialize(ref pid);
                stream.Serialize(ref path);

            }
            else
            {
                // Receive updated state information

                stream.Serialize(ref pid);
                stream.Serialize(ref path);

                newState.timestamp = info.timestamp;
                newState.parentId = pid;
                newState.path = path;

            }
        }

        void Update()
        {
            if (photonView.isMine) { return; }

            UpdateParent();

        }

        void UpdateParent()
        {

            var actualNref = NetworkReference.FromTransform(transform.parent);
            var newNref = NetworkReference.FromIdAndPath(newState.parentId, newState.path);
            if (actualNref != newNref)
            {
                //Debug.Log("Reparenting from " + actualNref + " to " + newNref);
                GameObject newParent = newNref.FindObject();
                //Debug.Log("New parent " + newParent);
                transform.parent = newParent != null ? newParent.transform : null;
            }
        }

    }
}