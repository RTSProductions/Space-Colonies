using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    float rate = 1;

    float waitKillPlayer;



    // Start is called before the first frame update
    void Start()
    {
        if (waitKillPlayer > 0 && Time.time >= waitKillPlayer)
        {
            Destroy(FindObjectOfType<CharacterController>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rate = GetComponent<Rigidbody>().mass / 20000;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.transform.tag != "Black Hole" && collision.collider.transform.tag != "Player" && collision.collider.transform.tag != "Creature")
        {
            if (collision.collider.transform.localScale.x < Vector3.one.x)
            {
                Destroy(collision.collider.gameObject);
            }
            else
            {
                rate = (GetComponent<Rigidbody>().mass - collision.rigidbody.mass) / 5000;

                if (collision.collider.transform.tag == "Star")
                {
                    Light star = collision.collider.GetComponentInChildren<Light>();
                    star.intensity = Mathf.Lerp(star.intensity, star.intensity - (star.intensity / 5), rate * Time.deltaTime);
                }
                GetComponent<Rigidbody>().mass = Mathf.Lerp(GetComponent<Rigidbody>().mass, GetComponent<Rigidbody>().mass + collision.rigidbody.mass - (collision.rigidbody.mass / 5), rate * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (GetComponent<Rigidbody>().mass * 0.0039f), rate * Time.deltaTime);
                GetComponent<CelestialBody>().raiuds = GetComponent<Rigidbody>().mass * 0.0039f;
                GetComponent<CelestialBody>().surfaceGravity = GetComponent<Rigidbody>().mass / 10;

                collision.rigidbody.mass = Mathf.Lerp(collision.rigidbody.mass, collision.rigidbody.mass - (collision.rigidbody.mass / 5), rate * Time.deltaTime);
                collision.collider.transform.localScale = Vector3.Lerp(collision.collider.transform.localScale, Vector3.one * (collision.collider.transform.localScale.x - collision.collider.transform.localScale.x / 5), rate * Time.deltaTime);
            }
        }
        else if (collision.collider.transform.tag == "Player")
        {
            rate = (GetComponent<Rigidbody>().mass - collision.rigidbody.mass) / 10000;
            collision.collider.GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(collision.collider.GetComponentInChildren<Camera>().fieldOfView, 179, rate * Time.deltaTime);
            collision.collider.GetComponentInChildren<Camera>().farClipPlane = Mathf.Lerp(collision.collider.GetComponentInChildren<Camera>().farClipPlane, 0.5f, rate * Time.deltaTime);

            collision.collider.GetComponentInChildren<Camera>().backgroundColor = Color.Lerp(collision.collider.GetComponentInChildren<Camera>().backgroundColor, Color.black, (rate / 12) * Time.deltaTime);

            if (waitKillPlayer == 0)
                waitKillPlayer = Time.time + 15;

            if (Time.time >= waitKillPlayer)
            {
                Destroy(collision.collider.gameObject);
            }
        }
        else if (collision.collider.transform.tag == "Creature")
        {
            rate = (GetComponent<Rigidbody>().mass - collision.rigidbody.mass) / 10000;

            collision.collider.GetComponent<LiveEntity>().Die(" Was spegitified");
        }
    }
}
