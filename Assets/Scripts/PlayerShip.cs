using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public float shipSpeed = 8f;
    public float shipUpForce = 1500;

    bool jetpacking;

    Transform cameraT;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    void Start()
    {
        cameraT = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        transform.Rotate(Vector3.forward * Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY);

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Vertical"), 0, -Input.GetAxisRaw("Horizontal")).normalized;
        Vector3 targetMoveAmount = moveDir * shipSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButton("Jump"))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.up * shipUpForce * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(-transform.up * shipUpForce * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.left * 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.left * -100 * Time.deltaTime);
        }


    }
    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
