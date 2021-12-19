using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
  private Rigidbody2D rb;
  private AudioClip _clipRecord;
  private int _sampleWindow=128;
  public string Name_input_device;
  public float loudness = 0;
 
    void Start()
   {
        rb = GetComponent<Rigidbody2D>();
   }

  void Update()
 {
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("menu");
        }
        if (Name_input_device != Options.device_name || !Microphone.IsRecording(Name_input_device))
        {
            ReserRecoredClip();
        }
  loudness = GetAveragedVolume() * Options.sensivity_global;
  rb.AddForce(transform.up * loudness);
 }

  float GetAveragedVolume()  // метод получения громкости
  {
    float levelMax = 0;
    float[] waveData = new float[_sampleWindow];
    int micPosition = Microphone.GetPosition(Name_input_device) - (_sampleWindow + 1);
    if (micPosition < 0)
    {
      return 0;
    }
    _clipRecord.GetData(waveData, micPosition);
    for (int i = 0; i < _sampleWindow; i+=2)
    {
      float wavePeak = Mathf.Sqrt(waveData[i]);
      if (levelMax < wavePeak)
      {
        levelMax = wavePeak;
      }
    }
    return levelMax;
  }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Let")
        {
            SceneManager.LoadScene("game_scene");
        }
    }
    void ReserRecoredClip()
    {
        foreach (string item in Microphone.devices)
        {
            Microphone.End(item);
        }
        Name_input_device = Options.device_name;
        _clipRecord = Microphone.Start(Name_input_device, true, 3599, 3000);
    }
}
