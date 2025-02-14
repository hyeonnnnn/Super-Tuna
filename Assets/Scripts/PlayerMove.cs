using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float maxSpeed;
    private Vector2 targetVelocity;
    private Vector3 targetRotation;
    private bool isDash;
    private float dashGuage;

    [SerializeField] private GameObject tunaPrefab;
    private bool isDead = false;
    
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        maxSpeed = 3f;
    }

    private void Update()
    {
        if(isDead) return;
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector2 currentVelocity = rigid.linearVelocity;

        if (currentVelocity == targetVelocity) return;

        Vector2 lerpedVector = Vector2.Lerp(currentVelocity, targetVelocity, 0.5f);

        currentVelocity = (lerpedVector - currentVelocity) * Time.deltaTime * 10 + currentVelocity;

        if (Mathf.Abs((currentVelocity - targetVelocity).sqrMagnitude) <= 0.1f)
        {
            currentVelocity = targetVelocity;
        }
        rigid.linearVelocity = currentVelocity;
    }

    private void Rotate()
    {

    }

    public void OnMove(InputValue input)
    {
        Vector2 inputDir = input.Get<Vector2>();
        ChangeTargetVelocity(inputDir);
        ChangeTargetRotation(inputDir);
    }

    private void ChangeTargetVelocity(Vector2 input)
    {
        targetVelocity = input * maxSpeed;
        Debug.Log(targetVelocity);
    }

    private void ChangeTargetRotation(Vector2 input)
    {
        Vector3 targetEulerAngle = tunaPrefab.transform.rotation.eulerAngles;

        float VerticalRotation = targetEulerAngle.y;
        float HorizontalRotation = targetEulerAngle.x;

        if(input.x != 0)
        {
            VerticalRotation = Mathf.Sign(input.y) == 1 ? 0 : 180;
        }

        if(input.y != 0)
        {
            HorizontalRotation = Mathf.Sign(input.x) * 30;
        }

        targetEulerAngle.y = VerticalRotation;
        targetEulerAngle.x = HorizontalRotation;
    }

    public void OnSprint(InputValue input)
    {
        bool tryDash;
        if(input.Get() != null)
        {
            tryDash = true;
        }
        else
        {
            tryDash = false;
        }
        Dash(tryDash);

    }

    private void Dash(bool isTryingDash)
    {
        if(isTryingDash && dashGuage > 0)
        {
            isDash = true;
        }
        else
        {
            isDash = false;
        }
    }
}
