using UnityEngine;
using System.Collections;

using UnityEngine.VR.WSA.Input;

public class GestureManager : MonoBehaviour {

	//Reference to the created Gesture Recognizer
	GestureRecognizer gr;

    public static GestureManager Instance { get; private set; }
    public GameObject boxToPlace;

    public GameObject openBoxAnimation;
    public int tapNum = 0;
    public GameObject beeScript;
    public AudioSource testSound;
    public VoiceManager vm;
    private bool testContinue;
    private bool isTakingInput;
    private bool isCalm;
    private int currentTap;

	/// <summary>
	/// Method called when the object is first created.
	/// </summary>
	void Start () {
        Instance = this;

        //Create a new Gesture Recognizer
        gr = new GestureRecognizer();

		//Register the air tap event.
		gr.TappedEvent += OnTap;

		//Start capturing gestures.
		gr.StartCapturingGestures();
        testContinue = false;
        isTakingInput = false;
        isCalm = true;
        currentTap = 0;
	}

    //Method pointed at by the gesture recognizer to handle an air tap.
    private void OnTap(InteractionSourceKind source, int tapCount, Ray headRay) {
        if (tapNum == 1)
        {
            boxToPlace.GetComponent<TapToPlaceParent>().placing = false;
            tapNum++;
        }
        
        
        
        //Next stage of Animation
        if (isCalm)
        {
            isCalm = false;
            tapNum++;
            if (tapNum == 2 || tapNum == 3)
            {
                testSound.Play();
                isTakingInput = true;
            }
        }
    }
    void Update()
    {
        //if hololens is ready for input && the voice manager has grabbed the input
        if(isTakingInput && (vm.toContinue == true))
        {
            //taps will function
            testContinue = true;
        }
        //if the testsound has stopped playing and the the bee has already hit it's goal.
        if (!testSound.isPlaying && beeScript.GetComponent<beeAI>().firstCollision == false)
        {
            //will allow taps
            isCalm = true;
            //will reset the boice manager for the next prompt
            vm.toContinue = false;
        }
        else
        {
            isCalm = false;
        }
        //if the player is not mashing buttons and they have not responded and the sound is over
        if (tapNum > currentTap && (!testContinue) && isCalm)
        {
            //wait for an appropriate response
            tapNum--;
        }
        //handle each tap
        tapHandler();
    }

    void tapHandler()
    {
        //if it's the first tap, the beeAI goes to the next waypoints, after the testSound finished playing 
        //and only once(testContinue is set to false so this doesn't happen repeatedly)
        if (tapNum == 2 && (testContinue) && (!testSound.isPlaying))
        {
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
            beeScript.GetComponent<beeAI>().cWaypoint = 0;
            beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsIntermediate;
            beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "intermediate current";
            beeScript.GetComponent<beeAI>().diff = LevelState.State.INTERMEDIATE;
            beeScript.GetComponent<beeAI>().firstCollision = true;
            testContinue = false;
            currentTap++;
        }
        if (tapNum == 3 && (testContinue) && (!testSound.isPlaying))
        {
            beeScript.GetComponent<beeAI>().cWaypoint = 0;
            beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsAdvanced;
            beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "advanced current";
            beeScript.GetComponent<beeAI>().diff = LevelState.State.ADVANCED;
            beeScript.GetComponent<beeAI>().firstCollision = true;
            testContinue = false;
            currentTap = 6;
        }
        //on the 3rd and 4th tap we no longer ask for anxiety levels because this is the bee retreating
        if (tapNum == 4)
        {
            beeScript.GetComponent<beeAI>().cWaypoint = 0;
            beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsIntermediate;
            beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "intermediate current";
            beeScript.GetComponent<beeAI>().diff = LevelState.State.INTERMEDIATE;
            beeScript.GetComponent<beeAI>().firstCollision = true;
        }
        if (tapNum == 5)
        {
            beeScript.GetComponent<beeAI>().cWaypoint = 0;
            beeScript.GetComponent<beeAI>().currentWaypoints = beeScript.GetComponent<beeAI>().waypointsBeginner;
            beeScript.GetComponent<beeAI>().currentWaypoints[beeScript.GetComponent<beeAI>().cWaypoint].gameObject.tag = "beginner current";
            beeScript.GetComponent<beeAI>().diff = LevelState.State.BEGINNER;
            beeScript.GetComponent<beeAI>().firstCollision = true;
        }
        //on the last tap the box closes
        if (tapNum == 6)
        {
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
            tapNum = 1;
            currentTap = 1;
        }
    }
    //update for debugging in unity
	void UpdateKey() {
        //if you press input key (say "ten" in hololens) and the app is ready for input(as per later function)
        if (Input.GetKeyDown("k") && (isTakingInput))
        {
            //the program will be allowed to advance
            testContinue = true;
        }
        //if the testSound is not playing and they haven't yet hit the waypoint
        if (!testSound.isPlaying && beeScript.GetComponent<beeAI>().firstCollision == false)
        {
            isCalm = true;
        }
        else
        {
            isCalm = false;
        }
        //if the player is not mashing buttons and they have not pressed k and the sound is over
        if(tapNum > currentTap && (!testContinue) && isCalm)
        {
            print("subtracting");
            tapNum--;
        }
        tapHandler();
        // if the user taps, the tapNum is incremented and if it's the first or second tap the app will play the audio and begin taking input
        if (Input.GetKeyDown("space") && (isCalm))
        {
            isCalm = false;
            print(tapNum);
            tapNum++;
            print(tapNum);
            if (tapNum == 1 || tapNum == 2)
            {
                testSound.Play();
                isTakingInput = true;
            }
        }
	
	}

}
