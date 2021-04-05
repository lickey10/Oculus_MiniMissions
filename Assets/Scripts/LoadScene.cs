using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadScene : MonoBehaviour {
    //public Object SceneToLoad = null;
    public string Tag = "Player";
    public OVROverlay overlay;
    public OVROverlay text;
    public OVRCameraRig vrRig;
    public int SceneIndexToLoad = -1;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (Tag.Trim().Length == 0 || other.transform.root.tag == Tag)
        {
            other.transform.root.GetComponentsInChildren<OVRScreenFade>()?.ToList().ForEach(x => x.FadeOut());

            if (SceneIndexToLoad > -1)
                LoadSceneAsync(SceneIndexToLoad);
            else
                LoadSceneAsync(PlayerPrefs.GetInt("CurrentLevel", 1));
        }
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
