using System.Collections;
using UnityEngine;

public class Rocket : Projectile
{
  
    public override void Initialize(Vector3 dir, int dmg)
    {
        direction = dir;
        damage = dmg;
        speed = 20f;
        GameObject effectInstance = Instantiate(shootEffect, transform.position, Quaternion.identity);
        Destroy(effectInstance, 2f);
    }

 

  
}
