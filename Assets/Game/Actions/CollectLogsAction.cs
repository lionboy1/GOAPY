using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CollectLogsAction : GoapAction {

	bool completed = false;
	float startTime = 0;
	public float workDuration = 5; // seconds
    public Backpack ownInv;
	public Inventory forest;
	Animator anim;
	NavMeshAgent _agent;
	// [SerializeField] GameObject objToPickup;
	[SerializeField] Transform pickupHand;
	public float ArrivalDistance = 2.0f;
	Tree tree;
	
	public CollectLogsAction () {
		addPrecondition ("hasLogs", true);
		addEffect ("doJob", true);
		name = "pick up log";
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
	void Update()
	{
		
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
		target = GameObject.FindGameObjectWithTag("Log");
		if(target != null)
		{
			return true;
		}
		return false;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0 )
		{
			startTime = Time.time;
		}

		if (Time.time - startTime > workDuration) 
		{
			anim.SetTrigger("pickUp");

			ownInv.logsToDeliver += 5;
			ownInv.logs -= 5;
			// Debug.Log("Parenting log to hand");
			completed = true;
		}
		return true;
	}
	
}