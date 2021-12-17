﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using RiseReign;

public class Worker : MonoBehaviour, IGoap
{
	NavMeshAgent agent;
	Vector3 previousDestination;
	Inventory inv;
	public Inventory windmill;
	public bool interrupt = false;
	bool hide = false;

	void Start()
	{
		agent = this.GetComponent<NavMeshAgent>();
		inv = this.GetComponent<Inventory>();
	}

	public HashSet<KeyValuePair<string,object>> GetWorldState () 
	{
		HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();
		worldData.Add(new KeyValuePair<string, object>("hasStock", (windmill.flourLevel > 4) ));
		worldData.Add(new KeyValuePair<string, object>("hasFlour", (inv.flourLevel > 1) ));
		worldData.Add(new KeyValuePair<string, object>("hasDelivery", (inv.breadLevel > 4) ));		
		worldData.Add(new KeyValuePair<string, object>("canSeePlayer", true ));		
		worldData.Add(new KeyValuePair<string, object>("Hide", true ));
		return worldData;
	}


	public HashSet<KeyValuePair<string,object>> CreateGoalState ()
	{
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		goal.Add(new KeyValuePair<string, object>("doJob", true ));

		return goal;
	}


	public bool MoveAgent(GoapAction nextAction) {
		
		if(interrupt)
		{
			GetComponent<GoapAgent>().DataProvider().PlanAborted(nextAction);
			PlanAborted(nextAction);

			interrupt = false;
			return true;
		}
		agent.SetDestination(nextAction.target.transform.position);
		
		//if we don't need to move anywhere
		if(previousDestination == nextAction.target.transform.position)
		{
			nextAction.setInRange(true);
			return true;
		}
		
		// agent.SetDestination(nextAction.target.transform.position);
		
		if (agent.hasPath && agent.remainingDistance < 2) {
			nextAction.setInRange(true);
			previousDestination = nextAction.target.transform.position;
			return true;
		} 
		
		else
			return false;
	}

	void Update()
	{
		if(agent.hasPath)
		{
			Vector3 toTarget = agent.steeringTarget - this.transform.position;
         	float turnAngle = Vector3.Angle(this.transform.forward,toTarget);
         	agent.acceleration = turnAngle * agent.speed;
		}
		if(GetComponent<Sight>().isInFOV)
		{
			interrupt = true;
		}
		
	}

	public void PlanFailed (HashSet<KeyValuePair<string, object>> failedGoal)
	{

	}

	public void PlanFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
	{

	}

	public void ActionsFinished ()
	{

	}

	public void PlanAborted (GoapAction aborter)
	{
		GetComponent<GoapAgent>().DataProvider().ActionsFinished();
		aborter.doReset();//Calling from GoapAction.cs
		aborter.reset ();	//Calling from action scripts
	}

	public void SetHide(bool hideOrNot)
	{
		hide = hideOrNot;
		interrupt = true;
	}
}
