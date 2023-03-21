using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2AudioMananger : MonoBehaviour
{
    private GameObject canvas;

    private Button btn;
    private Slider fuel;

    public AudioSource audios;
    public AudioClip engine;
    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
        canvas = GameObject.Find("Canvas");
        btn = canvas.transform.Find("EnginesToggleBtn").GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        fuel = canvas.transform.Find("FuelBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fuel.value == 0)
        {
            Invoke("SlowStep1", 0);
            Invoke("SlowStep2", 1);
            Invoke("SlowStep3", 2);
            Invoke("SlowStep4", 3);
            Invoke("SlowStep5", 4);
        }
    }

    void TaskOnClick()
    {
        audios.pitch = 2.5f;
        audios.clip = engine;
        audios.Play();
    }

    void SlowStep1()
    {
        audios.pitch = 1.5f;
        audios.volume = .8f;
    }

    void SlowStep2()
    {
        audios.pitch = 1f;
        audios.volume = .6f;
    }

    void SlowStep3()
    {
        audios.pitch = .5f;
        audios.volume = .4f;
    }

    void SlowStep4()
    {
        audios.pitch = .1f;
        audios.volume = .2f;
    }

    void SlowStep5()
    {
        audios.Stop();
    }
}
