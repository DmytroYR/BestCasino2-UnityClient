using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.VO;
using UnityEngine;

public class LineManager : MonoBehaviour {

    public GameObject[] lines;

    private List<LineWinResult> activeLines;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showLines(List<LineWinResult> value)
    {
        activeLines = value;

        for (int i = 0; i < activeLines.Count; i++)
        {
            lines[activeLines[i].lineNumber].SetActive(true);
        }
    }


}
