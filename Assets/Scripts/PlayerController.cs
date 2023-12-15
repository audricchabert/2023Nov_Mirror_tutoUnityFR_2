using UnityEngine;

//RequireComponent fait en sorte qu'on ne puisse pas supprimer un component dépendant .via l'inspecteur
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //SerializeField fait en sorte que la variable soit visible et modifiable depuis l'inspecteur, au niveau du script
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;
    [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [Header("Y_Joint options")]
    [SerializeField]
    private float jointSpring= 20f;
    [SerializeField]
    private float jointMaxForce = 40;


    private PlayerMotor playerMotor;
    private ConfigurableJoint playerConfigurablejoint;
    private Animator animator;

    private void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        playerConfigurablejoint = GetComponent<ConfigurableJoint>();
        SetJointsSettings(jointSpring);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Calculer la vélocité du mouvement du joueur (avant-arrière et sur les côtés)
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        //Jouer animation thruster
        //the "ForwardVelocity" value is the one used in the animator, in the parameters pane
        animator.SetFloat("ForwardVelocity", zMov);

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
        float verticalRotation = xRot* mouseSensitivityY;
        playerMotor.RotateCamera(verticalRotation);

        Vector3 thrusterVelocity = Vector3.zero;
        //gestion du jetpack 
        if (Input.GetButton("Jump"))
        {
            thrusterVelocity = Vector3.up * thrusterForce;
            SetJointsSettings(0f);
        }
        else // todo : voir si ce n'est pas plus performant de remplacer ce else par un GetButtonUp, pour juste désactiver une seule fois quand on relache le bouton
        {
            SetJointsSettings(jointSpring);
        }

        playerMotor.ApplyThruster(thrusterVelocity);
    }

    private void SetJointsSettings(float _jointSpring)
    {
        //note : il semble qu'on doive utiliser jointMaxForce dans tous les appels vu qu'on doit créer un nouveau JointDrive à chaque fois, et pas juste le modifier
        playerConfigurablejoint.yDrive = new JointDrive { positionSpring  = _jointSpring, maximumForce = jointMaxForce };
    }
}
