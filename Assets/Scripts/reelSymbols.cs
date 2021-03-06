﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reelSymbols : MonoBehaviour
{
    private Sprite[] images;
    public int ordinal; // image identifier
    public float reelPosition;  // location of symobl on path between reel start and end point - o to 1 
    public SpriteRenderer baseSprite;
    
    // ANIMATION Variables
    private Sprite[] activeAnim;
    private int animFrame;
    private bool animating;

    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;

    public void initalize( int o)
    {
        images = SymbolManager.Instance.symbols;

        animator = baseSprite.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
        animator.enabled = false;

        setOrdinal(o);
    }


    public void setOrdinal(int value, bool animated = false)
    {
        if (animating)
            animStop();

        ordinal = value;

        if (!animated)
        {
            if (images[ordinal] == null )
            {
                Debug.Log("symbol not found");
                baseSprite.sprite = null; 
            }
            else
                baseSprite.sprite = images[ordinal];
        }
        else
            animStart();
        
    }

    public void animStart()
    {
        if ( SymbolManager.Instance.animations.ContainsKey(ordinal)  )
        {
            animating = true;
            animator.enabled = true;
            animatorOverrideController["Idle"] = SymbolManager.Instance.animations[ordinal];
            baseSprite.GetComponent<Animator>().Play("Idle");
        }
        else
        {
            baseSprite.sprite = images[ordinal];
        }
        
    }

    public void animStop()
    {
        animating = false;
        animator.enabled = false;
        baseSprite.sprite = images[ordinal];
    }


}