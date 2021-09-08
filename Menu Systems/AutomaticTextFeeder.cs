using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutomaticTextFeeder : MonoBehaviour
{
    public bool speechActive;               //Is the script active.

    public TextMeshProUGUI displayedText;   //The text object being updated.
    public string[] textsToDisplay;         //The string lines, typed into Unity Editor.
    private char[][] stringSeparated;       //All the string lines, broken into individual letters.

    public float timeBetweenLetters;        //How long should there be between letters appearing.
    private bool textCurrentlyTyping;       //Is a line currently being written.

    private int textIndex;                  //Which line is currently being displayed.
    private bool textFinished;              //Has all lines finished being written.

    public GameObject speechBubble;         //The speech bubble that appears when talking.
    public GameObject speechEllipsis;       //The elipsis ... appearing when more text to be viewed.

    // Start is called before the first frame update
    void Awake()
    {
        //Initiate 2D char array to match the size for the amount of strings to parse. 
        //Each string is stored in a separate dimension of the array.
        stringSeparated = new char[textsToDisplay.Length][];

        for (int i = 0; i < textsToDisplay.Length; i++)
        {
            stringSeparated[i] = textsToDisplay[i].ToCharArray();
        }

        //stringSeparated = textToDisplay.ToCharArray();
        //Debug.Log(stringSeparated[5]);

        //DisplayString();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetConfirm() && !textFinished && !textCurrentlyTyping)
        {
            DisplayString();
        }

        if(textIndex == textsToDisplay.Length && !textCurrentlyTyping)
        {
            if (InputManager.GetConfirm())
            {
                toggleSpeechBubble(false);
            }
        }
    }

    private void DisplayString()
    {
        StartCoroutine(DisplayLetters());
        textIndex++;

        if (textIndex >= stringSeparated.Length)
        {
            textFinished = true;
        }
    }

    public IEnumerator DisplayLetters()
    {
        textCurrentlyTyping = true;
        StartCoroutine(DisplayEllipsis(false));

        displayedText.text = "";

        foreach (char letter in stringSeparated[textIndex])
        {
            displayedText.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        textCurrentlyTyping = false;
        StartCoroutine(DisplayEllipsis(true));

        //displayedText.text += "\n \n";
    }

    //Turns the speech bubble on and off.
    public void toggleSpeechBubble(bool speechBubbleStatus)
    {
        textIndex = 0;
        textFinished = false;
        displayedText.text = "";
        speechBubble.SetActive(speechBubbleStatus);

        if (speechBubbleStatus)
        {
            DisplayString();
        }
    }

    private IEnumerator DisplayEllipsis(bool displayStatus)
    {
        yield return new WaitForSeconds(0.3f);

        if (displayStatus)
        {
            speechEllipsis.SetActive(true);
        }
        else
        {
            speechEllipsis.SetActive(false);
        }
    }
}
