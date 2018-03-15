using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : WeaponBase {

    public override void FireOneBullet()
    {
        fireSound.Play();
    }
}
