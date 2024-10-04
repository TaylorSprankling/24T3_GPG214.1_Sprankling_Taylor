using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureLoader : MonoBehaviour
{
    private string folderPath = Application.streamingAssetsPath;
    private string fileName = "sterwarz.png";
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(folderPath, fileName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadTexture();
        }
    }

    private void LoadTexture()
    {
        if (File.Exists(filePath))
        {
            // Read the data of the file
            byte[] data = File.ReadAllBytes(filePath);
            // Create temporary texture
            Texture2D texture = new Texture2D(2, 2);
            // Converts data to texture
            texture.LoadImage(data);
            // Create a sprite with texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Change sprite with new sprite
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
