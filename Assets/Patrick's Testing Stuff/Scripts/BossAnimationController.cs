// Written By Patrick Leonard

using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

# nullable enable

public class BossAnimationController : MonoBehaviour
{
    [Header("Idle Settings")]
    public float IdleSpinSpeed = 1;
    public float bobFrequency = 4;
    public float bobAmplitude = 5;
    public float snapbackSpeed = 1;
    

    [Header("Target Tracking Setting")]
    public Transform? PlayerTarget = null;
    public float snapSpeed = 1;

    [Header("Constant Spin Setting")]
    public float constantSpinSpeed = 1;
    public float spinSmoothSpeed = 5f;

    [Header("Debugging")]
    [SerializeField]
    private bool DEBUG_STOP = false;
    [SerializeField]
    private AnimationState currentState;

    private Dictionary<AnimationState, Action> animationModes = new();
    private Vector3 origin_pos;
    private Quaternion origin_rot;
    private float? targetYaw;

    // What animation states can we ask this to do?
    public enum AnimationState
    {
        None,
        Idle,
        TowardsPlayer,
        ConstantSpin,
        ResetAnimation
    }

    public void SetAnimationState(AnimationState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    private void None()
    {
        // Do Nothing
    }

    private void ResetAnimation()
    {
        transform.position = origin_pos;
        transform.rotation = origin_rot;
        SetAnimationState(AnimationState.None);
    }

    private void Idle()
    {
        // Keep a copy of our current rotation.
        Vector3 currentRot = transform.eulerAngles;

        // Interpolate our new X angle.
        float newX = Mathf.LerpAngle(currentRot.x, -60.0f, Time.deltaTime * snapbackSpeed);

        // Set our object's rotation using our new X-value, maintaining our other rotations.
        transform.eulerAngles = new Vector3(newX, currentRot.y, currentRot.z);

        // Apply a neat little spin effect.
        transform.Rotate(new Vector3(0, 0, 1 * IdleSpinSpeed * Time.deltaTime));

        // How far up or down should we move?
        float updatedYPos = transform.position.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        // Set our object's position using our new Y-value, maintaining our other axis' positions.
        transform.position = new Vector3(transform.position.x, updatedYPos, transform.position.z);

    }

    private void TowardsPlayer()
    {
        if (PlayerTarget == null) Debug.LogError("NULL!");
        // Magic. Pure magic. I have no idea.
        Quaternion lookOnLook = Quaternion.LookRotation(PlayerTarget!.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * snapSpeed);
    }

    private void ConstantSpin()
    {
        // If our target yaw is null, grab our current yaw. Otherwise, just use the target yaw.
        float localTargetYaw = targetYaw ?? transform.rotation.eulerAngles.y;

        // Reset our position
        transform.position = origin_pos;

        // Move our desired yaw forward.
        targetYaw = Mathf.Repeat(localTargetYaw + constantSpinSpeed * Time.deltaTime, 360f);

        // Save a copy of our current rotation.
        Vector3 baseEuler = transform.rotation.eulerAngles;


        // Calculate what our actual yaw is going to be.
        float smoothYaw = Mathf.LerpAngle(baseEuler.y, localTargetYaw, Time.deltaTime * spinSmoothSpeed);
        float smoothPitch = Mathf.LerpAngle(baseEuler.x, 0f, Time.deltaTime);
        float smoothRoll = Mathf.LerpAngle(baseEuler.z, 0f, Time.deltaTime);

        // Save a copy of our current rotation and apply our new yaw.
        transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, smoothRoll);
    }

    void Start()
    {
        if (PlayerTarget == null) Debug.LogError("NULL!");

        origin_pos = transform.position;
        origin_rot = transform.rotation;

        animationModes = new()
        {
            {AnimationState.None, None },
            {AnimationState.Idle, Idle },
            {AnimationState.TowardsPlayer, TowardsPlayer },
            {AnimationState.ResetAnimation, ResetAnimation },
            {AnimationState.ConstantSpin, ConstantSpin}
        };
    }

    void Update()
    {
        if (DEBUG_STOP) return;
        if (currentState != AnimationState.ConstantSpin) targetYaw = null;
        animationModes[currentState].Invoke();
    }
}
