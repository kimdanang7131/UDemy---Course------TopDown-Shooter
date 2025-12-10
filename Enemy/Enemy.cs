using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 20;

    [Header("Idle data")]
    public float idleTime;
    public float agressionRange;

    [Header("Move data")]
    public float moveSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    private bool manualMovenment;
    private bool manualRotation;

    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public Transform player { get; private set; }

    public Animator anim { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        // 성능에 안좋기때문에 나중에 바꾸기
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        IntializePatrolPoints();
    }

    protected virtual void Update()
    {
    }

    public virtual void GetHit()
    {
        healthPoints--;
    }

    public virtual void HitImpact(Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody)
    {
        StartCoroutine(ImpactCoroutine(force, hitPoint, hitRigidbody));
    }

    private IEnumerator ImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);

        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, agressionRange);
    }

    public bool PlayerInAgressionRange() => Vector3.Distance(transform.position, player.position) < agressionRange;

    public Vector3 GetPatrolDestination()
    {
        Vector3 destionation = patrolPoints[currentPatrolIndex].transform.position;
        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;

        return destionation;
    }

    private void IntializePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }

    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    public void ActivateManualMovenment(bool manualMovenment) => this.manualMovenment = manualMovenment;
    public bool ManualMovementActive() => manualMovenment;

    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualRotationActive() => manualRotation;

    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
}
