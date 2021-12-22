using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using RiseReign;

public abstract class Worker : MonoBehaviour, IGoap
{
	NavMeshAgent agent;
	Vector3 previousDestination;
	// Inventory inv;
	[Tooltip("Stockpile")]
	public Inventory stockpile;
	public Inventory ownInv;
	public Inventory forest;
	public bool interrupt = false;
	bool hide = false;
	public float AggroDistance = 1000.0f;//Line of sight

	public float ArrivalDistance = 1.5f;//Stopping distance
	void Start()
	{
		agent = this.GetComponent<NavMeshAgent>();
		// inv = this.GetComponent<Inventory>();
		ownInv = this.GetComponent<Inventory>();
	}

	public HashSet<KeyValuePair<string,object>> GetWorldState () 
	{
		HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();
		worldData.Add(new KeyValuePair<string, object>("canSeePlayer", false ));
		worldData.Add(new KeyValuePair<string, object>("hasStock", (stockpile.flourLevel > 4) ));
		worldData.Add(new KeyValuePair<string, object>("hasFlour", (ownInv.flourLevel > 1) ));
		worldData.Add(new KeyValuePair<string, object>("hasDelivery", (ownInv.breadLevel > 4) ));
		// worldData.Add(new KeyValuePair<string, object>("hasTrees", (stockpile.trees > 1) ));
		// worldData.Add(new KeyValuePair<string, object>("hasLogs", (inv.trees > 0) ));
		
		//Wood Cutter
		worldData.Add(new KeyValuePair<string, object>("hasTrees", (forest.logs > 3) ));
		worldData.Add(new KeyValuePair<string, object>("hasLogs", (ownInv.logs > 3) ));
		worldData.Add(new KeyValuePair<string, object>("hasLogsDelivery", (ownInv.logsToDeliver > 2) ));
		
		//Hiding		
		// worldData.Add(new KeyValuePair<string, object>("Hide", false ));
		return worldData;
	}


	public abstract HashSet<KeyValuePair<string,object>> CreateGoalState ();
	// {
	// 	// HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
	// 	// goal.Add(new KeyValuePair<string, object>("doJob", true ));

	// 	// return goal;
	// }


	public bool MoveAgent(GoapAction nextAction) {
		
		// if(interrupt)
		// {
		// 	// GetComponent<GoapAgent>().DataProvider().PlanAborted(nextAction);
		// 	PlanAborted(nextAction);
		// 	// previousDestination = Vector3.zero;
		// 	// interrupt = false;
			
		// 	return true;
		// }
		agent.SetDestination(nextAction.target.transform.position);
		
		//if we don't need to move anywhere
		if(previousDestination == nextAction.target.transform.position)
		{
			nextAction.setInRange(true);
			return true;
		}
		
		agent.SetDestination(nextAction.target.transform.position);
		
		if (agent.hasPath && agent.remainingDistance < 2) {
			nextAction.setInRange(true);
			previousDestination = nextAction.target.transform.position;
			return true;
		} 
		
		else
			return false;


		// float fDistance = Vector3.Distance(transform.position, nextAction.target.transform.position);//Get distance to target
        // if (fDistance < AggroDistance)//If it is in aggro range
        // {
        //     GetComponent<NavMeshAgent>().isStopped = false;
        //     GetComponent<NavMeshAgent>().SetDestination(nextAction.target.transform.position);//Let the nav mesh do the work
        //     Vector3 v3LookDirection = nextAction.target.transform.position - transform.position;//Look at the target
        //     v3LookDirection.y = 0;
        //     Quaternion qRotation = Quaternion.LookRotation(v3LookDirection);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, qRotation, 0.005f);
        // }

        // if (interrupt)
        // {
        //     GetComponent<GoapAgent>().DataProvider().PlanAborted(nextAction);

        //     PlanAborted(nextAction);
        //     interrupt = false;

        //     return true;
        // }

        // if (fDistance <= ArrivalDistance)//If I have arrived
        // {
        //     nextAction.setInRange(true);
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
	}

	void Update()
	{
		if(agent.hasPath)
		{
			Vector3 toTarget = agent.steeringTarget - this.transform.position;
         	float turnAngle = Vector3.Angle(this.transform.forward,toTarget);
         	agent.acceleration = turnAngle * agent.speed;
		}
		// if(GetComponent<Sight>().isInFOV)
		// {
		// 	interrupt = true;
		// }
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
		aborter.reset ();	//Calling from action scripts
		aborter.doReset();//Calling from GoapAction.cs
	}

	public bool GetNeedsToHide()
	{
		return hide;
	}

	public void SetHide(bool hideOrNot)
	{
		hide = hideOrNot;
		interrupt = true;
	}
}
