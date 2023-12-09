using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform lillineObj;
    public Transform ardenObj;
    public Transform feliciaObj;
    public GameObject playerGameObj;
    public PlayerMove playerScript;

    public GameObject basicCam;
    public GameObject combatCam;
    public GameObject reticle;

    public bool locked;

    public float rotationSpeed;

    public Transform comabtLookAt;

    public static CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Locked
    }
    // Update is called once per frame
    void Update()
    {
        if (!PlayerMove.GetInstance().dead)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (!playerScript.sprinting)
                {
                    SwitchCameraStyle(CameraStyle.Combat);
                    playerScript.canSprint = false;
                    if(playerScript.currentChar == PlayerMove.Character.Arden) playerScript.Block();
                }

            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (currentStyle == CameraStyle.Combat)
                {
                    SwitchCameraStyle(CameraStyle.Basic);
                    playerScript.canSprint = true;
                    playerScript.isChanneling = false;
                    playerScript.beam.SetActive(false);
                    if (playerScript.currentChar == PlayerMove.Character.Arden) playerScript.Unblock();
                }
            }

            if (player)
            {
                Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
                orientation.forward = viewDir.normalized;
            }


            if (currentStyle == CameraStyle.Basic)
            {

                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

                if (inputDir != Vector3.zero)
                {
                    lillineObj.forward = Vector3.Slerp(lillineObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                    ardenObj.forward = Vector3.Slerp(ardenObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                    feliciaObj.forward = Vector3.Slerp(lillineObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else
            {
                Vector3 dirToCombatLookAt = comabtLookAt.position - new Vector3(transform.position.x, comabtLookAt.position.y, transform.position.z);
                orientation.forward = dirToCombatLookAt.normalized;

                lillineObj.forward = dirToCombatLookAt.normalized;
                ardenObj.forward = dirToCombatLookAt.normalized;
                feliciaObj.forward = dirToCombatLookAt.normalized;
            }
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        basicCam.SetActive(false);
        combatCam.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            basicCam.SetActive(true);
            basicCam.GetComponent<CinemachineFreeLook>().m_XAxis = combatCam.GetComponent<CinemachineFreeLook>().m_XAxis;
            basicCam.GetComponent<CinemachineFreeLook>().m_YAxis = combatCam.GetComponent<CinemachineFreeLook>().m_YAxis;
            reticle.SetActive(false);
            playerScript.AnimSetting(0);
        }
        if (newStyle == CameraStyle.Combat)
        {
            combatCam.SetActive(true);
            combatCam.GetComponent<CinemachineFreeLook>().m_XAxis = basicCam.GetComponent<CinemachineFreeLook>().m_XAxis;
            combatCam.GetComponent<CinemachineFreeLook>().m_YAxis = basicCam.GetComponent<CinemachineFreeLook>().m_YAxis;
            reticle.SetActive(true);
            playerScript.AnimSetting(1);
        }
        currentStyle = newStyle;
    }
}
