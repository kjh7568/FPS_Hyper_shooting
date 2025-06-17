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

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckObjectUnderCursor();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipItem();
        }
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

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("스페이스 바 눌림!");
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isCanDash)
        {
            StartCoroutine(Dash(move));
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
    
    private void CheckObjectUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // 5f는 감지 거리
        {
            GameObject target = hit.collider.gameObject;
            Debug.Log($"커서 아래 감지된 오브젝트: {target.name}");

            // 예시: DroppedItem 스크립트가 붙어있는지 확인
            DroppedItem item = target.GetComponent<DroppedItem>();
            if (item != null)
            {
                item.PrintArmor();
            }
        }
        else
        {
            Debug.Log("감지된 오브젝트 없음");
        }
    }

    private void EquipItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f)) // 5f는 감지 거리
        {
            GameObject target = hit.collider.gameObject;

            DroppedItem item = target.GetComponent<DroppedItem>();
            
            Player.localPlayer.inventory.EquipArmor(item.dropedItem);
        }
        else
        {
            Debug.Log("감지된 오브젝트 없음");
        }
    }
}