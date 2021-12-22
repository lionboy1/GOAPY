using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CutTreeAction : GoapAction {

	bool completed = false;
	float startTime = 0;
	public float workDuration = 10; // seconds
    public Inventory forest;
	public Inventory ownInv;
	Animator anim;
	NavMeshAgent _agent;
	[SerializeField] GameObject log;
	[SerializeField] Transform pickupPosition;
	public float ArrivalDistance = 2.0f;
	
	public CutTreeAction () {
		addPrecondition ("hasTrees", true);
		addEffect ("doJob", true);
		addEffect ("hasFallenLogs", true);
		name = "Cut down trees";
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
		target = GameObject.FindGameObjectWithTag("Tree");
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
			// Debug.Log("Distance to forest " + _agent.remainingDistance);
			if (startTime == 0 )
			{
				Debug.Log("Starting: " + name);
				startTime = Time.time;
				anim.SetTrigger("cutTree");

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
			if( _agent.remainingDistance <= 3)
			{
				Debug.Log("Finished: " + name);
				
				StartCoroutine("SpawnLog", 0.5f);
            	
				
				completed = true;
			}
				
		}
		return true;
	}
	IEnumerator SpawnLog()
	{
		yield return new WaitForSeconds(5f);
		Instantiate(log, pickupPosition.position, Quaternion.identity);
		ownInv.logs += 5;
		forest.logs -= 5;
		anim.ResetTrigger("cutTree");
	}
	
}
