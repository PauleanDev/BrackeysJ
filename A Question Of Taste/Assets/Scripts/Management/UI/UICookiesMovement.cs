using Unity.VisualScripting;
using UnityEngine;

public class UICookiesMovement : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollingTime;
    private float currentScrollingTime;
    [SerializeField] private Vector2 directions;

    // Components
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        currentScrollingTime = scrollingTime;
    }

    private void Update()
    {
        rectTransform.anchoredPosition += directions.normalized * scrollSpeed;
        currentScrollingTime -= Time.deltaTime;

        if (currentScrollingTime <= 0)
        {
            directions = -directions;
            currentScrollingTime = scrollingTime;
        }
    }
}
