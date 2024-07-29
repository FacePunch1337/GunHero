using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    private List<GameObject> bullets = new List<GameObject>();
    private List<GameObject> rockets = new List<GameObject>();

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        bullets.Add(newBullet);
        return newBullet;
    }

    public GameObject GetRocket()
    {
        foreach (GameObject rocket in rockets)
        {
            if (!rocket.activeInHierarchy)
            {
                return rocket;
            }
        }

        GameObject newRocket = Instantiate(rocketPrefab);
        rockets.Add(newRocket);
        return newRocket;
    }
}
