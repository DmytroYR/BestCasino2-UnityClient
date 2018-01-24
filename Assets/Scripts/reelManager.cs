using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelManager : MonoBehaviour {

    public int[] spin_reel = { 0, 0, 0, 1, 1, 1, 2, 2, 2,3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6,  7, 7, 7, 8, 8, 8, 9, 9, 9,10,10, 10 };
    public int[] result_reel = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public int result_position = -1;


    public reelSymbols[] symbols;
    public Transform startMarker;
    public Transform endMarker;


    public float spin_speed = 4;
    public AnimationCurve speed_modifier_Start;
    public AnimationCurve speed_modifier_Stop;

    public float max_symbols = 40;

    public float pull_down_time;
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
    private float symbols_passed;
    private float stoptime = 0f;
    private List<int> orderSymbols = new List<int>();

    private int reelPosition;

    // Use this for initialization
    void Start() {
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        totalTime = 0;
        initalize();
    }

    void initalize()
    {
        reelPosition = Random.Range(0, result_reel.Length);
        for (int j = 0; j < symbols.Length; j++)
        {
            symbols[j].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, 1f / symbols.Length * j);
            symbols[j].reelPosition = 1f / symbols.Length * j;
            var a = (float)spin_reel[reelPosition] / 10f;
            symbols[j].GetComponent<Animator>().Play("symbol_anim", 0, (float)spin_reel[reelPosition]/10f);
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
                //float distanceToEndpoint = symbols[i].reelPosition;


                float fracJourney = symbols[i].reelPosition;//  1 - distanceToEndpoint / journeyLength;

                if (anticipationActive )
                    fracJourney += Time.deltaTime * spin_speed * spinModifier * anticipation_speed_mul;
                else
                    fracJourney += Time.deltaTime * spin_speed * spinModifier;

                if (fracJourney > 1)
                {
                    fracJourney = fracJourney - 1;
                    symbols_passed++;
                    stoptime = totalTime;

                    if (symbols_passed >=  (max_symbols + (anticipationActive ? anticipation_max_addition : 0) ) )
                    {
                        if (result_position != -1)
                        {
                            symbols_passed = max_symbols;
                            spinning = false;
                            last_spin = true;
                        }
                    }
                    else
                    {
                        reelPosition++;

                        if (reelPosition == spin_reel.Length) reelPosition = 0;

                        symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, (float)spin_reel[reelPosition] / 10f);
                        //symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, Random.Range(0f, 1f));
                    }
                }
                else if (fracJourney < 0)
                {
                    fracJourney = fracJourney + 1;
                    reelPosition--;

                    if (reelPosition == -1) reelPosition = spin_reel.Length - 1;

                    symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, (float)spin_reel[reelPosition] / 10f);
                    //symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, Random.Range(0f, 1f));
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
                    //symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, (symbols_passed - max_symbols) / 10);
                    symbols[i].GetComponent<Animator>().Play("symbol_anim", 0, (float) result_reel[result_position] / 10);
                    result_position++;
                    if (result_position == result_reel.Length) result_position = 0;
                }
                else
                    symbols[i].transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

                symbols[i].reelPosition = fracJourney;

                if (symbols_passed == max_symbols + symbols.Length + (anticipationActive ? anticipation_max_addition : 0))
                {
                    break;
                }
            }

            if (symbols_passed == max_symbols + symbols.Length + (anticipationActive ? anticipation_max_addition : 0) )
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


    public void spin()
    {
         if ( !spinning && !last_spin && !bounce )
        spinning = true;
        result_position = Random.Range(0, result_reel.Length);
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
            symbols_passed = max_symbols;
        }
    }
    
    private void shutItDown()
    {
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
    }
}