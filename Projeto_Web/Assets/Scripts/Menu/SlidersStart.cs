using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersStart : MonoBehaviour
{

    Slider self;
    public bool isMusic;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Slider>();

        if (isMusic)
            self.value = GameManager.singleton.music.volume;
        else
            self.value = GameManager.singleton.soundVolume;
    }
}
