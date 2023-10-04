using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class SpriteLoader : MonoBehaviour
{
    public SpriteRenderer playerImage;  // PlayerのImageコンポーネント

    // 画像のURL
    private string imageUrl = "https://cdn.ordinalswallet.com/inscription/preview/20938ad3e601f37ef062f3c2452178e1cbdf1b638038ad193af1318cd3048d5ei0";

    void Start()
    {
        StartCoroutine(LoadImageFromUrl());
        playerImage = GetComponent<SpriteRenderer>();
    }

    IEnumerator LoadImageFromUrl()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            playerImage.sprite = sprite;
        }
    }
}