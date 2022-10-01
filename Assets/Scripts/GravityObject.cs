using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public Rigidbody rb;

    [HideInInspector]
    public Transform currentShip;

    public static List<GravityObject> gravityObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if (gravityObjects == null)
            gravityObjects = new List<GravityObject>();

        gravityObjects.Add(this);
    }

    void OnDisable()
    {
        gravityObjects.Remove(this);
    }
}
