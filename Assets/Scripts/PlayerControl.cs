using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    InputAction moveAction;
    Vector2 moveamount;
    private Vector3 moveDir;
    [SerializeField] private float rotSpeed =100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");


    }
    private void OnEnable()
    {
         
        
    }
    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveamount = moveAction.ReadValue<Vector2>();

        moveDir = new Vector3(moveamount.x, 0, moveamount.y);

        // float targetRot = (Mathf.Atan2(moveamount.x, moveamount.y)) * Mathf.Rad2Deg;
        //  transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, targetRot, transform.eulerAngles.z),Time.deltaTime*5);

        Vector3 up = (transform.position - planet.position).normalized;
        Vector3 forward = Vector3.Cross(transform.right, up).normalized;
        Vector3 right = Vector3.Cross(up, transform.forward).normalized;

        var toMove = (forward * moveDir.z);
        rb.MovePosition(rb.position + toMove.normalized * speed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, moveDir.x * rotSpeed * Time.deltaTime, 0));
    


}


    private void FixedUpdate()
    {



    }
}
