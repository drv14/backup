using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
    public float fpsTargetDistance, enemyLookDistance, attackDistance, enemyMovementSpeed, damping;
    public Transform fpsTarget;




	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        //look at target if they are within range
        //enemy is main camera/user
        fpsTargetDistance = Vector3.Distance(fpsTarget.position, transform.position);
        if (fpsTargetDistance < enemyLookDistance)
        {
            lookAtPlayer();
        }
	}

    //rotates object to look at enemy
    void lookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(fpsTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);  
          
    }
}
