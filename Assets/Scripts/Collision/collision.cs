using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{

    public Transform AP85_Magazine;
    void Start()
    {
        Transform bullet = Instantiate(AP85_Magazine) as Transform;
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
    }
}