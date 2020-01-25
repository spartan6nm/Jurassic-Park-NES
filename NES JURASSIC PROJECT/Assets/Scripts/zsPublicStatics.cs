using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class zsPublicStatics {

    //Declare object pools
    public static gudcObject Bullets;

    //Declare classes
    public class gudcObject {
        //Declare
        public int NextPoolNumber;
        public float[] Lifetimes;
        public GameObject[] GameObjects;
        public Rigidbody2D[] Rigidbodies;
        public CircleCollider2D[] CircleColliders;
    }

    public static int gGetNextObjectPoolIndex(gudcObject udcObject) {
        //Declare
        int intIndex = udcObject.NextPoolNumber;
        //Increases and resets
        udcObject.NextPoolNumber = (udcObject.NextPoolNumber + 1) % udcObject.Lifetimes.Length; //Checks if past the max index and resets to zero
        //Return
        return intIndex;
    }

}