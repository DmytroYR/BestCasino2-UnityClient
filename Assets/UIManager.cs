using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance = null;
    public btn_handler spinButton;
    public btn_handler stopButton;
    public btn_handler autoButton;
    public Text bet_text;
    public Text won_text;
    public Text top_text;
    public Text playerName_text;
    public Text playerTitle_text;
    public Text playerBalance_text;
    public SpriteRenderer playerImage;

    public static UIManager Instance
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
    }
}
