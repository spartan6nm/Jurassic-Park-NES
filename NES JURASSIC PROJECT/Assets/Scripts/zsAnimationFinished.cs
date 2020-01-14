using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zsAnimationFinished : MonoBehaviour {

    //Declare
    [System.NonSerialized] public bool AnimationFinished = false;

    public void AnimationFinishedNow() {
        //Set
        AnimationFinished = true;
    }

}