using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseReign;

public class HideAction : GoapAction {

	//[SerializeField] float timeBetweenAttack = 1.0f;
	   
	bool m_sawPlayer = false;
    Animator anim;
    Sight sight;

	void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        sight = gameObject.GetComponent<Sight>();        
    }
    
    public HideAction(){
		// addPrecondition("canSeePlayer", false);
		// addEffect ("Hide", true);
		addEffect ("doJob", true);
    	name = "Find a place to hide";
	}

	void Update()
    {
        //
    }
    
    public override void reset() {
		target = null;
	}

	public override bool isDone(){
		return m_sawPlayer;
	}

	public override bool requiresInRange(){
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent)
	{
		if( sight.isInFOV == true || GetComponent<Worker>().GetNeedsToHide())
		{
			target = GameObject.FindGameObjectWithTag("HidingSpot");
			if (target != null)
			{
				Debug.Log("I found a hiding spot!");
				return true;
			}
		}	
		return false;
	}

	public override bool perform(GameObject agent)
	{
		if(m_sawPlayer)
        {
            Debug.Log("Hiding!");	
	        return true;
        }
        return false;
	}
}
