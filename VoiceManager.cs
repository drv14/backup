using UnityEngine;
using System.Collections;

using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviour {

    //Internal reference to a keywordRecognizer.
    public bool toContinue = false;
    private string response;
    private string[] oneToOneHundred = new string[102];
    private KeywordRecognizer kr;
    AudioSource auSo;
    public DataStorage ds;
    public WaitForSeconds fifteen = new WaitForSeconds(15f);

    // Use this for initialization
    void Start () {
         auSo = GetComponent<AudioSource>();

        CreateString();

		//Create a new keywordRecognizer, with the words from one to one hundred
		kr = new KeywordRecognizer(oneToOneHundred);

		//Register OnVoiceCommand to kr's onPhraseRecognized
		kr.OnPhraseRecognized += OnVoiceCommand;
    }
    /// <summary>
    /// starts coroutine so it can be called from another class
    /// </summary>
    public void StartListening()
    {
        StartCoroutine(OnStartListening());
    }

    /// <summary>
    /// listen for ten seconds
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnStartListening()
    {
        kr.Start();
        yield return fifteen;
        kr.Stop();
    }

    //when a keyword is recognized
	private void OnVoiceCommand(PhraseRecognizedEventArgs args)
	{
        if (!args.text.Equals("try again"))
        {
            //add the responses to the directory
            MoveOn(args.text);
        }
        else
        //keyword was "try again", so we wait for a new response
        {
            auSo.Play();
            StartCoroutine(OnStartListening());
        }
            
	}

    /// <summary>
    /// adds repsonse to directory and compares it to previous ratings to see if user is ready to move on called by OnVoiceCommand
    /// </summary>
    /// <param name="response"></param>
    private void MoveOn(string response)
    {
        //check anxiety rating... 
        //compare anxiety rating to highest datapoint that has existed
        //if the newest anxiety rating is higher, update it, if it is some % lower, consider it to have peaked
        //set toContinue to true
        ds.Add(response);
        if (ds.IsPeaked(response))
        {
            toContinue = true;
        }


    }

    /// <summary>
    /// creates an array of strings that has the numbers "one" to "one-hundred"
    /// </summary>
    private void CreateString()
    {
        for (int i = 0; i < oneToOneHundred.Length - 2; i++)
        {
            oneToOneHundred[i] = i.ToString();
        }
        //it doesn't currently deal with the "try again" situation, I guess I can just add it to the end of the oneToOnehundredArray..
        oneToOneHundred[oneToOneHundred.Length - 2] = "try again";
        oneToOneHundred[oneToOneHundred.Length - 1] = "main menu";
    }


    private void showList()
    {
        //put responseDirectory on the screen for the psychologist to record.
        //modify a agmeobject with dont destroy onload to store all data and display it in a new scene
        //load new scene
    }

}
