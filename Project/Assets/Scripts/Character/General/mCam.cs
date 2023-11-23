using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class mCam : MonoBehaviour
{
    GameController gc; FragmentControl frag;
    public PhotonView phView;

    [Header("Camera")]
    public Transform charBody, charHead;

    float maxY = 90, minY = -75;
    float rotationX = 0, rotationY = 0;

    public float senseX = 1.2f, senseY = 1.2f;
    float smoothRotX = 0, smoothRotY = 0;

    float smoothCoefX = 1.5f, smoothCoefY = 1.5f;
    float range = 2.5f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gc = FindObjectOfType(typeof(GameController)) as GameController;
        frag = FindObjectOfType(typeof(FragmentControl)) as FragmentControl;

        if (!phView.IsMine)
            gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.position = charHead.position;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.red);

        switch (gc.states)
        {
            case States.Play:
                FpsCamera();
                break;
            case States.Pause:
                break;
        }
    }

    void FpsCamera()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * senseY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * senseX;

        rotationX += smoothRotX;
        rotationY += smoothRotY;

        smoothRotX = Mathf.Lerp(smoothRotX, horizontalDelta, smoothCoefX);
        smoothRotY = Mathf.Lerp(smoothRotY, verticalDelta, smoothCoefY);

        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        charBody.localEulerAngles = new Vector3(0, rotationX, 0);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
