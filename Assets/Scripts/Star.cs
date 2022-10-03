using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag != "Black Hole")
        {
            if (collision.collider.TryGetComponent<LiveEntity>(out LiveEntity entity))
            {
                entity.Die(" Was burned by " + gameObject.name);
            }
            else
                Destroy(collision.collider.gameObject);
        }
    }
}
