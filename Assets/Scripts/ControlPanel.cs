using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public Transform platform;

    bool opening = true;

    public LayerMask controllPanel;

    public Transform CameraPoint;

    public GameObject ship;

    public GameObject player;

    Vector3 cameraPoint;

    Quaternion cameraRotation;

    bool inShip = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opening == true && platform.localRotation.y != -101.914f)
        {
            platform.localRotation = Quaternion.Slerp(platform.localRotation, Quaternion.Euler(0, -101.914f, 0), 1 * Time.deltaTime);
        }
        else if (opening == false && platform.localRotation.y != 0)
        {
            platform.localRotation = Quaternion.Slerp(platform.localRotation, Quaternion.Euler(0, -0, 0), 1 * Time.deltaTime);
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
                    SwitchPlayer();
                    return;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && inShip == true)
        {
            SwitchPlayer();
        }
    }

    void SwitchPlayer()
    {
        if (inShip == false)
        {
            cameraPoint = Camera.main.transform.localPosition;
            cameraRotation = Camera.main.transform.localRotation;

            Camera.main.transform.parent = CameraPoint;
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

            player.transform.parent = ship.transform;
            player.SetActive(false);

            ship.GetComponent<PlayerShip>().enabled = true;
            inShip = true;
            ship.GetComponent<Rigidbody>().isKinematic = true;
            ship.GetComponent<Rigidbody>().freezeRotation = true;
            ship.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            player.SetActive(true);
            player.transform.parent = null;

            ship.GetComponent<PlayerShip>().enabled = false;

            Camera.main.transform.parent = player.transform;
            Camera.main.transform.localPosition = cameraPoint;
            Camera.main.transform.localRotation = cameraRotation;
            inShip = false;
            ship.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}
