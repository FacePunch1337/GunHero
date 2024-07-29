using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : Enemy
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    public override void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    protected override void StopMoving()
    {
        agent.SetDestination(transform.position);
    }
}
