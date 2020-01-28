using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zsDNAText : MonoBehaviour {

    //Declare serializables
    [Header("Main")]
    [SerializeField] private Text TheText;
    [SerializeField] private float TimeBetweenCharacters = 0.025f;
    [SerializeField] private float TimeBetweenLines = 0.5f;
    [SerializeField] private float TimeWaitingAtEnd = 0.5f;
    [SerializeField] private zsAnimationFinished DNAAnimationFinishedScript;
    [Header("Line Movement")]
    [SerializeField] private float TwoLineYMovement = 35.6f;
    [SerializeField] private float OneLineYMovement = 18.6f;
    [SerializeField] private float LineMovementSpeed = 75f;

    //Declare privates
    private float fltWaitingAnimationTime = 0f;
    private int intIndex = 0;
    private string strText = strEMPTY;
    private enumDNATextBreakDowns enumDNATextBreakDown = enumDNATextBreakDowns.Timer;
    private enumDNATextModes enumDNATextMode = enumDNATextModes.RegularLine;
    private int intNumberOfWaitsAtEndOfLine = 0;
    private float fltLocalPositionY = 0f;

    //Declare enums
    private enum enumDNATextBreakDowns {
        Timer, Distance
    }
    private enum enumDNATextModes {
        RegularLine, WaitingAtEnd, NewLines //WaitingAtEnd = $, NewLines = strNEW_LINES, GoingDownTwoLines = /, GoingDownOneLine = |
    }

    //Declare constants
    private const string strEMPTY = "";
    private const string strWHITE_SQUARE = "=";
    private const string strWAITING_AT_END = "$";
    private const string strGOING_DOWN_TWO_LINES = "/";
    private const string strGOING_DOWN_ONE_LINE = "|";
    private const int intNUMBER_OF_WAITS_AT_END_OF_LINE = 3;

    private void Start() {
        //Set
        fltWaitingAnimationTime = TimeBetweenCharacters;
        //Set
        strText = TheText.text;
        //Empty
        TheText.text = strEMPTY;
    }

    private void Update() {
        //Check if animation is finished
        if (DNAAnimationFinishedScript.AnimationFinished) {
            //Check the break down
            switch (enumDNATextBreakDown) {
                default: //enumDNATextBreakDowns.Timer
                    //Check the timer
                    if (fltWaitingAnimationTime > 0f) {
                        //Subtract
                        fltWaitingAnimationTime -= Time.deltaTime;
                        //Check if time up
                        if (fltWaitingAnimationTime <= 0f) {
                            //Check DNA text mode
                            switch (enumDNATextMode) {
                                case enumDNATextModes.RegularLine: //"Just characters"
                                    //Check if not empty
                                    if (TheText.text.Length > 0) {
                                        //Check if should remove last character
                                        if (!blnHasAReturnLine()) {
                                            //Remove last character square
                                            TheText.text = TheText.text.Substring(0, TheText.text.Length - 1);
                                        }
                                    }
                                    //Add a character
                                    TheText.text += strText.Substring(intIndex, 1) + strWHITE_SQUARE;
                                    //Increase index
                                    intIndex += 1;
                                    //Check next string
                                    if (strText.Substring(intIndex, 1) == strWAITING_AT_END) {
                                        //Set DNA text mode
                                        enumDNATextMode = enumDNATextModes.WaitingAtEnd;
                                        //Set
                                        intNumberOfWaitsAtEndOfLine = 0;
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeWaitingAtEnd;
                                        //Increase index
                                        intIndex += 1;
                                    } else if (blnCheckForGoingDownTwoLines(intIndex)) {
                                        //Change DNA text mode and break down
                                        ChangeDNATextModeAndBreakDown(TwoLineYMovement);
                                    } else if (blnCheckForGoingDownOneLine(intIndex)) {
                                        //Change DNA text mode and break down
                                        ChangeDNATextModeAndBreakDown(OneLineYMovement);
                                        ////Increase index
                                        intIndex += 1;
                                    } else {
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeBetweenCharacters;
                                    }
                                    break;
                                case enumDNATextModes.WaitingAtEnd: //"$"
                                    //Check type of wait
                                    switch (intNumberOfWaitsAtEndOfLine) {
                                        default: //0
                                        case 2:
                                            //Remove last character square
                                            TheText.text = TheText.text.Substring(0, TheText.text.Length - 1);
                                            break;
                                        case 1:
                                            //Show white square at end
                                            TheText.text += strWHITE_SQUARE;
                                            break;
                                    }
                                    //Increase
                                    intNumberOfWaitsAtEndOfLine += 1;
                                    //Check if number of waits is up
                                    if (intNumberOfWaitsAtEndOfLine == intNUMBER_OF_WAITS_AT_END_OF_LINE) {
                                        //Increase index to check the next character
                                        intIndex += 1;

                                        //string strWhatever1 = strText.Substring(intIndex, 1);
                                        //string strWhatever2 = strText.Substring(intIndex-1, 1);
                                        //string strWhatever3 = strText.Substring(intIndex+1, 1);
                                        //string strWhatever4 = strText.Substring(intIndex+2, 1);

                                        //Check for return
                                        if (blnHasAReturnLine()) {
                                            //Set DNA text mode
                                            enumDNATextMode = enumDNATextModes.NewLines;
                                            //Reset timer
                                            fltWaitingAnimationTime = TimeBetweenLines;

                                            //Decrease index
                                            intIndex -= 1;
                                            //Skip line
                                            TheText.text += strText.Substring(intIndex, 2);
                                            //Increase index
                                            intIndex += 1;

                                        } else if (blnCheckForGoingDownTwoLines(intIndex - 1)) {
                                            //Change DNA text mode and break down
                                            ChangeDNATextModeAndBreakDown(TwoLineYMovement);



                                            ////Increase index
                                            //intIndex += 1;

                                        } else if (blnCheckForGoingDownOneLine(intIndex - 1)) {
                                            //Change DNA text mode and break down
                                            ChangeDNATextModeAndBreakDown(OneLineYMovement);




                                        } else {
                                            //Set DNA text mode
                                            enumDNATextMode = enumDNATextModes.RegularLine;
                                            //Reset timer
                                            fltWaitingAnimationTime = TimeBetweenCharacters;

                                            //Decrease index
                                            intIndex -= 1;
                                            //Skip line
                                            TheText.text += strText.Substring(intIndex, 2);
                                            //Increase index
                                            intIndex += 1;

                                        }
                                    } else {
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeWaitingAtEnd;
                                    }
                                    break;
                                case enumDNATextModes.NewLines: //"Return lines"
                                    //Set DNA text mode
                                    enumDNATextMode = enumDNATextModes.RegularLine;
                                    //Reset timer
                                    fltWaitingAnimationTime = TimeBetweenCharacters;
                                    break;
                            }
                        }
                    }
                    break;
                case enumDNATextBreakDowns.Distance: // "/" and "|"
                    //Move up
                    if (transform.MoveTowardsCheck(new Vector3(transform.localPosition.x, fltLocalPositionY, 0f), LineMovementSpeed * Time.deltaTime, false)) {
                        ////Check if going down two lines
                        //if (blnCheckForGoingDownTwoLines()) {
                        //    //intIndex += 2;
                        //} else {
                        //    intIndex += 1;
                        //}
                        //Change break down mode
                        enumDNATextBreakDown = enumDNATextBreakDowns.Timer;
                        //Set DNA text mode
                        enumDNATextMode = enumDNATextModes.RegularLine;
                        //Increase index
                        //intIndex += 1;
                        //intIndex += 2; //Figure out when to do intIndex += 1;
                        //Reset timer
                        fltWaitingAnimationTime = TimeBetweenCharacters;
                    }
                    break;
            }
        }
    }

    private void ChangeDNATextModeAndBreakDown(float fltMovement) {
        //Set
        fltLocalPositionY = transform.localPosition.y + fltMovement;
        //Change DNA text break down mode
        enumDNATextBreakDown = enumDNATextBreakDowns.Distance;
    }

    private bool blnCheckForGoingDownTwoLines(int intIndexToCheck) {
        //Return
        return strText.Substring(intIndexToCheck, 1) == strGOING_DOWN_TWO_LINES;
    }

    private bool blnCheckForGoingDownOneLine(int intIndexToCheck) {
        //Return
        return strText.Substring(intIndexToCheck, 1) == strGOING_DOWN_ONE_LINE;
    }

    private bool blnHasAReturnLine() {
        //Check length
        if (intIndex < strText.Length - 2) {
            //Return
            return strText.Substring(intIndex, 2) == System.Environment.NewLine;
        } else {
            //Return
            return false;
        }
    }

}