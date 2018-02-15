using com.tangelogames.extensions.model.vo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

    public GameObject[] lines;

    public int cycleTime = 2;//seconds between line switch
    public bool cyclic = true;// cycling between line display 

    private int lineIndex = 0;
    private GameObject showingLine;

    private bool alphaUp = false;

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
        if (showingLine)
        {
            SpriteRenderer sr = showingLine.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a += alphaUp ? 0.1f : -0.1f;
            if (c.a > 1)
                alphaUp = false;
            else if (c.a < 0)
                alphaUp = true;
            sr.color = new Color(1, 1, 1, c.a);
        }

    }

    private void hideAll()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetActive(false);
        }
    }
    private void showAll()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetActive(true);
        }
    }

    IEnumerator Step()
    {
        while(true)
        {
            hideAll();

            showLine(lines[activeLines[lineIndex].lineNumber]);
            
            lineIndex++;
            if (lineIndex == activeLines.Count) lineIndex = 0;

            yield return new WaitForSeconds(cycleTime);

        }
    }

    private void showLine(GameObject line)
    {
        alphaUp = false;
        showingLine = line;
        showingLine.SetActive(true);
        showingLine.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
    }
    
    internal void stop()
      {
        StopCoroutine("Step");
        activeLines = null;

        hideAll();
    }

    public void showLines(List<LineWinResult> value)
    {
        activeLines = value;
        StartCoroutine("Step");

    }


}
