using System;

/// <summary>
/// Класс, хранящий информацию о настройках
/// </summary>
[Serializable]
public class Info
{
    /// <summary>
    /// Текущий набор изображений (номер)
    /// </summary>
    public int PicturePack;
    /// <summary>
    /// Текущий фон (номер)
    /// </summary>
    public int BG;
    /// <summary>
    /// Текущая ширина экрана
    /// </summary>
    public int ScreenWidth;
    /// <summary>
    /// Текущая высота экрана
    /// </summary>
    public int ScreenHeight;
    /// <summary>
    /// Текущее состояние активности полноэкранного режима
    /// </summary>
    public bool FullScreen;
    /// <summary>
    /// Текущее имя игрока
    /// </summary>
    public string Name;
    /// <summary>
    /// Конструктор типа
    /// </summary>
    /// <param name="picturePack">Текущий набор изображений (номер)</param>
    /// <param name="bg">Текущий фон (номер)</param>
    /// <param name="screenWidth">Текущая ширина экрана</param>
    /// <param name="screenHeight">Текущая высота экрана</param>
    /// <param name="fullScreen">екущее состояние активности полноэкранного режима</param>
    /// <param name="name">Текущее имя игрока</param>
    public Info(int picturePack, int bg, int screenWidth, int screenHeight, bool fullScreen, string name)
    {
        PicturePack = picturePack;
        BG = bg;
        ScreenHeight = screenHeight;
        ScreenWidth = screenWidth;
        FullScreen = fullScreen;
        Name = name;
    }
}
