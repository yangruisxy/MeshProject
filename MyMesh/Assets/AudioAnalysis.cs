using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalysis : MonoBehaviour {
    AudioSource _audioSource;
    public  float[] _samples = new float[256];
    public static float[] _freqBand = new float[20];
	// Use this for initialization
	void Start () {
        _audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        GetSpectrumAudioSource();
        MakeFrequencyBand();
	}

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand()
    {
        //
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 20; ++i)
        {
            float average = 0;

            if (i == 2 || i == 4 || i == 8 || i == 12 || i == 16)
            {
                ++power;
                sampleCount = (int)Mathf.Pow(2, power);
                if (i == 16)
                {
                    sampleCount += 37;
                }
            }
            Debug.Log(sampleCount);
            Debug.Log(count);
            for (int j = 0; j < sampleCount; ++j)
            {
                average += _samples[count] * (count + 1);
                ++count;
            }
            average /= count;
            _freqBand[i] = average * 10;

        }
        //for(int i = 0; i < 400; ++i)
        //{
        //    _freqBand[i] = _samples[i];
        //}
    }
}
