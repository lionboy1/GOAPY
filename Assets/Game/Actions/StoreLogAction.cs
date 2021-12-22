using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class StoreLogAction : GoapAction {

	bool completed = false;
	float startTime = 0;
	public float workDuration = 2; // seconds
	public float ArrivalDistance;
	Animator anim;
	NavMeshAgent _agent;
	public Inventory ownInv;
	
	public StoreLogAction () {
		// addPrecondition ("hasLogs", true);
		addPrecondition ("hasLogsDelivery", true);  
		addEffect ("doJob", true);
		name = "Deliver logs to saw mill";
	}
	void Start()
	{
		
		anim = GetComponent<Animator>();
		if( anim == null)
		{
			Debug.Log("No animator found!");;
		}
		_agent = GetComponent<NavMeshAgent>();
		if( _agent == null)
		{
			Debug.Log("No NavMeshAgent found!");;
		}
	}
	
	public override void reset ()
	{
		completed = false;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return completed;
	}
	
	public override bool requiresInRange ()
	{
		return true; 
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{	
		target = GameObject.FindGameObjectWithTag("Market");
		if(target != null)
		{
			return true;
		}
		return false;
	}
	
	public override bool perform (GameObject agent)
	{
		float dist = Vector3.Distance(target.transform.position, transform.position);
		if(_agent.remainingDistance < 3)
		{
			Debug.Log("Distance to market " + _agent.remainingDistance);
			if (startTime == 0 )
			{
				Debug.Log("Starting: " + name);
				startTime = Time.time;
				// if(!_agent.pathPending || !_agent.hasPath)
				// {
				// 	startTime = Time.time;
				// }
			}
		}
		
		if(Time.time - startTime > workDuration /*!_agent.hasPath || _agent.pathStatus == NavMeshPathStatus.PathComplete*/)
		{
			if (_agent.remainingDistance < 3)
			{
				Debug.Log("Finished: " + name);
				if(ownInv.logsToDeliver >= 5)
				{
					ownInv.logsToDeliver -= 5;
					completed = true;
				}
			} 
		}
		return true;
	}
	
}
