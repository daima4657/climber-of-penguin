using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI—p‚ÉéŒ¾

public class Record : MonoBehaviour
{
    private float current_time;
    public float Current_time
    {
        get => current_time;
        set => current_time = value;
    }
    private float limit_time;
    public float Limit_time
    {
        get => limit_time;
        set => limit_time = value;
    }
    public Text recordText;
    public Text limitText;
    // Start is called before the first frame update
    void Start()
    {
        SetLimit();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GManager.instance.GameClearFlag)
        {
            current_time += Time.deltaTime;
            recordText.text = current_time.ToString("F2");
        }

        if (current_time > limit_time)
        {
            TimeOver();
        }
    }

    public void Reset()
    {
        current_time = 0;
        limit_time = 0;
        SetLimit();
    }
    public void SetLimit()
    {
        limit_time = GManager.instance.progress < 1000 ? current_time + 20 - (GManager.instance.progress / 100) : 10;
        limitText.text = limit_time.ToString("F2");
    }

    public void TimeOver()
    {
        GManager.instance.DataResetFlag = true;
    }
}
