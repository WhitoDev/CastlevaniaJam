using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour 
{
    const int PIXELPERUNITSIZE = 100;
    public PixelPerfectCamera PPCameraScript;

    public Transform targetTransform;
    public float camSpeed = 1;
    public float horizontalPan = 0;
    public float verticalPan = 0;
    
    public bool shakeCamera;
    public float shakeIntensity = 5;
    public float stabilizingSpeed = 0.01f;

	void Start () 
    {
        PlayerPrefs.DeleteAll();
	}
	
	void FixedUpdate () 
    {
        CheckPlayerInput();

        if (targetTransform != null)
        {
            PositionateCamera();            
        }

	}

    void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus) && PPCameraScript.cameraZoom > 1)
        {
            PPCameraScript.cameraZoom--;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            PPCameraScript.cameraZoom++;
        }

        if (!shakeCamera && Input.GetKeyDown(KeyCode.F))
        {
            var shakeDirection = new Vector2(Mathf.Sign(UnityEngine.Random.Range(-1, 1)), Mathf.Sign(UnityEngine.Random.Range(-1, 1)));
            StopCoroutine(ShakeScreen(shakeIntensity, shakeDirection, stabilizingSpeed));
            StartCoroutine(ShakeScreen(shakeIntensity, shakeDirection, stabilizingSpeed));
        }

    }

    public void PositionateCamera()
    {
        Vector3 previousPosition = this.transform.position;
        Vector3 targetPosition = targetTransform.position;

        targetPosition = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);

        //Apply horizontal pan
        targetPosition = new Vector3(targetPosition.x + horizontalPan * targetTransform.GetComponentsInChildren<Transform>()[1].localScale.x, targetPosition.y, targetPosition.z);

        //Apply vertical pan
        targetPosition = new Vector3(targetPosition.x, targetPosition.y + verticalPan, targetPosition.z);

        //Lerp previous position of the camera with the new target position
        targetPosition = Vector3.Lerp(previousPosition, targetPosition, Time.deltaTime * camSpeed);

        //Assign the new position to the camera
        this.transform.position = targetPosition;
    }

    #region Corutines    

    IEnumerator ShakeScreen(float Intensity, Vector2 direction, float StabilizingSpeed)
    {
        float shakeForce = Intensity;
        shakeCamera = true;
        for (shakeForce = Intensity; Mathf.Abs(shakeForce) > 0.001f; shakeForce *= stabilizingSpeed * -1)
        {            
            Vector3 newShakeCamPos = this.transform.position;            
            newShakeCamPos = new Vector3(newShakeCamPos.x + shakeForce * direction.x, newShakeCamPos.y + shakeForce * direction.y, newShakeCamPos.z);
            this.transform.position = newShakeCamPos;            
            yield return null;
        }
        shakeCamera = false;

        yield break;
    }

    #endregion
}
