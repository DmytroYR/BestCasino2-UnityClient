using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class btn_handler : MonoBehaviour  {

    public Sprite normal;
    public Sprite highlight;
    public Sprite pressed;
    public Sprite disabled;

    public UnityEvent OnClick;


    public bool enable_btn
    {
        set
        {
            enabled = value;

            if (!enabled)
                spr.sprite = disabled;
            else
                spr.sprite = normal;
        }
        get
        {
            return enabled;
        }
        
    }


    SpriteRenderer spr;
	
	void Start () {
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = normal;
    }

    void Awake()
    {
        if (OnClick == null)
            OnClick = new UnityEvent();
    }



    void OnMouseUp()
    {
        OnClick.Invoke(); // CLICK 
        spr.sprite = normal;
        //enable_btn = false;
        //gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        if (highlight && enabled)
            spr.sprite = highlight;
    }

    private void OnMouseExit()
    {
        if ( enabled)
            spr.sprite = normal;
    }


    void OnMouseDown()
    {
        if (pressed && enabled)
            spr.sprite = pressed;
    }


}
