using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ImageLoader : MonoBehaviour
{
    public Image playerImage;  // PlayerのImageコンポーネント

    void Start()
    {
        LoadImageFromManager();
        playerImage = GetComponent<Image>();
    }

    void LoadImageFromManager()
    {
        if (PlayerImageManager.Instance.selectedCharacter != null)
        {
            playerImage.sprite = PlayerImageManager.Instance.selectedCharacter;
        }
    }
}