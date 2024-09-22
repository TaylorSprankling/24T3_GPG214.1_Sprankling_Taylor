using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPreferencesLoader : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "AudioPreferences.xml";
    private string filePath;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterAudioGroupParameter = "MasterVolume";
    [SerializeField] private string musicAudioGroupParameter = "MusicVolume";
    [SerializeField] private string sFXAudioGroupParameter = "SFXMasterVolume";

    private void Start()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Creating " + folderPath + " as it does not currently exist");
            Directory.CreateDirectory(folderPath);
        }
        filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
        {
            Debug.Log("Creating " + filePath + " as it does not currently exist");
            CreateVolumePreferences();
        }
        else
        {
            GetVolumePreferences();
        }
    }

    private void CreateVolumePreferences()
    {
        AudioPreferences audioPreferences = new AudioPreferences();
        audioMixer.GetFloat(masterAudioGroupParameter, out float masterVol);
        audioPreferences.MasterVolume = masterVol;
        audioMixer.GetFloat(musicAudioGroupParameter, out float musicVol);
        audioPreferences.MusicVolume = musicVol;
        audioMixer.GetFloat(sFXAudioGroupParameter, out float sFXMasterVol);
        audioPreferences.SFXMasterVolume = sFXMasterVol;
        XmlSerializer serializer = new XmlSerializer(typeof(AudioPreferences));
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, audioPreferences);
        }
    }

    private void GetVolumePreferences()
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            if (File.Exists(filePath))
            {
                AudioPreferences audioPreferences = new AudioPreferences();
                XmlSerializer serializer = new XmlSerializer(typeof(AudioPreferences));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    audioPreferences = (AudioPreferences)serializer.Deserialize(reader);
                }
                audioMixer.SetFloat(masterAudioGroupParameter, audioPreferences.MasterVolume);
                audioMixer.SetFloat(musicAudioGroupParameter, audioPreferences.MusicVolume);
                audioMixer.SetFloat(sFXAudioGroupParameter, audioPreferences.SFXMasterVolume);
            }
            else
            {
                Debug.LogError("Audio preferences file path does not exist");
            }
        }
        else
        {
            Debug.LogError("No file name provided for audio preferences");
        }
    }
}
