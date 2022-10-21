using UnityEngine;

public class MapCameraController : MonoBehaviour
{

    [SerializeField] float MoveSpeed;

    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * -MoveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * -MoveSpeed * Time.deltaTime);
        }
    }
}
