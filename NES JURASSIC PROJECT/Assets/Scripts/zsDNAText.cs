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
    [SerializeField] private int BlinkCount = 2;
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
    private bool blnWhiteSquareBlink = false;
    private int intBlinkCount = 0;

    //Declare enums
    private enum enumDNATextBreakDowns {
        Timer, Distance
    }
    private enum enumDNATextModes {
        RegularLine, WaitingAtEnd //WaitingAtEnd = $, GoingDownTwoLines = /, GoingDownOneLine = |
    }

    //Declare constants
    private const string strEMPTY = "";
    private const string strWHITE_SQUARE = "=";
    private const string strWAITING_AT_END = "$";
    private const string strGOING_DOWN_TWO_LINES = "/";
    private const string strGOING_DOWN_ONE_LINE = "|";
    private const string strRETURN = "\r";
    private const string strNEW_LINE = "\n";

    private void Start() {
        //Set
        fltWaitingAnimationTime = TimeBetweenCharacters;
        //Set
        strText = TheText.text;
        //Empty
        TheText.text = strEMPTY;
        //Set
        intBlinkCount = BlinkCount + 1;
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
                                        //Check for white square
                                        if (TheText.text.Substring(TheText.text.Length - 1, 1) == strWHITE_SQUARE) {
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
                                        //Reset
                                        blnWhiteSquareBlink = true;
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeWaitingAtEnd;
                                        //Increase index
                                        intIndex += 1;
                                    } else if (blnCheckForGoingDownOneLine(intIndex)) {
                                        //Change DNA text mode and break down
                                        ChangeDNATextModeAndBreakDown(OneLineYMovement);
                                    } else {
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeBetweenCharacters;
                                    }
                                    break;
                                case enumDNATextModes.WaitingAtEnd: //"$"
                                    //Check blink
                                    if (blnWhiteSquareBlink) {
                                        //Remove last character square
                                        TheText.text = TheText.text.Substring(0, TheText.text.Length - 1);
                                    } else {
                                        //Show white square at end
                                        TheText.text += strWHITE_SQUARE;
                                    }
                                    //Change blink variable
                                    blnWhiteSquareBlink = !blnWhiteSquareBlink;
                                    //Increase
                                    intNumberOfWaitsAtEndOfLine += 1;
                                    //Check if number of waits is up
                                    if (intNumberOfWaitsAtEndOfLine == intBlinkCount) {
                                        //Check if index is not greater than the string text length
                                        if (intIndex < strText.Length) {
                                            //Check if has a return line
                                            while (blnHasAReturnLine()) {
                                                //Add to text
                                                TheText.text += strText.Substring(intIndex, 1);
                                                //Increase index
                                                intIndex += 1;
                                            }
                                            //Check next character in the string text
                                            if (blnCheckForGoingDownTwoLines(intIndex)) {
                                                //Change DNA text mode and break down
                                                ChangeDNATextModeAndBreakDown(TwoLineYMovement);
                                            } else if (blnCheckForGoingDownOneLine(intIndex)) {
                                                //Change DNA text mode and break down
                                                ChangeDNATextModeAndBreakDown(OneLineYMovement);
                                            } else {
                                                //Set DNA text mode
                                                enumDNATextMode = enumDNATextModes.RegularLine;
                                                //Reset timer
                                                fltWaitingAnimationTime = TimeBetweenCharacters;
                                            }
                                        } else {
                                            //Reset timer
                                            fltWaitingAnimationTime = TimeWaitingAtEnd;
                                        }
                                    } else {
                                        //Reset timer
                                        fltWaitingAnimationTime = TimeWaitingAtEnd;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
                case enumDNATextBreakDowns.Distance: // "/" and "|"
                    //Move up
                    if (transform.MoveTowardsCheck(new Vector3(transform.localPosition.x, fltLocalPositionY, 0f), LineMovementSpeed * Time.deltaTime, false)) {
                        //Change break down mode
                        enumDNATextBreakDown = enumDNATextBreakDowns.Timer;
                        //Set DNA text mode
                        enumDNATextMode = enumDNATextModes.RegularLine;
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
        //Increase index
        intIndex += 1;
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
        if (intIndex < strText.Length - 1) {
            //Declare
            string strNewLine = strText.Substring(intIndex, 1);
            //Return
            return strNewLine == strRETURN || strNewLine == strNEW_LINE;
        } else {
            //Return
            return false;
        }
    }

}