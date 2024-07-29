using UnityEngine;

public class FlyingEnemy : Enemy
{
    public float flightHeight = 5.0f;

    public override void Move(Vector3 destination)
    {
        Vector3 targetPosition = new Vector3(destination.x, flightHeight, destination.z);
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    protected override void StopMoving()
    {
        // Оставить врага на текущем месте и повернуть его к игроку
        if (target != null)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
