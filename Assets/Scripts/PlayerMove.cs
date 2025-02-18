using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    private Vector2 targetVelocity;
    private Quaternion targetRotation;
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
    private float lerpTimeCount = 0f;

    [SerializeField] private GameObject tunaPrefab;
    private Rigidbody rigid;

    [SerializeField] Slider dashDebugSlider;


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        maxSpeed = 3f;
        targetRotation = tunaPrefab.transform.rotation;
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

    //playerInput event함수
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
        //(계산된 보간값) - (계산전 값)으로 보간해서 나온 차이를 deltatime*10을 곱해서 0.1초에 0.5f만큼의 보간이 이루어 지도록 계산
        currentVelocity = (lerpedVector - currentVelocity) * Time.deltaTime * 10 + currentVelocity;

        if (Mathf.Abs((currentVelocity - applyVelocity).sqrMagnitude) <= 0.1f)
        {
            currentVelocity = applyVelocity;
        }

        rigid.linearVelocity = currentVelocity;
    }

    private void Rotate()
    {
        Quaternion applyRotation = tunaPrefab.transform.localRotation;
        applyRotation = Quaternion.Slerp(applyRotation, targetRotation, lerpTimeCount);
        lerpTimeCount += Time.deltaTime * 0.07f;

        tunaPrefab.transform.localRotation = applyRotation;
    }

    private void ChangeTargetVelocity(Vector2 input)
    {
        targetVelocity = input * maxSpeed;
    }

    private void ChangeTargetRotation(Vector2 input)
    {
        Vector3 changedRotation = targetRotation.eulerAngles;
        lerpTimeCount = 0;
        if (input.x != 0)
        {
            changedRotation.y = Mathf.Sign(input.x) == 1 ? 90.1f : 269.9f;
        }

        if(input.y != 0)
        {
            changedRotation.x = Mathf.Sign(input.y) * -30;
        }
        else
        {
            changedRotation.x = tunaPrefab.transform.localRotation.eulerAngles.x;
        }

        if(input.x != 0 && input.y == 0)
        {
            changedRotation.x = 0;
        }

        Debug.Log(changedRotation);
        targetRotation = Quaternion.Euler(changedRotation);
    }

    //playerInput event함수
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
        if(!isDash && isDashKeyDown && DashGuage > 0 && targetVelocity.sqrMagnitude != 0)
        {
            isDash = true;
            StopCoroutine(currentDashCoroutine);
            currentDashCoroutine = StartCoroutine(DashCoroutine());
        }
        else if(isDash && (!isDashKeyDown || DashGuage <= 0 || targetVelocity.sqrMagnitude == 0))
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
        dashDebugSlider.value = DashGuage;
    }

    


}
