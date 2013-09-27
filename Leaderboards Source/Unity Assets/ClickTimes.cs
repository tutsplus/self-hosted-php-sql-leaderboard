using UnityEngine;
using System.Collections;

public class ClickTimes : MonoBehaviour {

    public GameObject counter; //This counts down.
    public GameObject toptext; //This is the text at the top. We can also identify it by name, which we'll show later!
    private bool allowedToClick; //Whether the player has control
    private bool firstClick; //Whether the player has clicked at all yet.

	void Start () {
        //For every text object
        foreach(GUIText text in FindObjectsOfType(typeof(GUIText)) as GUIText[])
        {
            if (text != guiText) text.fontSize = Mathf.FloorToInt(Screen.height * 0.08f); // If it isn't attached to this object (i.e. our central text) we set it to this size
            else text.fontSize = Mathf.FloorToInt(Screen.height * 0.18f); //For our central text, we set it to this size.
        }
        
        //We now initialise our variables.
        allowedToClick = true;
        firstClick = false;
	}

    //Our function to count down
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1); //We wait one second
        counter.guiText.text = (System.Int32.Parse(counter.guiText.text) - 1).ToString();  //It's more sensible to store this as an integer, but let's explore some string manipulation.
        //We'll make the new text equal to the old number with 1 subtracted.

        //If we haven't hit 0, keep going
        if (counter.guiText.text != "0")
        {
            StartCoroutine(Countdown());
        }
        else
        {
            //Otherwise the player can't click anymore
            allowedToClick = false;
            GetComponent<HighScore>().Setscore(System.Int32.Parse(guiText.text)); //Plus we send our score through to our HighScore class
            toptext.guiText.text = "Enter your username."; // And we request a username
            GetComponent<NameEnter>().enabled = true; // And enable our next class
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (allowedToClick && Input.GetMouseButtonUp(0)) // If the user is allowed to click and chooses to
        {
            if (!firstClick) //If they haven't clicked yet, the countdown starts.
            {
                firstClick = true;
                StartCoroutine(Countdown());
            }
            guiText.text = (System.Int32.Parse(guiText.text) + 1).ToString(); //And the score goes up. Again, we could easily store this as an int.
        }
	}
}
