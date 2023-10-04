using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorSelectManager : MonoBehaviour
{
    public List<Sprite> characters;  // キャラクターのスプライトリスト
    public Image characterDisplay;   // キャラクターを表示するImageコンポーネント
    private int currentIndex = 0;    // 現在選択されているキャラクターのインデックス

    void Start()
    {
        // 最初のキャラクターを表示
        if (characters.Count > 0)
        {
            characterDisplay.sprite = characters[currentIndex];
        }
    }

    // 次のキャラクターを表示するメソッド
    public void NextCharacter()
    {
        if (characters.Count > 0)
        {
            currentIndex = (currentIndex + 1) % characters.Count;
            characterDisplay.sprite = characters[currentIndex];
        }
    }

    // 前のキャラクターを表示するメソッド
    public void PreviousCharacter()
    {
        if (characters.Count > 0)
        {
            currentIndex = (currentIndex - 1 + characters.Count) % characters.Count;
            characterDisplay.sprite = characters[currentIndex];
        }
    }
}