using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    public Transform firePoint;
    public GameObject drop;
    public Transform target;
    private Pool bulletPool;
  


    private float nextAttackTime;

    public void Initialize(Transform playerTransform, Pool pool)
    {
        target = playerTransform;
        bulletPool = pool;

        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned.");
        }

        if (bulletPool == null)
        {
            Debug.LogError("BulletPool is not assigned.");
        }
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= range)
        {
            // Остановить движение и атаковать
            StopMoving();
            if (Time.time >= nextAttackTime)
            {
                Attack(target);
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        else
        {
            // Перемещаться к цели
            Move(target.position);
        }
    }
    protected virtual void StopMoving() { }
   
    public override abstract void Move(Vector3 destination);

    public override void Attack(Transform player)
    {
        if (bulletPool == null)
        {
            Debug.LogError("BulletPool is not assigned. Cannot shoot.");
            return;
        }

        Vector3 directionToTarget = (player.position - firePoint.position).normalized;
        gameObject.transform.forward = directionToTarget;
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Projectile>().Initialize((player.position - firePoint.position).normalized, damage);
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthUI();
        if (health <= 0)
        {
            Death();
        }
    }
    public override void AddHP(int hp)
    {
        if (health < 100)
        {
            health += hp;
            UpdateHealthUI();
        }
    }


    private void UpdateHealthUI()
    {
        float healthPercentage = (float)health / 100f;
        healthBarFill.fillAmount = healthPercentage;
    }

    public override void Death()
    {

        GameObject effectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effectInstance, 2f);
        Instantiate(drop, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rigidbody.velocity = Vector3.zero;
        }

        Character character = collision.other.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(100);
        }
       
        
    }


}
