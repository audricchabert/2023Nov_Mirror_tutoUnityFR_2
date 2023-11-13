using UnityEngine;

//RequireComponent fait en sorte qu'on ne puisse pas supprimer un component dépendant .via l'inspecteur
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    //SerializeField fait en sorte que la variable soit visible et modifiable depuis l'inspecteur, au niveau du script
    [SerializeField]
    private float speed;

    private PlayerMotor playerMotor;

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        // Calculer la vélocité du mouvement du joueur
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        
    }
}
