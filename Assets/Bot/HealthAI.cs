using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAI : MonoBehaviour
{
    public int health;

    public void ApllyDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
