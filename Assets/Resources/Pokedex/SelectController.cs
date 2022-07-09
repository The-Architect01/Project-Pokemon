using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectController : MonoBehaviour {

    #region Elements
    public TextAsset DB;
    public PokedexLoad DBViewer;
    public GameObject DS;
    public GameObject SS;
    public GameObject DBPrefab;
    public GameObject DBDisplay;
    public GameObject Error;
    public static Dictionary<string, CaptureStatus> DBSource;
    public List<DBControl> DBCollection;
    #endregion

    // Start is called before the first frame update
    void Start() {
        DBSource = /*new Dictionary<string, CaptureStatus>();*/ Engine.PkmnSeen;
        //try {
        //    foreach (string Pkmn in DB.text.Split('\n').Skip(1)) { DBSource.Add(Pkmn.Split('_')[1], (CaptureStatus)Random.Range(0,3)); }
        //} catch { }
        if (Engine.DexSeen == 0) {
            Error.SetActive(true);
            return;
        }
        foreach(string pkmn in DBSource.Keys.Reverse()) {
            if (DBSource[pkmn] != CaptureStatus.NotSeen) {
                DBControl Entry = Instantiate(DBPrefab, DBDisplay.transform).GetComponent<DBControl>();
                Entry.Pokemon = pkmn;
                Entry.CaptureStatus = DBSource[pkmn];
                Entry.Controller = DBViewer;
                Entry.DataScreen = DS;
                Entry.SelectScreen = SS;
                Entry.Load(DB);
                Entry.transform.SetSiblingIndex(0);
                DBCollection.Add(Entry);
            }
        }
        DBViewer.CurrentDex = int.Parse(DBCollection.Last().NumberDisplay.text);
        DBViewer.LoadPkmnData();
    }

    public void Search(Text Query) {
        bool Found = false;
        try {
            int Number = int.Parse(Query.text);
            foreach (DBControl Select in DBCollection) {
                Select.transform.gameObject.SetActive(Select.NumberDisplay.text.Contains(Query.text));
                if (Select.isActiveAndEnabled) { Found = true; }
            }
        } catch {
            foreach (DBControl Select in DBCollection) {
                Select.transform.gameObject.SetActive(Select.Pokemon.IndexOf(Query.text, System.StringComparison.OrdinalIgnoreCase) != -1);
                if (Select.isActiveAndEnabled) { Found = true; }
            }
        }
        Error.SetActive(!Found);
    }

    public void BackArrow() {
        SS.SetActive(true);
        DS.SetActive(false);
    }
}