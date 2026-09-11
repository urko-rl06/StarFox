using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float xySpeed = 10.0f; //Movimiento en el plano XY
    public float rotationSpeed = 10000f;
    public float tiltMultiplier = 15f;
    [SerializeField] InputActionReference moveAction;

    public GameObject aimParent;
    public Transform model;

    void Start()
    {
    }

    void Update()
    {
        Vector2 xyVector = moveAction.action.ReadValue<Vector2>();
        LocalMove(xyVector.x, xyVector.y,xySpeed);
        ClampPosition(); //
        RotationLook(xyVector.x, xyVector.y, rotationSpeed);
        HorizontalTilt(model, xyVector.x, tiltMultiplier, 0.1f);
    }

    void LocalMove(float width, float height, float speed)
    {
        transform.localPosition += new Vector3(width, height, 0) * speed * Time.deltaTime;
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float x, float y, float rotSpeed)
    { 
        aimParent.transform.localPosition = new Vector3 (x,y, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimParent.transform.position), Mathf.Deg2Rad * rotSpeed * Time.deltaTime);
    }

    void HorizontalTilt(Transform target, float axis,float tiltMultiplier, float lerpTime)
    {
        Vector3 targetEulerAngles = target.localEulerAngles;
        target.localEulerAngles = 
        new Vector3
        (targetEulerAngles.x, 
        targetEulerAngles.y, 
        Mathf.LerpAngle(targetEulerAngles.z, -axis * tiltMultiplier, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimParent.transform.position, .5f);
        Gizmos.DrawSphere(aimParent.transform.position, .15f);
    }
}
