using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт, отвечающий за загрузку изображений на табло изображений в сеансе игры.
/// </summary>
public class AtStartImageBoard : MonoBehaviour {

    /// <summary>
    /// Массив элементов табло
    /// </summary>
    public Transform[] images;   
    /// <summary>
    /// Метод, загружающий изображения в элементы табло
    /// </summary>
    /// <param name="picturePack"></param>
    public void CreateImages(int picturePack)
    {
        
        string info = "PicturePack" + picturePack.ToString() + "q2";
        images = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform t in transform)
        {
            images[i++] = t;
        }
        for (i = 0; i < images.Length; i++)
        {
            images[i].GetComponent<RawImage>().texture = Resources.Load("Images/" + info + "/" + (i + 1).ToString()) as Texture;
        }
    }
    /// <summary>
    /// Метод, отмечающий выбранное по номеру изображение собранным
    /// </summary>
    /// <param name="i">Номер изображения</param>
    public void ImageComplete(int i)
    {
        images[i].GetComponent<RawImage>().color = Color.white;
    }
	
}
