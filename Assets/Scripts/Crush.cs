using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    public GameObject debrisPrefab;

    public Material[] materials;

    public LayerMask mask;

    public int debrisCountPer = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((mask & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            CrushDebris();
        }
        //CrushDebris();
    }

    void CrushDebris()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            for (int j = 0; j < debrisCountPer; j++)
            {
                var obj = Instantiate(debrisPrefab, transform.position, Quaternion.identity);

                obj.GetComponent<MeshRenderer>().sharedMaterial = materials[i];

                float randScale = Random.Range(0.1f, 10f);

                obj.transform.localScale = Vector3.one * randScale;

                obj.GetComponent<Rigidbody>().mass = randScale;
            }
        }

        Debug.Log(transform.name + " was crushed!");

        Destroy(gameObject);
    }
}
