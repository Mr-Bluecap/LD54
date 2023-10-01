using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance;

    [SerializeField]
    Image clientImage;

    [SerializeField]
    TextMeshProUGUI clientNameText;
    
    [SerializeField]
    List<Sprite> characterSprites;

    [SerializeField]
    List<string> characterNames;

    Sprite currentSprite;
    string currentName;

    void Awake()
    {
        Instance = this;
    }

    public void ShowClient()
    {
        clientImage.enabled = true;
    }

    public void HideClient()
    {
        clientImage.enabled = false;
        clientNameText.text = "";
    }
    
    public void CreateNewClient()
    {
        ShowClient();
        
        SetupSprite();
        SetupName();
    }

    void SetupSprite()
    {
        var possibleSprite = GetRandomSprite();

        while (possibleSprite == currentSprite)
        {
            possibleSprite = GetRandomSprite();
        }

        currentSprite = possibleSprite;
        clientImage.sprite = currentSprite;
    }

    void SetupName()
    {
        var possibleName = GetRandomName();

        while (possibleName.Equals(currentName))
        {
            possibleName = GetRandomName();
        }
        
        currentName = possibleName;
        clientNameText.text = currentName;
    }

    Sprite GetRandomSprite()
    {
        var randomSpriteIndex = Random.Range(0, characterSprites.Count);
        return characterSprites[randomSpriteIndex];
    }
    
    string GetRandomName()
    {
        var randomNameIndex = Random.Range(0, characterNames.Count);
        return characterNames[randomNameIndex];
    }
}
