using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private float maxSpeed;
    private Vector2 targetVelocity;
    private Vector3 targetRotation;
    public bool isDash { get; private set; }
    private float _dashGuage;
    public float DashGuage {
        get
        {
            return _dashGuage;
        }
        private set
        {
            if (value < 0) value = 0;
            if(value > 100 ) value = 100;
            _dashGuage = value;
        } 
    }
    private Coroutine currentDashCoroutine;
    private bool isDashKeyDown = false;
    private bool isDead = false;

    [SerializeField] private GameObject tunaPrefab;
    private Rigidbody rigid;

    [SerializeField] Slider debugSlider;

    public Action ZeroDashGuageAction;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        ZeroDashGuageAction += TryDash;
        maxSpeed = 3f;
        currentDashCoroutine = StartCoroutine(RecoverCoroutine());
    }

    private void Update()
    {
        DebugFunc();
        if (isDead) return;
        Move();
        Rotate();
        TryDash();
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputDir = input.Get<Vector2>();
        ChangeTargetVelocity(inputDir);
        ChangeTargetRotation(inputDir);
    }

    private void Move()
    {
        Vector2 currentVelocity = rigid.linearVelocity;
        Vector2 applyVelocity = targetVelocity;

        if (isDash) applyVelocity *= 2;
        if (currentVelocity == applyVelocity) return;

        Vector2 lerpedVector = Vector2.Lerp(currentVelocity, applyVelocity, 0.5f);
        currentVelocity = (lerpedVector - currentVelocity) * Time.deltaTime * 10 + currentVelocity;

        if (Mathf.Abs((currentVelocity - applyVelocity).sqrMagnitude) <= 0.1f)
        {
            currentVelocity = applyVelocity;
        }

        rigid.linearVelocity = currentVelocity;
    }

    private void Rotate()
    {

    }

    private void ChangeTargetVelocity(Vector2 input)
    {
        targetVelocity = input * maxSpeed;
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
        if (input.Get() != null)
        {
            isDashKeyDown = true;
        }
        else
        {
            isDashKeyDown = false;
        }
    }


    private void TryDash()
    {
        if(!isDash && isDashKeyDown && DashGuage > 0)
        {
            isDash = true;
            StopCoroutine(currentDashCoroutine);
            currentDashCoroutine = StartCoroutine(DashCoroutine());
        }
        else if(isDash && (!isDashKeyDown || DashGuage <= 0))
        {
            isDash = false;
            StopCoroutine(currentDashCoroutine);
            currentDashCoroutine = StartCoroutine(RecoverCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        while(!isDead)
        {
            DashGuage -= 2;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator RecoverCoroutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.1f);
            DashGuage += 1;
        }
    }

    public void DebugFunc()
    {
        debugSlider.value = DashGuage;
    }

    


}
