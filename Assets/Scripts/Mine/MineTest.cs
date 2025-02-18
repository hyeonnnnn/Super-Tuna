using UnityEngine;

public class MineTest : MonoBehaviour
{
    public float moveSpeed = 5f;  // 이동 속도
    public float rotationSpeed = 10f;  // 회전 속도
    public float acceleration = 0.1f;  // 이동 가속도
    public float deceleration = 0.1f; // 이동 감속도

    private Vector3 moveDirection;  // 이동 방향
    private Vector3 currentVelocity;  // 현재 속도
    private Rigidbody rb;  // Rigidbody 컴포넌트

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 가져오기
    }

    void Update()
    {
        HandleMovement();  // 이동 처리
    }

    // 이동 처리
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A, D
        float vertical = Input.GetAxis("Vertical");      // W, S

        // 이동 방향 계산 (대각선 처리)
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 이동 가속도 및 감속도 적용
        if (moveDirection.magnitude > 0.1f)  // 이동 입력이 있을 때
        {
            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * moveSpeed, acceleration * Time.deltaTime);
        }
        else  // 이동 입력이 없을 때
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // Rigidbody를 이용하여 이동
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }
}
