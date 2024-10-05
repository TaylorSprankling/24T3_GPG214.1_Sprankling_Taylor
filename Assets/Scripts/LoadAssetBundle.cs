using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath;
    private string folderName = "AssetBundles";
    private string fileName = "tophatbundle";
    private string filePath;
    private AssetBundle topHatBundle;
    private GameObject topHatInstance;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, folderName, fileName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadTopHatBundle();
        }
    }

    private void LoadTopHatBundle()
    {
        if (File.Exists(filePath))
        {
            topHatBundle = AssetBundle.LoadFromFile(filePath);
        }
        else
        {
            Debug.Log($"{filePath} doesn't exist or isn't found");
        }

        if (topHatBundle == null)
        {
            return;
        }
        var topHatPrefab = topHatBundle.LoadAsset<GameObject>("TopHat");

        if (topHatPrefab != null && topHatInstance == null)
        {
            topHatInstance = Instantiate(topHatPrefab, gameObject.transform);
        }
        topHatBundle.Unload(false);
    }
}
