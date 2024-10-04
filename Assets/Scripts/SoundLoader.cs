using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundLoader : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "pewsound.wav";
    private string filePath;
    private AudioClip shootyAudioClip;
    private RandomAudioPlayer randomAudioPlayer;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, fileName);
        randomAudioPlayer = GetComponent<RandomAudioPlayer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadSound();
        }
    }

    private void LoadSound()
    {
        if (File.Exists(filePath))
        {
            // Read data and store in byte array
            byte[] data = File.ReadAllBytes(filePath);
            // Converty byte array to float array, divide by 2 due to 2 bit = byte
            float[] floatArray = new float[data.Length / 2];

            for (int i = 0; i < floatArray.Length; i++)
            {
                // Converty data to 16 bit integer, move offset by 2
                short bitValue = System.BitConverter.ToInt16(data, i * 2);
                // Normalize value, 32678 being the max value possible
                floatArray[i] = bitValue / 32768f;
            }
            // Create clip
            shootyAudioClip = AudioClip.Create("ShootyAudioClip", floatArray.Length, 1, 44100, false);
            // Set audio clip to file data
            shootyAudioClip.SetData(floatArray, 0);

            for (int i = 0; i < randomAudioPlayer.clips.Length; i++)
            {
                // Replace all shooty clips with loaded shooty clip
                randomAudioPlayer.clips[i] = shootyAudioClip;
            }
        }
    }
}
