using UnityEngine.AI;
using UnityEngine;

namespace RiseReign
{
    public class ProcessLumberAction : GoapAction {

		bool completed = false;
		float startTime = 0;
		public float workDuration = 5; // seconds
	    public Backpack ownInv;
		public Inventory lumbermill;
		Animator anim;
		NavMeshAgent _agent;
		// [SerializeField] GameObject objToPickup;
		[SerializeField] Transform pickupHand;
		[SerializeField] GameObject lumber;
		[SerializeField] Transform lumberPile;
		Tree tree;

		public ProcessLumberAction () {
			addPrecondition ("gotLogs", true);
			addEffect ("makeLumber", true);
			name = "Saw logs into lumber";
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
			target = GameObject.FindGameObjectWithTag("Sawmill");
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
				anim.SetTrigger("saw");

				lumbermill.lumber += 5;
				ownInv.logs -= 5;
				Instantiate(lumber, lumberPile.position, Quaternion.identity);
				completed = true;
			}
			return true;
		}

	}
}