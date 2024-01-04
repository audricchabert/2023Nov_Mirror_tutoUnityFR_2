using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 internalVelocity;
    private Vector3 internalRotation;
    private float internalVerticalRotation = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 internalThrusterVelocity;

    private Rigidbody playerRigidbody;

    [SerializeField]
    private float cameraRotationLimit = 85f;


    [SerializeField]
    private Camera playerCamera;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        //captures the mouse, can be disabled by pressing ESC
        //note : when having the mouse locked , we cannot click the network manager buttons "stop host" to quit the game
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //this method is just used to set the desired movement value. the movement will be processed in PerformMovement
    public void MovePlayer(Vector3 _velocity)
    {
        internalVelocity = _velocity;
    }


    //this method is just used to set the desired rotation value. the movement will be processed in PerformRotation
    public void RotatePlayer(Vector3 _rotation)
    {
        internalRotation = _rotation;
    }

    public void RotateCamera(float _verticalRotation)
    {
        internalVerticalRotation = _verticalRotation;
    }

    public void ApplyThruster(Vector3 _thrusterVelocity)
    {
        internalThrusterVelocity = _thrusterVelocity;
    }

    private void PerformMovement()
    {
        //only perform the movement when the velocity is not null
        if(internalVelocity != Vector3.zero)
        {
            //todo : check if its not better to use rigidbody.AddForce instead of MovePosition
            playerRigidbody.MovePosition(playerRigidbody.position + internalVelocity * Time.fixedDeltaTime);
            
        }

        if(internalThrusterVelocity != Vector3.zero)
        {
            playerRigidbody.AddForce(internalThrusterVelocity * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        //perform the lateral/horizontal rotation using the rigidbody
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(internalRotation));


        //perform the vertical rotation using the camera
        currentCameraRotationX -= internalVerticalRotation;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, +cameraRotationLimit);
        playerCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    
}
