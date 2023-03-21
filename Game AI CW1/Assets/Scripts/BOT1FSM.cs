using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BOT1FSM : MonoBehaviour
{
    public enum NPCState
    {
        Patrol,
        Chase,
        Attack,
        Retreat
    };

    public NPCState currentState;

    public Transform player;

    public NavMeshAgent agent;
    public float patrolRadius = 10f;
    private Vector3 randomPatrolPoint;

    public float chaseDistance = 10f;
    public float attackDistance = 2f;

    bool alreadyAttacked;

    //public int attackDamage = 10;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Slider healthSlider;

    public float timeBetweeenAttacks;

    public int maxHealth = 100;
    public int retreatThreshold = 30;
    private int currentHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = NPCState.Patrol;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Patrol:
                Patrol();
                break;
            case NPCState.Chase:
                Chase();
                break;
            case NPCState.Attack:
                Attack();
                break;
            case NPCState.Retreat:
                Retreat();
                break;
        }
    }

    void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            randomPatrolPoint = RandomNavMeshLocation(patrolRadius);
            agent.SetDestination(randomPatrolPoint);
        }

        if (Vector3.Distance(transform.position, player.position) < chaseDistance)
        {
            currentState = NPCState.Chase;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentState = NPCState.Attack;
        }
        else if (Vector3.Distance(transform.position, player.position) > chaseDistance)
        {
            currentState = NPCState.Patrol;
        }
    }

    void Attack()
    {
        transform.LookAt(player.position);

        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            currentState = NPCState.Chase;
        }
        else if(!alreadyAttacked)
        {
            Shoot();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweeenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(player.position - firePoint.position));
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 1000);
        Destroy(bullet, 2f);
    }

    void Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + (transform.position - player.position).normalized * 10f, agent.speed * Time.deltaTime);
        Debug.Log("Retreating");
        if (currentHealth > retreatThreshold)
        {
            currentState = NPCState.Chase;
        }
    }

    //public void TakeDamage(int damage)
    //{
    //Debug.Log("NPC took " + damage + " damage.");
    //currentHealth -= damage;

    //if (currentHealth < retreatThreshold)
    //{
    //Debug.Log("NPC is retreating.");
    //currentState = NPCState.Retreat;
    //}

    //if (currentHealth <= 0)
    //{
    //Debug.Log("NPC is dead.");
    //Destroy(gameObject);
    //}
    //}

    public void TakeDamage(int amount)
    {
        Debug.Log("NPC took " + amount + " damage.");
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        Debug.Log("Enemy Current Health: " + currentHealth);

        if (currentHealth < retreatThreshold)
        {
        Debug.Log("NPC is retreating.");
        currentState = NPCState.Retreat;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("NPC is dead.");
            Destroy(gameObject);
        }
    }

    Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}
