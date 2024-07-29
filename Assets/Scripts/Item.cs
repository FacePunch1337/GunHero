using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, Time.deltaTime * 100);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Gear"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.AddHP(30);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Player") && gameObject.CompareTag("MachineGun"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.SwitchWeapon(Player.Weapon.Machingun);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Player") && gameObject.CompareTag("RocketGun"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.SwitchWeapon(Player.Weapon.Rocketgun);
                Destroy(gameObject);
            }
        }
    }


}
