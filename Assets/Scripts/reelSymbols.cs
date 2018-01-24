using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelSymbols : MonoBehaviour
{
    public Sprite[] images;
    public Dictionary<int, Sprite[]> animations;
    public int ordinal; // image identifier
    public float reelPosition;  // location of symobl on path between reel start and end point - o to 1 

    private SpriteRenderer spriteRenderer;
    
    // ANIMATION Variables
    private Sprite[] activeAnim;
    private int animFrame;
    private bool animating;
    
    void Start( )
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void initalize(Sprite[] i, int o)
    {
        images = i;
        setOrdinal(o);
    }

    public void addAnimation(Sprite[] anim, int ordinal)
    {
        animations.Add(ordinal, anim);
    }

    public void setOrdinal(int value, bool animated = false)
    {
        if (animating)
            animStop();

        ordinal = value;

        if (!animated)
            spriteRenderer.sprite = images[ordinal];
        else
            animStart();
        
    }

    public void animStart()
    {
        if ( animations.ContainsKey(ordinal)  )
        {
            animating = true;
            activeAnim = animations[ordinal];
            animFrame = 0;
        }
        
    }

    public void animStop()
    {
        animating = false;
  
    }

      // Update is called once per frame
    void Update()
    {
       if (animating)
        {
            spriteRenderer.sprite = activeAnim[animFrame];
            animFrame++;
            if (animFrame == activeAnim.Length) animFrame = 0;
        }
    }
}