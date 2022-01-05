using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CollectLumberAction : GoapAction {

	//Builders will collect lumber to start building
	bool completed = false;
	float startTime = 0;
	public float workDuration = 10; // seconds
    public Inventory ownInv;
	public Inventory stockpile;
	public Inventory lumbermill;
	Animator anim;
	NavMeshAgent _agent;
	bool pickupAttached = false;
	public GameObject buildingPackage;
	// [SerializeField] GameObject objToPickup;
	[SerializeField] Transform pickupHand;
	
	public CollectLumberAction () {
		// addPrecondition ("hasTools", true);
		addPrecondition ("hasLumber", true);
		addPrecondition ("hasTool", true);
		addEffect ("pickedUpLumber", true);
		name = "Collect building supplies";
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
		if(pickupAttached)
		{
			target.transform.position = pickupHand.position;
			target.transform.rotation = pickupHand.rotation;
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
		target = GameObject.FindGameObjectWithTag("Stockpile");
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

		if (Time.time - startTime > workDuration) 
		{
			if( _agent.remainingDistance <= 2)
			{
				Debug.Log("Finished: " + name);
				anim.SetTrigger("pickUp");
				lumbermill.lumber -= 5;
				ownInv.lumber += 5;
				// StartCoroutine("CollectBuildingPackage", 0.3f);
				// target.transform.parent = pickupHand;
				pickupAttached = true;
				Debug.Log("Parenting log to hand");
				
			}
		}
		return true;
	}
	IEnumerator CollectBuildingPackage()
	{
		yield return new WaitForSeconds(0.2f);
		
		Instantiate(buildingPackage, pickupHand.position, pickupHand.rotation);
		stockpile.logs -= 1;
		stockpile.tools -= 1;
		stockpile.buildingSupplies += 1;

		completed = true;
		// if(stockpile.logs >= 5 && stockpile.tools >= 5)
		// {
		// 	stockpile.buildingSupplies = 1;
		// 	completed = true;
		// }
	}
	
}
