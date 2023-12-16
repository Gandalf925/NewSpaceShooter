using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerImageManager : MonoBehaviour
{
    public static PlayerImageManager Instance { get; private set; }

    [SerializeField] CharactorSelectManager charactorSelectManager;
    [SerializeField] GetCharacterSprite getCharacterSprite;

    public List<Sprite> images = new List<Sprite>();
    public Sprite defaultSprite;
    public Sprite selectedCharacter;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator DownloadImages(List<string> ids)
    {
        List<Coroutine> downloadCoroutines = new List<Coroutine>();

        foreach (string id in ids)
        {
            string imageUrl = $"https://cdn.ordinalswallet.com/inscription/content/{id}";
            Coroutine coroutine = StartCoroutine(DownloadImage(imageUrl));
            downloadCoroutines.Add(coroutine);
        }

        foreach (Coroutine coroutine in downloadCoroutines)
        {
            yield return coroutine; // 各ダウンロードが完了するのを待つ
        }

        // すべてのダウンロードが完了した後の処理
        charactorSelectManager.GetCharacterSpriteList();
        charactorSelectManager.enterAddressPanel.SetActive(false);
        getCharacterSprite.waitingVFX.SetActive(false);
    }

    private IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            images.Add(sprite);
        }
        else
        {
            Debug.LogError("Error downloading image: " + request.error);
        }

        charactorSelectManager.GetCharacterSpriteList();

        yield return new WaitForSeconds(2f);

        charactorSelectManager.enterAddressPanel.SetActive(false);
        getCharacterSprite.waitingVFX.SetActive(false);
    }
}