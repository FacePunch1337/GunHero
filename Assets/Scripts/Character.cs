using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    public float moveSpeed;
    public int health;
    public float attackSpeed;
    public int damage;
    public float range;
    public Image healthBarFill;
    public GameObject deathEffect;
    public ParticleSystem driveEffect;
    public Rigidbody rigidbody;

   
   
        
    public abstract void Move(Vector3 direction);
    public abstract void Attack(Transform target);
    public abstract void TakeDamage(int damage);
    public abstract void AddHP(int hp);
    public abstract void Death();
}
