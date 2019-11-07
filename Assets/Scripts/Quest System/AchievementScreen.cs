using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementScreen : MonoBehaviour
{
    //References to all the UI elements to make Appear/Disappear
    public Image backPanel;
    public Text titleText;
    public Text descriptionText;

    //Max time for individual animation aspects
    public float timeOnScreen = 5f;
    public float disappearTime = 3f;

    //Current time for individual animation aspects
    float timeUp = 0f;
    float timeDisappearing = 0f;

    //test for three 6's
    int sixes = 0;
    float doubleTapTimer = 0f;
    float doubleTapMax = 0.4f;

    private void Awake()
    {
        //Make everything disappear before the scene starts up
        backPanel.color = SetAlpha(backPanel.color, 0f);

        titleText.color = SetAlpha(titleText.color, 0f);

        descriptionText.color = SetAlpha(descriptionText.color, 0f);
    }

    private void Update()
    {
        if (doubleTapTimer > 0f)
            doubleTapTimer = Mathf.Max(doubleTapTimer - Time.deltaTime, 0f);

        //Small keystroke test for custom event
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            if (doubleTapTimer > 0f)
            {
                ++sixes;

                //Fire the devil event if you've uttered the number of the beast
                if (sixes == 2)
                    DevilEvent.FireEvent(new DevilEvent());
            }
            else
            {
                sixes = 0;
            }

            doubleTapTimer = doubleTapMax;
        }

        //If the UI is visible, constantly drain the timers until the UI is gone (capped at 0f)
        if (timeUp > 0)
            timeUp = Mathf.Max(timeUp - Time.deltaTime, 0f);
        else if (timeDisappearing > 0)
        {
            timeDisappearing = Mathf.Max(timeDisappearing - Time.deltaTime, 0f);

            backPanel.color = SetAlpha(backPanel.color, timeDisappearing / disappearTime);

            titleText.color = SetAlpha(titleText.color, timeDisappearing / disappearTime);

            descriptionText.color = SetAlpha(descriptionText.color, timeDisappearing / disappearTime);
        }
    }

    //Return a new color that has the RGB of one element, and a custom alpha
    Color SetAlpha(Color imgColor, float alpha)
    {
        return new Color(imgColor.r, imgColor.g, imgColor.b, alpha);
    }

    //When a message is received, put it onto the screen
    //set the UI to visible, and make the title and description for the achievement
    //Also reset the timers
    public void AchievementMessage(string name, string desc)
    {
        timeUp = timeOnScreen;
        timeDisappearing = disappearTime;

        titleText.text = name;
        descriptionText.text = desc;

        backPanel.color = SetAlpha(backPanel.color, 1f);

        titleText.color = SetAlpha(titleText.color, 1f);

        descriptionText.color = SetAlpha(descriptionText.color, 1f);
    }
}
