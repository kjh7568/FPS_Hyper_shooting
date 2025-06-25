using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int RELOAD = Animator.StringToHash("Reload");
    private static readonly int SHOOT = Animator.StringToHash("Shoot");
    private CharacterController characterController;

    [Header("Movement Settings")] private const float DASH_SPEED = 20f;
    private bool isCanDash = true;
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Mouse Settings")] [SerializeField]
    private float mouseSensitivity = 50f;
    
    [Header("사운드 설정")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;
    
    public float MouseSensitivity
    {
        get => mouseSensitivity;
        set => mouseSensitivity = value;
    }

    private float cameraPitch = 0f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private LayerMask groundMask;

    [Header("Animation Settings")]
    [SerializeField]private Animator animator;
    
    public bool isOpenPanel = false;

    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        SetCursor();
    }

    private void Update()
    {
        if (isOpenPanel) return;
        
        MovePlayer();
        ApplyGravity();
        RotatePlayer();
    }

    private void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MovePlayer()
    {
        if (Player.localPlayer == null) return;

        Player player = Player.localPlayer;

        Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 moveDirection = transform.TransformDirection(inputAxis.normalized);

        float moveSpeed = CalculateMoveSpeed(player);
        characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));

        HandleFootstepSound(inputAxis);
        HandleJump();
        HandleDash(moveDirection);
    }

    private float CalculateMoveSpeed(Player player)
    {
        return player.playerStat.moveSpeed *
               player.inventory.EquipmentStat.multiplierMovementSpeed *
               player.coreStat.coreMovementSpeed;
    }

    private void HandleFootstepSound(Vector3 inputAxis)
    {
        bool isMoving = inputAxis.magnitude > 0.1f && IsGrounded();

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.clip = footstepClip;
                footstepSource.loop = true;
                footstepSource.Play();
            }
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }
    }

    private void HandleDash(Vector3 moveDirection)
    {
        if (Input.GetKey(KeyCode.LeftShift) && isCanDash)
        {
            StartCoroutine(Dash(moveDirection));
        }
    }

    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, groundMask);
    }
    
    private void RotatePlayer()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) *
                             (mouseSensitivity * Time.deltaTime);

        transform.Rotate(Vector3.up * mouseInput.x);

        cameraPitch -= mouseInput.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private IEnumerator Dash(Vector3 direction)
    {
        float elapsed = 0f;
        float dashDuration = 0.2f;

        isCanDash = false;

        while (elapsed < dashDuration)
        {
            characterController.Move(direction * (DASH_SPEED * Time.deltaTime));
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(DashCooldown_Coroutine());
    }

    private IEnumerator DashCooldown_Coroutine()
    {
        var coolTime = Player.localPlayer.playerStat.dashCoolTime * (1 - Player.localPlayer.inventory.EquipmentStat.dashCooldownReduction);
        
        yield return new WaitForSeconds(coolTime);
        isCanDash = true;
    }

    #region Animation
    
    public void SetShootAnimation(bool shoot)
    {
        animator.SetBool(SHOOT, shoot);
    }

    public void SetReloadAnimation()
    {
        animator.SetTrigger(RELOAD);
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.speed = 1f + speed;
    }

    #endregion
}