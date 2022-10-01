using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCruiser : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;
    public float shipSpeed = 20;
    public float gravity = 10;
    public Transform gravityPoint;

    Vector3 moveAmount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Vector3 bodyForward = transform.forward;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(bodyForward, direction) * transform.rotation, 0.5f * Time.deltaTime);
        }
        else if (target == null && transform.parent != null)
        {
            transform.parent = null;
        }
    }

    private void FixedUpdate()
    {
        foreach (GravityObject rb in GravityObject.gravityObjects)
        {
            if (rb.currentShip == this.transform)
                FakeGravity(rb.rb);
        }
        if (target != null)
        {
            Vector3 movementDir = (target.transform.position - transform.position).normalized;

            if (moveAmount.x < movementDir.x)
                moveAmount = Vector3.Lerp(moveAmount, movementDir * shipSpeed, 0.5f * Time.deltaTime);
            else
                moveAmount = movementDir * shipSpeed;
            rb.MovePosition(transform.position + moveAmount * Time.fixedDeltaTime);
        }
        else
        {
            moveAmount = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<GravityObject>(out GravityObject obj))
        {
            obj.currentShip = this.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<GravityObject>(out GravityObject obj))
        {
            obj.currentShip = null;
        }
    }

    void FakeGravity(Rigidbody rbToAttract)
    {
        if (rbToAttract.transform.parent == this.transform)
        {
            rbToAttract.transform.parent = null;
        }
        Vector3 targetDir = (rbToAttract.transform.up + gravityPoint.up).normalized;
        Vector3 bodyUp = rbToAttract.transform.up;
        if (rbToAttract.transform.tag == "Player" || rbToAttract.transform.tag == "Creature")
        {

            rbToAttract.transform.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * rbToAttract.transform.rotation;

        }
        rbToAttract.AddForce(gravityPoint.up * -gravity);
        if (moveAmount != Vector3.zero)
            rbToAttract.MovePosition(rbToAttract.transform.position + moveAmount * Time.fixedDeltaTime);
    }
}
