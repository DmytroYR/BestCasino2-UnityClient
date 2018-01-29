using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : MonoBehaviour {

    private static SymbolManager instance = null;

    public Sprite[] symbols;

    public Dictionary<int, AnimationClip> animations = new Dictionary<int, AnimationClip>();

    public AnimationClip[] InsertAnimations;
    public int[] InsertAnimationsOrdinals;

    public static SymbolManager Instance
    {
        get 
        {
           return instance;
            
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < InsertAnimations.Length; i++)
        {
            animations[InsertAnimationsOrdinals[i]] = InsertAnimations[i];
        }
    }
    
}
