using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SaveAudioPreferences : MonoBehaviour
{
    private MixerSliderLink mixerSliderLink;
    private Slider volumeSlider;
    private AudioMixer audioMixer;
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "AudioPreferences.xml";
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, fileName);

        CheckForFile();

        mixerSliderLink = GetComponent<MixerSliderLink>();
        volumeSlider = GetComponent<Slider>();
        audioMixer = mixerSliderLink.mixer;
        volumeSlider.onValueChanged.AddListener(SliderValueChange);
    }

    private void CheckForFile()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Creating " + folderPath + " as it does not currently exist");
            Directory.CreateDirectory(folderPath);
        }
        if (!File.Exists(filePath))
        {
            Debug.Log("Creating " + filePath + " as it does not currently exist");
            AudioPreferences audioPreferences = new AudioPreferences();
            XmlSerializer serializer = new XmlSerializer(typeof(AudioPreferences));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, audioPreferences);
            }
        }
    }

    private void SliderValueChange(float value)
    {
        CheckForFile();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath); // Load the saved audio settings data to prevent overwriting other nodes
        XmlNode xmlNode = xmlDocument.SelectSingleNode("/AudioPreferences/" + mixerSliderLink.mixerParameter); // Select the appropriate node
        if (xmlNode == null)
        {
            // Create node if it doesn't exist
            Debug.Log("Creating " + mixerSliderLink.mixerParameter + " XML node as it does not currently exist");
            XmlNode newXmlNode = xmlDocument.CreateElement(mixerSliderLink.mixerParameter);
            xmlDocument.DocumentElement.AppendChild(newXmlNode);
            xmlNode = xmlDocument.SelectSingleNode("/AudioPreferences/" + mixerSliderLink.mixerParameter); // Select created node
        }
        xmlNode.InnerText = (mixerSliderLink.minAttenuation + value * (mixerSliderLink.maxAttenuation - mixerSliderLink.minAttenuation)).ToString(); // Change node value accordingly
        xmlDocument.Save(filePath); // Save altered audio settings file
    }
}
