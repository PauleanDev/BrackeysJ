using UnityEngine;

public class BackgroundMovementLoop : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private MeshRenderer background;

    private void Awake()
    {
        background = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        background.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0f);
    }

}
