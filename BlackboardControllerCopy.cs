using System.Collections;
using System.Linq;
using UnityEngine;

public class BlackboardControllerCopy : MonoBehaviour {

    private int textureSize = 2000;
    private int penSize = 5;
    private Texture2D texture;
    private Color[] color;

    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    public Shader shader;


    // Use this for initialization
    void Start () {
        Renderer renderer = GetComponent<Renderer>();
        Material mater = new Material(Shader.Find("Custom/AlphaIllu"));
        this.texture = new Texture2D(textureSize, textureSize);
        FillTextureWithTransparency(this.texture);
        mater.mainTexture = texture;
        renderer.material = mater;
    }
	
	// Update is called once per frame
	void Update () {

        int x = (int)(posX * textureSize - (penSize / 2));
        int y = (int)(posY * textureSize - (penSize / 2));
        
        if(touchingLast && x > penSize && y> penSize && x < textureSize - penSize && y < textureSize - penSize)
        {
            texture.SetPixels(x, y, penSize, penSize, color);
            
            for (float t = 0.01f; t < 1.00f; t += 0.1f)
            {
                int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                if (lerpX > penSize && lerpY > penSize && lerpX < textureSize - penSize && lerpY < textureSize - penSize)
                    texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }

            texture.Apply();
        }

        this.lastX = (float)x;
        this.lastY = (float)y;

        this.touchingLast = this.touching;
    }

    public void ToggleTouch(bool touching)
    {
        this.touching = touching;
    }

    public void SetTouchPosition(float x, float y)
    {
        this.posX = x;
        this.posY = y;
    }

    public void SetColor(Color color)
    {
        this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color>();
    }

    public static void FillTextureWithTransparency(Texture2D texture)
    {
        
        Color[] colors = new Color[texture.width * texture.height];
        for (int i = 0; i < texture.width * texture.height; i++) {
            colors[i] = Color.clear;
        }
        
        texture.SetPixels(colors);
        texture.Apply();
    }

}
