using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {
    //public Object SceneToLoad = null;
    public string Tag = "Player";
    public OVROverlay overlay;
    public OVROverlay text;
    public OVRCameraRig vrRig;
    //public int SceneIndexToLoad = -1;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //first scene is the robot lab - next scenes/levels are in sync with index
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        if (Tag.Trim().Length == 0 || other.transform.root.tag == Tag)
            LoadSceneAsync(currentLevel);
    }

    void LoadSceneAsync(int sceneIndex)
    {
        //DebugUIBuilder.instance.Hide();
        StartCoroutine(ShowOverlayAndLoad(sceneIndex));
    }

    IEnumerator ShowOverlayAndLoad(int sceneIndex)
    {
        if (text)
        {
            text.transform.position = vrRig.centerEyeAnchor.position + Vector3.ProjectOnPlane(vrRig.centerEyeAnchor.forward, Vector3.up).normalized * 3f;
            text.enabled = true;
        }

        if (overlay)
            overlay.enabled = true;

        // Waiting to prevent "pop" to new scene
        yield return new WaitForSeconds(1f);
        // Load Scene and wait til complete
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return null;
    }
}
