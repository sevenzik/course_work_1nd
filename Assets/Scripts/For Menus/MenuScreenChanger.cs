using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

delegate void i_delegate<T>(T q);

/// <summary>
/// Скрипт, отвечающий за загрузку всех панелей, доступных из
/// главного меню, сохранение и применение настроек, хранение 
/// и представление текстового вида рекордов, сериализацию настроек
/// и рекордов для восстановления при повторном запуске, а также
/// запуск игровых уровней.
/// </summary>
public class MenuScreenChanger : MonoBehaviour {

    /// <summary>
    /// Объект - панель главного меню
    /// </summary>
    public GameObject MainMenuPanel;
    /// <summary>
    /// Объект - панель выбора уровней
    /// </summary>
    public GameObject PlayPanel;
    /// <summary>
    /// Объект - панель рекордов
    /// </summary>
    public GameObject RecordPanel;
    /// <summary>
    /// Объект - панель настроек
    /// </summary>
    public GameObject SelectPanel;
    /// <summary>
    /// Объект - панель правил игры
    /// </summary>
    public GameObject RulesPanel;
    /// <summary>
    /// Объект - панель "О программе"
    /// </summary>
    public GameObject AboutPanel;
    /// <summary>
    /// Объект - кнопка возврата
    /// </summary>
    public GameObject ReturnButton;
    /// <summary>
    /// Объект - поверхность (фон)
    /// </summary>
    public GameObject Plane;
    /// <summary>
    /// Объект - отображаемое имя игрока 
    /// </summary>
    public GameObject CurrentNameText;
    /// <summary>
    /// Объект - поле ввода имени игрока
    /// </summary>
    public GameObject InputName;
    /// <summary>
    /// Объект - выпадающий список выбора набора изображений для кубиков
    /// </summary>
    public Dropdown PicturePackDropdown;
    /// <summary>
    /// Объект - выпадающий список выбора набора изображений для фона
    /// </summary>
    public Dropdown BGDropdown;
    /// <summary>
    /// Объект - выпадающий список выбора активности полноэкранного режима
    /// </summary>
    public Dropdown FullscreenDropdown;
    /// <summary>
    /// Объект - выпадающий список выбора разрешения окна 
    /// </summary>
    public Dropdown ResolutionDropdown;
    /// <summary>
    /// Объект - изображение текущего набора изображений
    /// </summary>
    public GameObject ImagePicturePack;
    /// <summary>
    /// Массив объектов - панели рекордов каждого уровня сложности
    /// </summary>
    public Text[] LvlXText = new Text[3];
    /// <summary>
    /// Последний активный уровень сложности
    /// </summary>
    static public int LastLevel = 2;
    /// <summary>
    /// Текущий набор изображений (номер)
    /// </summary>
    static public int PicturePack = 1;
    /// <summary>
    /// Текущий фон (номер)
    /// </summary>
    static public int BG = 0;
    /// <summary>
    /// Текущая ширина экрана
    /// </summary>
    static public int ScreenWidth = 1024;
    /// <summary>
    /// Текущая высота экрана
    /// </summary>
    static public int ScreenHeight = 768;
    /// <summary>
    /// Текущее состояние активности полноэкранного режима
    /// </summary>
    static public bool FullScreen = false;
    /// <summary>
    /// Текущее имя игрока
    /// </summary>
    static public string Name = "Username1";
    /// <summary>
    /// Список рекордов
    /// </summary>
    static public List<Record>[] records = new List<Record>[3];
    
