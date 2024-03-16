using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : BTNode
{
    List<GameObject> _waypoints = new();
    int _currentWaypointIndex;
    NavMeshAgent _navMeshAgent;
    Transform _transform;

    float _lastMoveTime = 0f;
    bool _isFirstTimeEvaluating = true;

    public Patrolling(ref Dictionary<string, object> blackBoard)
    {
        _dataContext = blackBoard;
        _waypoints = (List<GameObject>)GetData("waypoints");
        _navMeshAgent = (NavMeshAgent)GetData("navMeshAgent");
        _currentWaypointIndex = (int)GetData("currentWaypointIndex");
        _transform = (Transform)GetData("transform");
    }

    public override NodeState Evaluate()
    {
        if (_waypoints.Count > 0)
        {
            if (_isFirstTimeEvaluating)
            {
                // Find the closest waypoint only the first time we come in the evaluate function
                float smallestDistance = Mathf.Infinity;

                for (int i = 0; i < _waypoints.Count; i++)
                {
                    float distance = Vector3.Distance(_transform.position, _waypoints[i].transform.position);
                    if (distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        _currentWaypointIndex = i;
                    }
                }

                _isFirstTimeEvaluating = false;
            }

            GameObject currentWaypoint = _waypoints[_currentWaypointIndex];
            _navMeshAgent.SetDestination(currentWaypoint.transform.position);

            // Check if we are still going to a waypoint
            if (_navMeshAgent.pathPending || _navMeshAgent.hasPath)
                return NodeState.RUNNING;

            // Check if the past is inferior to 1 second
            if (Time.time - _lastMoveTime < 1f)
                return NodeState.RUNNING;

            // Update the time of the last movement and move on to the next waypoint.
            _lastMoveTime = Time.time;
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}