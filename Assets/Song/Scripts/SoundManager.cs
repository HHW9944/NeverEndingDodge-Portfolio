using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundMusic;  // 배경음악 재생을 위한 AudioSource 변수
    // Start is called before the first frame update
    void Start()
    {
        // 배경음악 재생
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;  // 음악이 계속 반복되도록 설정
            backgroundMusic.Play();       // 배경음악 재생 시작
        }
    }

}
