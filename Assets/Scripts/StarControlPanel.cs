using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarControlPanel : MonoBehaviour
{
    public Transform platform;

    bool opening = true;

    public LayerMask controllPanel;

    public float turnAmount = 101.914f;

    public GameObject ship;

    bool flying = false;

    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (opening == true && platform.localRotation.x != turnAmount)
        {
            platform.localRotation = Quaternion.Slerp(platform.localRotation, Quaternion.Euler(turnAmount, 0, 0), 1 * Time.deltaTime);
        }
        else if (opening == false && platform.localRotation.x != 0)
        {
            platform.localRotation = Quaternion.Slerp(platform.localRotation, Quaternion.Euler(0, 0, 0), 1 * Time.deltaTime);
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, controllPanel))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    opening = !opening;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    if (flying == false)
                        StartFly();
                    else
                        EndFly();
                    return;
                }
            }
        }
    }

    void StartFly()
    {
        if (targetTransform != null)
        {
            ship.GetComponent<StarCruiser>().target = targetTransform;
            flying = true;
        }
        else
        {
            Debug.Log("Pick A Destination");
        }
    }

    void EndFly()
    {
        ship.GetComponent<StarCruiser>().target = null;
        flying = false;
    }

}
