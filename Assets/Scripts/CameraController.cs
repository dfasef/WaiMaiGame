using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 40;
    public float zoomSpped = 50;

    private Camera mainCamera;

    public float minSize = 5;
    public float maxSize = 10;
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        transform.Translate(new Vector3(horizontal * speed, vertical * speed , 0) * Time.deltaTime, Space.World);
        mainCamera.orthographicSize = Mathf.Clamp
            (mainCamera.orthographicSize - scroll * zoomSpped * Time.deltaTime,minSize,maxSize);

    }
}
