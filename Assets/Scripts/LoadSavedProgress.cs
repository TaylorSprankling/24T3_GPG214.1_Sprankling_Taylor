using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSavedProgress : MonoBehaviour
{
    private TransitionPoint transitionPoint;
    [SerializeField] private GameObject transitionLoadObject;
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "GameSave.json";
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, fileName);
        transitionPoint = GetComponent<TransitionPoint>();
    }

    private bool CheckForFile()
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            Debug.Log(filePath + " does not currently exist");
            return false;
        }
    }

    public void LoadPrevious()
    {
        if (CheckForFile())
        {
            string gameSaveString = File.ReadAllText(filePath); // Save file text to string
            PlayerSaveData gameSave = JsonUtility.FromJson<PlayerSaveData>(gameSaveString); // Convert json string to game save data
            if (gameSave != null)
            {
                // Change transition variables according to save data so that game starts from the zone the player left off with same items & health
                transitionPoint.newSceneName = gameSave.zone;
                transitionPoint.transitioningGameObject = transitionLoadObject;
                transitionLoadObject.GetComponent<Damageable>().SetHealth(gameSave.health);
                if (gameSave.key1)
                {
                    transitionLoadObject.GetComponent<InventoryController>().AddItem("Key1");
                }
                if (gameSave.key2)
                {
                    transitionLoadObject.GetComponent<InventoryController>().AddItem("Key2");
                }
                if (gameSave.key3)
                {
                    transitionLoadObject.GetComponent<InventoryController>().AddItem("Key3");
                }
                if (gameSave.weapons)
                {
                    transitionLoadObject.GetComponent<InventoryController>().AddItem("Staff");
                    transitionLoadObject.GetComponent<InventoryController>().AddItem("Gun");
                }
            }
        }
    }
}
