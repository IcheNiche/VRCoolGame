using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class WeaponBase : Photon.PunBehaviour
{

    [Header("Bullets")]

    public float bulletSpeed = 0;
    public float bulletDamage = 0;
    public float bulletPenatration = 0;

    [Header("Weapon behaviour")]
    [Tooltip("the fire rate")]
    public float fireRate = 0.5f;
    public float recoil = 0;
    public float spread = 0;

    [Header("Sounds")]
    public AudioSource fireSound;
    public AudioSource fireEmptySound;
    public AudioSource magazinSnappedSound;
    public AudioSource reloadSound;
    

    [Header("Paricle emitter")]
    public ParticleSystem muzzle;
    public ParticleSystem bulletSpread;

    [Header("Linked Objects")]
    public Transform bulletSpawn;

    private bool fired;
    private VRTK_InteractableObject interactableObject;

    private void Awake()
    {
        interactableObject = GetComponent<VRTK_InteractableObject>();
        interactableObject.InteractableObjectUsed += new InteractableObjectEventHandler(StartFireGun);
        interactableObject.InteractableObjectUnused += new InteractableObjectEventHandler(StopFireGun);
    }

    void StartFireGun(object sender, InteractableObjectEventArgs e)
    {
        fired = true;
        SendShoot();
        InvokeRepeating("FireOneBullet", 0.0f, fireRate);
    }

    public virtual void FireOneBullet()
    {

    }

    void StopFireGun(object sender, InteractableObjectEventArgs e)
    {
        fired = false;
        SendShoot();
        CancelInvoke("FireOneBullet");
    }

    void SendShoot()
    {
        photonView.RPC("Shooting", PhotonTargets.AllViaServer, fired);
    }

    [PunRPC]
    void Shooting(bool active)
    {
        fired = active;
        if (fired)
        {
            InvokeRepeating("FireOneBullet", 0.0f, fireRate);
        } else
        {
            CancelInvoke("FireOneBullet");
        }
    }
}
