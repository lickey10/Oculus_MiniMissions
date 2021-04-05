using UnityEngine;
using System.Collections;

public class guiTextSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 GetComponent<UnityEngine.UI.Text>().text= 	"\nArrow Keys: Move\nLeft Shift: Sneak\nZ: Use Switch\nX: Attract Attention";
	
	}

}
