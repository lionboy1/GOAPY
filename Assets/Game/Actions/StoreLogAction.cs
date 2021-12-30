using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class StoreLogAction : GoapAction {

	bool completed = false;
	float startTime = 0;
	public float workDuration = 2; // seconds
	Animator anim;
	NavMeshAgent _agent;
	public Backpack ownInv;
	
	public StoreLogAction () {
		// addPrecondition ("hasLogs", true);
		addPrecondition ("hasLogsDelivery", true);
		addEffect ("hasLogsDelivery", false);  
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
		if (startTime == 0)
		{
			Debug.Log("Starting: " + name);
			startTime = Time.time;
		}
		
		if(Time.time - startTime > workDuration)
		{
			Debug.Log("Finished: " + name);
			// Invoke("UpdateInventories", 1f);
			ownInv.logsToDeliver -= 5;
			if(ownInv.logsToDeliver < 0)
			{
				ownInv.logsToDeliver = 0;
			}
			completed = true;
		}
		return true;
	}
	void UpdateInventories()
	{
		ownInv.logsToDeliver -= 5;
		if(ownInv.logsToDeliver < 0)
		{
			ownInv.logsToDeliver = 0;
		}
	}
}
