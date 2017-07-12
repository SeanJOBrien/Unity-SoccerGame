using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Shooting : MonoBehaviour {
	public GameObject ballPrefab;
	public float power;
	public GameObject ball;
	public GameObject arrowGuide;
	public GameObject arrow;
	public Text score;
	public Text textDisplay;
	public Slider PowerBar;
	private int waypointCounter;
	private int goalScore;
	private bool powerIncreasing;
	private int levelNumber;
	private float messageTimer;
	private float shotTimer;
	private string messageForLevel;
	private GameObject MainCamera;
	private GameObject TrackCamera;

	// Use this for initialization
	void Start () {
		goalScore = 0;
		waypointCounter = 1;
		power = 0.0f;
		PowerBar.maxValue = 100;
		PowerBar.minValue = 1;
		powerIncreasing = false;
		textDisplay.gameObject.SetActive(false);
		ball = GameObject.FindGameObjectWithTag("Ball");
		arrowGuide = GameObject.Find("ArrowGuide");
		arrow = GameObject.Find("Arrow");
		levelNumber = int.Parse(Regex.Match(Application.loadedLevelName, @"\d+").Value);
		textDisplay.gameObject.SetActive(true);
		TrackCamera = GameObject.FindGameObjectWithTag("camera");
		MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		SelectCamera(1);
		switch(levelNumber)
		{
		case 1:
			messageForLevel = "Score 4 or more goals to continue! \n" +
				"Put the ball in the goal. \n" +
				"Level One has no obstacles on this level.";
			break;
		case 2:
			messageForLevel = "Score 4 or more goals to continue! \n" +
				"Put the ball in the goal. \n" +
				"Level Two has one goal keeper.";
			break;
		case 3:
			messageForLevel = "Score 4 or more goals to continue! \n" +
				"Put the ball in the goal. \n" +
				"Level Three has one goal keeper and wind force.";
			break;
		case 4:
			messageForLevel = "Score 4 or more goals to continue! \n" +
				"Put the ball in the goal. \n" +
				"Level Four has one goal keeper, wind force and a number of defencive lines.";
			break;
		case 0:
			messageForLevel = "Knock down the crates.";
			break;
		}
		textDisplay.text = messageForLevel;
		messageTimer = 5.0f;
		shotTimer = 60.0f;
	}
	
	// Update is called once per frame
	void Update () {
		KeyInput();
		if(powerIncreasing)
		{
			PowerBar.value += 1;
			if(PowerBar.value == PowerBar.maxValue)
			{
				PowerBar.value = PowerBar.minValue;
			}
		}
		if (messageTimer < 0.0f)
		{
			textDisplay.gameObject.SetActive(false);
		}
		if (shotTimer < 0.0f)
		{
			miss();
		}
		if (shotTimer < 60.0f)
		{
			shotTimer -= Time.deltaTime;
		}
		messageTimer -= Time.deltaTime;
	}
	public void scored()
	{
		SelectCamera(1);
		goalScore++;
		score.text = "Score: "+ goalScore;
		textDisplay.gameObject.SetActive(true);
		score.text = "Score: " + goalScore;
		if(levelNumber == 0)
		{
			textDisplay.text = "Hit!";
		}else{
			textDisplay.text = "GOAL!";
		}
		messageTimer = 5.0f;
		waypointCounter++;
		if (waypointCounter < 6)
		{
			locationSet();
		} else {
			if (goalScore > 3 || levelNumber == 0)
			{
				Application.LoadLevel("Level_"+(levelNumber+1));
			} else {
				Application.LoadLevel("Level_"+(levelNumber));
			}
		}
	}
	public void miss()
	{
		SelectCamera(1);
		textDisplay.gameObject.SetActive(true);
		score.text = "Score: " + goalScore;
		textDisplay.text = "Miss...";
		messageTimer = 5.0f;
		waypointCounter++;
		if (waypointCounter < 6)
		{
			locationSet();
		} else {
			if (goalScore > 3 || levelNumber == 0)
			{
				Application.LoadLevel("Level_"+(levelNumber+1));
			} else {
				Application.LoadLevel("Level_"+(levelNumber));
			}
		}
	}
	void locationSet()
	{
		shotTimer = 60.0f;
		GameObject wp = GameObject.Find("WP_" + waypointCounter);
		Instantiate(ballPrefab,wp.transform.position , Quaternion.identity);
		GameObject.Find("Player").transform.position = new Vector3(wp.transform.position.x+10.0f,wp.transform.position.y+5.5f,wp.transform.position.z);
		ball.tag = "oldBall";
		arrow.tag = "ArrowOld";
		arrowGuide.tag = "ArrowGuideOld";
		ball = GameObject.FindGameObjectWithTag("Ball");
		arrow = GameObject.FindGameObjectWithTag("ArrowCurrent");
		arrowGuide = GameObject.FindGameObjectWithTag("ArrowGuideCurrent");
	}


	void KeyInput()
	{
		if (Vector3.Distance(gameObject.transform.position, ball.transform.position) < 6.0)
		{
			arrow.renderer.enabled = true;
			if (Input.GetKeyDown("return"))
			{
				powerIncreasing = true;
			}
			if (Input.GetKeyUp("return"))
			{
				SelectCamera(0);
				powerIncreasing = false;
				Vector3 ballP = ball.transform.position;
				Vector3 ArrowGuideP = arrowGuide.transform.position;
				Vector3 shootingDirection = new Vector3(ArrowGuideP.x-ballP.x,ArrowGuideP.y-ballP.y,ArrowGuideP.z-ballP.z);
				shootingDirection = shootingDirection.normalized*PowerBar.value;
				shootingDirection.y+=5;
				ball.rigidbody.AddForce(shootingDirection, ForceMode.Impulse);
				Vector3 frictionForce = ball.rigidbody.velocity*-PowerBar.value;
				ball.rigidbody.AddForce(frictionForce);
				PowerBar.value = PowerBar.minValue;
				shotTimer = 20.0f;
			}
			if (Input.GetKeyDown("i"))
			{
				arrowGuide.transform.RotateAround(ball.transform.position, new Vector3(0.0f,0.0f,1.0f), -5);
			}
			if (Input.GetKeyDown("k"))
			{
				arrowGuide.transform.RotateAround(ball.transform.position, new Vector3(0.0f,0.0f,1.0f), 5);
			}
			if (Input.GetKeyDown("j"))
			{
				arrowGuide.transform.RotateAround(ball.transform.position, new Vector3(0.0f,1.0f,0.0f), -5);
			}
			if (Input.GetKeyDown("l"))
			{
				arrowGuide.transform.RotateAround(ball.transform.position, new Vector3(0.0f,1.0f,0.0f), 5);
			}
		} else {
			arrow.renderer.enabled = false;
		}
		if(Input.GetKeyDown("m"))
		{
			audio.mute = !audio.mute;
		}
		if(Input.GetKeyDown("b"))
		{
			textDisplay.text = messageForLevel;
			textDisplay.gameObject.SetActive(true);
			messageTimer = 5.0f;
		}
		if(Input.GetKeyDown("n"))
		{
			textDisplay.text = "Controls:\n" +
				"WASD - Movement. \n" +
				"IJKL - Ball Directional Angle \n" +
				"Enter Down - Start Shot. \n" +
				"Enter Release - Select Power. \n" +
				"B - Level Objective \n" +
				"N - Instructions.\n" +
				"M - Mute.";
			textDisplay.gameObject.SetActive(true);
			messageTimer = 5.0f;
		}
	}
	
	void SelectCamera (int index) { 
		if (index == 0){ 
			TrackCamera.camera.active = true; 
			MainCamera.camera.active = false;
			// Deactivate all other cameras 
		}else{ 
			TrackCamera.camera.active = false; 
			MainCamera.camera.active = true;
		}
	}
}
