using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Vector3 patrolAreaCenter; // 巡逻区域的中心点
    public Vector3 patrolAreaSize;  // 巡逻区域的大小
    private NavMeshAgent agent;

    public Transform player;        // 玩家对象
    public float detectionRange = 10f; // 视野范围
    public float loseSightTime = 3f;   // 丢失视野后等待时间

    private Vector3 initialPosition; // 初始位置
    private bool isChasingPlayer = false;
    private float loseSightTimer = 0f;


    void Start()
    {
        initialPosition = transform.position; // 记录初始位置
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        CheckPlayerVisibility();
    }

    void Patrol()
    {
        // 检查是否到达目标点
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToRandomPoint();
        }
    }

    void MoveToRandomPoint()
    {
        // 在巡逻区域内随机生成一个点
        Vector3 randomPoint = patrolAreaCenter + new Vector3(
            Random.Range(-patrolAreaSize.x / 2, patrolAreaSize.x / 2),
            0,
            Random.Range(-patrolAreaSize.z / 2, patrolAreaSize.z / 2)
        );

        NavMeshHit hit;
        // 确保随机点在 NavMesh 上
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void ChasePlayer()
    {
        // 如果玩家在视野范围内，追逐玩家
        agent.SetDestination(player.position);
        agent.speed = 4f;
        agent.angularSpeed = 1080f;
        this.GetComponentInChildren<Animator>().SetBool("isRunning", true);

        // 检查是否丢失视野
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            loseSightTimer += Time.deltaTime;
            if (loseSightTimer >= loseSightTime)
            {
                loseSightTimer = 0f;
                isChasingPlayer = false;
                agent.SetDestination(initialPosition); // 回到初始位置
                agent.speed = 0.5f; // 恢复巡逻速度
                agent.angularSpeed = 360f;
                this.GetComponentInChildren<Animator>().SetBool("isRunning", false);
            }
        }
        else
        {
            loseSightTimer = 0f; // 重置丢失视野计时器
        }
    }

    void CheckPlayerVisibility()
    {
        // 检查玩家是否在视野范围内
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasingPlayer = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        // 可视化巡逻区域
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(patrolAreaCenter, patrolAreaSize);
    }
}
