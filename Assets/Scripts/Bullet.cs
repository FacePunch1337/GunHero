using UnityEngine;

public class Bullet : Projectile
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