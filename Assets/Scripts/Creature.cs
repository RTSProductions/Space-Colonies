using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

    public float walkSpeed = 8;

    public Rigidbody rb;

    public Transform target;

    Vector3 moveAmount;

    float dist;

    public float visionRadius = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(transform.position, target.position);
        if (dist <= visionRadius)
        {
            Vector3 movementDir = (target.transform.position - transform.position).normalized;
            moveAmount = movementDir * walkSpeed;
        }
    }

    void Move()
    {
        if (dist <= visionRadius)
        {
            rb.MovePosition(transform.position + moveAmount * Time.fixedDeltaTime);
            FaceTarget();
        }
            
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 bodyForward = transform.forward;
        transform.rotation = Quaternion.FromToRotation(bodyForward, direction) * transform.rotation;
    }
}
