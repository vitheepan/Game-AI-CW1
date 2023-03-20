using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyCurrentHealth;

    private bool isDead;

    void Start()
    {
        enemyCurrentHealth = enemyHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        enemyCurrentHealth -= amount;
        Debug.Log("Enemy Current Health: " + enemyCurrentHealth);

        if (enemyCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
