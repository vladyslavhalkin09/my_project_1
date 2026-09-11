using UnityEngine;

public class LootBeam : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private int textureHeight = 128;

    private Material _material;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _material.mainTexture = GenerateBeamTexture();
    }

    public void ApplyRarity(ItemData.Rarity rarity)
    {
        if (_material == null) _material = GetComponent<MeshRenderer>().material;
        _material.color = RarityUtils.GetColor(rarity);
    }

    private Texture2D GenerateBeamTexture()
    {
        Texture2D tex = new Texture2D(4, textureHeight, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Repeat;

        for (int y = 0; y < textureHeight; y++)
        {
            float t = (float)y / (textureHeight - 1);
            float alpha = Mathf.Pow(1f - t, 2f);
            Color c = Color.white;
            c.a = alpha;
            for (int x = 0; x < 4; x++) tex.SetPixel(x, y, c);
        }

        tex.Apply();
        return tex;
    }

    private void Update()
    {
        _material.mainTextureOffset += new Vector2(0f, scrollSpeed * Time.deltaTime);
    }
}