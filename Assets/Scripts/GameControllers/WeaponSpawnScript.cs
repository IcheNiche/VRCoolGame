using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnScript : Photon.PunBehaviour
{

    public Transform spawnGrab;

    public void SpawnWeapon(string weaponPrefabName)
    {
        var weapon = PhotonNetwork.Instantiate(weaponPrefabName, spawnGrab.position, spawnGrab.rotation, 0);
    }
}
