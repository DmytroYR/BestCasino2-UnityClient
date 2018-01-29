using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour {


    public int[] spin_reel = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10 };
    public int[] result_reel = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    
    public reelManager[] reels;

	// Use this for initialization
	void Start () {
        
        initalizeReels();
    }

    private void Awake()
    {
        

    }

    void initalizeReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spin_reel = spin_reel;
            reels[i].result_reel = result_reel;
            reels[i].initalize( Random.Range(0,spin_reel.Length) );
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void spin()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].spin();

        }
    }

    public void stop()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].hard_Stop();

        }
    }
    
}
