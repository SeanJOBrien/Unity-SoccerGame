using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Ball : MonoBehaviour {
	private ConstantForce WindForce;
	public float Shoot;
	private int levelNumber;
	public AudioClip hit;
	// Use this for initialization
	void Start () {
		WindForce = gameObject.GetComponent<ConstantForce>();
		WindForce.enabled = false;
		levelNumber = int.Parse(Regex.Match(Application.loadedLevelName, @"\d+").Value);
	}
	
	// Update is called once per frame
	void Update () {
		if(levelNumber < 3)
		{
			WindForce.enabled = false;
		}
	}
	void OnTriggerEnter(Collider other) {
		if(other.name == "GoalCollider" && gameObject.tag == "Ball" &&levelNumber > 0)
		{
			GameObject.Find ("Launcher").GetComponent<Shooting>().scored();
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.tag == "Ground")
		{
			WindForce.enabled = false;
		}else if(collision.collider.tag == "Crate"){
			if(gameObject.tag == "Ball")
			{
				GameObject.Find ("Launcher").GetComponent<Shooting>().scored();
			}
		}else{
			audio.PlayOneShot(hit);
		}

	}
	void OnCollisionExit(Collision collision)
	{
		if(collision.collider.tag == "Ground")
		{
			WindForce.enabled = true;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.name == "OnField" && gameObject.tag == "Ball")
		{
			GameObject.Find ("Launcher").GetComponent<Shooting>().miss();
		}
	}
}
