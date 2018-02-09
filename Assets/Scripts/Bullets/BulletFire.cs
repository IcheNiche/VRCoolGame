using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BulletFire : MonoBehaviour
{

    public float fireTime = .25f;
    public Hand hand;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Player player;

    public int pooledAmount = 20;
    private List<GameObject> bullets;

    public bool poolWillGrow = true;

    private void Awake()
    {
        player = Player.instance;

        if (player == null)
        {
            Debug.LogError("BulletFireScript: No Player instance found in map.");
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bullets.Add(obj);

        }
    }

    private void Update()
    {
        SteamVR_Controller.Device controller = hand.controller;

        if (controller != null)
        {
            if (controller.GetHairTrigger())
            {

                FireBullet();

            }
        }

    }


    void FireBullet()
    {
        GameObject bullet = GetPooledObject();

        if (bullet == null) return;

        bullet.transform.position = bulletSpawn.position;
        bullet.transform.rotation = bulletSpawn.rotation;
        bullet.SetActive(true);

    }


    private GameObject GetPooledObject()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        if (poolWillGrow)
        {
            GameObject obj = Instantiate(bulletPrefab);
            bullets.Add(obj);
            return obj;
        }

        return null;
    }
}
