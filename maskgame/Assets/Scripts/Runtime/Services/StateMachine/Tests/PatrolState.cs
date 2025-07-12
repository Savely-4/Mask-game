using Runtime.Services.StateMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolState : State
{
    public List<Transform> Points;
    public List<Transform> Targets;

    private NavMeshAgent _agent;
    private Vector3 _move;
    PatrolState(StateMachine stateMachine) : base (stateMachine)
    {
        _agent = _agent.GetComponent<NavMeshAgent>();
        _agent.enabled = true;
        //agent.speed
        if (_agent == null) Debug.LogError("Agent Null");
    }
    protected override void OnUpdate()
    {
        if (AgentInTargetPositions())
            NewPartolPositions();
    }
    private void NewPartolPositions()
    {
        foreach(Transform i in Points)
        {
            if(i == _agent.transform)
            {
                Targets.AddRange(Points);
                Targets.Remove(i);
            }
        }
        _agent.destination = Targets[Random.Range(0,Targets.Count)].position;
        _move = _agent.destination;
    }
    private bool AgentInTargetPositions()
    {
        if(_agent.transform.position == _move)
        {
            Targets.Clear();
            return true;
        }
        return false;
    }
    
}
