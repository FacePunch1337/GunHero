using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
   
    public float speed;
    public GameObject hitEffect;
    public GameObject shootEffect;
    protected bool hitProjectile;
    protected int damage;
    protected Vector3 direction;

    public abstract void Initialize(Vector3 dir, int dmg);

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

   public void OnTriggerEnter(Collider other)
    {
       
        if (hitEffect != null)
        {
   
            Character character = other.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(damage);
                Debug.Log(damage);
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                hitProjectile = true;
        
                GameObject effectInstance = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effectInstance, 2f); 
            }
        }

        gameObject.SetActive(false);
    }
}
