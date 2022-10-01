using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickUp : MonoBehaviour
{
    public LineRenderer lr;

    public LayerMask whatIsGrabable;
    public Transform camera;
    private static Vector3 currentGrapplePosition;
    private float maxDistance = 100f;
    private float castRaiuds = 2;
    private SpringJoint joint;
    Rigidbody rb;

    public static bool grappleGrab = false;

    public static bool up = false;

    public Transform objectHolder;

    private static Vector3 objectHolderPosition;
    private Transform k;

    // Start is called before the first frame update
    void Start()
    {
        objectHolderPosition = objectHolder.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }

        objectHolderPosition = objectHolder.position;
        if (joint != null)
        {
            joint.connectedAnchor = objectHolderPosition;
        }


    }
    void LateUpdate()
    {
        DrawRope();
    }
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.SphereCast(camera.position, castRaiuds, camera.forward, out hit, maxDistance, whatIsGrabable) && grappleGrab == false)
        {
            Debug.Log(hit.transform.name + "Was Grabbed");

            rb = hit.transform.gameObject.GetComponent<Rigidbody>();
            if (rb != null && up == false && rb.isKinematic == false && rb.mass <= 20)
            {
                joint = hit.transform.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = objectHolderPosition;

                float distanceFromPoint = Vector3.Distance(hit.transform.position, objectHolderPosition);

                //The distance grapple will try to keep from grapple point. 
                joint.maxDistance = 0;
                joint.minDistance = 0;

                //Adjust these values to fit your game.
                joint.spring = 6.52f;
                joint.damper = 2f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
                currentGrapplePosition = rb.transform.position;
                grappleGrab = true;
            }

        }
    }



    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        //lr.positionCount = 0;
        Destroy(joint);
        rb = null;
        lr.positionCount = 0;
        grappleGrab = false;
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = rb.transform.position;

        lr.SetPosition(0, objectHolder.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    //public Vector3 GetGrapplePoint()
    //{
    //    return ;
    //}
}
