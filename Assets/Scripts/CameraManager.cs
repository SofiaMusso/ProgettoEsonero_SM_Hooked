using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float speed;

    private void FixedUpdate()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
