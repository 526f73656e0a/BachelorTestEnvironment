using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";


    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private float currentSteerAngle;
    private float currentbreakForce; 
    private float carRigidBodySpeed;
    // A flag if the spacebar is pressed (if the car should be braking)
    private bool isBreaking;


    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float carRigidBodyTopSpeed;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private int speedThreshold;
    [SerializeField] private int stepsBelowThreshold;
    [SerializeField] private int stepsAboveThreshold;


    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] public Rigidbody carRigidBody;


  
    private void HandleMotor()
    {

        // Calculate the current speed of the car in km/h
        carRigidBodySpeed = carRigidBody.velocity.magnitude * 3.6f;

        // Car can only accelerate up to a certain km/h 
        if (carRigidBodySpeed < carRigidBodyTopSpeed)
        {
            // Car accelerates depending on motorForce
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;

            // If the spacebar is pressed the car breaks
            currentbreakForce = isBreaking ? breakForce : 0f;
            ApplyBreaking();
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0 * motorForce;
            frontRightWheelCollider.motorTorque = 0* motorForce;
            rearLeftWheelCollider.motorTorque = 0* motorForce;
            rearRightWheelCollider.motorTorque = 0* motorForce;

            // If the spacebar is pressed even beyond the max speed 
            currentbreakForce = isBreaking ? breakForce : 0f;
            ApplyBreaking();
        }
    }
    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }
    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    private void UpdateWheels()
    {
        // Update the visual of every single Wheel
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos; 
    }
    // Explain what is DownForce
    private void addDownForce()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude);
    }
    private void Update()
    {
        HandleMotor();
        GetInput();
        HandleSteering();
        UpdateWheels();
        addDownForce();

    }
    public void Start()
    {
        speedThreshold = (int)((float)speedThreshold / 3.6f);
        frontLeftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
        frontRightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
        rearLeftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
        rearRightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, stepsBelowThreshold, stepsAboveThreshold);
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);

    }
}
