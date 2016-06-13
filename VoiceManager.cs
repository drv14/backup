using UnityEngine;
using System.Collections;

using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviour {

	//Internal reference to a keywordRecognizer.
	KeywordRecognizer krZero;
    KeywordRecognizer krFifty;

    //Reference the CubeSpinner which we will change.
    public bool toContinue = false;
    private string[] zero = new string[50];
    private string[] fifty = new string[51];

	// Use this for initialization
	void Start () {
        createString();

		//Create a new keywordRecognizer, with the words from one to one hundred
		krZero = new KeywordRecognizer(zero);

        krFifty = new KeywordRecognizer(fifty);

        

		//Register OnVoiceCommandBelow to krZero's onPhraseRecognized
		krZero.OnPhraseRecognized += OnVoiceCommandBelow;

		//Start listening for gestures.
		krZero.Start();

        //Register OnVoiceCommandAbove to krFifty's onPhraseRecognized
        krFifty.OnPhraseRecognized += OnVoiceCommandAbove;

        //Start listening for gestures.
        krFifty.Start();
    }

	private void OnVoiceCommandBelow(PhraseRecognizedEventArgs args)
	{
        toContinue = false;
	}

    private void OnVoiceCommandAbove(PhraseRecognizedEventArgs args)
    {
        toContinue = true;
    }

    private void createString()
    {
        for (int i = 0; i < zero.Length; i++)
        {
            zero[i] = i.ToString();
        }
        for (int j = 50; j < fifty.Length + 50; j++)
        {
            fifty[j - 50] = j.ToString();
        }

    }
}
