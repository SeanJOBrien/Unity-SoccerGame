using UnityEngine;
using System.Collections;

public class Keeper : MonoBehaviour {
	public Animator anim;
	public AnimatorStateInfo currentBaseState;
	public int StrafeLeft = Animator.StringToHash("Base Layer.StrafeLeft");
	public int StrafeRight = Animator.StringToHash("Base Layer.StrafeRight");

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		if (currentBaseState.nameHash == StrafeLeft){
			//1
			transform.Translate(Vector3.left*10 * Time.deltaTime);
			if(gameObject.transform.position.z > 208.0f)
			{
				anim.SetInteger("Direction", 0);
			}
		}else if (currentBaseState.nameHash == StrafeRight){
			//0
			transform.Translate(Vector3.right*10 * Time.deltaTime);
			if(gameObject.transform.position.z < 175.0f)
			{
				anim.SetInteger("Direction", 1);
			}
		}
	}	
}
