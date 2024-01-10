using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name = "default weapon";
    public float damage = 10f;
    public float range = 100f;

    //for semi auto weapon, fireRate = 0f
    public float fireRate = 0f;

    public GameObject graphics;
}
