using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyCurrentHealth;
    public Slider healthSlider;

    private bool isDead;

    void Start()
    {
        enemyCurrentHealth = enemyHealth;
        healthSlider.maxValue = enemyHealth;
        healthSlider.value = enemyCurrentHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        enemyCurrentHealth -= amount;
        healthSlider.value = enemyCurrentHealth;
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
