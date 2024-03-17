using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyBT : MonoBehaviour
{
    // Variable used for patrol
    [SerializeField] List<GameObject> _waypoints = new();

    private Dictionary<string, object> _blackBoard = new();
    private BTNode _root;

    void Start()
    {
        _blackBoard["waypoints"] = _waypoints;
        _blackBoard["currentWaypointIndex"] = 0;
        _blackBoard["navMeshAgent"] = GetComponent<NavMeshAgent>();

        _blackBoard["transform"] = transform;
        _blackBoard["player"] = GameObject.FindWithTag("Player");

        _blackBoard["hasAlreadyEnter"] = false;
        GameMaster.Instance.Attack += OnAttack;

        _blackBoard["isFrozen"] = true;
        GameMaster.Instance.Freeze += OnFreeze;
        GameMaster.Instance.Unfreeze += OnUnfreeze;

        ConstructTree();
    }

    void OnFreeze()
    {
        _blackBoard["isFrozen"] = true;
    }

    void OnUnfreeze()
    {
        _blackBoard["isFrozen"] = false;
    }

    void OnAttack()
    {
        _blackBoard["isFrozen"] = false;
        _blackBoard["hasAlreadyEnter"] = false;
    }


    void ConstructTree()
    {
        _root = new Selector(new List<BTNode>
        {
            new Enter(ref _blackBoard),
            /*
            new Sequence(new List<BTNode> { // Death Handling part
                // IsDead
                // Die
            }),
            */
            new Sequence(new List<BTNode> { // Time stop handling part
                new IsTimeFrozen(transform, ref _blackBoard)
            }),
            

            new Sequence(new List<BTNode> { // Attack player part
                new IsPlayerInAttackRange(transform, ref _blackBoard),
                new AttackPlayer(transform)
            }),

            new Sequence(new List<BTNode> { // Movement/detection to player part
                new IsPlayerSeen(transform, ref _blackBoard),
                new MoveToPlayer(transform, ref _blackBoard)
            }),

            new Patrolling(ref _blackBoard)
        });
    }

    // Update is called once per frame
    void Update()
    {
        _root.Evaluate();
    }
}