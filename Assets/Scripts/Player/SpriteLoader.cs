using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SpriteLoader : MonoBehaviour
{
    public SpriteRenderer playerImage;  // PlayerのImageコンポーネント

    void Start()
    {
        LoadImageFromManager();
        playerImage = GetComponent<SpriteRenderer>();
    }

    void LoadImageFromManager()
    {
        if (PlayerImageManager.Instance.selectedCharacter != null)
        {
            playerImage.sprite = PlayerImageManager.Instance.selectedCharacter;
        }
    }
}