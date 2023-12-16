using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

[Serializable]
public class ApiResponse
{
    public Inscription[] inscriptions;
}

[Serializable]
public class Inscription
{
    public string id;
    public Collection collection;
    // 他のフィールドも必要に応じてここに追加
}

[Serializable]
public class Collection
{
    public string name;
    // 他のフィールドも必要に応じてここに追加
}

public class GetCharacterSprite : MonoBehaviour
{
    CharactorSelectManager charactorSelectManager;
    private string apiUrl = "https://turbo.ordinalswallet.com/wallet/";

    private List<string> spacePepesIds = new List<string>();

    public GameObject errorBar;
    public Transform errorBarStartPos;
    public Transform errorBarStopPosition;
    public GameObject waitingVFX;
    private void Start()
    {
        charactorSelectManager = GetComponent<CharactorSelectManager>();
    }

    public void EnterAddress()
    {
        if (charactorSelectManager.enterAddressInputField.text != "" || charactorSelectManager.enterAddressInputField.text != null)
        {
            GetPlayerAddress();
        }
        return;
    }

    private void GetPlayerAddress()
    {
        waitingVFX.SetActive(true);
        charactorSelectManager.enterAddressPopup.SetActive(false);

        if (charactorSelectManager.enterAddressInputField.text != "" || charactorSelectManager.enterAddressInputField.text != null)
        {
            // ユーザーの入力値をトリミングしてサニタイズ
            string address = charactorSelectManager.enterAddressInputField.text.Trim();
            // 不可視文字を削除
            address = System.Text.RegularExpressions.Regex.Replace(address, @"\u200B", "");

            Debug.Log("Cleaned Address: " + address);

            apiUrl = "https://turbo.ordinalswallet.com/wallet/" + address;

            StartCoroutine(GetPepeIDs());
        }
        return;
    }


    public IEnumerator GetPepeIDs()
    {
        Debug.Log("Requesting URL: " + apiUrl);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                StartCoroutine(ShowErrorPopup());
                charactorSelectManager.characters.Add(PlayerImageManager.Instance.defaultSprite);
                charactorSelectManager.enterAddressPanel.SetActive(false);
                waitingVFX.SetActive(false);

            }
            else
            {
                ExtractData(webRequest.downloadHandler.text);
            }
        }
    }

    void ExtractData(string jsonData)
    {
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonData);

        foreach (var inscription in response.inscriptions)
        {
            if (inscription.collection != null && inscription.collection.name == "Space Pepes")
            {
                spacePepesIds.Add(inscription.id);
            }
        }

        StartDownloadingImages();
    }


    public void StartDownloadingImages()
    {
        StartCoroutine(PlayerImageManager.Instance.DownloadImages(spacePepesIds));
    }

    public IEnumerator ShowErrorPopup()
    {
        errorBar.transform.DOMove(errorBarStopPosition.position, 1f);
        yield return new WaitForSeconds(5f);

        errorBar.transform.DOMove(errorBarStartPos.position, 1f);
    }
}