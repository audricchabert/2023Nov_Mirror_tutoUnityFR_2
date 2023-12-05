﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 internalVelocity;
    private Vector3 internalRotation;
    private Vector3 internalVerticalRotation;

    private Rigidbody playerRigidbody;

    [SerializeField]
    private Camera playerCamera;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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

    public void RotateCamera(Vector3 _verticalRotation)
    {
        internalVerticalRotation = _verticalRotation;
    }

    private void PerformMovement()
    {
        //only perform the movement when the velocity is not null
        if(internalVelocity != Vector3.zero)
        {
            //todo : check if its not better to use rigidbody.AddForce instead of MovePosition
            playerRigidbody.MovePosition(playerRigidbody.position + internalVelocity * Time.fixedDeltaTime);
            
        }
    }

    private void PerformRotation()
    {
        //perform the lateral/horizontal rotation using the rigidbody
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(internalRotation));


        //perform the vertical rotation using the camera
        playerCamera.transform.Rotate(-internalVerticalRotation);

    }
}