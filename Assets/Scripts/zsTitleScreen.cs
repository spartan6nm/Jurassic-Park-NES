using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zsTitleScreen : MonoBehaviour {

    //Declare serializables
    [SerializeField] private zsAnimationFinished TitleAnimationFinishedScript;
    [SerializeField] private float[] TimeBeforeDrools;
    [SerializeField] private Animator[] DroolAnimators;

    //Declare private
    private bool[] ablnTimeBeforeDrools;
    private int intLength;

    private void Start() {
        //Expand
        ablnTimeBeforeDrools = new bool[TimeBeforeDrools.Length];
        //Set
        intLength = TimeBeforeDrools.Length - 1;
    }

    private void Update() {
        //Check if script is finished
        if (TitleAnimationFinishedScript.AnimationFinished && !ablnTimeBeforeDrools[0]) {
            //Subtract time
            TimeBeforeDrools[0] -= Time.deltaTime;
            //Check timer
            if (TimeBeforeDrools[0] <= 0f) {
                //Enable animator
                DroolAnimators[0].enabled = true;
                //Set
                ablnTimeBeforeDrools[0] = true;
            }
        }
        //Loop
        for (int intLoop = 0; intLoop < intLength; intLoop++) {
            //Do droll timer
            DroolTimer(intLoop, intLoop + 1);
        }
    }

    private void DroolTimer(int intIndex1, int intIndex2) {
        //Check if next timer is ready
        if (ablnTimeBeforeDrools[intIndex1] && !ablnTimeBeforeDrools[intIndex2]) {
            //Subtract time
            TimeBeforeDrools[intIndex2] -= Time.deltaTime;
            //Check timer
            if (TimeBeforeDrools[intIndex2] <= 0f) {
                //Enable animator
                DroolAnimators[intIndex2].enabled = true;
                //Set
                ablnTimeBeforeDrools[intIndex2] = true;
            }
        }
    }

}