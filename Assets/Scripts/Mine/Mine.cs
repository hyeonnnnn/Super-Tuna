using UnityEngine;

public class Mine : MonoBehaviour
{
    public float damage = 30f;
    public float pushForce = 10f;
    public GameObject explosionEffect;
    private bool isTriggered = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isTriggered || !collision.gameObject.CompareTag("Player")) return;

        isTriggered = true;
        Explode();
        ApplyDamageAndPush(collision.gameObject);
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void ApplyDamageAndPush(GameObject player)
    {
        HungerSystem hungerSystem = player.GetComponent<HungerSystem>();
        if (hungerSystem != null)
        {
            hungerSystem.DecreaseHunger(damage);
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDirection = player.transform.position - transform.position;
            pushDirection.Normalize();
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}