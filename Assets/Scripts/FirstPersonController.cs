using UnityEngine;
using System.Collections;
public class FirstPersonController : MonoBehaviour {

    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public float walkSpeed = 8f;
    public float jumpForce = 220;
    public float jetpackForce = 10;
    public LayerMask groundMask;

    public Transform groundCheck;

    bool flyMode = false;

    bool jetpacking;

    bool grounded;
    Transform cameraT;
    float verticalLookRotation;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    void Start()
    {
        cameraT = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {

        float speed = walkSpeed;

        if (flyMode)
            speed = 100;

        if (jetpacking)
            speed *= 3;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * speed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.up * jumpForce);
        }
        else if (Input.GetButtonDown("Jump") && !grounded)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.up * jetpackForce * Time.deltaTime);
            jetpacking = true;
        }
        else if (Input.GetButton("Jump") && jetpacking)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.up * jetpackForce * Time.deltaTime);
            jetpacking = true;
        }
        else if (Input.GetButtonUp("Jump") && jetpacking)
        {
            jetpacking = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && !flyMode)
        {
            flyMode = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && flyMode)
        {
            flyMode = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !grounded)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(-transform.up * jetpackForce * Time.deltaTime);
        }

        grounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        
    }
    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}