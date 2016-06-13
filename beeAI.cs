using UnityEngine;
using System.Collections;

public class beeAI : MonoBehaviour {

    public LevelState ls;
    public LevelState.State diff;

    public Transform[] waypointsBeginner;
    public Transform[] waypointsIntermediate;
    public Transform[] waypointsAdvanced;

    public Transform[] currentWaypoints;
    public int cWaypoint = 0;

    private Vector3 targetPos;
    public Vector3 moveDirection;

    public bool hitEnvironment;
    public Vector3 toReverse;
    public int counter;

    string tagLevel;
    public bool firstCollision;
    public bool checkForCollision;



    // Use this for initialization
    void Start()
    {
        diff = LevelState.State.BEGINNER;
        currentWaypoints = waypointsBeginner;
        currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
        counter = 0;
        hitEnvironment = false;
        firstCollision = true;
    }
	
	void FixedUpdate () {
        if (counter == 0)
        {
            Move(.5f);
        }
        else if (counter > 0)
        {
            Move(.5f, toReverse);
        }
        
    }

    void Move(float accel)
    {
        //Set target Position to waypoint position
        targetPos = currentWaypoints[cWaypoint].position;
        //stores the direction that the object was going when it hit the wall;
        toReverse = Random.insideUnitSphere;
        //Set direction to move towards
        moveDirection = (targetPos - transform.position + toReverse * 2f).normalized;
        //Move's the object
        transform.Translate(moveDirection * Time.deltaTime * accel, Space.World);
    }
    void Move(float accel, Vector3 reverseVector)
    {
        //Set target Position to waypoint position
        targetPos = currentWaypoints[cWaypoint].position;
        //Set direction to move towards
        moveDirection = (targetPos - transform.position + toReverse * 2f).normalized;
        //moves the object the opposite direction of the wall
        transform.Translate((-reverseVector * 2f).normalized * Time.deltaTime * accel, Space.World);
        counter--;
    }


    /// <summary>
    /// enters a collider with a certain tag at a certain state
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        switch (diff)
        {
            //is at the beginner state
            case LevelState.State.BEGINNER:
                //if the collider entered is the waypoint we want, go to the next one
                //if the collider entered is any other way point, do nothing
                //if the collider entered is a wall, bounce off
                BeginnerState(other);
                break;
            case LevelState.State.INTERMEDIATE:
                IntermediateState(other);
                break;
            case LevelState.State.ADVANCED:
                AdvancedState(other);
                break;
        }
    }

    /// <summary>
    /// Handles the beginner level AI waypoints
    /// </summary>
    /// <param name="other"></param>
    void BeginnerState(Collider other)
    {
        if (other.gameObject.tag == "beginner current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "beginner removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {
                
                //start moving towards the next waypoint
                cWaypoint++;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
            }
            else
            {
                //start moving towards the next waypoint if you were at the last one
                cWaypoint = 0;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "beginner current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }

   
    /// <summary>
    /// Handles the Intermediate Level AI waypoints
    /// 
    /// </summary>
    /// <param name="other"></param>
    void IntermediateState(Collider other)
    {
        if (other.gameObject.tag == "intermediate current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "intermediate removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {
                //start moving towards the next waypoint
                cWaypoint++;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "intermediate current";
            }
            else
            {
                //start moving towards the next waypoint
                cWaypoint = 0;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "intermediate current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }
    /// <summary>
    /// Handles Adanved AI waypoints
    /// </summary>
    /// <param name="other"></param>
    void AdvancedState(Collider other)
    {
        if (other.gameObject.tag == "advanced current")
        {
            if (firstCollision)
            {
                firstCollision = false;
            }
            //set current waypoint to be ignored
            other.gameObject.tag = "advanced removed";

            //make sure you're not at the last waypoint
            if (cWaypoint + 1 < currentWaypoints.Length)
            {
                //start moving towards the next waypoint
                cWaypoint++;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "advanced current";
            }
            else
            {
                //start moving towards the next waypoint
                cWaypoint = 0;
                //set next waypoint to be used
                currentWaypoints[cWaypoint].gameObject.tag = "advanced current";
            }
        }
        if (other.gameObject.tag == "wall")
        {
            //for 6 iterations of fixed update go backwards - bounce into wall effect
            counter = 6;
        }
    }

}
