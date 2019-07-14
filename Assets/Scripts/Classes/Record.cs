using System;

/// <summary>
/// Класс, хранящий информацию о рекордах
/// </summary>
[Serializable]
public class Record
{
    /// <summary>
    /// Строка, хранящая время рекорда
    /// </summary>
    public string _Record;
    /// <summary>
    /// Строка, хранящая имя владельца рекорда
    /// </summary>
    public string Name;

    /// <summary>
    /// Конструктор типа
    /// </summary>
    /// <param name="record">Рекорд</param>
    /// <param name="name">Имя владельца рекорда</param>
    public Record(string record, string name)
    {
        _Record = record;
        Name = name;
    }
}
