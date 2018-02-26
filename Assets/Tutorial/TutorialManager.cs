using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public string[] adviceSubjects;
    public string[] adviceContents;

    private Text subjectText;
    private Text contentsText;

    private void Start()
    {
        subjectText = GetComponentsInChildren<Text>()[0];
        contentsText = GetComponentsInChildren<Text>()[1];

        subjectText.text = adviceSubjects[0];
        contentsText.text = adviceContents[0];
    }
}
