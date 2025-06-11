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

    private float cameraPitch = 0f;
    [SerializeField] private Transform playerCamera;

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

        if (Input.GetKey(KeyCode.Space))
        {
            // Debug.Log(characterController.isGrounded);
        }

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
        Vector3 inputAxis = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 move = transform.TransformDirection(inputAxis);

        characterController.Move(move * (Player.localPlayer.playerStat.moveSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isCanDash)
        {
            StartCoroutine(Dash(move));
        }
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

    private void RotatePlayer()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) *
                             (mouseSensitivity * Time.deltaTime);

        transform.Rotate(Vector3.up * mouseInput.x);

        cameraPitch -= mouseInput.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
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
        yield return new WaitForSeconds(Player.localPlayer.playerStat.dashCoolTime);
        isCanDash = true;
    }

    public void SetShootAnimation(bool shoot)
    {
        animator.SetBool(SHOOT, shoot);
    }

    public void SetReloadAnimation()
    {
        animator.SetTrigger(RELOAD);
    }
}