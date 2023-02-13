using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class FollowAI : MonoBehaviour
{
    public Animator anim;
    public enum States { Patrol, Follow, Attack}
    public NavMeshAgent agent;
    public Transform target;

    public Transform[] wayPoints;
    private int currentwayPoint;

    public States currentStates;


    public float maxFollowDistance = 15f;
    public float shootDistance = 10f;
    public Weapon attackWeapon;

    private bool inSight;
    private Vector3 directionToTarget;
    GameObject Target;

    // Start is called before the first frame update
    private void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        Target = GameObject.FindGameObjectWithTag("Target");
    }


   


    // Update is called once per frame 
    private void Update()
    {
        UpdateStates();
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        directionToTarget = target.position - transform.position;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, directionToTarget.normalized, out hitInfo))
        {
            inSight = hitInfo.transform.CompareTag("Player");
        }
    }

    private void UpdateStates()
    {
        switch (currentStates)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Follow:
                Follow();
                break;
            case States.Attack:
                Attack();
                break;
        }
    }

    private void Patrol()
    {

        if(agent.destination != wayPoints[currentwayPoint].position)
        {
            agent.destination = wayPoints[currentwayPoint].position;
        }
        //Di chuyển đến waypoint tiếp theo
        if (HasReaches())
        {
            currentwayPoint = (currentwayPoint + 1) % wayPoints.Length;
        }
        if ( inSight && directionToTarget.magnitude <= maxFollowDistance )
        {
            currentStates = States.Follow;
        }

        anim.SetBool("run", true);

    }


    public void Follow()
    {
        if (directionToTarget.magnitude <= shootDistance && inSight )
        {
            agent.ResetPath();
            currentStates = States.Attack;

        }
        else
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
                anim.SetBool("run", true);
                anim.SetBool("fire", false);
            }
            if(directionToTarget.magnitude > maxFollowDistance)
            {
                currentStates = States.Patrol;
            }
        }
    }

    private void Attack()
    {
        if(!inSight || directionToTarget.magnitude > shootDistance)
        {
            currentStates = States.Follow;
        }

        transform.LookAt(Target.transform.position);
                attackWeapon.Fire();
        anim.SetBool("run", false);

    }

 private bool HasReaches()
    {
        return (agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }
}

