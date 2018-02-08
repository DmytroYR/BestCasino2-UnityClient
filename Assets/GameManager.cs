using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VO;

public class GameManager : MonoBehaviour {

    public SymbolManager symbolManager;
    public SpinManager playerSpinManager;
    public SpinManager[] socialSpinManagers;
    
    
	// Use this for initialization
	void Start () {
		
	}
	
    void SpinResultHandler()
    {
        //playerSpinManager.setResultData();

        /*
        SpinResultVO sr = new SpinResultVO();
        sr.lines = new List<LineWinResult>();
        sr.lines.Add(new LineResult());
        */
    }

    
	// Update is called once per frame
	void Update () {
		
	}
}
