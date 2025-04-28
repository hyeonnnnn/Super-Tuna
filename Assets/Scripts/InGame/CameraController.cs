using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed = 5.0f;
    [SerializeField] GameObject player;

    private void FixedUpdate()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        this.transform.Translate(moveVector);
    }

}
