using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт, отвечающий за подсчёт очков, ведение таймера и возможность поставить игру на паузу.
/// </summary>
public class ScoreScript : MonoBehaviour {

    /// <summary>
    /// Объект - панель паузы
    /// </summary>
    public GameObject PausePanel;
    /// <summary>
    /// Объект, объединяющий все кубики
    /// </summary>
    public GameObject CubeManager;
    /// <summary>
    /// Объект - кнопка паузы
    /// </summary>
    public GameObject PauseButton;
    /// <summary>
    /// Объект - панель завершения уровня
    /// </summary>
    public GameObject FinishPanel;
    /// <summary>
    /// Объект - текст завершения уровня
    /// </summary>
    public Text FinishText;
    /// <summary>
    /// Текущий счёт
    /// </summary>
    public static int score = 0;
    /// <summary>
    /// Текущий таймер
    /// </summary>
    public static float timer = 0;
    /// <summary>
    /// Миллисекунды таймера
    /// </summary>
    static string ms;
    /// <summary>
    /// Секунды таймера
    /// </summary>
    static string sec;
    /// <summary>
    /// Минуты таймера
    /// </summary>
    static string min;
    /// <summary>
    /// Текст, отображающий таймер и счёт
    /// </summary>
    Text text;  
    /// <summary>
    /// Флаг завершённости уровня
    /// </summary>
    bool finished;
    /// <summary>
    /// Метод инициализации, задающий стартовые значения
    /// </summary>
    void Start () {
        text = transform.GetComponent<Text>();
        PausePanel.SetActive(false);
        score = 0;
        timer = 0;
        finished = false;
        Time.timeScale = 1;
    }
    /// <summary>
    /// Метод загрузки главного меню
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    /// <summary>
    /// Метод паузы игры
    /// </summary>
    public void Pause()
    {
        CubeManager.GetComponent<ChangePlaces>().paused = !CubeManager.GetComponent<ChangePlaces>().paused;
        Time.timeScale= (Time.timeScale + 1) % 2;
        PausePanel.SetActive(!PausePanel.activeSelf);
        PauseButton.SetActive(!PauseButton.activeSelf);
    }
    /// <summary>
    /// Метод, отвечающий за корректное отображение таймера и текущего счёта, а также проверяющий, завершён ли уровень
    /// </summary>
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape) && !FinishPanel.activeSelf)
        {
            Pause();
        }
        else
        if (score != 6)
        {
            
            if (!CubeManager.GetComponent<ChangePlaces>().paused)
            {
                score = CubeManager.GetComponent<CompleteChecker>().Progress();


                timer += Time.deltaTime;
                ms = ((int)(100 * (timer - (int)timer))).ToString();
                sec = ((int)(timer % 60)).ToString();
                min = ((int)(timer / 60)).ToString();
                if (ms.Length == 1)
                    ms += "0";
                if (sec.Length == 1)
                    sec = "0" + sec;
                if (min.Length == 1)
                    min = "0" + min;
                min = min[0] + " " + min[1];
                sec = sec[0] + " " + sec[1];
                ms = ms[0] + " " + ms[1];

                text.text = "T i m e : " + min + " : " + sec+ " : " + ms + "\nS c o r e : " + score + " / 6";
            }
            
        }
        else if (!finished)
        {
            Record record = new Record(min + " : " + sec + " : " + ms, MenuScreenChanger.Name);
            MenuScreenChanger.records[MenuScreenChanger.LastLevel - 2].Add(record);
            MenuScreenChanger.SortAndSaveRecords();
            FinishText.text = "WELL DONE\nYour Time :\n" + min + " : " + sec + " : " + ms;
            FinishPanel.SetActive(true);
            finished = true;
        }
    }
}
