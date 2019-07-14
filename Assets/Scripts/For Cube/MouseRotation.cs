using UnityEngine;

/// <summary>
/// Скрипт кубика, отвечающий за возможность вращения кубика.
/// </summary>
public class MouseRotation : MonoBehaviour {

    /// <summary>
    /// Объект, объединяющий все кубики
    /// </summary>
    public GameObject CubeManager;
    /// <summary>
    /// Скорость врщаения кубиков
    /// </summary>
    public float rotSpeed = 20;
    /// <summary>
    /// Значение, отслеживающее нажатие ЛКМ на объекте и отжимание ЛКМ
    /// </summary>
    bool pressed;
    /// <summary>
    /// Метод инициализации
    /// </summary>
    private void Start()
    {
        pressed = false;
    }
    /// <summary>
    /// Метод, отслеживающий нажатие на кубик
    /// </summary>
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            pressed = true;
        }
        
    }
    /// <summary>
    /// Метод, реализующий вращение кубика при взаимодействии с ним мыши
    /// </summary>
    private void Update()
    {        
        if (!CubeManager.GetComponent<ChangePlaces>().paused)
        {
            if (pressed && !CubeManager.GetComponent<CompleteChecker>().complete && !CubeManager.GetComponent<CompleteChecker>().glue)
            {                
                float rotX = Input.GetAxis("Mouse X") * rotSpeed;
                float rotY = Input.GetAxis("Mouse Y") * rotSpeed;
                transform.Rotate(rotY, rotX, 0);
            }
            else
            {
                Vector3 rot = transform.rotation.eulerAngles;
                float y = Mathf.Round((float)(rot.y / 90.0)) * 90;
                float x = Mathf.Round((float)(rot.x / 90.0)) * 90;
                float z = Mathf.Round((float)(rot.z / 90.0)) * 90;
                transform.eulerAngles = new Vector3(x, y, z);
            }            
            if (!Input.GetMouseButton(0))
            {
                pressed = false;
            }
        }    
    }
}
