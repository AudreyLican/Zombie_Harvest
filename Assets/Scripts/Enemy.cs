using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;

    private NavMeshAgent navAgent;

    public bool isDead;

    //Destroy zombie after amount of time
    private SelfDestroy selfDestroy;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        selfDestroy = GetComponent<SelfDestroy>();
        if (selfDestroy != null)
        {
            selfDestroy.enabled = false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            isDead = true;
            int randomValue = Random.Range(0, 2); // 0 or 1

            if (randomValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            
            

            // Death Sound
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);

            selfDestroy.enabled = true;
            if (selfDestroy != null)
            {
                selfDestroy.timeForDestruction = 15f;
                StartCoroutine(selfDestroy.DestroySelf(15f));
                //selfDestroy.StartCoroutine(selfDestroy.DestroySelf(selfDestroy.timeForDestruction)); 
            }
        }
        else
        {
            animator.SetTrigger("DAMAGE");

            // Hurt Sound
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); // Attacking // Stop Attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 30f); // Detection (Start Chasing)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 33f); // Stop Chasing
    }
}
