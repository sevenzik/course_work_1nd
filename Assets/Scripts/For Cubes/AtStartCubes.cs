using UnityEngine;

/// <summary>
/// Скрипт, отвечающий за применение текстур к кубикам и установку фона в сеансе игры.
/// </summary>
public class AtStartCubes : MonoBehaviour {
    /// <summary>
    /// Объект - поверхность (фон)
    /// </summary>
    public GameObject Plane;
    /// <summary>
    /// Метод, применяющий текстуры нужного набора изображений и уровня сложности к кубикам
    /// </summary>
    /// <param name="picturePack">Номер набора изображений</param>
    /// <param name="level">Уровень сложности</param>
    public void UseTextures(int picturePack, int level)
    {
        string info = "PicturePack" + picturePack.ToString() + "q" + level.ToString();
        for (int i = 0; i < transform.GetComponent<ChangePlaces>().cubes.Length; i++)
        {
            transform.GetComponent<ChangePlaces>().cubes[i].GetComponent<Renderer>().material.mainTexture =
                Resources.Load("Images/" + info + "/__" + (i+1).ToString()) as Texture;
        }
        Plane.GetComponent<Renderer>().material.mainTexture = Resources.Load("Images/BGs/" + MenuScreenChanger.BG.ToString()) as Texture;
    }
    /// <summary>
    /// Метод, вызывающийся регулярно, устанавливающий заданное разрешение
    /// </summary>
	void Update ()
    {
        Screen.SetResolution(MenuScreenChanger.ScreenWidth, MenuScreenChanger.ScreenHeight, MenuScreenChanger.FullScreen);
    }
}
