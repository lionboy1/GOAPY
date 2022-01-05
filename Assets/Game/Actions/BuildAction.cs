using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BuildAction : GoapAction {

	//The builder will proceed to build site
	

	//Automatically deplete stockpile of what is needed to make package

	//Packager lives at stockpile building
	bool completed = false;
	float startTime = 0;
	public float workDuration = 5; // seconds
    public Inventory ownInv;
	Animator anim;
	NavMeshAgent _agent;
	bool pickupAttached = false;
	// [SerializeField] GameObject objToPickup;
	[SerializeField] Transform pickupHand;
	public float ArrivalDistance = 2.0f;
	
	public BuildAction () {
		addPrecondition ("pickedUpLumber", true);
		addEffect ("construct", true);
		name = "Build";
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
			return true;
			// if( _agent.pathStatus == NavMeshPathStatus.PathComplete)
		}
		return false;
	}
	
	public override bool perform (GameObject agent)
	{
		
		if (startTime == 0 && target != null)
		{
			Debug.Log("Starting: " + name);
			// anim.SetTrigger("build");
			startTime = Time.time;
		}

		if (Time.time - startTime > workDuration) 
		{
			Debug.Log("Finished: " + name);
			anim.SetTrigger("pickUp");
			StartCoroutine("ShowBuildProgress", 0.3f);
			// target.transform.parent = pickupHand;
			pickupAttached = true;
			Debug.Log("Parenting log to hand");
				
		}
		return true;
	}
	//Call outside script to visualize building structure
	//being built. 
	IEnumerator ShowBuildProgress()
	{
		yield return new WaitForSeconds(4f);
		
		ownInv.buildingSupplies -= 1;
		completed = true;
		
	}
	
}
