using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTexture;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture[0], new Vector2(16, 16), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorTexture[currentFrame], new Vector2(16, 16), CursorMode.Auto);
        }
    }
}
