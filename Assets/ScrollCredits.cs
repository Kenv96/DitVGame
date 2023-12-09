using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    public float scrollSpeed = 20f;

    RectTransform rectTransform;

    bool scrolling;
    public void StartScroll()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -Screen.height / 2f);
        scrolling = true;
        Destroy(gameObject, 40f);
    }

    void Update()
    {
        if (scrolling)
        {
            rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            if (rectTransform.anchoredPosition.y > Screen.height / 2f)
            {
               // gameObject.SetActive(false);
            }
        }
    }
}
