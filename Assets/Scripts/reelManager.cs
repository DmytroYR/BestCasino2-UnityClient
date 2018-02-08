using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelManager : MonoBehaviour {

    public int[] spin_reel; 
    public int[] result_reel;
    public Sprite[] temp_symbols;
    
    public int result_position = -1;

    public reelSymbols[] symbols;
    public Transform startMarker;
    public Transform endMarker;
    
    public float spin_speed = 4;
    public AnimationCurve speed_modifier_Start;
    public AnimationCurve speed_modifier_Stop;

    public float spin_time = 10;//seconds until transition to last spin

    public float pull_up_time = 1;
    public float bounce_time = 2;
    public float anticipation_max_addition;
    public float anticipation_speed_mul;

    public bool spinning = false;

    public  bool anticipationActive = false;

    private bool first_spin = true;
    private bool last_spin = false;
    
    private bool bounce = false;

    private float journeyLength;
    private float totalTime;
    private float spinModifier = 1;
    private int symbols_passed;
    private float stoptime = 0f;
    private List<int> orderSymbols = new List<int>();

    private int reelPosition;
    private List<int> result_symbols;

    void Start() {
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        totalTime = 0;
        //initalize(Random.Range(0, 20));
        //spin();
    }

    public void initalize(int startPosition)
    {
        reelPosition = startPosition;
        for (int j = 0; j < symbols.Length; j++)
        {
            symbols[j].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, 1f / symbols.Length * j);
            symbols[j].initalize( spin_reel[reelPosition]);

            reelPosition++;

            if (reelPosition == spin_reel.Length) reelPosition = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (spinning)
        {
            if (first_spin)
            {
                first_spin = false;
                for (int i = 0; i < symbols.Length; i++)
                {
                    symbols[i].reelPosition = 1 - Vector3.Distance(symbols[i].transform.position, endMarker.position) / journeyLength;
                }
            }


            totalTime += Time.deltaTime;

            if (totalTime <= pull_up_time)
            {
                spinModifier = speed_modifier_Start.Evaluate(totalTime / pull_up_time);
            }
            else
            {
                spinModifier = 1;
            }



            for (int i = 0; i < symbols.Length; i++)
            {
                float fracJourney = symbols[i].reelPosition;

                if (anticipationActive )
                    fracJourney += Time.deltaTime * spin_speed * spinModifier * anticipation_speed_mul;
                else
                    fracJourney += Time.deltaTime * spin_speed * spinModifier;

                if (fracJourney > 1)
                {
                    fracJourney = fracJourney - 1;
                    symbols_passed++;
                    stoptime = totalTime;

                    if (totalTime >= spin_time) // Buildup animation
                    {
                        if (result_symbols.Count > 0 )
                        {
                            symbols_passed = 0;// max_symbols;
                            spinning = false;
                            last_spin = true;
                        }
                    }
                    else
                    {
                        reelPosition++;
                        if (reelPosition == spin_reel.Length) reelPosition = 0;

                        symbols[i].setOrdinal(spin_reel[reelPosition]);

                    }
                }
                else if (fracJourney < 0)
                {
                    fracJourney = fracJourney + 1;
                    reelPosition--;
                    if (reelPosition == -1) reelPosition = spin_reel.Length - 1;
                    
                    symbols[i].setOrdinal(spin_reel[reelPosition]);
                }


                symbols[i].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
                symbols[i].reelPosition = fracJourney;
            }

        }
        else if (last_spin)
        {
            totalTime += Time.deltaTime;

            for (int i = 0; i < symbols.Length; i++)
            {
                float fracJourney = symbols[i].reelPosition;//  1 - distanceToEndpoint / journeyLength;
                fracJourney += Time.deltaTime * spin_speed;

                if (fracJourney > 1)
                {
                    symbols_passed++;
                    orderSymbols.Insert(0, i);
                    fracJourney = fracJourney - 1;
                    symbols[i].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

                    // SETTING OF SYMBOLS THAT WILL DISPLAYED AFTER STOP
                    if (result_symbols[result_symbols.Count - symbols_passed] == -1)
                        symbols[i].setOrdinal(spin_reel[reelPosition], true);
                    else
                        symbols[i].setOrdinal(result_symbols[result_symbols.Count - symbols_passed], true);


                }
                else
                    symbols[i].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

                symbols[i].reelPosition = fracJourney;

                if (symbols_passed == symbols.Length) // Build animation adjustment
                {
                    break;
                }
            }

            if (symbols_passed == symbols.Length) // Build animation adjustment
            {
                for (int j = 0; j < symbols.Length; j++)
                {
                    symbols[orderSymbols[j]].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, 1f / symbols.Length * j);
                    symbols[orderSymbols[j]].reelPosition = 1f / symbols.Length * j;
                }

                last_spin = false;
                bounce = true;
            }
        }
        else if (bounce)
        {
            totalTime += Time.deltaTime;
            spinModifier = speed_modifier_Stop.Evaluate((totalTime - stoptime) / bounce_time);

            if (totalTime < stoptime + bounce_time)
            {
                for (int i = 0; i < symbols.Length; i++)
                {
                    float fracJourney = symbols[i].reelPosition;//  1 - distanceToEndpoint / journeyLength;
                    fracJourney = fracJourney + spinModifier;
                    symbols[i].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
                }
            }
            else
                shutItDown();


        }
    }


    public void spin( )
    {
        if ( !spinning && !last_spin && !bounce )
        {
            spinning = true;
            
        }
    }

    public void setReelData(List<int> symbols)
    {
        result_symbols = new List<int>();
        result_symbols.Add(-1);
        result_symbols.AddRange(symbols);
        result_symbols.Add(-1);
    }

    public void hard_Stop()
    {
        if (spinning)
        { 
            last_spin = true;
            bounce = false;
            first_spin = false;
            
            spinning = false;
            anticipationActive = false;
            symbols_passed = 0;
        }
    }
    
    private void shutItDown()
    {
        OnReelStopped(new EventArgs());

        bounce = false;
        first_spin = true;
        spinning = false;
        
        last_spin = false;
        anticipationActive = false;
        orderSymbols = new List<int>();
        symbols_passed = 0;
        totalTime = 0;
        stoptime = 0;
        result_position = -1;
        result_symbols = new List<int>();
        //spin();
    }

    public event EventHandler ReelStopped;
    protected virtual void OnReelStopped(EventArgs e)
    {
        if (ReelStopped != null)
            ReelStopped(this, e);
    }

}