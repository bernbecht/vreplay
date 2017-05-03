#pragma strict

import System.Collections.Generic;

var speed : float = 7; //player's movement speed
var gravity : float = 10; //amount of gravitational force applied to the player
private var controller : CharacterController; //player's CharacterController component
private var moveDirection : Vector3 = Vector3.zero;

var start_x : float = 0;
var start_y : float = 0;
var start_z : float = 0;


var replay=false;

public class ReplayStep {
    public var value : Vector3;
        public var isGrounded : boolean = true;
        public var valueAir : Vector3;
    }


var inputList :List.<ReplayStep> = new List.<ReplayStep>();
//var inputList = new Array();
var currentIndex : int = 0;




function Start () {
    Debug.Log("start");
    controller = transform.GetComponent(CharacterController);

    start_x = transform.position.x;
    start_y = transform.position.y;
    start_z = transform.position.z;
}

function Update () {
    if(replay){
    
        if(currentIndex >= inputList.Count){
            Debug.Log("finished");
            return;
        }

        if(inputList[currentIndex].isGrounded){
            controller.SimpleMove(inputList[currentIndex].value);
        }else{
            controller.Move(inputList[currentIndex].value);
        }
        currentIndex++;

        return;
    }


    var currentResult = new ReplayStep();// {isGrounded:true,value:null};

	//APPLY GRAVITY
	if(moveDirection.y > gravity * -1) {
		moveDirection.y -= gravity * Time.deltaTime;
	}
	controller.Move(moveDirection * Time.deltaTime);
	var left = transform.TransformDirection(Vector3.left);

	if(controller.isGrounded) {
		if(Input.GetKeyDown(KeyCode.Space)) {
			moveDirection.y = speed;
		}
		else if(Input.GetKey("w")) {
		    currentResult.value = transform.forward * speed;
		    controller.SimpleMove(currentResult.value);
		}
		else if(Input.GetKey("s")) {
		    currentResult.value = transform.forward * -speed;
		    controller.SimpleMove(currentResult.value);
		}
		else if(Input.GetKey("a")) {
		    currentResult.value = left*speed;
		    controller.SimpleMove(currentResult.value);
		}
		else if(Input.GetKey("d")) {
		    currentResult.value = left*-speed;
		    controller.SimpleMove(currentResult.value);
		}else if(Input.GetKey("m")){
		    replay = true;
		    //todo reset position
		    //transform.position = new Vector3();
		    transform.position.x = start_x;
		    transform.position.y = start_y;
		    transform.position.z = start_z;

		
		    transform.rotation = Quaternion.identity;
            
		}
	}
	else {
	    currentResult.isGrounded = false;
		if(Input.GetKey("w")) {
			var relative : Vector3;
			relative = transform.TransformDirection(0,0,1);
			currentResult.valueAir = relative * Time.deltaTime * speed / 1.5;
			controller.Move(currentResult.valueAir);
		}
	}

	inputList.Add(currentResult);



}
function OnTriggerEnter(hit : Collider) {
 
	if(hit.transform.tag == "LoadNewScene") {
		hit.transform.GetComponent(LoadNewScene).DisplayScene();
	}
}
function OnTriggerExit(hit : Collider) {
	if(hit.transform.tag == "LoadNewScene") {
		hit.transform.GetComponent(LoadNewScene).HideScene();
	}

	Debug.Log("exit");



}
function OnControllerColliderHit (hit : ControllerColliderHit) {
	if(hit.transform.GetComponent.<Rigidbody>()) {
		hit.transform.GetComponent.<Rigidbody>().AddForce(10 * transform.forward);
	}
}