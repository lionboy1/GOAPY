using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CollectLogsAction : GoapAction {

	bool completed = false;
	float startTime = 0;
	public float workDuration = 10; // seconds
    public Inventory ownInv;
	Animator anim;
	NavMeshAgent _agent;
	// [SerializeField] GameObject objToPickup;
	[SerializeField] Transform pickupHand;
	public float ArrivalDistance = 2.0f;
	
	public CollectLogsAction () {
		// addPrecondition ("haslogs", true);
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
			// _agent.SetDestination(target.transform.position);
			return true;
			// if( _agent.pathStatus == NavMeshPathStatus.PathComplete)
			// return true;
		}
		return false;
	}
	
	public override bool perform (GameObject agent)
	{
		float dist = Vector3.Distance(target.transform.position, transform.position);
		if( _agent.remainingDistance <= 3)
		{
			if (startTime == 0 && target != null)
			{
				Debug.Log("Starting: " + name);
				// anim.SetTrigger("pickUp");
				startTime = Time.time;

			}
		}
		// if (startTime == 0 )
		// {
		// 	Debug.Log("Starting: " + name);
		// 	startTime = Time.time;
		// 	Debug.Log(startTime);
		// 	anim.SetTrigger("cutTree");
		// }

		if (Time.time - startTime > workDuration) 
		{
			if( _agent.remainingDistance <= 2)
			{
				Debug.Log("Finished: " + name);
				anim.SetTrigger("pickUp");
				StartCoroutine("AttachPickup", 0.3f);
				Debug.Log("Parenting log to hand");
				
			}
		}
		return true;
	}
	IEnumerator AttachPickup()
	{
		yield return new WaitForSeconds(4f);
		
		ownInv.logsToDeliver += 5;
		ownInv.logs -= 5;
		if(ownInv.logsToDeliver >= 5)
		{
			ownInv.logsToDeliver = 5;
			completed = true;
		}
	}
	
}
