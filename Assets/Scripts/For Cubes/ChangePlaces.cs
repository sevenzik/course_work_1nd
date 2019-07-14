using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт, отвечающий за возможность менять выбранные кубики местами, а также вызывающий загрузку текстур сеанса игры.
/// </summary>
public class ChangePlaces : MonoBehaviour {

    /// <summary>
    /// Массив кубиков уровня
    /// </summary>
    public Transform[] cubes;
    /// <summary>
    /// Массив двух кубиков, выбранных для перемещения
    /// </summary>
    public Transform[] cubesForChange = new Transform[2];
    /// <summary>
    /// Флаг активной паузы
    /// </summary>
    public bool paused;
    /// <summary>
    /// Табло изображений
    /// </summary>
    public GameObject ImageBoard;
    /// <summary>
    /// Кнопка, отмечающая конец сборки
    /// </summary>
    public GameObject CompleteButton;
    /// <summary>
    /// Количество готовых к перемещению кубиков
    /// </summary>
    public int changeChecker = 0;
    /// <summary>
    /// Метод инициализации, получающий кубики уровня и загружающий текстуры
    /// </summary>
    void Start () {
        cubes = new Transform[transform.childCount];
        int i = 0;        
        foreach (Transform t in transform)
        {
            cubes[i++] = t;
        }
        changeChecker = 0;
        paused = false;
        transform.GetComponent<AtStartCubes>().UseTextures(MenuScreenChanger.PicturePack,MenuScreenChanger.LastLevel);
        ImageBoard.GetComponent<AtStartImageBoard>().CreateImages(MenuScreenChanger.PicturePack);
    }
    /// <summary>
    /// Метод, осуществляющий обмен двух кубиков позициями
    /// </summary>
    /// <param name="cube">Кубик для перемещения</param>
    public void Change(Transform cube)
    {
        CompleteButton.GetComponent<Button>().interactable = false;
        cubesForChange[changeChecker] = cube;
        cube.GetComponent<Renderer>().material.shader = Shader.Find("Legacy Shaders/Self-Illumin/Specular");
        changeChecker++;
        if (changeChecker == 2)
        {
            changeChecker = 0;
            Vector3 tmp = cubesForChange[0].position;
            cubesForChange[0].position = cubesForChange[1].position;
            cubesForChange[1].position = tmp;
            int itmp = cubesForChange[0].GetComponent<Changer>().number;
            cubesForChange[0].GetComponent<Changer>().number = cubesForChange[1].GetComponent<Changer>().number;
            cubesForChange[1].GetComponent<Changer>().number = itmp;
            foreach (Transform c in cubesForChange)
            {
                c.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            }
            CompleteButton.GetComponent<Button>().interactable = true;
        }
    }
}
