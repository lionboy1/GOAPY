using UnityEngine;

namespace RiseReign
{
    public class DeliverIronOreAction : GoapAction {

		bool completed = false;
		float startTime = 0;
		public float workDuration = 2; // seconds
		public Inventory stockpile;
		public Backpack ownInv;

		public DeliverIronOreAction () {
			addPrecondition ("hasIronOre", true); 
			addEffect ("mineIronOre", true);
			name = "Deliver Iron Ore";
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

			if (Time.time - startTime > workDuration) 
			{
				Debug.Log("Finished: " + name);
				ownInv.ironOre -= 5;
				// stockpile.ironOre += 5;
				completed = true;
			}
			return true;
		}

	}
}