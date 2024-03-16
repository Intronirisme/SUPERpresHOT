using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyBT : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<string, object> _globalData = new();
    private BTNode _root;

    void Start()
    {
        _globalData["transform"] = transform;
        _globalData["player"] = GameObject.FindWithTag("Player");
        _globalData["navAgent"] = GetComponent<NavMeshAgent>();
        ConstructTree();
    }

    void ConstructTree()
    {
        _root = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode> { // Enter handling part
                // HasEnter
                // Enter
            }),

            new Sequence(new List<BTNode> { // Time stop handling part
                // IsTimeStoped
                // Frozen
            }),

            new Sequence(new List<BTNode> { // Death Handling part
                // IsDead
                // Die
            }),

            new Sequence(new List<BTNode> { // Attack player part
                // IsPlayerInAttackRange
                // AttackPlayer
            }),

            new Sequence(new List<BTNode> { // Movement/detection to player part
                new IsPlayerSeen(transform),
                new MoveToPlayer(transform)
            }),

            // Patrol task
        });
    }

    // Update is called once per frame
    void Update()
    {
        _root.Evaluate();
    }
}