using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewObject : MonoBehaviour
{
    [Range (0.01f, 10)]
    public float raidus = 1f;

    public ViewType viewType;

    public ViewColor viewColor;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        if (viewColor == ViewColor.Black)
        {
            Gizmos.color = Color.black;
        }
        if (viewColor == ViewColor.Blue)
        {
            Gizmos.color = Color.blue;
        }
        if (viewColor == ViewColor.Green)
        {
            Gizmos.color = Color.green;
        }
        if (viewColor == ViewColor.Grey)
        {
            Gizmos.color = Color.grey;
        }
        if (viewColor == ViewColor.Gray)
        {
            Gizmos.color = Color.gray;
        }
        if (viewColor == ViewColor.Red)
        {
            Gizmos.color = Color.red;
        }
        if (viewColor == ViewColor.White)
        {
            Gizmos.color = Color.white;
        }
        if (viewColor == ViewColor.Clear)
        {
            Gizmos.color = Color.clear;
        }
        if (viewColor == ViewColor.Cyan)
        {
            Gizmos.color = Color.cyan;
        }
        if (viewColor == ViewColor.Magenta)
        {
            Gizmos.color = Color.magenta;
        }

        if (viewType == ViewType.Cube)
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(raidus,raidus,raidus));
            
        }
        if (viewType == ViewType.WireSphere)
        {
            Gizmos.DrawWireSphere(this.transform.position, raidus);
        }
        if (viewType == ViewType.Sphere)
        {
            Gizmos.DrawSphere(this.transform.position, raidus);
        }

    }
}
public enum ViewType
{
    Cube, WireSphere, Sphere
}
