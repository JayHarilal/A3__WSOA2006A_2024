using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void takeDamage(int damagerAmount)
    {
        HP -= damagerAmount;

        if (HP <= 0)
        {
            print("Player dead");
        }
        else
        {
            print("player hurt");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            takeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
