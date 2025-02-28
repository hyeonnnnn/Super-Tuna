 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	public float backgroundSize;
	public float parallaxSpeed;
	public float z = 0;

	public Transform cameraTransform;
	public Transform[] layers;
	public float viewZone = 10;
	public int leftIndex;
	public int rightIndex;
	public float lastCameraX;
	public float lastCameraY;
	
	void Start () {
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++){
			layers [i] = transform.GetChild (i);
		}

		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	void Update () {
		
		parallaxHor ();
		parallaxVer ();


		if(cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone) ){
			ScrollLeft ();			
		}

		if(cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone) ){
			ScrollRight ();			
		}

	}

	private void parallaxHor(){
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right * (deltaX * parallaxSpeed);
		lastCameraX = cameraTransform.position.x;
	}

	private void parallaxVer(){
		float deltaY = cameraTransform.position.y - lastCameraY;
		transform.position += Vector3.up * (deltaY * parallaxSpeed);
		lastCameraY = cameraTransform.position.y;	
	}

	private void ScrollLeft(){
 
		layers [rightIndex].position = new Vector3(layers[leftIndex].position.x - backgroundSize, layers[leftIndex].position.y, z );
		leftIndex = rightIndex;
		rightIndex--;
		if(rightIndex < 0){
			rightIndex = layers.Length - 1;
		}
	}

	private void ScrollRight(){
 
		layers [leftIndex].position = new Vector3(layers[rightIndex].position.x + backgroundSize, layers[leftIndex].position.y, z );
		rightIndex = leftIndex;
		leftIndex++;
		if(leftIndex == layers.Length){
			leftIndex = 0;	
		}
	}


}
