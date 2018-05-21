using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class UniExtender : MonoBehaviour
{

    static UniExtender _instance;

    public static UniExtender Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject ("UniExtender", typeof(UniExtender)).GetComponent<UniExtender> ();
                _instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
            return _instance;
        }
    }

    public static IEnumerator Step (float T, System.Action<float> Step)
    {
        var P = 0f;
        while (P <= 1f) {
            P = Mathf.Clamp01 (P + (Time.deltaTime / T));
            Step (Mathf.SmoothStep (0, 1, P));
            yield return null;
        }
    }
 
 
 
}






