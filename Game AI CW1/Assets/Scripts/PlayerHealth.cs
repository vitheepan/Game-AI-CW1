using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public void AddHealth(float amount)
    {
        health += amount;
        Debug.Log("Health Added, Current Health: " + health);
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;
        Debug.Log("Health Decreased, Current Health: " + health);

        if(health <= 0)
        {
            health = 0;
            Debug.Log("Player is dead!");
        }
    }
}
