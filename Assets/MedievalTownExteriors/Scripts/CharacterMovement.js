#pragma strict


import System.Collections.Generic;

var USE_POSITIONS = true;

var USE_FRICITON = true;

var accelerationSpeed: float = 100.4; //player's movement speed
var gravity : float = 10; //amount of gravitational force applied to the player
private var controller : CharacterController; //player's CharacterController component
private var moveDirection : Vector3 = Vector3.zero;

private var currentSpeed_x : float = 0;
private var currentSpeed_z : float = 0;

var FRICTION : float = 1;//0.95;
var MAX_SPEED : float = 2.5;

var start_x : float = 0;
var start_y : float = 0;
var start_z : float = 0;

var start_quaternion =  null;

//var seeker = go.GetComponent("VRCapture");
//seeker.StartPath(go.transform.position, destinationPoint);  


var replay=false;

var isPositionCapturing = false;

var mkeydown =false;

public class ReplayStep {
    public var value : Vector3;

    public var value_y : Vector3;
    public var value_z : Vector3;
    public var value_x : Vector3;

    public var value_y_f : float;
    public var value_z_f : float;
    public var value_x_f : float;
    public var isGrounded : boolean = true;
    public var valueAir : Vector3;
}


var inputList :List.<ReplayStep> = new List.<ReplayStep>();
        //var inputList = new Array();
        var currentIndex : int = 0;
        var speed = MAX_SPEED;
        function Start () {
            Debug.Log("start");
            controller = transform.GetComponent(CharacterController);

            start_x = transform.position.x;
            start_y = transform.position.y;
            start_z = transform.position.z;
        }

        function Update () {
            var mk=Input.GetKey("m");
            var M = false;
            if(!mkeydown && mk){
                M=true;
            }
            mkeydown = mk;

           // var M = Input.GetKey("m");
           // Debug.Log("M: "+M);
         
            if(replay){
                Debug.Log("M: "+M);
                if(currentIndex >= inputList.Count || M){   // if there are no frames left, or m is presse to abort
                                                            // then stop capturing and reset everything
                    Debug.Log("finished recording: "+new Date());
                   // EditorUtility.DisplayDialog("finished recording");
                    VRCapture.VRCapture.Instance.StopCapture();
                    //Application.Quit();
                    inputList = new List.<ReplayStep>();
                    currentIndex=0;
                    replay = false;
                    return;
                }

                // if there are still frames left, change the movement
                if(inputList[currentIndex].isGrounded){
                    //controller.SimpleMove(inputList[currentIndex].value);

                    if(!USE_POSITIONS){
                        controller.SimpleMove(inputList[currentIndex].value_z);
                        controller.SimpleMove(inputList[currentIndex].value_x);
                    }else{
                        controller.transform.position.x = inputList[currentIndex].value_x_f;
                        controller.transform.position.y = inputList[currentIndex].value_y_f;
                        controller.transform.position.z = inputList[currentIndex].value_z_f;
                    }
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
                    if(USE_FRICITON)
                        currentSpeed_z +=accelerationSpeed;
                    else
                        controller.SimpleMove(transform.forward * speed);
                }
                if(Input.GetKey("s")) {
                    //currentResult.value = transform.forward * -speed;
                    //controller.SimpleMove(currentResult.value);
                    if(USE_FRICITON)
                        currentSpeed_z -= accelerationSpeed;
                    else
                        controller.SimpleMove(transform.forward * -speed);
                }
                if(Input.GetKey("a")) {
                    //currentResult.value = left*speed;
                    //controller.SimpleMove(currentResult.value);
                    if(USE_FRICITON)
                        currentSpeed_x += accelerationSpeed;
                    else
                        controller.SimpleMove(left * speed);
                }
                if(Input.GetKey("d")) {
                    // currentResult.value = left*-speed;
                    //controller.SimpleMove(currentResult.value);
                    if(USE_FRICITON)
                        currentSpeed_x -= accelerationSpeed;
                    else
                        controller.SimpleMove(left*-speed);
                }
		

                currentSpeed_x = currentSpeed_x*FRICTION;
                currentSpeed_z = currentSpeed_z*FRICTION;

                if(currentSpeed_x > MAX_SPEED) currentSpeed_x=MAX_SPEED;
                if(currentSpeed_z > MAX_SPEED) currentSpeed_z=MAX_SPEED;

                if(currentSpeed_x < -MAX_SPEED) currentSpeed_x=-MAX_SPEED;
                if(currentSpeed_z < -MAX_SPEED) currentSpeed_z=-MAX_SPEED;

           
                if(!USE_POSITIONS && USE_FRICITON){
                    currentResult.value_z = transform.forward * currentSpeed_z;
                    currentResult.value_x = left * currentSpeed_x;
                
                    controller.SimpleMove(currentResult.value_z);// transform.forward * currentSpeed_z);
                    controller.SimpleMove(currentResult.value_x); //left * currentSpeed_x);
                }

                if(USE_POSITIONS || !USE_FRICITON){
                    currentResult.value_z_f = controller.transform.position.z;
                    currentResult.value_y_f = controller.transform.position.y;
                    currentResult.value_x_f = controller.transform.position.x;

                    controller.SimpleMove(transform.forward * currentSpeed_z);// transform.forward * currentSpeed_z);
                    controller.SimpleMove(left * currentSpeed_x); //left * currentSpeed_x);
                }
		
                if(M && !replay){   // if it is not replaying, but m is hit,

                   
                    if(isPositionCapturing){    // check if it is recording the positions
                                                // if it is recording positions already, then start replying the keyframes and record them
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
                        // otherwise start recording of the position
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