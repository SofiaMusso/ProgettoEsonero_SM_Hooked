using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght;
    private float startPos;

    public GameObject cam;

    public float parallaxEffect;
    public float camSpeed;

    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (temp < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
