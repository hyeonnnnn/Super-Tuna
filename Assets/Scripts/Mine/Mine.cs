using UnityEngine;
using System;
using System.Collections;

public class Mine : MonoBehaviour
{
    public int damage = 30;
    public float pushForce = 10f;
    public GameObject explosionEffect;
    private bool isTriggered = false;
    private Transform playerTransform;

    public event Action<GameObject> ExplodeEvent;

    private void Start()
    {
        StartCoroutine(Despawn());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTriggered || !collision.gameObject.CompareTag("Player")) return;

        isTriggered = true;
        Explode();
        ApplyDamageAndPush(collision.gameObject);
    }

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 3f);
        Destroy(gameObject, 3f);
        ExplodeEvent?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    void ApplyDamageAndPush(GameObject player)
    {
        HungerSystem hungerSystem = player.GetComponent<HungerSystem>();
        if (hungerSystem != null)
        {
            hungerSystem.DecreaseHunger(damage, DyingReason.Mine);
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDirection = player.transform.position - transform.position;
            pushDirection.Normalize();
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private IEnumerator Despawn()
    {
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - playerTransform.position) > 400f)
            {
                ExplodeEvent?.Invoke(gameObject);
                Destroy(gameObject);
                yield break;
            }
            yield return new WaitForSeconds(5f);
        }
    }

    public void ResetMine()
    {
        isTriggered = false;
        gameObject.SetActive(true);
    }
}