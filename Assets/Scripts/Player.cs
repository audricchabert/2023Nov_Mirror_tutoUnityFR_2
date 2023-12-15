using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    // SyncVar makes the variable synced on all instances
    [SyncVar]
    private float currentHealth;

    [SerializeField]
    Behaviour[] componentsToDisableOnDeath;
    //this bool[] wasEnabledOnStart will contain if the corresponding index in [] componentsToDisableOnDeath was enabled or not at the game launch
    private bool[] wasEnabledOnStart;

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }

    public void Setup()
    {
        wasEnabledOnStart = new bool[componentsToDisableOnDeath.Length];
        for(int i = 0; i < componentsToDisableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = componentsToDisableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < componentsToDisableOnDeath.Length; i++)
        {
            componentsToDisableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        Collider collider = GetComponent<Collider>();
        if( collider != null)
        {
            collider.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant " + currentHealth + " points de vie");
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < componentsToDisableOnDeath.Length; i++)
        {
            componentsToDisableOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name + " a été éliminé");

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
