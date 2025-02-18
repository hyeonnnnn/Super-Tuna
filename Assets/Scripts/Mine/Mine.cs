using UnityEngine;

public class Mine : MonoBehaviour
{
    public float damage = 30f;
    public float pushForce = 10f;
    public GameObject explosionEffect;
    private bool isTriggered = false;

    [SerializeField] private HungerSystem hungerSystem;

    void Start()
    {
        hungerSystem = FindFirstObjectByType<HungerSystem>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            Explode();
            ApplyDamageAndPush(collision.gameObject);
            isTriggered = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void ApplyDamageAndPush(GameObject player)
    {
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