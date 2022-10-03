using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveEntity : MonoBehaviour
{
    public Species species;

    public float health = 100;

    public List<GameObject> drops = new List<GameObject>();

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log(gameObject.name + " has died");
        gameObject.name = "Dead";

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        foreach(GameObject drop in drops)
        {
            Instantiate(drop, transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
    }

    public void Die(string message)
    {
        Debug.Log(gameObject.name + message);
        gameObject.name = "Dead";

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        foreach (GameObject drop in drops)
        {
            Instantiate(drop, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > 100)
            health = 100;
    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(30);

        Destroy(gameObject);
    }
}
