using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWASD : MonoBehaviour
{
    CharacterController cc;

    private Transform sprite, cam;
    private float forwardMovement, sidewaysMovement, verticalVelocity;
    private float playerWalkingSpeed = 3f;
    private Vector3 camPoz;
    [HideInInspector] public bool wRozmowie;

    private bool czyEkwipunekOtwarty;
   
    void Awake()
    {
        sprite = this.transform.GetChild(0).transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        clickNieWalka.ekwipunekWidoczny += CzyEkwipunekOtwarty;
    }
    private void OnDestroy()
    {
        clickNieWalka.ekwipunekWidoczny -= CzyEkwipunekOtwarty;
    }
    void CzyEkwipunekOtwarty(bool czy)
    {
        czyEkwipunekOtwarty = czy;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        if (wRozmowie  || czyEkwipunekOtwarty)
        {
            forwardMovement = 0;
            sidewaysMovement = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed * 2;
            sidewaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(forwardMovement != 0 && sidewaysMovement != 0)
        {
            forwardMovement /= 1.4f;
            sidewaysMovement /= 1.4f;
        }
        if (cc.isGrounded == false)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        Vector3 playerMovement = new Vector3(sidewaysMovement, verticalVelocity, forwardMovement);
        cc.Move(sprite.rotation * playerMovement * Time.deltaTime);
    }

    void LateUpdate()
    {
        camPoz = cam.transform.position;
        camPoz.y = sprite.position.y;
        sprite.LookAt(camPoz);
        sprite.Rotate(0f, 180f, 0f);
    }
}
