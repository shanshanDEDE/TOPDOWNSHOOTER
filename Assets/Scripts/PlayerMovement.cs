using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;

    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;

    [Header("移動參數")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float trunSpeed;
    [SerializeField] private float gravityScale = 9.81f;
    private float speed;
    private float verticalVelocity;

    public Vector2 moveInput { get; private set; }
    private Vector3 movementDirection;

    private bool isRunning;


    private void Start()
    {
        player = GetComponent<Player>();

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = walkSpeed;

        //定義新版控制器事件
        AssignInputEvents();
    }

    private void Update()
    {
        //移動
        ApplyMovement();
        //瞄準面相準星
        ApplyRotation();
        //動畫
        AnimatorControllers();

    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        //第三,四個參數是線性插值
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void ApplyRotation()
    {
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, trunSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);

        //重力
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded == false)
        {
            //Edit的projectSetting那邊有個physics/Gravity設定,預設為9.81所以這邊用9.81
            verticalVelocity -= gravityScale * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
        {
            verticalVelocity = -0.5f;
        }
    }

    #region 新版輸入系統

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Charcater.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Charcater.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Charcater.Run.performed += context =>
        {
            speed = runSpeed;
            isRunning = true;
        };

        controls.Charcater.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;

        };
    }

    #endregion
}
