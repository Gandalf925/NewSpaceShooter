using UnityEngine;

public class OpeningTextScroll : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public float scrollDistance = 100f;

    private RectTransform rectTransform;
    private Camera mainCamera;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(rectTransform.position);
        Vector3 position = mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, scrollDistance));

        position.z += scrollSpeed * Time.deltaTime;

        if (position.z > scrollDistance)
        {
            // テキストが画面外に出たらリセット
            position.z = -scrollDistance;
        }

        rectTransform.position = mainCamera.WorldToScreenPoint(position);
    }
}