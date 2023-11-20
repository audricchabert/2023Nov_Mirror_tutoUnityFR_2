using UnityEngine;

//RequireComponent fait en sorte qu'on ne puisse pas supprimer un component dépendant .via l'inspecteur
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    //SerializeField fait en sorte que la variable soit visible et modifiable depuis l'inspecteur, au niveau du script
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;
    [SerializeField]
    private float mouseSensitivityY = 3f;

    private PlayerMotor playerMotor;

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        // Calculer la vélocité du mouvement du joueur (avant-arrière et sur les côtés)
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        playerMotor.MovePlayer(velocity);

        //Calculer la rotation du joueur (pour regarder sur les côtés : donc lateralRotation)
        //note : on utilise le mouvement de la souris sur l'axe X (mouse X) pour associer une rotation du joueur sur l'axe y (yRot)
        float yRot = Input.GetAxisRaw("Mouse X");

        // sur ce vector, on met bien yRot à la position Y du vecteur (les 3 valeurs du vecteurs devraient correspondre respectivement à X, Y, Z dans cet ordre)
        Vector3 lateralRotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        playerMotor.RotatePlayer(lateralRotation);

        //Calculer la rotation du joueur (pour regarder en haut en bas )
        //note : on utilise le mouvement de la souris sur l'axe Y (mouse Y) pour associer une rotation du joueur sur l'axe x (xRot)
        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 verticalRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;
        playerMotor.RotateCamera(verticalRotation);
    }
}
