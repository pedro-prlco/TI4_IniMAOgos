using UnityEngine;

public class MapCameraController : MonoBehaviour
{

    public static bool CanInteract = true;

    [SerializeField] float MoveSpeed;

    [SerializeField] float minX, maxX;
    [SerializeField] float minZ, maxZ;

    void Update()
    {

        if(!CanInteract)
        {
            return;
        }

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

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(clampedX, transform.position.y, clampedY);
    }

#if UNITY_EDITOR
    
    [UnityEditor.CustomEditor(typeof(MapCameraController))]
    class MapCameraControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginVertical(GUI.skin.box);
            if(GUILayout.Button("Set min"))
            {
                MapCameraController controller = target as MapCameraController;
                controller.minX = controller.transform.position.x;
                controller.minZ = controller.transform.position.z;
            }
            if(GUILayout.Button("Set max"))
            {
                MapCameraController controller = target as MapCameraController;
                controller.maxX = controller.transform.position.x;
                controller.maxZ = controller.transform.position.z;
            }
            GUILayout.EndVertical();
        }
    }
#endif
}
