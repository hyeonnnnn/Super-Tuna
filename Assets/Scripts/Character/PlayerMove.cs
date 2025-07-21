using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private const float maxDashGauge = 100f;

    private float maxSpeed = 3f;
    private float rotateSpeed = 0.3f;
    private float _dashGauge;
    private float lerpTimeCount = 0f;
    public bool isDash { get; private set; }
    private bool isDashKeyDown = false;
    private bool isDead = false;

    public float DashGuage {
        get
        {
            return _dashGauge;
        }
        private set
        {
            if (value < 0) value = 0;
            if(value > 100 ) value = maxDashGauge;
            _dashGauge = value;
            OnDashGaugeChanged?.Invoke(value, maxDashGauge);
        } 
    }

    private Rigidbody rigid;
    private HungerSystem hunger;

    private Vector2 targetVelocity;
    private Quaternion targetRotation;

    public static event Action<float, float> OnDashGaugeChanged;
    private Coroutine currentDashCoroutine;

    [SerializeField] private GameObject tunaPrefab;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        targetRotation = tunaPrefab.transform.rotation;
        currentDashCoroutine = StartCoroutine(RecoverCoroutine());
        HungerSystem.OnDeath += DeathMove;
    }

    private void Update()
    {
        Move();
        if (isDead) return;
        Rotate();
        TryDash();
    }

    private void OnDestroy()
    {
        HungerSystem.OnDeath -= DeathMove;
    }

    //playerInput event�Լ�
    public void OnMove(InputValue input)
    {
        if(isDead) return;
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
        //(���� ������) - (����� ��)���� �����ؼ� ���� ���̸� deltatime*10�� ���ؼ� 0.1�ʿ� 0.5f��ŭ�� ������ �̷�� ������ ���
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
        lerpTimeCount += Time.deltaTime * rotateSpeed;

        tunaPrefab.transform.localRotation = applyRotation;
    }

    private void ChangeTargetVelocity(Vector2 input)
    {
        targetVelocity = input * maxSpeed;
    }

    private void ChangeTargetRotation(Vector2 input)
    {
        Vector3 changedRotation = targetRotation.eulerAngles;

        float maxRightRotateAngle = 90.1f;
        float maxLeftRoateAngle = 269.9f;
        float maxVerticalRotateAngle = 30;

        lerpTimeCount = 0;
        if (input.x != 0)
        {
            changedRotation.y = Mathf.Sign(input.x) == 1 ? maxRightRotateAngle : maxLeftRoateAngle;
        }

        if(input.y != 0)
        {
            changedRotation.x = Mathf.Sign(input.y) * -maxVerticalRotateAngle;
        }
        else
        {
            changedRotation.x = tunaPrefab.transform.localRotation.eulerAngles.x;
        }

        if(input.x != 0 && input.y == 0)
        {
            changedRotation.x = 0;
        }

        targetRotation = Quaternion.Euler(changedRotation);
    }

    //playerInput event�Լ�
    public void OnSprint(InputValue input)
    {
        if (isDead) return;
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

    private void DeathMove()
    {
        isDead = true;
        tunaPrefab.transform.localRotation = Quaternion.Euler(0f,90f, 0f);
        targetVelocity = Vector2.zero;
    }

    public bool GetPlayerIsDead()
    {
        return isDead;
    }
}
