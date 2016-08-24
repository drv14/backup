using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
    //object that has tap to place parent script
    public GameObject thingWithTapScript;
    //bee mouse or spider object used for typeOfAnimal enum
    public GameObject scriptType;
    //used to tell the oben box animation to play
    public GameObject openBoxAnimation;

    //the sound bits
    AudioSource anxietyRating, stimulusRating, beeAttention, crawlingMouse, crawlingSpider, 
        flyingBee, mouseAttention, mouseColor, mouseSqueaking, repeat, spiderAttention, 
        spiderColor, thanks, yellowBlack;

    AudioSource[] aSources;

    public bool aIsPlaying, sIsPlaying = false;

    bool hasPlayedOnce = false;

    public int level = 0;

    public enum typeOfAnimal {ERROR = 0, BEE, MOUSE, SPIDER};
    public typeOfAnimal tyOfAn;

    public VoiceManager vm;
    WaitForSeconds thirty;
    float messageCounter = 0f;

    // Use this for initialization
    void Start () {
        //gets the audio sources
        aSources = GetComponents<AudioSource>();
        //sets the audio sources
        anxietyRating = aSources[0];
        stimulusRating = aSources[1];
        beeAttention = aSources[2];
        crawlingMouse = aSources[3];
        crawlingSpider = aSources[4];
        flyingBee = aSources[5];
        mouseAttention = aSources[6];
        mouseColor = aSources[7];
        mouseSqueaking = aSources[8];
        repeat = aSources[9];
        spiderAttention = aSources[10];
        spiderColor = aSources[11];
        thanks = aSources[12];
        yellowBlack = aSources[13];

        isAnythingPlaying(anxietyRating);

        //one more to add for the "try again" recording

        thirty = new WaitForSeconds(30f);
        StartCoroutine(TimeHandler());
        
    }
	
    /// <summary>
    /// keeps track of when to call message handler and ask for anxiety rating
    /// </summary>
    /// <returns></returns>
	IEnumerator TimeHandler()
    {
        //yield return null;
        //run once in the background
        while (true)
        {
            //every minute we ask for anxiety rating
            if (!isAnythingPlaying(anxietyRating))
                vm.StartListening();
            yield return thirty;
            messageCounter += 30f;
            //every other 30 seconds we play a recording
            MessageHandler(messageCounter);

            yield return thirty;
            messageCounter += 30f;
            //reset the message handler to 0
            if (messageCounter == 180)
                messageCounter = 0;
        }
    }
    
    /// <summary>
    /// handles the messages that need to be played every minute (starting at 0:30)
    /// </summary>
    /// <param name="counter"></param>
    void MessageHandler(float counter)
    {
        if (counter == 30)
        {
            if (tyOfAn == typeOfAnimal.BEE)
                isAnythingPlaying(beeAttention);
            else if (tyOfAn == typeOfAnimal.MOUSE)
                isAnythingPlaying(mouseAttention);
            else if (tyOfAn == typeOfAnimal.SPIDER)
                isAnythingPlaying(spiderAttention);
            else
                Debug.Log("GameManager TimeHandler got no animal type");
        }
        else if (counter == 90)
        {
            if (tyOfAn == typeOfAnimal.BEE)
                isAnythingPlaying(yellowBlack);
            else if (tyOfAn == typeOfAnimal.MOUSE)
                isAnythingPlaying(mouseColor);
            else if (tyOfAn == typeOfAnimal.SPIDER)
                isAnythingPlaying(spiderColor);
            else
                Debug.Log("GameManager TimeHandler got no animal type");
        }
        else if (counter == 150)
        {
            if (tyOfAn == typeOfAnimal.BEE)
                isAnythingPlaying(flyingBee);
            else if (tyOfAn == typeOfAnimal.MOUSE)
                isAnythingPlaying(crawlingMouse);
            else if (tyOfAn == typeOfAnimal.SPIDER)
                isAnythingPlaying(crawlingSpider);
            else
                Debug.Log("GameManager TimeHandler got no animal type");
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if(vm.toContinue == true)
        {
            level++;
            vm.toContinue = false;
        }
        //if there's no rating message playing
        if (!isAnythingPlaying())
        {
            //controls what we do each level
            switch (level)
            {
                case 0:
                    //if we've created the box
                    if (thingWithTapScript.GetComponent<TapToPlaceParent>().firstPass == false)
                    {
                        level++;
                    }
                    break;
                case 1:
                    HandleLevels(tyOfAn);
                    break;
                case 2:
                    HandleLevels(tyOfAn);
                    break;
                case 3:
                    HandleLevels(tyOfAn);
                    break;
                case 4:
                    isAnythingPlaying(stimulusRating);
                    break;
                case 5:
                    isAnythingPlaying(thanks);
                    break;
            }
        }
    }

    /// <summary>
    /// switch statement handling different animals
    /// </summary>
    /// <param name="toa"></param>
    public void HandleLevels(typeOfAnimal toa)
    {
        switch (toa)
        {
            case typeOfAnimal.ERROR:
                Debug.Log("There is a problem with HandleLevels");
                break;
            case typeOfAnimal.BEE:
                BeeAI();
                break;
            case typeOfAnimal.MOUSE:
                GroundAI();
                break;
            case typeOfAnimal.SPIDER:
                GroundAI();
                break;
        }

    }

    /// <summary>
    /// handles function of waypoints for bee and box animation
    /// </summary>
    void BeeAI()
    {
        if (level == 1)
        {
        }
        else if (level == 2)
        {
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
            scriptType.GetComponent<beeAI>().cWaypoint = 0;
            scriptType.GetComponent<beeAI>().currentWaypoints = scriptType.GetComponent<beeAI>().waypointsIntermediate;
            scriptType.GetComponent<beeAI>().currentWaypoints[scriptType.GetComponent<beeAI>().cWaypoint].gameObject.tag = "intermediate current";
            scriptType.GetComponent<beeAI>().diff = LevelState.State.INTERMEDIATE;
            scriptType.GetComponent<beeAI>().firstCollision = true;
        }
        else if (level == 3)
        {
            scriptType.GetComponent<beeAI>().cWaypoint = 0;
            scriptType.GetComponent<beeAI>().currentWaypoints = scriptType.GetComponent<beeAI>().waypointsAdvanced;
            scriptType.GetComponent<beeAI>().currentWaypoints[scriptType.GetComponent<beeAI>().cWaypoint].gameObject.tag = "advanced current";
            scriptType.GetComponent<beeAI>().diff = LevelState.State.ADVANCED;
            scriptType.GetComponent<beeAI>().firstCollision = true;
        }
    }

    /// <summary>
    /// handles box animation and placement of mouse on level 3
    /// </summary>
    void GroundAI()
    {
        if (level == 2)
            openBoxAnimation.GetComponent<Animator>().SetTrigger("Open");
        else if (level == 3)
            scriptType.transform.localPosition = new Vector3(0, 0, -1.5f);
    }

    /// <summary>
    /// loops though the audio sources and checks if anything is playing
    /// </summary>
    /// <returns></returns>
    public bool isAnythingPlaying()
    {
        for (int ii = 0; ii < aSources.Length; ii++)
        {
            if (aSources[ii].isPlaying)
                return true;
        }
        return false;
    }

    /// <summary>
    /// overload plays the given audiosource if nothing is playing
    /// </summary>
    /// <returns></returns>
    public bool isAnythingPlaying(AudioSource toPlay)
    {
        for (int ii = 0; ii < aSources.Length; ii++)
        {
            if (aSources[ii].isPlaying)
                return true;
        }
        toPlay.Play();
        return false;
    }


}
