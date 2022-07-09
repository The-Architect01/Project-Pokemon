using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatePkmn : MonoBehaviour {

    public Image host;
    public bool IsFront = true;
    public bool IsShiny = false;
    public string PkmnName;
    public float Delay = .03f;
    //public Texture2D PKMN;

    public Sprite[] Frames;

    string PKMNTEMP;

    // Start is called before the first frame update
    void Start() {
        Frames = Resources.LoadAll<Sprite>($"Pokemon Sprites\\" +
            $"{(IsShiny ? "Shiny" : "Normal")}\\" +
            $"{(IsFront ? "Front" : "Back")}\\" +
            $"{(IsFront ? "Front" : "Back")}" +
            $"{(IsShiny ? "Shiny" : string.Empty)}-" +
            $"{PkmnName}-spriteSheet");
        PKMNTEMP = PkmnName;
        if (Frames.Length != 0) { StartCoroutine(Animation()); }
    }

    private void Update() {
        if(PKMNTEMP != PkmnName) {
            StopCoroutine(Animation());
            Frames = Resources.LoadAll<Sprite>($"Pokemon Sprites\\" +
            $"{(IsShiny ? "Shiny" : "Normal")}\\" +
            $"{(IsFront ? "Front" : "Back")}\\" +
            $"{(IsFront ? "Front" : "Back")}" +
            $"{(IsShiny ? "Shiny" : string.Empty)}-" +
            $"{PkmnName}-spriteSheet");
            PKMNTEMP = PkmnName;
            if (Frames.Length != 0) { StartCoroutine(Animation()); }
        }
    }

    IEnumerator Animation() {
        while (Frames.Length != 0) {
            foreach (Sprite sp in Frames) {
                if (PKMNTEMP != PkmnName) { break; }
                host.sprite = sp;
                yield return new WaitForSecondsRealtime(Delay);
            }
        }
    }
}
public static class PkmnGifData {

    static Dictionary<string, int[]> _Data = new Dictionary<string, int[]>();
    public static Dictionary<string, int[]> GifData { get { Refresh(); return _Data; } }

    static void Refresh() {
        string Data = Resources.Load<TextAsset>("GifData").text;
        foreach (string Line in Data.Split('\n')) {
            try {
                _Data.Add(Line.Split(':')[0].Split(',')[0], new int[] { int.Parse(Line.Split(',')[1]), int.Parse(Line.Split(',')[2]), int.Parse(Line.Split(',')[3]) });
            } catch { Debug.Log("Error"); }
        }
    }
   
}