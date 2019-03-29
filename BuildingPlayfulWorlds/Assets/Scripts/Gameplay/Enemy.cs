using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemy : MonoBehaviour 
{
    NavMeshAgent agent;

    [Header("Default Settings: ")]
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Vector2 speed = new Vector2(7, 15);
    [SerializeField] float maxFollowingDistance = 50;
    [SerializeField] float damageAmount = 5;

    //JumpSettings
    [SerializeField] float jumpHeight = 2;

    [Header("Leader Settings: ")]
    [SerializeField] bool isLeader;
    [SerializeField] bool isFollowingPlayer;
    [SerializeField] int followerAmount;
    [SerializeField] float damageMultiplier = 1.5f;
    [SerializeField] float wanderingStepTime = 5;
    float defaultWanderingStepTime;
    bool playerFound = false;

    //Settings: -wanderingSpeed / -runSpeed --- wanderingSpace (het volume van een object)


    [Header("References: ")]
    public GameObject PlayerObject;
    [SerializeField] GameObject followerPrefab;
    [SerializeField] MeshRenderer spawnMesh;

    [HideInInspector] public GameObject LeaderObject;


    Animator anim;
    Rigidbody rb;

    public enum States { Idle, Running, Attacking, Wandering }
    public States currentState = States.Idle;

    float defaultSpeed;

    bool isJumping = false;

    Coroutine jumpAcrossRoutine;
    Coroutine attackRoutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = playerObject.transform.position;
        agent.speed = defaultSpeed = Random.Range(speed.x, speed.y);

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        defaultWanderingStepTime = wanderingStepTime;
        wanderingStepTime = 0;
    }

    private void Start()
    {
        StartRunning();

        if (isLeader)
            SpawnFollowers();
    }

    void StartJumpingAcross()
    {
        isJumping = true;

        if (jumpAcrossRoutine != null) StopCoroutine(jumpAcrossRoutine);
        jumpAcrossRoutine = StartCoroutine(IJumpAcross());
    }
    IEnumerator IJumpAcross()
    {
        //rb.AddForce();
        yield return null;
    }

    private Vector3 CalculateLowestDistance(Vector3 _mainPoint, Vector3 _pointA, Vector3 _pointB)
    {
        if (Vector3.Distance(_mainPoint, _pointA) < Vector3.Distance(_mainPoint, _pointB))
            return _pointA;
        else
            return _pointB;
    }

    private void Update()
    {
        isFollowingPlayer = CheckIfPlayerIsInFollowingRange();

        if (isFollowingPlayer)
            agent.destination = PlayerObject.transform.position;

        HandleStates();

        //animatie rent af en toe niet
        if (agent.velocity.x > 1 || agent.velocity.x < -1 || agent.velocity.z > 1 || agent.velocity.z < -1)
            anim.SetBool("isRunning", true);
        else //if (agent.velocity.x < 0.1f || agent.velocity.z < 0.1f)
        {
            anim.SetBool("isRunning", false);

        }

        //VOEG EIGEN OPGENOMEN GELUIDJES TOE VOOR DAMAGE EN FLAMINGO GEKRIJS. pauwen: "PAUW PAUW"

        //if (isLeader)
        //    Debug.Log("FlamingoX: " + agent.velocity.x + " - " + "FlamingoZ: " + agent.velocity.z);

        //CUSTOM SPRING PHYSICS
        //if (agent.isOnOffMeshLink)
        //{
        //    switch (agent.currentOffMeshLinkData.linkType)
        //    {
        //        default:
        //        case OffMeshLinkType.LinkTypeDropDown:

        //            //Vector3 _lowestDistance = LowestDistance(transform.position, agent.currentOffMeshLinkData.startPos, agent.currentOffMeshLinkData.endPos);

        //            //if(_lowestDistance == agent.currentOffMeshLinkData.startPos)
        //            //spring naar beneden
        //            break;

        //        case OffMeshLinkType.LinkTypeJumpAcross:
        //            if (!isJumping)
        //                StartJumpingAcross();


        //            break;
        //    }
        //}
    }

    void HandleStates()
    {
        if (CheckIfInAttackingRange())
        {
            RotateTowardsTarget();
            if (currentState != States.Attacking)
                StartAttacking();
        }
        else if (isFollowingPlayer)
        {
            if (currentState != States.Running)
            {
                agent.destination = PlayerObject.transform.position;
                StartRunning();
            }
        }
        else
            WanderAround();

    }

    bool CheckIfInAttackingRange()
    {
        return Vector3.Distance(agent.transform.position, PlayerObject.transform.position) <= agent.stoppingDistance;
    }

    bool CheckIfPlayerIsInFollowingRange()
    {
        return Vector3.Distance(agent.transform.position, PlayerObject.transform.position) <= maxFollowingDistance;
    }

    void StartAttacking()
    {
        currentState = States.Attacking;
        agent.isStopped = true;

        if (attackRoutine != null) StopCoroutine(attackRoutine);
        attackRoutine = StartCoroutine(IAttack());
    }

    void StartRunning()
    {
        currentState = States.Running;
        ChangeAnimation(States.Running);
        agent.isStopped = false;
    }


    float MultiplyDamage()
    {
        if (isLeader)
            return damageAmount * damageMultiplier;
        else
            return damageAmount;
    }

    IEnumerator IAttack()
    {
        ChangeAnimation(States.Attacking);

        while (anim.GetBool("isAttacking"))
        {
            Debug.Log("attackingWhileLoop");
            yield return null;
        }

        PlayerObject.GetComponent<FirstPersonController>().TakeDamage(MultiplyDamage());

        if (CheckIfInAttackingRange())
            StartAttacking();
        else
        {

            StartRunning();
        }
        yield return null;
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (PlayerObject.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void ChangeAnimation(States _state)
    {
        switch (_state)
        {
            default:
            case States.Idle:
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);
                break;
            case States.Running:
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking", false);
                break;
            case States.Attacking:
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", true);
                break;
            case States.Wandering:
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking", false);
                break;
        }
    }

    void SpawnFollowers()
    {
        for (int i = 0; i < followerAmount; i++)
        {
            Vector2 _randomXZ = new Vector2(Random.Range(0, 2), Random.Range(0, 2));
            Vector3 _randomPos = transform.position;

            _randomPos.x += _randomXZ.x;
            _randomPos.z += _randomXZ.y;

            GameObject _obj = Instantiate(followerPrefab, _randomPos, followerPrefab.transform.rotation, transform.parent);

            _obj.GetComponent<Enemy>().LeaderObject = gameObject;
            _obj.GetComponent<Enemy>().PlayerObject = PlayerObject;
        }
    }

    void WanderAround()
    {
        if (currentState != States.Wandering)
        {
            currentState = States.Wandering;
            ChangeAnimation(States.Wandering);
        }

        if (isLeader)
        {
            if (wanderingStepTime <= 0)
            {
                Vector3 _pos = spawnMesh.bounds.center + new Vector3(Random.Range(-spawnMesh.bounds.size.x / 2, spawnMesh.bounds.size.x / 2), 0, Random.Range(-spawnMesh.bounds.size.z / 2, spawnMesh.bounds.size.z / 2));
                agent.destination = _pos;
                wanderingStepTime = defaultWanderingStepTime;
                //StartRunning();
            }
            else wanderingStepTime -= Time.deltaTime;
        }

        else
        {
            agent.destination = LeaderObject.transform.position;
        }
    }
}
