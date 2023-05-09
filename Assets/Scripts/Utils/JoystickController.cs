using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image outerFrameImage;  // 外枠のイメージ
    private Image joystickImage;  // ジョイスティックのイメージ
    private RectTransform outerFrameTransform;  // 外枠のRectTransform
    private RectTransform joystickTransform;  // ジョイスティックのRectTransform

    public float maxDistanceFromCenter = 50.0f;  // 最大距離
    private Vector2 joystickCenterPos;  // ジョイスティックの中心位置

    private Vector2 inputVector = Vector2.zero;  // ジョイスティックの移動量

    private void Start()
    {
        // ジョイスティックのイメージを取得する
        joystickImage = transform.GetChild(1).GetComponent<Image>();
        // 外枠のイメージを取得する
        outerFrameImage = GetComponent<Image>();

        // 外枠とジョイスティックのRectTransformを取得する
        outerFrameTransform = GetComponent<RectTransform>();
        joystickTransform = joystickImage.GetComponent<RectTransform>();

        // ジョイスティックのRectTransformを修正する
        joystickTransform.anchoredPosition = Vector2.zero; // UIの中央に配置
        joystickImage.preserveAspect = true; // アスペクト比を保持する
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerFrameTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            Vector2 relativePos = pos - joystickCenterPos;
            relativePos = Vector2.ClampMagnitude(relativePos, maxDistanceFromCenter);
            relativePos /= maxDistanceFromCenter;

            // ジョイスティックを移動する
            Vector2 newPosition = joystickCenterPos + relativePos * maxDistanceFromCenter;

            joystickTransform.anchoredPosition = newPosition;
            inputVector = relativePos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pressPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerFrameTransform, eventData.position, eventData.pressEventCamera, out pressPos))
        {
            joystickCenterPos = pressPos;
            joystickTransform.anchoredPosition = pressPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickTransform.anchoredPosition = joystickCenterPos;
    }

    private void LateUpdate()
    {
        // JoystickImageを親RectTransform内に収まるようにする
        Vector2 joystickImagePos = joystickTransform.anchoredPosition;
        Vector2 outerFrameSize = outerFrameTransform.rect.size;
        float halfJoystickImageWidth = joystickTransform.rect.width / 2f;
        float halfJoystickImageHeight = joystickTransform.rect.height / 2f;

        // x座標の調整
        if (joystickImagePos.x + halfJoystickImageWidth > outerFrameSize.x / 2f)
        {
            joystickImagePos.x = outerFrameSize.x / 2f - halfJoystickImageWidth;
        }
        else if (joystickImagePos.x - halfJoystickImageWidth < -outerFrameSize.x / 2f)
        {
            joystickImagePos.x = -outerFrameSize.x / 2f + halfJoystickImageWidth;
        }
        // y座標の調整
        if (joystickImagePos.y + halfJoystickImageHeight > outerFrameSize.y / 2f)
        {
            joystickImagePos.y = outerFrameSize.y / 2f - halfJoystickImageHeight;
        }
        else if (joystickImagePos.y - halfJoystickImageHeight < -outerFrameSize.y / 2f)
        {
            joystickImagePos.y = -outerFrameSize.y / 2f + halfJoystickImageHeight;
        }

        // 修正した位置を反映する
        joystickTransform.anchoredPosition = joystickImagePos;
    }

    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    public float GetVerticalValue()
    {
        return inputVector.y;
    }
}
