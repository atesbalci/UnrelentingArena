#pragma strict

var acc:int = 10;

function Start () {
	
}

function Update () {
	var amount:int = 0;
	if (Input.GetKey ("up"))
		amount += acc;
	if (Input.GetKey ("down"))
		amount -= acc;
	
	rigidbody.AddRelativeForce(amount, 0, 0);
}