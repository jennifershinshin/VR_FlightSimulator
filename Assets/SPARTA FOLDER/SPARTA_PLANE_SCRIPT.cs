 using UnityEngine;
using System.Collections;

public class SPARTA_PLANE_SCRIPT : MonoBehaviour {
		

	//OBJECTS TO GET REGARDING THE SPARTA AIRPLANE
	private GameObject spartacannon1, spartacannon2,  plane_camera_parent;
	private Animator spartacannonanim1, spartacannonanim2, anim;
	private AudioSource plane_audio;
	//CONTROLLER VARIABLES
	private bool  dpad_down_ready,xbutton_ready,is_flying, is_accelerating;
	private int  xbutton_current, dpad_down_select;
	private float forward_speed,stall_speed, forward_constant, dpad_down_current, dpad_down_delaytime, 
				plane_enginesound_pitch, plane_pitch_volume;
	private float gyroX, gyroY, roll_speed, pitch_speed, yaw_speed, gyromultiX, gyromultiY, gyrotiming, exaustfloat, 
	delaytime_forcannonweapondoor, verticalaxis_pos, horizontalaxis_pos, max_speed;
	//CAMERA VARIABLES
	private Vector3 temp;
	private float plane_rot_X, plane_rot_Y, plane_rot_Z, right_stick_horizontal, right_stick_vertical, camera_multiplier;


	// Use this for initialization
	void Start () {
		spartacannon1 = GameObject.Find ("SPARTA CANNON ANIMATED");
		spartacannon2 = GameObject.Find ("SPARTA CANNON ANIMATED (1)");
		plane_camera_parent = GameObject.Find ("sparta plane main camera parent object");
		anim = gameObject.GetComponentInChildren<Animator> ();
		spartacannonanim1 = spartacannon1.GetComponentInParent<Animator> ();
		spartacannonanim2 = spartacannon2.GetComponentInParent<Animator> ();
		plane_audio = gameObject.GetComponent<AudioSource> ();


		roll_speed = 0.0f;
		pitch_speed = 0.0f;
		yaw_speed = 0.0f;
		gyrotiming = 0;
		exaustfloat = 0.0f;
		delaytime_forcannonweapondoor = 0.0f;
		 
		forward_speed = 0.0f;
		forward_constant = 0.0f;
		verticalaxis_pos = 0.0f; horizontalaxis_pos = 0.0f;
		max_speed = -0.5f;
		stall_speed = -0.15f;
		is_flying = false;

		dpad_down_delaytime = 0.0f;
		dpad_down_current = 0.0f;
		dpad_down_ready = true;
		dpad_down_select = 0;

		xbutton_current = 0;
		xbutton_ready = true;

		plane_pitch_volume = 0.200f;
		plane_enginesound_pitch = 1.0f;
		is_accelerating = false;


	}
	
