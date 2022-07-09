using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DBControl : MonoBehaviour {

    public string Pokemon { get; set; }
    public CaptureStatus CaptureStatus { get; set; }

    public GameObject DataScreen;
    public GameObject SelectScreen;
    public PokedexLoad Controller;

    public Text NumberDisplay;
    public Text NameDisplay;
    public Image PkmnDisplay;
    public Image TypeDisplay;

    public void Load(TextAsset DB) {
        int i = 0;
        foreach(string line in DB.text.Split('\n').Skip(1)) {
            i++;
            string[] DBEntry = line.Split('_');
            if(DBEntry[1] == Pokemon) {
                NumberDisplay.text = i.ToString("000");
                NameDisplay.text = Pokemon;
                TypeDisplay.color = TypeColors.GetColorByType(DBEntry[2]);
                PkmnDisplay.sprite = Resources.Load<Sprite>($"UI Elements\\Dex\\Pokemon\\{Pokemon}");
                PkmnDisplay.color = CaptureStatus == CaptureStatus.Caught ? Color.white : Color.black;
            }
        }
    }

    public void OnHoverEvent() {
        Controller.CurrentDex = int.Parse(NumberDisplay.text);
        Controller.LoadPkmnData();
    }

    public void Click() {
        Controller.CurrentDex = int.Parse(NumberDisplay.text);
        Controller.LoadPkmnData();
        DataScreen.SetActive(true);
        SelectScreen.SetActive(false);
    }

}
