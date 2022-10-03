using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(LiveEntity))]
public class Creature : MonoBehaviour
{
    Species species;

    public float walkSpeed = 8;

    public Rigidbody rb;

    public Transform target;

    Vector3 moveAmount;

    float dist;

    public float visionRadius = 10;

    public float hunger;

    [EnumFlags]
    public Species diet;

    CreatureState currentState;

    // Start is called before the first frame update
    void Start()
    {
        species = GetComponent<LiveEntity>().species;

        currentState = CreatureState.exploring;
    }

    // Update is called once per frame
    void Update()
    {
        hunger += Time.deltaTime * 1 / CreatureAttributes.maxHunger;
        if (target != null)
        {
            if (target.gameObject.name == "Dead")
            {
                target = null;
                currentState = CreatureState.exploring;
                return;
            }
            dist = Vector3.Distance(transform.position, target.position);
            Vector3 movementDir = (target.transform.position - transform.position).normalized;
            moveAmount = movementDir * walkSpeed;
            if (dist <= 2f)
            {
                switch (currentState)
                {
                    case CreatureState.exploring:
                        target = null;
                        break;
                    case CreatureState.chasingFood:
                        if (target.TryGetComponent<LiveEntity>(out LiveEntity entity))
                        {
                            entity.TakeDamage(10);
                        }
                        else if (target.TryGetComponent<Food>(out Food food))
                        {
                            food.Eat(this);
                            currentState = CreatureState.exploring;
                        }
                        else
                            currentState = CreatureState.exploring;
                        break;
                    case CreatureState.escapingPredator:
                        if (target.TryGetComponent<LiveEntity>(out LiveEntity entityFight))
                        {
                            entityFight.TakeDamage(10);
                        }
                        break;
                }
            }
        }
        FindOthers();

        if (hunger >= 1)
        {
            GetComponent<LiveEntity>().TakeDamage(10);
            hunger = 0.7f;
        }
    }

    void Move()
    {
        Ray forward = new Ray(transform.position + Vector3.up, transform.forward);
        Ray right = new Ray(transform.position + Vector3.up, transform.right);
        Ray left = new Ray(transform.position + Vector3.up, -transform.right);
        RaycastHit hit;
        if (Physics.Raycast(forward, out hit, visionRadius))
        {
            if (!Physics.Raycast(right, out hit, visionRadius))
            {
                Vector3 stirAmount = transform.right * 5;
                Vector3 movementDir = (target.transform.position + stirAmount - transform.position).normalized;
                moveAmount = movementDir * walkSpeed;
            }
            else if (!Physics.Raycast(left, out hit, visionRadius))
            {
                Vector3 stirAmount = -transform.right * 10;
                Vector3 movementDir = (target.transform.position + stirAmount - transform.position).normalized;
                moveAmount = movementDir * walkSpeed;
            }
        }
        rb.MovePosition(transform.position + moveAmount * Time.fixedDeltaTime);
        FaceTarget();

    }

    private void FixedUpdate()
    {
        if (target != null)
            Move();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 bodyForward = transform.forward;
        transform.rotation = Quaternion.FromToRotation(bodyForward, direction) * transform.rotation;
    }

    void FindOthers()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, visionRadius);

        float lowestPreyDist = 10000;
        float lowestPredatorDist = 10000;

        Transform currentPOI = null;

        foreach (var other in others)
        {

            if (other.TryGetComponent<LiveEntity>(out LiveEntity entity))
            {
                float dist = Vector3.Distance(transform.position, entity.transform.position);
                if (currentState == CreatureState.exploring && lowestPredatorDist > visionRadius)
                {
                    if (hunger >= CreatureAttributes.criticalHunger && diet.HasFlag(entity.species))
                    {
                        if (dist < lowestPreyDist)
                        {
                            currentPOI = entity.transform;
                            lowestPreyDist = dist;
                            currentState = CreatureState.chasingFood;
                        }
                    }
                    if (entity.TryGetComponent<Creature>(out Creature creature))
                    {
                        if (creature.diet.HasFlag(species) && diet.HasFlag(entity.species) && creature.target == this.transform)
                        {
                            currentPOI = entity.transform;
                            lowestPreyDist = dist;
                            lowestPredatorDist = dist;
                            currentState = CreatureState.chasingEnemy;
                        }
                        else if (creature.diet.HasFlag(species) && creature.target == this.transform)
                        {
                            lowestPredatorDist = dist;
                            currentPOI = entity.transform;
                            currentState = CreatureState.escapingPredator;
                        }
                    }
                }
            }
            else if (other.TryGetComponent<Food>(out Food food))
            {
                if (hunger >= CreatureAttributes.criticalHunger && diet.HasFlag(food.species))
                {
                    if (dist < lowestPreyDist)
                    {
                        currentPOI = food.transform;
                        lowestPreyDist = dist;
                        currentState = CreatureState.chasingFood;
                    }
                }
            }
        }
        if (target != null && currentPOI == null)
            return;
        target = currentPOI;
        if (target == null)
        {
            target = GetWaypoint();
        }
    }

    Transform GetWaypoint()
    {
        Collider[] others = Physics.OverlapSphere(transform.position, visionRadius);

        float closestWaypointDist = 1000;

        Transform closestWaypoint = null;

        WaypointHolder waypointHolder = null;

        foreach (var other in others)
        {
            if (other.TryGetComponent<WaypointHolder>(out WaypointHolder wH))
            {
                waypointHolder = wH;
            }
        }
        if (waypointHolder != null)
        {
            foreach (Transform waypoint in waypointHolder.waypoints)
            {
                float dist = Vector3.Distance(transform.position, waypoint.position);
                if (dist < closestWaypointDist && dist >= 10)
                {
                    closestWaypoint = waypoint;
                    closestWaypointDist = dist;
                }
            }
        }

        return closestWaypoint;
    }
}