	// Update is called once per frame
	void Update () {
		xboxOneControllerUpdate ();
		plane_camera_class ();


	}
	void xboxOneControllerUpdate(){//THIS WORKS WITH THE XBOX ONE CONTROLLER ON PC
		
//ROLL AND PITCH
		transform.Rotate (roll_speed, yaw_speed, pitch_speed);
		horizontalaxis_pos = Input.GetAxis ("Horizontal");//reference
		verticalaxis_pos = Input.GetAxis ("Vertical");//reference
		gyroY = horizontalaxis_pos;//this controls animation of rolling wing flaps
		gyroX = verticalaxis_pos;  //		||
		roll_speed = horizontalaxis_pos * 2.0f;//this controls the plane rotation ingame
		pitch_speed = verticalaxis_pos;//		||
		gyrotiming += 1 * Time.deltaTime;//<- THIS IS TO CONTROL SURFACES FLICKERING BY SMOOTHING THE MOVEMENTS.
		if (gyrotiming > 0.05f) {
			anim.SetFloat ("SPARTA PLANE PITCH FLOAT", gyroX);
			anim.SetFloat ("SPARTA PLANE ROLL FLOAT", gyroY);

//THIS IS TO CONTROL FLAPS DOWN IF THE AIRCRAFT IS GOING SLOW IE WHEN ITS GOING TO LAND OR TO PROMOTE LIFT
			if (forward_speed > stall_speed) {
					anim.SetBool ("SPARTA PLANE IF FLYING FAST", false);
				} else {
					anim.SetBool ("SPARTA PLANE IF FLYING FAST", true);
				}//ends here
//THIS IS TO PREVENT INPUT FROM GOING ABOVE 1
			if (gyroX > 1) {
				gyroX = 1;
			}
			if (gyroX < -1) {
				gyroX = -1;
			}
			if (gyroY > 1) {
				gyroY = 1;
			}
			if (gyroY < -1) {
				gyroY = -1;
			}
			gyrotiming = 0;
		}

//THIS IS FOR THE WEAPONS DOOR
	
		if ((Input.GetButtonDown ("xbox x button"))&&(xbutton_current == 0)&&(xbutton_ready)) {//this simulate tapping when weapons door are closed
			xbutton_current = 1;
			xbutton_ready = false;
		} 
		if ((Input.GetButtonDown ("xbox x button"))&&(xbutton_current == 1)&&(xbutton_ready)) {//this simulate tapping when weapons door are opened
			xbutton_current = 0;
			xbutton_ready = false;
		}
		if (!Input.GetButton ("xbox x button")) {
			xbutton_ready = true;
		}
		delaytime_forcannonweapondoor -= 0.1f;
		if ((Input.GetButton("xbox x button"))&&(xbutton_current == 1)) {//THIS OPEN WEAPONS DOOR
			anim.SetBool ("SPARTA PLANE WEAPONS DOOR BOOL", true);
			spartacannonanim1.SetBool ("SPARTA CANNON BOOL", true);
			spartacannonanim2.SetBool ("SPARTA CANNON BOOL", true);
			delaytime_forcannonweapondoor = 0.5F;
		}
		if ((Input.GetButton("xbox x button"))&&(xbutton_current == 0)) {//THIS CLOSE WEAPONS DOOR
			
			if (delaytime_forcannonweapondoor < 0.0F) {
				anim.SetBool ("SPARTA PLANE WEAPONS DOOR BOOL", false);
				spartacannonanim1.SetBool ("SPARTA CANNON BOOL", false);
				spartacannonanim2.SetBool ("SPARTA CANNON BOOL", false);
				delaytime_forcannonweapondoor = 0;
			}
		}
	
			
//THIS IS FOR YAW
//THIS IS FOR YAW
		if (Input.GetButton("xbox left bumper")) {
			anim.SetBool ("SPARTA PLANE YAW LEFT BOOL", true);
			yaw_speed = -0.2f;
			} else {
					anim.SetBool ("SPARTA PLANE YAW LEFT BOOL", false);
					
			}
		if (Input.GetButton("xbox right bumper")) {
			anim.SetBool ("SPARTA PLANE YAW RIGHT BOOL", true);
			yaw_speed = 0.2f;
			} else {
					anim.SetBool ("SPARTA PLANE YAW RIGHT BOOL", false);
					
			}
		if ((!Input.GetButton("xbox left bumper"))&&(!Input.GetButton("xbox right bumper"))){
			yaw_speed = 0.0f;
		}
	

//THIS CONTROL EXAUST
		//this controls the exaust by using the "exaustfloat" timing
		if (Input.GetButton ("xbox y button")) {
			if (exaustfloat < 2.0f) {
				exaustfloat += 0.1f;
			}
		} else {
			if (exaustfloat > 0.1f) {
				exaustfloat -= 0.2f;
			}
		}
		if (exaustfloat > 1.3f) {
			anim.SetBool ("SPARTA PLANE EXAUST BOOL", true);
		}
		if (exaustfloat < 1.3f) {
			anim.SetBool ("SPARTA PLANE EXAUST BOOL", false);
		}
//THIS IS FOR CONTROLLING TRUST
//THIS IS TO CONTROL PLANE SPEED
		forward_speed += forward_constant * Time.deltaTime;
		transform.Translate (forward_speed, 0, 0);
		if (forward_speed < -0.08f) {//this will make the plane go is_flying true to go to "is actually flying and not to go to landing mode".
			is_flying = true;
		}
		if (is_flying) {//this will control the plane from going slower than stall speed to full stop in mid air.
			if (forward_speed > -0.08f) {
				forward_speed = -0.08f;
			}
		}

		if (forward_speed < max_speed) {//this prevent/controls the plane from going beyond set max speed
			forward_speed = max_speed;
		}
		if (forward_speed > 0.0f) {//this prevents the plane from going backward :P
			forward_speed = 0.0f;
		}
		if ((Input.GetButton ("xbox a button")) && (forward_speed < 0.08f)) {//backward or brake
			
			forward_constant += 0.0005f;
			is_accelerating = false;
		}
		
		if ((Input.GetButton ("xbox y button")) && (forward_speed > max_speed)) {//forward
			
			forward_constant -= 0.001f;
			is_accelerating = true;
		}
		if ((!Input.GetButton ("xbox a button")) && (!Input.GetButton ("xbox y button"))) {
			forward_constant = 0; is_accelerating = false;
			if (forward_speed < 0.0f) {//this will slow it down simulating air drag
				forward_speed += 0.0001f;//this is the rate at which is slowing down
			}

		}
//THIS IS FOR CONTROLLING LANDING GEAR
		dpad_down_current = Input.GetAxis ("Dpad down button");
		if (dpad_down_delaytime > 0.0f) {//this controls the delay time from wheels up and down so it wont be toggle in half way
			dpad_down_delaytime -= 0.02f;
		}
		if ((dpad_down_delaytime <= 0.0f)&&(dpad_down_current == 0)) {//this variables gives the ok to toggle wheel up and down
			dpad_down_ready = true;
		}


		if ((dpad_down_current == -1)&&(dpad_down_ready)&&(dpad_down_select == 0)){//THIS CLOSES LANDING WHEELS
			anim.SetBool ("SPARTA PLANE WHEELS BOOL", false);
			dpad_down_ready = false;
			dpad_down_select = 1;
			dpad_down_delaytime = 1.0f;
		}
		if ((dpad_down_current == -1)&&(dpad_down_ready)&&(dpad_down_select == 1)){//THIS OPENES LANDING WHEELS
			anim.SetBool ("SPARTA PLANE WHEELS BOOL", true);
			dpad_down_ready = false;
			dpad_down_select = 0;
			dpad_down_delaytime = 1.0f;
		}
//ENGINE SOUND
//ENGINE SOUND
		plane_audio.pitch = plane_enginesound_pitch + 1.0f;
		plane_audio.volume = plane_pitch_volume;
		if (is_flying) {
			
			if (is_accelerating){//this accelerates engine noise to match with accelerate button
				if (plane_enginesound_pitch < 2.10f) {
					plane_enginesound_pitch += 0.8f * Time.deltaTime;
				}
			}
			if (is_accelerating == false) {//this decelerate engine noise to match with deceleration button
				if (plane_enginesound_pitch > 1.5f) {
					plane_enginesound_pitch -= 0.8f * Time.deltaTime;
				}
			}
		}


}



//CAMERA CLASS
	void plane_camera_class(){
		camera_multiplier = 179;
		right_stick_horizontal = camera_multiplier * Input.GetAxis ("Horizontal right stick");
		right_stick_vertical = camera_multiplier * Input.GetAxis ("Vertical right stick");

		plane_rot_X = transform.eulerAngles.x;
		plane_rot_Y = transform.eulerAngles.y;
		plane_rot_Z = transform.eulerAngles.z;


		temp = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		plane_camera_parent.transform.position = temp;
		plane_camera_parent.transform.eulerAngles = new Vector3 (plane_rot_X,right_stick_horizontal += plane_rot_Y,right_stick_vertical += plane_rot_Z);

	}
	void reset_plane(){//this class hold all the needed parameters to reset the plane to its starting point with 0 speed.
		forward_speed = 0.0f;
		is_flying = false;
		gameObject.transform.position = new Vector3 (203,55,180);
		gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
	}
	void OnCollisionEnter(Collision col){
		
			gameObject.GetComponent<Rigidbody> ().isKinematic = true;//this will stop all physics movement from collision to bring the plane to a halt
			reset_plane ();//this resets the plane to starting point
			gameObject.GetComponent<Rigidbody> ().isKinematic = false;//this will resume all movements.
			//the plane mesh collider must stay with convex checked and rigidbody iskinematic unchecked for it to collide with the terrain


	}
	void OnCollisionStay(Collision col){



	}
	void OnCollisionExit(Collision col){


	}
}
