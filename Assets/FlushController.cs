using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlushController : MonoBehaviour
{
    public static FlushController instance { get;private set; }
    Image img;

    void Start()
    {
        instance = this;
        img = GetComponent<Image>();
        img.color = Color.clear;
    }

    void Update()
    {
        {
            this.img.color = Color.Lerp(this.img.color, Color.clear, Time.deltaTime);
        }
    }
    public void BlueScreen()
    {
        this.img.color = new Color(0f, 0.7f, 1.0f, 0.5f);
    }
    public void RedScreen()
    {
        this.img.color = new Color(0.5f, 0f, 0f, 0.5f);
    }

    public void YellowScreen() 
    {
        //â©êF
        this.img.color = new Color(1.0f, 1.0f, 0.0f, 0.2f);
    }
}
