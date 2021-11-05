using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
//Script for Car Controller. Used youtube video as reference to have the car move. 
//https://youtu.be/Z4HA8zJhGEk
// only car mechanics are from video
public class CarController : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private GameObject car;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    
    private AudioSource acceleration;
    [SerializeField] private GameObject breaks;
    public Quaternion initialRotation;

    private void Start()
    {
        breaks.SetActive(false);
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        CarFlip();
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxis(Horizontal);
        verticalInput = Input.GetAxis(Vertical);
        isBreaking = Input.GetKey(KeyCode.Space);
       
    }
    //rotating car once it is flipped
    private void CarFlip() 
    {
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("flipping car");
            transform.rotation = initialRotation;
        }
    }
    
    private void HandleMotor()
    {
        acceleration = GetComponent<AudioSource>();
        acceleration.PlayOneShot(acceleration.clip,0.3f);
        Debug.Log("sound playing");
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        breaks.SetActive(false);
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    public void ApplyBreaking()
    {
        breakLightOn();
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot
;       wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    //controlling the break lights
    private IEnumerable breakLightCD()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("stop break lights");
        breaks.SetActive(false);
    }
    // setting breaklights on
    private void breakLightOn()
    {
        if (isBreaking)
        {
            Debug.Log("break light on");
            breaks.SetActive(true);
            StartCoroutine("breakLightCD");
        }
       
    }
    
}