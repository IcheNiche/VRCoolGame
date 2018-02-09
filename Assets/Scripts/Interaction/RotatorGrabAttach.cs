using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.GrabAttachMechanics;

public class RotatorGrabAttach : VRTK_RotatorTrackGrabAttach
{

    public Rigidbody grabbedRigidBody;

    public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        bool ret = base.StartGrab(grabbingObject, givenGrabbedObject, givenControllerAttachPoint);

        grabbedObjectRigidBody = grabbedRigidBody;

        return ret;
    }

}
