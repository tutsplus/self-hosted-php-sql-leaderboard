using UnityEngine;
using System.Collections;
using System.Text;

public class NameEnter : MonoBehaviour {

    private int MaxNameLength=10;

    private StringBuilder playerNameTemp;
    private bool backspacepossible;
    private bool initialpress;
    

    void Start()
    {
        //Disable the ability to click and change the counter
        GetComponent<ClickTimes>().enabled = false;

        //Start our stringbuilder and set its text to _
        playerNameTemp = new StringBuilder();
        playerNameTemp.Append("_");
        backspacepossible = true;
        initialpress = false;

        //Disable all our guiTexts except the top text
        foreach(GUIText text in FindObjectsOfType(typeof(GUIText)) as GUIText[]){
            if (text.name != "Toptext"&&text!=guiText)
            {
                text.guiText.enabled = false;
            }
        }
    }

    //Enables the user to hold down the backspace button and for it to erase every 0.05 seconds.
    IEnumerator BackspaceConstantHold()
    {
        yield return new WaitForSeconds(0.05f);
        backspacepossible = true;
    }

    //Starts constantly erasing characters after 0.15 seconds of holding backspace.
    IEnumerator BackspaceInitialHold()
    {
        yield return new WaitForSeconds(0.15f);
        backspacepossible = true;
    }

    void Update()
    {
        //If the player's name is less than the maximum length we allow
        if (playerNameTemp.Length < MaxNameLength)
        {
            //For every character they type
            foreach (char c in Input.inputString)
            {
                //If the character is _, space or alphanumerical
                if (char.IsLetterOrDigit(c) || c == '_' || c == ' ')
                {
                    if (!initialpress) //If they haven't pressed before
                    {
                        initialpress = true; //Now they have!
                        playerNameTemp.Remove(0, 1); //And remove the underscore
                    }
                    //Now add the letter
                    playerNameTemp.Append(c);
                }
            }
        }
        //If they press backspace and there's more than 0 characters
        if(playerNameTemp.Length>0){
            if (Input.GetKeyDown(KeyCode.Backspace)){
                if (!initialpress)
                {
                    initialpress = true; //They've pressed a button, the _ will already be removed
                }
                //We won't register backspace being held down..
                backspacepossible = false;
                StartCoroutine(BackspaceInitialHold()); //.. Until this coroutine finishes.
                playerNameTemp.Remove(playerNameTemp.Length - 1, 1); //And we remove the last character
            }
            else if(backspacepossible&&Input.GetKey(KeyCode.Backspace)){ //If it's allowed to hold backspace
                backspacepossible = false; // It's not anymore! Until..
                StartCoroutine(BackspaceConstantHold()); // This finishes
                playerNameTemp.Remove(playerNameTemp.Length - 1, 1); // And then we remove the last character again
            }
        }
        //If the length's above 0 and they've inputted something (i.e. it doesn't display "_"
        if (playerNameTemp.Length > 0 && initialpress)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // If they hit return
            {
                foreach(GUIText text in FindObjectsOfType(typeof(GUIText)) as GUIText[])
                {
                    text.enabled = false; //We disable all our text
                }
                GetComponent<HighScore>().SetName(playerNameTemp.ToString()); // We set our username
                GetComponent<HighScore>().enabled = true; // And we turn on the highscore class
                enabled = false; // And turn this off.
            }
        }
        guiText.text = playerNameTemp.ToString(); // This actually displays the information!
    }
}