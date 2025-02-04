using UnityEngine.Events;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public UnityEvent onHealthZero;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onHealthZero.Invoke(); // Trigger end game event
        }
    }
}