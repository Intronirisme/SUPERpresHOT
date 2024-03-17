using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBT : BehaviorTree
{
//    public Vector2Int TargetPos = new Vector2Int(19, 10);
    protected override void InitBlackboard()
    {
        base.InitBlackboard();
        _blackboard["navAgent"] = gameObject.GetComponent<NavMeshAgent>();
        //_blackboard["botScript"] = gameObject.GetComponent<Bot>();
        _blackboard["target"] = null;
        _blackboard["player"] = null;
        _blackboard["goal"] = null;
    }

    protected override void ConstructTree()
    {
        _root = new Selector(new List<BTNode> { 
            new Sequence(new List<BTNode> { //Attaque
                new PlayerInRange(ref _blackboard, 1.5f),
                //new Attack(ref _blackboard)
            }),
            new Sequence(new List<BTNode> { //Chasse
                
            }),
            new Sequence(new List<BTNode> { //Patrouille
                
            })
        });
    }
}
