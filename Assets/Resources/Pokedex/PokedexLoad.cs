using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PokedexLoad : MonoBehaviour {

    public TextAsset DexDataEntries;
    #region UI
    public Button NextPKMN;
    public Button PrevPKMN;
    public Button NextForm;
    public Button PrevForm;

    public GameObject PkmnGO;
    public Image PkmnDisplay;
    public Image Pkmn;

    public AudioSource Cry;
    public Image Type1;
    public Image Type2;

    public Text Name;
    public Text Genus;
    public Text Entry;
    public Text Height;
    public Text Weight;
    public Text CurrentMon;
    public Text CurrentForm;
    public string[] Forms;

    //public GameObject SelectionScreen;
    public GameObject DataScreen;
    public GameObject Form;
    #endregion
    public int CurrentDex = 1;
    public int CurrentFormIndex = 0;
    public CaptureStatus HasSeenPkmn = CaptureStatus.NotSeen;

    void Start() {
        Debug.Log(Monica.AccountName);
        NextPKMN.onClick.AddListener(delegate {
            CurrentDex++;
            if(CurrentDex > 251) { CurrentDex = 1; }
            LoadPkmnData();
        });
        PrevPKMN.onClick.AddListener(delegate {
            CurrentDex--;
            if(CurrentDex == 0) { CurrentDex = 251; }
            LoadPkmnData();
        });
        NextForm.onClick.AddListener(delegate {
            CurrentFormIndex++;
            if(CurrentFormIndex > Forms.Length-1) { CurrentFormIndex = 0; }
            UpdateForms();
        });
        PrevForm.onClick.AddListener(delegate {
            CurrentFormIndex--;
            if (CurrentFormIndex == - 1) { CurrentFormIndex = Forms.Length - 1; }
            UpdateForms();
        });
        LoadPkmnData();
    }

    public void LoadPkmnData() {
        CurrentFormIndex = 0;
        Forms = null;

        TextAsset line = DexDataEntries;//Resources.Load("Pokedex\\PokedexData") as TextAsset;
        string[] Data = line.text.Split('\n')[CurrentDex].Split('_');

        CaptureStatus Status = SelectController.DBSource[Data[1]];//Engine.GetSeen(Data[1]);
        //PkmnDisplay.PkmnName = Data[1];
        PkmnDisplay.sprite = Resources.Load<Sprite>($"UI Elements\\Dex\\Pokemon\\{Data[1]}");
        _4thWall._4thWall.ChangeName(Data[1]);

        if (Status == CaptureStatus.NotSeen) {
            Pkmn.color = new Color(0, 0, 0);
            Type1.gameObject.SetActive(false);
            Type2.gameObject.SetActive(false);
            
            Cry.clip = null;
            CurrentMon.text = "??????";
            
            Name.text = "??????";
            Genus.gameObject.SetActive(false);
            Form.SetActive(false);
            Height.text = "???'???\"";
            Weight.text = "????.?? lbs.";
            Entry.text = "No data found...";
        } else {
            Cry.clip = Resources.Load($"Pokemon Cries\\{Data[1]}") as AudioClip;
            Debug.Log(line.text.Split('\n')[CurrentDex]);
            CurrentMon.text = Data[1];
            Name.text = Data[1];

            Pkmn.color = Status == CaptureStatus.Seen ? new Color(128, 128, 128) : new Color(255, 255, 255);

            Genus.gameObject.SetActive(Status!=CaptureStatus.Seen);
            Genus.text = $"The {Data[4]} Pokémon";
            Height.text = Status == CaptureStatus.Seen ? "???'???\"" : $"{int.Parse(Data[5]) / 12}' {int.Parse(Data[5]) % 12}\"";
            Weight.text = Status == CaptureStatus.Seen ? "????.?? lbs." : $"{Data[6]} lbs.";

            Type1.gameObject.SetActive(true);
            Type1.sprite = Resources.Load<Sprite>($"UI Elements\\Types\\Type{Data[2]}");
            Type2.sprite = Resources.Load<Sprite>($"UI Elements\\Types\\Type{Data[3]}");
            Type2.gameObject.SetActive(Type2.sprite != null);
            Entry.text = Status == CaptureStatus.Seen ? "Not enough information..." : Data[7];

            if(Status == CaptureStatus.Caught) {
                Forms = Data.Skip(8).ToArray().Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                if (Forms != null && Forms.Length != 0) {
                    CurrentForm.text = $"Form: {Forms[CurrentFormIndex]}";
                    /*if (Forms[0] == "Female" && Forms[1] == "Male")
                        PkmnDisplay.PkmnName = Data[1] + "-" + Forms[CurrentFormIndex][0];
                    else
                        PkmnDisplay.PkmnName = Data[1] + "-" + Forms[CurrentFormIndex];*/
                }
            }

            //Form.SetActive(Forms != null && Forms.Length != 0 );
            //if (CurrentDex == 109) {
            //    Cry.clip = Resources.Load($"Pokemon Cries\\{Name.text} - {CurrentFormIndex + 1}") as AudioClip;
            //}
        }
    }

    void UpdateForms() {
        if(Forms.Length == 0) { return; }
        Debug.Log(CurrentFormIndex);
        CurrentForm.text = $"Form: {Forms[CurrentFormIndex]}";
        //if (Forms[0] == "Female" && Forms[1] == "Male")
        //    PkmnDisplay.PkmnName = Name.text + "-" + Forms[CurrentFormIndex][0];
        //else
        //    PkmnDisplay.PkmnName = Name.text + "-" + Forms[CurrentFormIndex];
        //PkmnDisplay.SetGifFromUrl(Application.streamingAssetsPath + $@"\Front\Regular\{Name.text} - {CurrentFormIndex+1}.gif");

        //PokemonDisplay.SetInteger("Form", CurrentFormIndex);
        if (CurrentDex == 110) {
            Cry.clip = Resources.Load($"Pokemon Cries\\{Name.text} - {CurrentFormIndex + 1}") as AudioClip;
        }
    }

    public void OnPointerClick() {
        Cry.Play();
    }
}
static class TypeColors {

    public static readonly Color NORMAL = new Color32(192,192,192,255);
    static readonly Color FIRE = new Color32(250,128,114,255);
    static readonly Color WATER  = new Color32(64,224,208,255);
    static readonly Color GRASS = new Color32(144,238,144,255);
    static readonly Color ELECTRIC = new Color32(240,230,140,255);
    static readonly Color ICE = new Color32(176,224,230,255);
    static readonly Color FIGHTING = new Color32(139, 0, 0,255);
    static readonly Color POISON = new Color32(173, 255, 47,255);
    static readonly Color GROUND = new Color32(205, 133, 63,255);
    static readonly Color FLYING = new Color32(135, 206, 235,255);
    static readonly Color PSYCHIC = new Color32(221, 160, 221,255);
    static readonly Color BUG = new Color32(154, 205, 50,255);
    static readonly Color ROCK = new Color32(244, 164, 96,255);
    static readonly Color GHOST = new Color32(147, 112, 219,255);
    static readonly Color DARK = new Color32(112, 128, 144,255);
    static readonly Color DRAGON = new Color32(205, 92, 92,255);
    static readonly Color STEEL = new Color32(220, 220, 220,255);
    static readonly Color FAIRY = new Color32(255, 182, 193,255);

    public static Color GetColorByType(string Type) {
        return (Color)typeof(TypeColors).GetField(Type.ToUpper(),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).GetValue(typeof(TypeColors));
    }
}