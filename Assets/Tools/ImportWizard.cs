/*using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
class MyTexturePostprocessor : AssetPostprocessor {

    int[] Temp;
    string Name;

    void OnPreprocessTexture() {

        if (assetPath.Contains("spriteSheet.png")) {
            int file = assetPath.LastIndexOf('/') + 1;
            string tempPath = (assetPath.Substring(file, assetPath.Length - file).Split('.')[0]);
            tempPath = tempPath.Substring(0, tempPath.Length - 12);
            Name = tempPath;

            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.mipmapEnabled = false;
            textureImporter.filterMode = FilterMode.Point;
        }
    }
    public void OnPostprocessTexture(Texture2D texture) {
        Debug.Log("Texture2D: (" + texture.width + " x " + texture.height + ")");

        Temp = PkmnGifData.GifData[Name];


        Debug.Log("Cols " + texture.width % 11);
        Debug.Log("Rows " + texture.height % 11);

        //string[] Data = GetData(texture.name);
        int colCount = Temp[1];//10;//int.Parse(Data[1]);
        int rowCount = Temp[0];//7;//int.Parse(Data[0]);
        int SpriteWidth = (int)Mathf.Ceil(texture.width / colCount);
        int SpriteHeight = (int)Mathf.Ceil(texture.height / rowCount);
        Debug.Log(SpriteWidth + " " + SpriteHeight);

        List<SpriteMetaData> metas = new List<SpriteMetaData>();

        int sprites = Temp[2];//63;//int.Parse(Data[2]);
        for (int r = rowCount - 1; r >= 0; r--) {
            for (int c = 0; c < colCount; c++) {
                if (sprites > 0) {
                    SpriteMetaData meta = new SpriteMetaData();
                    meta.rect = new Rect(c * SpriteWidth, r * SpriteHeight, SpriteWidth, SpriteHeight);
                    meta.name = (rowCount - r -1)+"-"+ (c);
                    metas.Add(meta);
                }
                sprites--;
            }
        }

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.spritesheet = metas.ToArray();
        AssetDatabase.Refresh();
    }
    public void OnPostprocessSprites(Texture2D texture, Sprite[] sprites) {
        Debug.Log($"Imported {texture.name}");
    }
}
#endif*/