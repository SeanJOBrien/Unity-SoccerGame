using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Ball").transform.position);
	}
}
