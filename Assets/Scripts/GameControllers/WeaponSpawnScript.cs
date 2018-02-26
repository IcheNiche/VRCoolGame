using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnScript : Photon.PunBehaviour
{

    public Transform spawnGrab;

    public GameObject mk18;

	public void SpawnMK18()
    {

        var weapon = PhotonNetwork.Instantiate(mk18.name, spawnGrab.position, spawnGrab.rotation, 0);


    }

}
