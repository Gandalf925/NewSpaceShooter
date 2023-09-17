using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// UIManagerクラスの定義
public class UIManager : MonoBehaviour
{
    // ライフを表すオブジェクトの配列
    public GameObject[] lifeObjects;

    // ポーズボタンのアイコン
    public Image pauseButtonIcon;

    // ポーズ時に表示する画像
    public Sprite pauseImage;

    // 再生時に表示する画像
    public Sprite playbackImage;

    // フルスクリーンボタン
    public Button fullscreenButton;

    // フルスクリーンボタンのアイコン
    public Image fullscreenButtonIcon;

    // フルスクリーン時に表示する画像
    public Sprite fullscreenIcon;

    // ウィンドウ表示時に表示する画像
    public Sprite windowIcon;

    // ブラックアウトパネル
    public Image blackoutPanel;

    public void FadeIn()
    {
        blackoutPanel.DOFade(0f, 2f);
    }

    public void FadeOut()
    {
        blackoutPanel.DOFade(1f, 2f);
    }
}
