using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("s"))
		{
			Application.LoadLevel("Level_0");
		}
		gameObject.transform.RotateAround(GameObject.Find("Centre").transform.position, new Vector3(0.0f,1.0f,0.0f), 0.5f);
		gameObject.transform.LookAt(GameObject.Find("Centre").transform.position);
	}
}