    /// <summary>
    /// Метод инициализации, восстанавливающий настройки из файла при повторном входе либо устанавливающий значения по умолчанию
    /// </summary>
    void Start() {
        if (!Directory.Exists("Settings"))
        {
            Directory.CreateDirectory("Settings");            
        }
        if (!Directory.Exists("Records"))
        {
            Directory.CreateDirectory("Records");
        }
        for (int i = 0; i < records.Length; i++)
        {
            records[i] = new List<Record>();
        }
        LoadSettingsFromFile();
        LoadRecordsFromFile();
        SaveSettingsToFile();
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreen);
        BGLoad();
        MainMenuPanel.SetActive(true);
        PlayPanel.SetActive(false);
        ReturnButton.SetActive(false);
        SelectPanel.SetActive(false);
        RecordPanel.SetActive(false);
        AboutPanel.SetActive(false);
        RulesPanel.SetActive(false);
    }
    /// <summary>
    /// Метод, синхронизируюющий значения в панели настроек с действительными значениями
    /// </summary>
    public void SaveSettings()
    {
        string name = "";
        for (int k = 0; k < Name.Length; k++)
        {
            name += Name[k] + " ";
        }
        CurrentNameText.GetComponent<Text>().text = "C u r r e n t : " + name;
        PicturePackDropdown.value = PicturePack - 1;
        PictureLoad();
        if (FullScreen)
            FullscreenDropdown.value = 1;
        else
            FullscreenDropdown.value = 0;

        if (ScreenWidth == 1024 && ScreenHeight == 768)
            ResolutionDropdown.value = 0;
        else if (ScreenWidth == 1366 && ScreenHeight == 768)
            ResolutionDropdown.value = 1;
        else if (ScreenWidth == 1200 && ScreenHeight == 900)
            ResolutionDropdown.value = 2;
        else if (ScreenWidth == 1600 && ScreenHeight == 900)
            ResolutionDropdown.value = 3;
        else if (ScreenWidth == 1920 && ScreenHeight == 1080)
            ResolutionDropdown.value = 4;

        BGDropdown.value = BG;
    }    
    /// <summary>
    /// Метод, активирующий переданную панель
    /// </summary>
    /// <param name="panelOpen">Панель для активации</param>
    public void ActivePanel(GameObject panelOpen)
    {
        panelOpen.SetActive(true);
        ReturnButton.SetActive(true);
    }
    /// <summary>
    /// Метод, дезактивирующий переданную панель
    /// </summary>
    /// <param name="panelOpen">Панель для дезактивации</param>
    public void ClosePanel(GameObject panelOpen)
    {
        panelOpen.SetActive(false);
    }
    /// <summary>
    /// Метод, дезактивирующий все панели и активирующий главное меню
    /// </summary>
    public void Return()
    {
        MainMenuPanel.SetActive(true);
        ReturnButton.SetActive(false);
        PlayPanel.SetActive(false);
        SelectPanel.SetActive(false);        
        RecordPanel.SetActive(false);
        AboutPanel.SetActive(false);
        RulesPanel.SetActive(false);
    }
    /// <summary>
    /// Метод, осуществляющий загрузку изображения в поле выбора текущего набора
    /// </summary>
    public void PictureLoad()
    {
        ImagePicturePack.GetComponent<RawImage>().texture = Resources.Load("Images/PicturePack" + PicturePack.ToString() + "q2/all") as Texture;
    }
    /// <summary>
    /// Метод, осуществляющий загрузку фона
    /// </summary>
    public void BGLoad()
    {
        Plane.GetComponent<Renderer>().material.mainTexture = Resources.Load("Images/BGs/" + BG.ToString()) as Texture;
    }
    /// <summary>
    /// Метод, сохраняющий настройки в файл
    /// </summary>
    public void SaveSettingsToFile()
    {
        if (!Directory.Exists("Settings"))
        {
            Directory.CreateDirectory("Settings");
        }
        string path = @"Settings/settings.ser";
        Info info = new Info(PicturePack, BG, ScreenWidth, ScreenHeight, FullScreen, Name);        
        WriteFileB(path, info);
    }
    /// <summary>
    /// Метод, выгружающий настройки из файла
    /// </summary>
    public void LoadSettingsFromFile()
    {
        string path = @"Settings/settings.ser";
        if (File.Exists(path))
        {
            DeserFileB<Info>(path, SettingsFromInfo);
        }
    }
    /// <summary>
    /// Метод, устанавливающий текущие настройки из переданной информации
    /// </summary>
    /// <param name="info">Информация о настройках</param>
    public void SettingsFromInfo(Info info)
    {
        PicturePack = info.PicturePack;
        BG = info.BG;
        ScreenHeight = info.ScreenHeight;
        ScreenWidth = info.ScreenWidth;
        FullScreen = info.FullScreen;
        Name = info.Name;
    }
    /// <summary>
    /// Метод, создающий текст с рекордами
    /// </summary>
    public void CreateTextForRecords()
    {
        for (int j = 0; j < records.Length; j++)
        {
            string s = "L e v e l " + (j+2).ToString() + "  : \n";
            for (int i = 0; i < records[j].Count; i++)
            {
                string name = "";
                for (int  k = 0; k < records[j][i].Name.Length; k++)
                {
                    name += records[j][i].Name[k] + " ";
                }
                name = name.Substring(0, name.Length - 1);
                s += (i + 1).ToString() + " - " + records[j][i]._Record + " - \n";
                s += name + "\n";
            }
            LvlXText[j].text = s;
        }
        
    }
    /// <summary>
    /// Метод, выгружающий рекорды из файла
    /// </summary>
    public void LoadRecordsFromFile()
    {
        for (int i = 0; i < records.Length; i++)
        {
            string path = @"Records/records" + (i+2).ToString() + ".ser";
            if (File.Exists(path))
            {
                LastLevel = i + 2;
                DeserFileB<List<Record>>(path, RecordsFromRecord);
            }
        }
        
    }
    /// <summary>
    /// Метод, записывающий рекорды из переданной информации
    /// </summary>
    /// <param name="r">Информация о рекордах</param>
    public void RecordsFromRecord(List<Record> r)
    {
        records[LastLevel - 2].Clear();
        for (int i = 0; i < r.Count; i++)
            records[LastLevel - 2].Add(r[i]);
    }
    /// <summary>
    /// Метод, меняющий номер текущего набора изображений на переданный
    /// </summary>
    /// <param name="number">Номер набора</param>
    public void ChangePicturePack(int number)
    {
        PicturePack = number + 1;
        PictureLoad();
        SaveSettingsToFile();
    }
    /// <summary>
    /// Метод, меняющий номер текущего фона на переданный
    /// </summary>
    /// <param name="number">Номер фона</param>
    public void ChangeBG(int number)
    {
        BG = number;
        BGLoad();
        SaveSettingsToFile();
    }
    /// <summary>
    /// Метод, осуществляющий загрузку уровня 2
    /// </summary>
    public void LoadLevel2()
    {
        SaveSettingsToFile();
        LastLevel = 2;
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        
    }
    /// <summary>
    /// Метод, осуществляющий загрузку уровня 3
    /// </summary>
    public void LoadLevel3()
    {
        SaveSettingsToFile();
        LastLevel = 3;
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);        
    }
    /// <summary>
    /// Метод, осуществляющий загрузку уровня 4
    /// </summary>
    public void LoadLevel4()
    {
        SaveSettingsToFile();
        LastLevel = 4;
        SceneManager.LoadScene("Level4", LoadSceneMode.Single);
    }
    /// <summary>
    /// Метод, вызывающийся регулярно, устанавливающий заданное разрешение
    /// </summary>
    void Update() {
        Screen.SetResolution(ScreenWidth, ScreenHeight, FullScreen);
    }
    /// <summary>
    /// Метод, изменяющий имя игрока
    /// </summary>
    public void ChangeName()
    {
        Name = InputName.GetComponent<InputField>().text;
        InputName.GetComponent<InputField>().text = "";
        SaveSettingsToFile();
        SaveSettings();     
    }
    /// <summary>
    /// Метод, изменяющий активность полноэкранного режима
    /// </summary>
    /// <param name="number">Параметр, где 1 отвечает за активный полноэкранный режим</param>
    public void FullScreenSetting(int number)
    {
        if (number == 1)
            FullScreen = true;
        else
            FullScreen = false;
        SaveSettingsToFile();
    }
    /// <summary>
    /// Метод, изменяющий разрешение окна программы
    /// </summary>
    /// <param name="number">Номер разрешения</param>
    public void ResolutionSetting(int number)
    {
        switch(number)
        {
            case 0: ScreenWidth = 1024; ScreenHeight = 768; break;
            case 1: ScreenWidth = 1366; ScreenHeight = 768; break;
            case 2: ScreenWidth = 1200; ScreenHeight = 900; break;
            case 3: ScreenWidth = 1600; ScreenHeight = 900; break;
            case 4: ScreenWidth = 1920; ScreenHeight = 1080; break;
        }
        SaveSettingsToFile();
    }
    /// <summary>
    /// Метод, завершающий программу
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
    /// <summary>
    /// Метод, сортирующий рекорды, оставляющй 10 лучших и записывающий их в файл
    /// </summary>
    static public void SortAndSaveRecords()
    {
        if (!Directory.Exists("Records"))
        {
            Directory.CreateDirectory("Records");
        }
        string path = @"Records/records" + LastLevel + ".ser"; 
        records[LastLevel - 2].Sort((a, b) => a._Record.CompareTo(b._Record));
        while (records[LastLevel - 2].Count > 10)
        {
            records[LastLevel - 2].RemoveAt(records[LastLevel - 2].Count - 1);
        }
        WriteFileB<List<Record>>(path, records[LastLevel - 2]);
    }
    /// <summary>
    /// Метод бинарной сериализации
    /// </summary>
    /// <typeparam name="T">Тип сериализуемого объекта</typeparam>
    /// <param name="path">Путь к файлу</param>
    /// <param name="q">Сериализумеый объект</param>
    static  void WriteFileB<T>(string path, T q)
    {       
        using (FileStream streamOut = new FileStream(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(streamOut, q);
            
        }
    }
    /// <summary>
    /// Метод бинарной десериализации
    /// </summary>
    /// <typeparam name="T">Тип десериализуемого объекта</typeparam>
    /// <param name="path">Путь к файлу</param>
    /// <param name="del">Делегат, применяемый к полученному объекту</param>
    static void DeserFileB<T>(string path, i_delegate<T> del)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream streamIn = new FileStream(path, FileMode.Open))
        {
            try
            {
                T info;
                info = (T)formatter.Deserialize(streamIn);
                del(info);
                streamIn.Close();
            }
            catch(Exception)
            {
                
            }            
        }
        

    }
}
