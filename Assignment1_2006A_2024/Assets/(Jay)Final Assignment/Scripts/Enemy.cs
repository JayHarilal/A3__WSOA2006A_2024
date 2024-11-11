using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void takeDamage(int damagerAmount)
    {
        HP -= damagerAmount;

        if (HP <= 0)
        {
            int randomValue = Random.Range(0, 2);
            if(randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            isDead = true;
            soundManager.Instance.zombieChannel2.PlayOneShot(soundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            soundManager.Instance.zombieChannel2.PlayOneShot(soundManager.Instance.zombieHurt);
        }
    }

}
