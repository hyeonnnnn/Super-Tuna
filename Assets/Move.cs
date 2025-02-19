using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    private Vector3 moveDirection = Vector3.down;
        
    private void Start()
    {
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while (true)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
