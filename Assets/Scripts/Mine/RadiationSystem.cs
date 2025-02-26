using System;
using System.Collections;
using UnityEngine;

public class RadiationSystem : MonoBehaviour
{ 
    private readonly float radiationDamage = 5f;
    private readonly float radiationSpreadSpeed = 5f;
    private readonly float radiationSpreadDuration = 300f;

    private void Start()
    {
        StartCoroutine(SpreadRadiation());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HungerSystem playerHunger))
            {
                playerHunger.AddHungerDecrease((int)radiationDamage);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    { 
        if (other.CompareTag("Player") && other.TryGetComponent(out HungerSystem playerHunger))
        { 
            playerHunger.AddHungerDecrease(0);
        }
    }
    
    private IEnumerator SpreadRadiation() 
    { 
        float elapsedTime = 0f;
        while (elapsedTime < radiationSpreadDuration) 
        { 
            transform.localScale += new Vector3(0, 0, radiationSpreadSpeed) * Time.deltaTime; 
            elapsedTime += Time.deltaTime; 
            yield return null;
        }
    }
}