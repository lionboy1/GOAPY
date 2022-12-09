using UnityEngine.AI;
using UnityEngine;

namespace RiseReign
{
    public class MineIronOreAction : GoapAction {

		bool completed = false;
		float startTime = 0;
		public float workDuration = 5; // seconds
	    public Backpack ownInv;
		public Inventory stockpile;
		Animator anim;
		NavMeshAgent _agent;
		// [SerializeField] GameObject objToPickup;
		[SerializeField] Transform pickupHand;

		public MineIronOreAction () {
			addEffect ("hasIronOre", true);
			name = "Mine Iron Ore";
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
			target = GameObject.FindGameObjectWithTag("IronOreMine");
			if(target != null)
			{
				return true;
			}
			return false;
		}

		public override bool perform (GameObject agent)
		{
			anim.SetTrigger("pickUp");
			if (startTime == 0 )
			{
				startTime = Time.time;
			}

			if (Time.time - startTime > workDuration) 
			{


				ownInv.ironOre += 5;
				completed = true;
			}
			return true;
		}

	}
}