using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveProgress : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "GameSave.json";
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, fileName);
        CheckForFolder();
    }

    private void CheckForFolder()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("Creating " + folderPath + " as it does not currently exist");
            Directory.CreateDirectory(folderPath);
        }
    }

    public void SaveCurrentProgress()
    {
        CheckForFolder();
        GameObject player = FindObjectOfType<PlayerCharacter>().gameObject;
        InventoryController playerInventory = player.GetComponent<InventoryController>();
        PlayerSaveData savedData = new PlayerSaveData();
        savedData.zone = SceneManager.GetActiveScene().name;
        savedData.health = player.GetComponent<Damageable>().CurrentHealth;
        savedData.key1 = playerInventory.HasItem("Key1");
        savedData.key2 = playerInventory.HasItem("Key2");
        savedData.key3 = playerInventory.HasItem("Key3");
        savedData.weapons = playerInventory.HasItem("Staff");
        string saveDataString = JsonUtility.ToJson(savedData);
        File.WriteAllText(filePath, saveDataString);
    }
}
