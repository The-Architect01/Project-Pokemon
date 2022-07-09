/*using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;

// This is only useful for spritesheets that need to be automatically sliced (Sprite Editor > Slice > Automatic)
public class AutoSpriteSlicer {
	//[MenuItem("Tools/Slice Spritesheets %&s")]
	public static void Slice() {
		var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);

		foreach (var texture in textures) {
			ProcessTexture(texture);
		}
	}

	static void ProcessTexture(Texture2D texture) {
		string path = AssetDatabase.GetAssetPath(texture);
		var importer = AssetImporter.GetAtPath(path) as TextureImporter;

		//importer.isReadable = true;
		importer.textureType = TextureImporterType.Sprite;
		importer.spriteImportMode = SpriteImportMode.Multiple;
		importer.mipmapEnabled = false;
		importer.filterMode = FilterMode.Point;
		importer.spritePivot = Vector2.down;
		importer.textureCompression = TextureImporterCompression.Uncompressed;

		var textureSettings = new TextureImporterSettings(); // need this stupid class because spriteExtrude and spriteMeshType aren't exposed on TextureImporter
		importer.ReadTextureSettings(textureSettings);
		textureSettings.spriteMeshType = SpriteMeshType.Tight;
		textureSettings.spriteExtrude = 0;

		importer.SetTextureSettings(textureSettings);

		int minimumSpriteSize = 16;
		int extrudeSize = 0;

		Rect[] rects = InternalSpriteUtility.GenerateAutomaticSpriteRectangles(texture, minimumSpriteSize, extrudeSize);
		var A_Sprite = rects.OrderBy(r => r.width * r.height).First().center;
		int colCount = 10;//rects.Where(r => r.Contains(new Vector2(r.center.x, A_Sprite.y))).Count();
		int rowCount = rects.Where(r => r.Contains(new Vector2(A_Sprite.x, r.center.y))).Count();
		Vector2Int spriteSize = new Vector2Int(texture.width / colCount, texture.height / rowCount);

		List<SpriteMetaData> metas = new List<SpriteMetaData>();

		for (int r = 0; r < rowCount; ++r) {
			for (int c = 0; c < colCount; ++c) {
				SpriteMetaData meta = new SpriteMetaData();
				meta.rect = new Rect(c * spriteSize.x, r * spriteSize.y, spriteSize.x, spriteSize.y);
				meta.name = string.Format("#{3} {0} ({1},{2})", Path.GetFileNameWithoutExtension(importer.assetPath), c, r, r * colCount + c);
				metas.Add(meta);
			}
		}

		TextureImporter textureImporter = (TextureImporter)importer;
		textureImporter.spritesheet = metas.ToArray();
		AssetDatabase.Refresh();
	}

	static List<Rect> SortRects(List<Rect> rects, float textureWidth) {
		List<Rect> list = new List<Rect>();
		while (rects.Count > 0) {
			Rect rect = rects[rects.Count - 1];
			Rect sweepRect = new Rect(0f, rect.yMin, textureWidth, rect.height);
			List<Rect> list2 = RectSweep(rects, sweepRect);
			if (list2.Count <= 0) {
				list.AddRange(rects);
				break;
			}
			list.AddRange(list2);
		}
		return list;
	}

	static List<Rect> RectSweep(List<Rect> rects, Rect sweepRect) {
		List<Rect> result;
		if (rects == null || rects.Count == 0) {
			result = new List<Rect>();
		} else {
			List<Rect> list = new List<Rect>();
			foreach (Rect current in rects) {
				if (current.Overlaps(sweepRect)) {
					list.Add(current);
				}
			}
			foreach (Rect current2 in list) {
				rects.Remove(current2);
			}
			list.Sort((a, b) => a.x.CompareTo(b.x));
			result = list;
		}
		return result;
	}
}*/