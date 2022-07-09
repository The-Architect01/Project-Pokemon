using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPkmn : MonoBehaviour {

    Text Name;
    Text Number;
    Image Caught;

    public void UpdatePkmn(int number, CaptureStatus captureStatus = CaptureStatus.NotSeen ) {
        
    }

}
public enum CaptureStatus {
    Seen = 1,
    Caught = 2,
    NotSeen = 0,
}
