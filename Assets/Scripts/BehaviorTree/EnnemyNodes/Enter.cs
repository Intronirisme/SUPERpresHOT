using System.Collections.Generic;
using UnityEngine;

public class Enter : BTNode
{
    bool _hasAlreadyEnter = false;

    Transform _transform;
    Transform _trajectoryStartWaypointTransform;
    Transform _trajectoryEndWaypointTransform;

    float _arrivalDistance = 1f; // Room to manoeuvre value

    float _movingSpeed;

    public Enter(ref Dictionary<string, object> blackBoard)
    {
        _dataContext = blackBoard;

        _transform = (Transform)GetData("transform");

        // Warning do not change order in scene it must be : Enemy -> StartWaypoint, EndWaypoint, EnemyBody
        _trajectoryStartWaypointTransform = _transform.parent.GetChild(0); 
        _trajectoryEndWaypointTransform = _transform.parent.GetChild(1);

        _movingSpeed = 
            Vector3.Distance(_trajectoryStartWaypointTransform.position, _trajectoryEndWaypointTransform.position)
            / GameMaster.Instance.AssaultDuration;

        // Teleport enemy to the first waypoint
        _transform.SetPositionAndRotation(_trajectoryStartWaypointTransform.position, Quaternion.identity);
    }

    public override NodeState Evaluate()
    {
        if (_hasAlreadyEnter == false)
        {
            // Make the enemy move to the second waypoint in N time at X speed
            Vector3 direction = (_trajectoryEndWaypointTransform.position - _trajectoryStartWaypointTransform.position).normalized;

            _transform.position += _movingSpeed * Time.fixedDeltaTime * direction;

            // If we finished moving to the end waypoint
            if (Vector3.Distance(_transform.position, _trajectoryEndWaypointTransform.position) <= _arrivalDistance)
            {
                _hasAlreadyEnter = true;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        return NodeState.FAILURE;
    }
}