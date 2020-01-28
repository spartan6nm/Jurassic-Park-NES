using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class zsUnityExtensions {

    //Declare enums
    public enum enumRotationAxes {
        X, Y, Z
    }

    /// <summary>
    /// Rotate by world or local angles over time with constant speed, returns true if finished.
    /// </summary>
    public static bool AngleRotation(this Transform trnTransform, ref float fltCurrentRotation, float fltRotationAmount, float fltRotationSpeed,
                                     enumRotationAxes enumLocalEulerAngleRotationAxis, bool blnWorld) {
        //Set current rotation
        fltCurrentRotation = Mathf.MoveTowards(fltCurrentRotation, fltRotationAmount, fltRotationSpeed);
        //Check if world or local
        if (blnWorld) {
            //Check axis
            switch (enumLocalEulerAngleRotationAxis) {
                case enumRotationAxes.X:
                    //Rotate
                    trnTransform.eulerAngles = new Vector3(fltCurrentRotation, 0f, 0f);
                    break;
                case enumRotationAxes.Y:
                    //Rotate
                    trnTransform.eulerAngles = new Vector3(0f, fltCurrentRotation, 0f);
                    break;
                case enumRotationAxes.Z:
                    //Rotate
                    trnTransform.eulerAngles = new Vector3(0f, 0, fltCurrentRotation);
                    break;
            }
        } else {
            //Check axis
            switch (enumLocalEulerAngleRotationAxis) {
                case enumRotationAxes.X:
                    //Rotate
                    trnTransform.localEulerAngles = new Vector3(fltCurrentRotation, 0f, 0f);
                    break;
                case enumRotationAxes.Y:
                    //Rotate
                    trnTransform.localEulerAngles = new Vector3(0f, fltCurrentRotation, 0f);
                    break;
                case enumRotationAxes.Z:
                    //Rotate
                    trnTransform.localEulerAngles = new Vector3(0f, 0, fltCurrentRotation);
                    break;
            }
        }
        //Return
        return fltCurrentRotation == fltRotationAmount;
    }

    /// <summary>
    /// Addresses a unity animator bug.
    /// </summary>
    public static void SetBoolFalseFix(this Animator amrAnimator, string strBoolParameter) {
        //Set bool false
        amrAnimator.SetBool(strBoolParameter, false);
        //Update the animator
        amrAnimator.Update(0);
    }

    /// <summary>
    /// Rotate towards with a check if at rotation, works with world and local rotation.
    /// </summary>
    public static bool RotateTowardsCheck(this Transform trnTransform, Quaternion quaGetTo, float fltRotateSpeed, bool blnWorld) {
        //Check if world or local
        if (blnWorld) {
            //Check if not there
            if (trnTransform.rotation != quaGetTo) {
                //Rotate towards
                trnTransform.rotation = Quaternion.RotateTowards(trnTransform.rotation, quaGetTo, fltRotateSpeed);
            }
            //Check if reached destination
            return trnTransform.rotation == quaGetTo;
        } else {
            //Check if not there
            if (trnTransform.localRotation != quaGetTo) {
                //Rotate towards
                trnTransform.localRotation = Quaternion.RotateTowards(trnTransform.localRotation, quaGetTo, fltRotateSpeed);
            }
            //Check if reached destination
            return trnTransform.localRotation == quaGetTo;
        }
    }

    /// <summary>
    /// Move towards with a check if at position, works with world and local position.
    /// </summary>
    public static bool MoveTowardsCheck(this Transform trnTransform, Vector3 v3GetTo, float fltMoveSpeed, bool blnWorld) {
        //Check if world or local
        if (blnWorld) {
            //Check if not there
            if (trnTransform.position != v3GetTo) {
                //Move towards
                trnTransform.position = Vector3.MoveTowards(trnTransform.position, v3GetTo, fltMoveSpeed);
            }
            //Check if reached destination
            return trnTransform.position == v3GetTo;
        } else {
            //Check if not there
            if (trnTransform.localPosition != v3GetTo) {
                //Move towards
                trnTransform.localPosition = Vector3.MoveTowards(trnTransform.localPosition, v3GetTo, fltMoveSpeed);
            }
            //Check if reached destination
            return trnTransform.localPosition == v3GetTo;
        }
    }

    /// <summary>
    /// Set position, rotation, and scale for this transform.
    /// </summary>
    public static void SetTransformation(this Transform trnTransform, Vector3 v3Position, Quaternion quaRotation, Vector3 v3Scale, bool blnWorld) {
        //Check if world or local
        if (blnWorld) {
            //Set world position
            trnTransform.position = v3Position;
            //Set world rotation
            trnTransform.rotation = quaRotation;
            //Set scale
            trnTransform.localScale = v3Scale;
        } else {
            //Set local position
            trnTransform.localPosition = v3Position;
            //Set local rotation
            trnTransform.localRotation = quaRotation;
            //Set local scale
            trnTransform.localScale = v3Scale;
        }
    }

    /// <summary>
    /// Set position, euler angles, and scale for this transform.
    /// </summary>
    public static void SetTransformation(this Transform trnTransform, Vector3 v3Position, Vector3 v3EulerAngles, Vector3 v3Scale, bool blnWorld) {
        //Check if world or local
        if (blnWorld) {
            //Set world position
            trnTransform.position = v3Position;
            //Set world rotation
            trnTransform.eulerAngles = v3EulerAngles;
            //Set scale
            trnTransform.localScale = v3Scale;
        } else {
            //Set local position
            trnTransform.localPosition = v3Position;
            //Set local rotation
            trnTransform.localEulerAngles = v3EulerAngles;
            //Set local scale
            trnTransform.localScale = v3Scale;
        }
    }

    /// <summary>
    /// Set parent, position, rotation, and scale for this transform, works with world and local position.
    /// </summary>
    public static void SetParentAndTransformation(this Transform trnTransform, Transform trnParent, Vector3 v3Position, Quaternion quaRotation, Vector3 v3Scale,
                                                  bool blnWorld) {
        //Set parent
        trnTransform.parent = trnParent;
        //Set transformation
        SetTransformation(trnTransform, v3Position, quaRotation, v3Scale, blnWorld);
    }

    /// <summary>
    /// Set parent, position, euler angles, and scale for this transform, works with world and local position.
    /// </summary>
    public static void SetParentAndTransformation(this Transform trnTransform, Transform trnParent, Vector3 v3Position, Vector3 v3EulerAngles,
                                                  Vector3 v3Scale, bool blnWorld) {
        //Set parent
        trnTransform.parent = trnParent;
        //Set transformation
        SetTransformation(trnTransform, v3Position, v3EulerAngles, v3Scale, blnWorld);
    }

    /// <summary>
    /// Set position and euler angles for this transform.
    /// </summary>
    public static void SetPositionAndEulerAngles(this Transform trnTransform, Vector3 v3Position, Vector3 v3EulerAngles, bool blnWorld) {
        //Check if world or local
        if (blnWorld) {
            //Set world position
            trnTransform.position = v3Position;
            //Set world rotation
            trnTransform.eulerAngles = v3EulerAngles;
        } else {
            //Set local position
            trnTransform.localPosition = v3Position;
            //Set local rotation
            trnTransform.localEulerAngles = v3EulerAngles;
        }
    }

}