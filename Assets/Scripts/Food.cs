using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Species species;

    public float healAmount = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Eat(Creature creature)
    {
        creature.hunger = 0;
        creature.GetComponent<LiveEntity>().Heal(healAmount);
        Destroy(gameObject);
    }
}
