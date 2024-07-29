using UnityEngine;
using System.Collections.Generic;


public class Player : Character
{
    public Joystick joystick;
    public Transform firePoint;
    private Pool pool;
    private List<Transform> targets = new List<Transform>();
    private float nextAttackTime;
    private Transform currentTarget; 
    public Weapon currentWeapon = Weapon.Machingun; 

    public enum Weapon
    {
        Machingun,
        Rocketgun
    }

    public void Initialize(Pool pool)
    {
        this.pool = pool;
      
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned.");
        }

        if (this.pool == null)
        {
            Debug.LogError("BulletPool is not assigned.");
        }
    }

    private void Start()
    {
        FollowCamera.Instance.target = gameObject.transform;
        joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            Debug.LogError("Rigidbody is not assigned.");
        }
        TargetZone.Instance.zone.SetActive(false);
    }

    void Update()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (direction.magnitude < 0.1f)
        {
           
            if (Time.time >= nextAttackTime)
            {
                Transform nearestTarget = FindNearestTarget();
                if (nearestTarget != null && nearestTarget != currentTarget)
                {
                    UpdateCurrentTarget(nearestTarget);
                }
                if (currentTarget != null)
                {
                    Attack(currentTarget);
                }
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        else
        {
            Move(direction);
            
        }


    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        switch (currentWeapon)
        {
            case Weapon.Machingun:
                damage = 30;
                range = 10;
                TargetZone.Instance.SetRadius(range);
                attackSpeed = 7;
                break;
            case Weapon.Rocketgun:
                damage = 100;
                range = 13;
                TargetZone.Instance.SetRadius(range);
                attackSpeed = 2;
                break;
            default:
                Debug.LogError("Unknown weapon type!");
                return;
        }
        Debug.Log("Switched to: " + currentWeapon);
       
    }

    public override void Move(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            driveEffect.Play();
            Vector3 move = direction.ToIso() * moveSpeed * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + move);
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public override void Attack(Transform target)
    {
        if (pool == null)
        {
            Debug.LogError("BulletPool is not assigned. Cannot shoot.");
            return;
        }

        Vector3 directionToTarget = (target.position - firePoint.position).normalized;
        gameObject.transform.forward = directionToTarget;

        GameObject projectile;

        switch (currentWeapon)
        {
            case Weapon.Machingun:
                projectile = pool.GetBullet();
                break;
            case Weapon.Rocketgun:
                projectile = pool.GetRocket();
                break;
            default:
                Debug.LogError("Unknown weapon type!");
                return;
        }

        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(directionToTarget);
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().Initialize(directionToTarget, damage);
    }

    private Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float minDistance = Mathf.Infinity;

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            Transform target = targets[i];
            if (target == null)
            {
                targets.RemoveAt(i);
                TargetZone.Instance.SetActiveZone(false);
                continue;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target;
                TargetZone.Instance.SetActiveZone(true);
            }
        }

        return nearestTarget;
    }

    private void UpdateCurrentTarget(Transform newTarget)
    {
        if (currentTarget != null)
        {
            Target oldTargetComponent = currentTarget.GetComponent<Target>();
            if (oldTargetComponent != null)
            {
                oldTargetComponent.ActivateOutline(false);
            }
        }

        currentTarget = newTarget;

        if (currentTarget != null)
        {
            Target newTargetComponent = currentTarget.GetComponent<Target>();
            if (newTargetComponent != null)
            {
                newTargetComponent.ActivateOutline(true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            
            Target targetComponent = other.GetComponent<Target>();
            if (targetComponent != null)
            {
                targets.Add(other.transform);
                Transform nearestTarget = FindNearestTarget();

                if (nearestTarget != null && nearestTarget != currentTarget)
                {
                    UpdateCurrentTarget(nearestTarget);
                }
                
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            
            Target targetComponent = other.GetComponent<Target>();
            if (targetComponent != null)
            {
                targets.Remove(other.transform);
                targetComponent.ActivateOutline(false);
                if (currentTarget == other.transform)
                {
                    UpdateCurrentTarget(FindNearestTarget());
                }

            }
            
        }
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
        Destroy(gameObject, 0.1f);
        GameManager.Instance.OnPlayerDeath();
    }


    
}
