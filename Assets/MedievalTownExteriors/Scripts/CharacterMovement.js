#pragma strict

import System.Collections.Generic;

var accelerationSpeed: float = 0.4; //player's movement speed
var gravity : float = 10; //amount of gravitational force applied to the player
private var controller : CharacterController; //player's CharacterController component
private var moveDirection : Vector3 = Vector3.zero;

private var currentSpeed_x : float = 0;
private var currentSpeed_z : float = 0;

var FRICTION : float = 0.95;
var MAX_SPEED : float = 5;

var start_x : float = 0;
var start_y : float = 0;
var start_z : float = 0;

var start_quaternion =  null;

//var seeker = go.GetComponent("VRCapture");
//seeker.StartPath(go.transform.position, destinationPoint);  


var replay=false;

var isPositionCapturing = false;

public class ReplayStep {
    public var value : Vector3;
    public var value_z : Vector3;
    public var value_x : Vector3;
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
            var M = Input.GetKey("m");
         
            if(replay){
                Debug.Log("M: "+M);
                if(currentIndex >= inputList.Count || M){
                    Debug.Log("finished recording: "+new Date());
                   // EditorUtility.DisplayDialog("finished recording");
                    VRCapture.VRCapture.Instance.StopCapture();
                    //Application.Quit();
                    inputList = new List.<ReplayStep>();
                    currentIndex=0;
                    replay = false;
                    return;
                }

                if(inputList[currentIndex].isGrounded){
                    //controller.SimpleMove(inputList[currentIndex].value);

                    controller.SimpleMove(inputList[currentIndex].value_z);
                    controller.SimpleMove(inputList[currentIndex].value_x);
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
                    moveDirection.y = accelerationSpeed;
                }
                else if(Input.GetKey("w")) {
                    //currentResult.value = transform.forward * speed;
                    //controller.SimpleMove(currentResult.value);
	
                    currentSpeed_z +=accelerationSpeed;
                }
                if(Input.GetKey("s")) {
                    //currentResult.value = transform.forward * -speed;
                    //controller.SimpleMove(currentResult.value);
                    currentSpeed_z -= accelerationSpeed;
                }
                if(Input.GetKey("a")) {
                    //currentResult.value = left*speed;
                    //controller.SimpleMove(currentResult.value);
                    currentSpeed_x += accelerationSpeed;
                }
                if(Input.GetKey("d")) {
                    // currentResult.value = left*-speed;
                    //controller.SimpleMove(currentResult.value);
                    currentSpeed_x -= accelerationSpeed;
                }
		

                currentSpeed_x = currentSpeed_x*FRICTION;
                currentSpeed_z = currentSpeed_z*FRICTION;

                if(currentSpeed_x > MAX_SPEED) currentSpeed_x=MAX_SPEED;
                if(currentSpeed_z > MAX_SPEED) currentSpeed_z=MAX_SPEED;

                if(currentSpeed_x < -MAX_SPEED) currentSpeed_x=-MAX_SPEED;
                if(currentSpeed_z < -MAX_SPEED) currentSpeed_z=-MAX_SPEED;

                currentResult.value_z = transform.forward * currentSpeed_z;
                currentResult.value_x = left * currentSpeed_x;

                controller.SimpleMove(currentResult.value_z);// transform.forward * currentSpeed_z);
                controller.SimpleMove(currentResult.value_x); //left * currentSpeed_x);
            	
		
                if(M && !replay){
                    if(isPositionCapturing){
                        Debug.Log("stop input capturing");
                        Debug.Log("start recording: "+new Date());
                        replay = true;
                        isPositionCapturing=false;
                        //todo reset position
                        //transform.position = new Vector3();
                        transform.position.x = start_x;
                        transform.position.y = start_y;
                        transform.position.z = start_z;
                      
		
                        transform.rotation = start_quaternion; //Quaternion.identity;

                        VRCapture.VRCapture.Instance.StartCapture();
                    }else{
                        Debug.Log("start input capturing");

                        start_x = transform.position.x;
                        start_y = transform.position.y;
                        start_z = transform.position.z;
                        start_quaternion = transform.rotation;
                        isPositionCapturing=true;
                    }
            
                }
            }
            else {
                currentResult.isGrounded = false;
                if(Input.GetKey("w")) {
                    var relative : Vector3;
                    relative = transform.TransformDirection(0,0,1);
                    currentResult.valueAir = relative * Time.deltaTime * accelerationSpeed / 1.5;
                    controller.Move(currentResult.valueAir);
                }
            }

            if(isPositionCapturing){
                inputList.Add(currentResult);
            }


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