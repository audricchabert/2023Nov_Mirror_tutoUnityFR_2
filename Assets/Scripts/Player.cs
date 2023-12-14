using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    // SyncVar makes the variable synced on all instances
    [SyncVar]
    private float currentHealth;

    private void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant " + currentHealth + " points de vie");
    }
}
