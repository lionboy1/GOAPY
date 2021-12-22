using System;
using UnityEngine.UI;
using UnityEngine;


namespace RiseReign
{
    public class Sight : MonoBehaviour
    {
    	#region variables
    	public Transform player;
    	public float maxAngle;
    	public float maxRadius;
    
        public bool isInFOV = false;
    	
    	#endregion
    	
    	#region gizmos
    	private void OnDrawGizmos()
    	{
    		Gizmos.color = Color.yellow;
    		Gizmos.DrawWireSphere(transform.position, maxRadius);
    		
    		//Quaternion.AngleAxis (angle, axis to rotate on).
    		Vector3 fovLine1 = Quaternion.AngleAxis( maxAngle, transform.up) * transform.forward * maxRadius;//Rotating the forward.
    		Vector3 fovLine2 = Quaternion.AngleAxis( -maxAngle, transform.up) * transform.forward * maxRadius;
    		
    		Gizmos.color = Color.blue;
    		Gizmos.DrawRay(transform.position, fovLine1);
    		Gizmos.DrawRay(transform.position, fovLine2);
    		
    		//Line between AI and player.
    		if(!isInFOV)
            {
                Gizmos.color = Color.red;
            }
    		else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(transform.position, (player.position-transform.position).normalized * maxRadius);
    		
    		
    		Gizmos.color = Color.black;
    		Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    	}	
    	#endregion
    	#region functions
    	public static bool inFOV( Transform checkingObject, Transform target, float maxAngle, float maxRadius)
	    {
		    Collider[] overlaps = new Collider[10];
		    int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps); //checkingObject is the AI, overlaps is the returned collider result that is checked.
		
            for (int i = 0; i < count + 1; i++)
            {
                    if(overlaps[i] !=null)
                    {
                           if(overlaps[i].transform == target)
                           {
                               Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                               directionBetween.y *= 0;//Prevent the height from being a factor in calculating the angle, for accuracy.

                               float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                               if(angle <= maxAngle)
                               {
                                   Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                                   RaycastHit hit;
		    					   
		    					   if(Physics.Raycast(ray, out hit, maxRadius))
		    					   {
		    							if(hit.transform == target)
		    							{
		    								return true;
		    							}
		    					   }


                               }
                           }
                    }
            }
            
            return false;
        }
    	#endregion
    
        void Update()
        {
            isInFOV = inFOV( transform, player, maxAngle, maxRadius);
        }
    }
}
