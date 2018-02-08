using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.VO;

public class SpinManager : MonoBehaviour {


    public int[] spin_reel = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };
    public int[] result_reel = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public LineManager lineManager;

    public reelManager[] reels;

    private int stoppedReelsCount = 0;
    private bool spinning = false;
    private SpinResultVO spin_result;

    // Use this for initialization
    void Start () {
        initalizeReels();
    }

    void initalizeReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spin_reel = spin_reel;
            reels[i].result_reel = result_reel;
            reels[i].initalize( Random.Range(0,spin_reel.Length) );
            reels[i].ReelStopped += OnReelStopped;
        }
    }


    public void spin()
    {
        if (spinning)
            return;

        spin_result = null;
        spinning = true;
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spin();
        }

        SpinResultVO res = new SpinResultVO();
        
        setResultData();
    }

    public void setResultData(SpinResultVO result )
    {
        spin_result = result;
        
        // SYMULATING A 15 SYMBOLS LIST 
        List<int> symbol_list = new List<int>();
        for (int i = 0; i < 15; i++)
        {
            symbol_list.Add(Random.Range(0, 11));
        }



        int symbol_runner = 0;
        int reelDisplaySize = 3;
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].setReelData(symbol_list.GetRange(symbol_runner, reelDisplaySize));
            symbol_runner += reelDisplaySize;
        }

    }

    public void stop()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].hard_Stop();

        }
    }

    
    private void OnReelStopped(object sender, System.EventArgs e)
    {
        stoppedReelsCount++;
        if (stoppedReelsCount >= reels.Length)
        {
            spinning = false;
            OnSpinFinished();
        }
    }

    private void OnSpinFinished()
    {
        if (spin_result.totalWon > 0 )
        {
            lineManager.showLines(spin_result.lines);
        }
    }


}
