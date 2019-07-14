using UnityEngine;

/// <summary>
/// Скрипт кубика, отвечающий за возможность выбора кубика
/// для того, чтобы менять кубики местами. 
/// </summary>
public class Changer : MonoBehaviour {
    /// <summary>
    /// Объект, объединяющий все кубики
    /// </summary>
    public GameObject CubeManager;    
    /// <summary>
    /// Порядковый номер кубика относительно расположения
    /// </summary>
    public int number;    
	/// <summary>
    /// Метод, отслеживающий нажатие правой кнопки мыши на кубик для последующего перемещения
    /// </summary>
	void OnMouseOver() {
        if (!CubeManager.GetComponent<ChangePlaces>().paused)
        {
            if (Input.GetMouseButtonDown(1) && !CubeManager.GetComponent<CompleteChecker>().complete && !CubeManager.GetComponent<CompleteChecker>().glue)
            {                
                CubeManager.GetComponent<ChangePlaces>().Change(transform);
            }
        }
           
    }
}
