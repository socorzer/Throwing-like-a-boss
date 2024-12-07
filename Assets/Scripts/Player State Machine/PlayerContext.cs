using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContext
{

    public PlayerContext(Camera cam,Transform cameraPosition,CharacterController cc, PlayerInput input, AnimationController anim, PlayerData data, Transform transform, LayerMask groundMask)
    {
        Controller = cc;
        Input = input;
        Data = data;   
        Transform = transform;
        GroundMask = groundMask;
        Animation = anim;
        Camera = cam;
        CameraPosition = cameraPosition;
    }
    public void SetVelocityY(float vectorY)
    {
        Velocity = new Vector3(Velocity.x, vectorY, Velocity.z);
    }
    public bool IsMoving { get; set; }
    public bool IsRunning { get; set; }
    public bool IsJumping { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsDead { get; set; }
    public bool IsFreeze { get; set; }
    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public PlayerData Data { get; private set; }
    public Transform Transform { get; private set; }
    public LayerMask GroundMask { get; private set; }
    public AnimationController Animation { get; private set; }
    public Camera Camera { get; private set; }
    public Transform CameraPosition { get; private set; }
    public float CurrentLookX { get; set; }
    public float CurrentLookY { get; set; }
    public float XRotation { get; set; }
    public float CurrentMoveZ { get; set; }
    public float CurrentMoveX { get; set; }
    public float CurrentGravity { get; set; }
    public Vector3 Velocity { get; set; }

}
