using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [Header("Idle Settings")]
    public float SpinSpeed = 1;
    public float bobFrequency = 4;
    public float bobAmplitude = 5;
    public float snapbackSpeed = 1;
    

    [Header("Target Tracking Setting")]
    public Transform PlayerTarget;
    public float snapSpeed = 1;

    [Header("Debugging")]
    [SerializeField]
    private bool DEBUG_STOP = false;
    [SerializeField]
    private AnimationState currentState;

    private Dictionary<AnimationState, Action> animationModes;
    private Vector3 origin_pos;
    private Quaternion origin_rot;

    // What animation states can we ask this to do?
    public enum AnimationState
    {
        None,
        Idle,
        TowardsPlayer,
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
        transform.Rotate(new Vector3(0, 0, 1 * SpinSpeed * Time.deltaTime));

        // How far up or down should we move?
        float updatedYPos = transform.position.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        // Set our object's position using our new Y-value, maintaining our other axis' positions.
        transform.position = new Vector3(transform.position.x, updatedYPos, transform.position.z);

    }

    private void TowardsPlayer()
    {
        // Magic. Pure magic. I have no idea.
        Quaternion lookOnLook = Quaternion.LookRotation(PlayerTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * snapSpeed);
    }

    void Start()
    {
        origin_pos = transform.position;
        origin_rot = transform.rotation;

        animationModes = new()
        {
            {AnimationState.None, None },
            {AnimationState.Idle, Idle },
            {AnimationState.TowardsPlayer, TowardsPlayer },
            {AnimationState.ResetAnimation, ResetAnimation }
        };
    }

    void Update()
    {
        if (DEBUG_STOP) return;
        animationModes[currentState].Invoke();
    }
}
