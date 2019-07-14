using UnityEngine;

/// <summary>
/// Структура, обозначающая положению кубика.
/// </summary>
public struct CubeInfo
{
    /// <summary>
    /// Положение кубика
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// Порядковая позиция по расположению
    /// </summary>
    public int place;

    /// <summary>
    /// Конструктор структуры
    /// </summary>
    /// <param name="v">Положение кубика</param>
    /// <param name="p">Порядковая позиция по расположению</param>
    public CubeInfo(Vector3 v, int p)
    {
        position = v;
        place = p;
    }
}
