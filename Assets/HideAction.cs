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
		addPrecondition("canSeePlayer", true);
		addEffect ("Hide", true);
		// addEffect ("doJob", true);
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
		return false;
	}

	public override bool checkProceduralPrecondition(GameObject agent)
	{
		if( sight.isInFOV == true)
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
		m_sawPlayer = true;
        Debug.Log("Hiding!");	
	    return m_sawPlayer;
	    
        // return true;
	}
}
