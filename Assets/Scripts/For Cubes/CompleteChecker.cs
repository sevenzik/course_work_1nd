using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт, отвечающий проверку собранной картинки,
/// анимацию кубиков в процессе сборки и отметку собранных картинок.
/// </summary>
public class CompleteChecker : MonoBehaviour {

    /// <summary>
    /// Набор векторов, описывающий все возможные позиции кубика с гранями, ориентированным к пользователю
    /// </summary>
    static Vector3[,] CubePositions = new Vector3[4,6];
    /// <summary>
    /// Генератор случайных чисел
    /// </summary>
    static System.Random rnd;
    /// <summary>
    /// Объект - текст на кнопке сборки/разборки кубиков
    /// </summary>
    public Text CompleteButtonText;
    /// <summary>
    /// Массив кубиков уровня
    /// </summary>
    public Transform[] cubes;
    /// <summary>
    /// Объект - панель игрового табло
    /// </summary>
    public GameObject ImageBoard;
    /// <summary>
    /// Массив "решений" игры, с которым сверяется проверка сборки
    /// </summary>
    public CubeInfo[][] cubeInfo;
    /// <summary>
    /// Флаг, отвечающий за активность процесса сближения кубиков
    /// </summary>
    public bool complete;
    /// <summary>
    /// Флаг, отвечающий за сдвинутое состояние кубиков
    /// </summary>
    public bool glue;
    /// <summary>
    /// Массив, отслеживающий прогресс уровня
    /// </summary>
    public bool[]  completes = new bool[6];
    /// <summary>
    /// Номер завершённого изображения
    /// </summary>
    int p;
    /// <summary>
    /// Вектора, задающие движение кубиков на различных уровнях
    /// </summary>
    Vector3[][] usualVectors = { new Vector3[]{ new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, 0.5f), new Vector3(-0.5f, 0, -0.5f), new Vector3(0.5f, 0, -0.5f) },
                                 new Vector3[]{ new Vector3(-0.35f, 0, 0.35f), new Vector3(0, 0, 0.35f), new Vector3(0.35f, 0, 0.35f), new Vector3(-0.35f, 0, 0),
                                     new Vector3(0, 0, 0), new Vector3(0.35f, 0, 0), new Vector3(-0.35f, 0, -0.35f), new Vector3(0, 0, -0.35f),new Vector3(0.35f, 0, -0.35f) },
                                 new Vector3[]{  new Vector3(-0.6f, 0, 0.6f), new Vector3(-0.2f, 0, 0.6f), new Vector3(0.2f, 0, 0.6f), new Vector3(0.6f, 0, 0.6f),
                                                 new Vector3(-0.6f, 0, 0.2f), new Vector3(-0.2f, 0, 0.2f), new Vector3(0.2f, 0, 0.2f), new Vector3(0.6f, 0, 0.2f),
                                                 new Vector3(-0.6f, 0, -0.2f), new Vector3(-0.2f, 0, -0.2f), new Vector3(0.2f, 0, -0.2f), new Vector3(0.6f, 0, -0.2f),
                                                 new Vector3(-0.6f, 0, -0.6f), new Vector3(-0.2f, 0, -0.6f), new Vector3(0.2f, 0, -0.6f), new Vector3(0.6f, 0, -0.6f)}
                                 };
    /// <summary>
    /// Метод инициализации, задающий стартовые значения уровня и случайно располагающий и поворачивающий кубики
    /// </summary>
    void Start () {
        rnd = new System.Random();
        complete = false;
        p = -1;
        glue = false;
        for (int i = 0; i < completes.Length; i++)
            completes[i] = false;

        CubePositions[0, 0] = new Vector3(0, 0, 0);
        CubePositions[0, 1] = new Vector3(270, 0, 0);
        CubePositions[0, 2] = new Vector3(0, 180, 180);
        CubePositions[0, 3] = new Vector3(90, 180, 0);
        CubePositions[0, 4] = new Vector3(0,270, 90);
        CubePositions[0, 5] = new Vector3(0, 90, 270);
        CubePositions[1, 0] = new Vector3(0, 90, 0);
        CubePositions[1, 1] = new Vector3(270, 90, 0);
        CubePositions[1, 2] = new Vector3(0, 270, 180);
        CubePositions[1, 3] = new Vector3(90, 270, 0);
        CubePositions[1, 4] = new Vector3(0, 0, 90);
        CubePositions[1, 5] = new Vector3(0, 180, 270);
        CubePositions[2, 0] = new Vector3(0, 180, 0);
        CubePositions[2, 1] = new Vector3(270, 180, 0);
        CubePositions[2, 2] = new Vector3(0, 0, 180);
        CubePositions[2, 3] = new Vector3(90, 0, 0);
        CubePositions[2, 4] = new Vector3(0, 90, 90);
        CubePositions[2, 5] = new Vector3(0, 270, 270);
        CubePositions[3, 0] = new Vector3(0, 270, 0);
        CubePositions[3, 1] = new Vector3(270, 270, 0);
        CubePositions[3, 2] = new Vector3(0, 90, 180);
        CubePositions[3, 3] = new Vector3(90, 90, 0);
        CubePositions[3, 4] = new Vector3(0, 180, 90);
        CubePositions[3, 5] = new Vector3(0, 0, 270);

        FindChilds();
        
        int n = 6;
        cubeInfo = new CubeInfo[n][];
        for (int i = 0; i < n; i++)
        {
            cubeInfo[i] = new  CubeInfo[transform.childCount];
        }

        CreateKeys();

        for(int i = 0; i < cubes.Length; i++)
        {
            cubes[i].eulerAngles = CubePositions[rnd.Next(4), rnd.Next(6)];
        }
        for(int i = 0; i < cubes.Length * 4; i++)
        {
            transform.GetComponent<ChangePlaces>().Change(cubes[rnd.Next(cubes.Length)]);
        }
    }
    /// <summary>
    /// Метод, получающий информацию о кубиках уровня
    /// </summary>
    void FindChilds()
    {
        cubes = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform t in transform)
        {
            cubes[i++] = t;
        }
    }
    /// <summary>
    /// Метод, задающий "решения" уровня
    /// </summary>
    public void CreateKeys()
    {
        if (MenuScreenChanger.LastLevel == 2)
        {
            cubeInfo[0][0] = new CubeInfo(CubePositions[0, 2], 1);
            cubeInfo[0][1] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[0][2] = new CubeInfo(CubePositions[0, 2], 2);
            cubeInfo[0][3] = new CubeInfo(CubePositions[0, 4], 4);

            cubeInfo[1][0] = new CubeInfo(CubePositions[0, 5], 2);
            cubeInfo[1][1] = new CubeInfo(CubePositions[0, 0], 1);
            cubeInfo[1][2] = new CubeInfo(CubePositions[0, 0], 4);
            cubeInfo[1][3] = new CubeInfo(CubePositions[0, 2], 3);

            cubeInfo[2][0] = new CubeInfo(CubePositions[0, 4], 3);
            cubeInfo[2][1] = new CubeInfo(CubePositions[0, 1], 4);
            cubeInfo[2][2] = new CubeInfo(CubePositions[0, 1], 1);
            cubeInfo[2][3] = new CubeInfo(CubePositions[0, 0], 2);

            cubeInfo[3][0] = new CubeInfo(CubePositions[0, 1], 4);
            cubeInfo[3][1] = new CubeInfo(CubePositions[0, 4], 2);
            cubeInfo[3][2] = new CubeInfo(CubePositions[0, 5], 1);
            cubeInfo[3][3] = new CubeInfo(CubePositions[0, 5], 3);

            cubeInfo[4][0] = new CubeInfo(CubePositions[0, 0], 2);
            cubeInfo[4][1] = new CubeInfo(CubePositions[0, 5], 1);
            cubeInfo[4][2] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[4][3] = new CubeInfo(CubePositions[0, 3], 4);

            cubeInfo[5][0] = new CubeInfo(CubePositions[0, 3], 4);
            cubeInfo[5][1] = new CubeInfo(CubePositions[0, 2], 3);
            cubeInfo[5][2] = new CubeInfo(CubePositions[0, 4], 2);
            cubeInfo[5][3] = new CubeInfo(CubePositions[0, 1], 1);
        }
        if (MenuScreenChanger.LastLevel == 3)
        {
            cubeInfo[0][0] = new CubeInfo(CubePositions[0, 1], 1);
            cubeInfo[0][1] = new CubeInfo(CubePositions[0, 1], 2);
            cubeInfo[0][2] = new CubeInfo(CubePositions[0, 2], 3);
            cubeInfo[0][3] = new CubeInfo(CubePositions[0, 0], 4);
            cubeInfo[0][4] = new CubeInfo(CubePositions[0, 4], 5);
            cubeInfo[0][5] = new CubeInfo(CubePositions[0, 2], 6);
            cubeInfo[0][6] = new CubeInfo(CubePositions[0, 2], 7);
            cubeInfo[0][7] = new CubeInfo(CubePositions[0, 1], 8);
            cubeInfo[0][8] = new CubeInfo(CubePositions[0, 5], 9);

            cubeInfo[1][0] = new CubeInfo(CubePositions[0, 2], 8);
            cubeInfo[1][1] = new CubeInfo(CubePositions[0, 4], 6);
            cubeInfo[1][2] = new CubeInfo(CubePositions[0, 3], 9);
            cubeInfo[1][3] = new CubeInfo(CubePositions[0, 3], 4);
            cubeInfo[1][4] = new CubeInfo(CubePositions[0, 5], 7);
            cubeInfo[1][5] = new CubeInfo(CubePositions[0, 0], 2);
            cubeInfo[1][6] = new CubeInfo(CubePositions[0, 3], 5);
            cubeInfo[1][7] = new CubeInfo(CubePositions[0, 0], 3);
            cubeInfo[1][8] = new CubeInfo(CubePositions[0, 4], 1);

            cubeInfo[2][0] = new CubeInfo(CubePositions[0, 4], 7);
            cubeInfo[2][1] = new CubeInfo(CubePositions[0, 2], 2);
            cubeInfo[2][2] = new CubeInfo(CubePositions[0, 0], 6);
            cubeInfo[2][3] = new CubeInfo(CubePositions[0, 4], 5);
            cubeInfo[2][4] = new CubeInfo(CubePositions[0, 1], 1);
            cubeInfo[2][5] = new CubeInfo(CubePositions[0, 3], 8);
            cubeInfo[2][6] = new CubeInfo(CubePositions[0, 1], 4);
            cubeInfo[2][7] = new CubeInfo(CubePositions[0, 3], 9);
            cubeInfo[2][8] = new CubeInfo(CubePositions[0, 0], 3);

            cubeInfo[3][0] = new CubeInfo(CubePositions[0, 5], 5);
            cubeInfo[3][1] = new CubeInfo(CubePositions[0, 0], 6);
            cubeInfo[3][2] = new CubeInfo(CubePositions[0, 5], 4);
            cubeInfo[3][3] = new CubeInfo(CubePositions[0, 1], 7);
            cubeInfo[3][4] = new CubeInfo(CubePositions[0, 0], 1);
            cubeInfo[3][5] = new CubeInfo(CubePositions[0, 1], 8);
            cubeInfo[3][6] = new CubeInfo(CubePositions[0, 5], 9);
            cubeInfo[3][7] = new CubeInfo(CubePositions[0, 2], 3);
            cubeInfo[3][8] = new CubeInfo(CubePositions[0, 2], 2);

            cubeInfo[4][0] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[4][1] = new CubeInfo(CubePositions[0, 5], 1);
            cubeInfo[4][2] = new CubeInfo(CubePositions[0, 1], 7);
            cubeInfo[4][3] = new CubeInfo(CubePositions[0, 2], 8);
            cubeInfo[4][4] = new CubeInfo(CubePositions[0, 2], 2);
            cubeInfo[4][5] = new CubeInfo(CubePositions[0, 4], 9);
            cubeInfo[4][6] = new CubeInfo(CubePositions[0, 0], 6);
            cubeInfo[4][7] = new CubeInfo(CubePositions[0, 4], 4);
            cubeInfo[4][8] = new CubeInfo(CubePositions[0, 1], 5);

            cubeInfo[5][0] = new CubeInfo(CubePositions[0, 0], 3);
            cubeInfo[5][1] = new CubeInfo(CubePositions[0, 3], 9);
            cubeInfo[5][2] = new CubeInfo(CubePositions[0, 4], 4);
            cubeInfo[5][3] = new CubeInfo(CubePositions[0, 5], 5);
            cubeInfo[5][4] = new CubeInfo(CubePositions[0, 3], 8);
            cubeInfo[5][5] = new CubeInfo(CubePositions[0, 5], 7);
            cubeInfo[5][6] = new CubeInfo(CubePositions[0, 4], 1);
            cubeInfo[5][7] = new CubeInfo(CubePositions[0, 5], 2);
            cubeInfo[5][8] = new CubeInfo(CubePositions[0, 3], 6);
        }
        if (MenuScreenChanger.LastLevel == 4)
        {
            cubeInfo[0][0] = new CubeInfo(CubePositions[0, 0], 1);
            cubeInfo[0][1] = new CubeInfo(CubePositions[0, 5], 2);
            cubeInfo[0][2] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[0][3] = new CubeInfo(CubePositions[0, 1], 4);
            cubeInfo[0][4] = new CubeInfo(CubePositions[0, 0], 5);
            cubeInfo[0][5] = new CubeInfo(CubePositions[0, 1], 6);
            cubeInfo[0][6] = new CubeInfo(CubePositions[0, 5], 7);
            cubeInfo[0][7] = new CubeInfo(CubePositions[0, 3], 8);
            cubeInfo[0][8] = new CubeInfo(CubePositions[0, 5], 9);
            cubeInfo[0][9] = new CubeInfo(CubePositions[0, 1], 10);
            cubeInfo[0][10] = new CubeInfo(CubePositions[0, 2], 11);
            cubeInfo[0][11] = new CubeInfo(CubePositions[0, 3], 12);
            cubeInfo[0][12] = new CubeInfo(CubePositions[0, 1], 13);
            cubeInfo[0][13] = new CubeInfo(CubePositions[0, 3], 14);
            cubeInfo[0][14] = new CubeInfo(CubePositions[0, 0], 15);
            cubeInfo[0][15] = new CubeInfo(CubePositions[0, 5], 16);

            cubeInfo[1][0] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[1][1] = new CubeInfo(CubePositions[0, 4], 12);
            cubeInfo[1][2] = new CubeInfo(CubePositions[0, 0], 6);
            cubeInfo[1][3] = new CubeInfo(CubePositions[0, 4], 11);
            cubeInfo[1][4] = new CubeInfo(CubePositions[0, 3], 2);
            cubeInfo[1][5] = new CubeInfo(CubePositions[0, 2], 16);
            cubeInfo[1][6] = new CubeInfo(CubePositions[0, 4], 10);
            cubeInfo[1][7] = new CubeInfo(CubePositions[0, 0], 4);
            cubeInfo[1][8] = new CubeInfo(CubePositions[0, 2], 15);
            cubeInfo[1][9] = new CubeInfo(CubePositions[0, 4], 1);
            cubeInfo[1][10] = new CubeInfo(CubePositions[0, 3], 7);
            cubeInfo[1][11] = new CubeInfo(CubePositions[0, 5], 9);
            cubeInfo[1][12] = new CubeInfo(CubePositions[0, 0], 13);
            cubeInfo[1][13] = new CubeInfo(CubePositions[0, 1], 5);
            cubeInfo[1][14] = new CubeInfo(CubePositions[0, 4], 8);
            cubeInfo[1][15] = new CubeInfo(CubePositions[0, 2], 14);

            cubeInfo[2][0] = new CubeInfo(CubePositions[0, 4], 6);
            cubeInfo[2][1] = new CubeInfo(CubePositions[0, 1], 7);
            cubeInfo[2][2] = new CubeInfo(CubePositions[0, 4], 5);
            cubeInfo[2][3] = new CubeInfo(CubePositions[0, 2], 14);
            cubeInfo[2][4] = new CubeInfo(CubePositions[0, 1], 12);
            cubeInfo[2][5] = new CubeInfo(CubePositions[0, 4], 15);
            cubeInfo[2][6] = new CubeInfo(CubePositions[0, 0], 4);
            cubeInfo[2][7] = new CubeInfo(CubePositions[0, 1], 11);
            cubeInfo[2][8] = new CubeInfo(CubePositions[0, 4], 13);
            cubeInfo[2][9] = new CubeInfo(CubePositions[0, 3], 10);
            cubeInfo[2][10] = new CubeInfo(CubePositions[0, 1], 3);
            cubeInfo[2][11] = new CubeInfo(CubePositions[0, 2], 2);
            cubeInfo[2][12] = new CubeInfo(CubePositions[0, 4], 1);
            cubeInfo[2][13] = new CubeInfo(CubePositions[0, 0], 16);
            cubeInfo[2][14] = new CubeInfo(CubePositions[0, 1], 9);
            cubeInfo[2][15] = new CubeInfo(CubePositions[0, 3], 8);

            cubeInfo[3][0] = new CubeInfo(CubePositions[0, 5], 5);
            cubeInfo[3][1] = new CubeInfo(CubePositions[0, 0], 11);
            cubeInfo[3][2] = new CubeInfo(CubePositions[0, 5], 15);
            cubeInfo[3][3] = new CubeInfo(CubePositions[0, 3], 10);
            cubeInfo[3][4] = new CubeInfo(CubePositions[0, 4], 4);
            cubeInfo[3][5] = new CubeInfo(CubePositions[0, 3], 14);
            cubeInfo[3][6] = new CubeInfo(CubePositions[0, 1], 13);
            cubeInfo[3][7] = new CubeInfo(CubePositions[0, 2], 9);
            cubeInfo[3][8] = new CubeInfo(CubePositions[0, 0], 16);
            cubeInfo[3][9] = new CubeInfo(CubePositions[0, 2], 3);
            cubeInfo[3][10] = new CubeInfo(CubePositions[0, 4], 6);
            cubeInfo[3][11] = new CubeInfo(CubePositions[0, 0], 8);
            cubeInfo[3][12] = new CubeInfo(CubePositions[0, 3], 12);
            cubeInfo[3][13] = new CubeInfo(CubePositions[0, 2], 7);
            cubeInfo[3][14] = new CubeInfo(CubePositions[0, 5], 2);
            cubeInfo[3][15] = new CubeInfo(CubePositions[0, 0], 1);

            cubeInfo[4][0] = new CubeInfo(CubePositions[0, 1], 16);
            cubeInfo[4][1] = new CubeInfo(CubePositions[0, 2], 10);
            cubeInfo[4][2] = new CubeInfo(CubePositions[0, 1], 7);
            cubeInfo[4][3] = new CubeInfo(CubePositions[0, 5], 12);
            cubeInfo[4][4] = new CubeInfo(CubePositions[0, 2], 11);
            cubeInfo[4][5] = new CubeInfo(CubePositions[0, 0], 15);
            cubeInfo[4][6] = new CubeInfo(CubePositions[0, 3], 1);
            cubeInfo[4][7] = new CubeInfo(CubePositions[0, 5], 2);
            cubeInfo[4][8] = new CubeInfo(CubePositions[0, 3], 3);
            cubeInfo[4][9] = new CubeInfo(CubePositions[0, 5], 13);
            cubeInfo[4][10] = new CubeInfo(CubePositions[0, 0], 8);
            cubeInfo[4][11] = new CubeInfo(CubePositions[0, 1], 5);
            cubeInfo[4][12] = new CubeInfo(CubePositions[0, 5], 4);
            cubeInfo[4][13] = new CubeInfo(CubePositions[0, 4], 14);
            cubeInfo[4][14] = new CubeInfo(CubePositions[0, 3], 9);
            cubeInfo[4][15] = new CubeInfo(CubePositions[0, 1], 6);

            cubeInfo[5][0] = new CubeInfo(CubePositions[0, 2], 3);
            cubeInfo[5][1] = new CubeInfo(CubePositions[0, 3], 4);
            cubeInfo[5][2] = new CubeInfo(CubePositions[0, 2], 8);
            cubeInfo[5][3] = new CubeInfo(CubePositions[0, 0], 2);
            cubeInfo[5][4] = new CubeInfo(CubePositions[0, 5], 13);
            cubeInfo[5][5] = new CubeInfo(CubePositions[0, 5], 12);
            cubeInfo[5][6] = new CubeInfo(CubePositions[0, 2], 7);
            cubeInfo[5][7] = new CubeInfo(CubePositions[0, 4], 14);
            cubeInfo[5][8] = new CubeInfo(CubePositions[0, 1], 11);
            cubeInfo[5][9] = new CubeInfo(CubePositions[0, 0], 6);
            cubeInfo[5][10] = new CubeInfo(CubePositions[0, 5], 15);
            cubeInfo[5][11] = new CubeInfo(CubePositions[0, 4], 5);
            cubeInfo[5][12] = new CubeInfo(CubePositions[0, 2], 10);
            cubeInfo[5][13] = new CubeInfo(CubePositions[0, 5], 16);
            cubeInfo[5][14] = new CubeInfo(CubePositions[0, 2], 9);
            cubeInfo[5][15] = new CubeInfo(CubePositions[0, 4], 1);
        }
    }
    /// <summary>
    /// Метод, считающий количество завершённых изображений
    /// </summary>
    /// <returns>Количество завершённых изображений</returns>
    public int Progress()
    {
        int c = 0;
        for (int i = 0; i < completes.Length; i++)
            if (completes[i])
                c++;
        return c;
    }
    /// <summary>
    /// Метод, срабатывающий при нажатии на кнопку сборки/разборки кубиков
    /// </summary>
    public void CheckComplete()
    {
        if (!glue)
        {
            bool checker;
            for (int i = 0; i < cubeInfo.Length; i++)
            {
                checker = true;
                for (int j = 0; j < cubeInfo[i].Length && checker; j++)
                {
                    CubeInfo tmp = new CubeInfo(cubes[j].eulerAngles, cubes[j].GetComponent<Changer>().number);
                    checker = (tmp.position == cubeInfo[i][j].position) && (tmp.place == cubeInfo[i][j].place) && (!completes[i]);
                }
                        
                if (checker)
                {
                    p = i;
                    i = cubeInfo.Length;
                }
            }
            complete = true;
            CompleteButtonText.text = "Divide";
        }
        else if (glue)
        {
            complete = false;
            CompleteButtonText.text = "Complete";
        }
    }
    /// <summary>
    /// Метод, вызывающий анимацию движения кубиков
    /// </summary>
    void Update() {
        if (!transform.GetComponent<ChangePlaces>().paused)
        {
            if (complete)
            {
                Array.Sort(cubes, (Transform a, Transform b) => a.GetComponent<Changer>().number - b.GetComponent<Changer>().number);

                Vector3[] change;

                if ((cubes[0].transform.position.x > 2.4 + transform.position.x && MenuScreenChanger.LastLevel == 4)
                    || (cubes[0].transform.position.x > 1.5 + transform.position.x  &&  MenuScreenChanger.LastLevel == 2) ||
                    ((cubes[0].transform.position.x > 1.7 + transform.position.x && MenuScreenChanger.LastLevel == 3)))
                {
                    change = new Vector3[cubes.Length];
                    for (int i = 0; i < cubes.Length; i++)
                    {
                        change[i] = usualVectors[MenuScreenChanger.LastLevel - 2][i];
                    }

                    CreateVectors(change);

                    for (int i = 0; i < cubes.Length; i++)
                    {
                        cubes[i].Translate(change[i] * Time.deltaTime);
                    }
                }
                else
                {
                    glue = true;
                    if (p >= 0 && p <= 5)
                    {
                        ImageBoard.GetComponent<AtStartImageBoard>().ImageComplete(p);
                        completes[p] = true;
                    }

                }
            }

            if (!complete && glue)
            {
                Vector3[] change;

                if ((cubes[0].transform.position.x < 3 + transform.position.x && MenuScreenChanger.LastLevel == 4)
                    || (cubes[0].transform.position.x < 2 + transform.position.x && MenuScreenChanger.LastLevel == 2) ||
                    ((cubes[0].transform.position.x < 2 + transform.position.x && MenuScreenChanger.LastLevel == 3)))
                {
                    change = new Vector3[cubes.Length];
                    for (int i = 0; i < cubes.Length; i++)
                    {
                        change[i] = -usualVectors[MenuScreenChanger.LastLevel-2][i];
                    }
                    CreateVectors(change);

                    for (int i = 0; i < cubes.Length; i++)
                    {
                        cubes[i].Translate(change[i] * Time.deltaTime);
                    }

                }
                else
                {
                    FindChilds();
                    glue = false;
                    
                }
            }
        }     
    }
    /// <summary>
    /// Метод, трансформирующий набор векторов в соответствиями с вращениями кубиков
    /// </summary>
    /// <param name="change">Исходный набор векторов</param>
    /// <returns>Выходной набор векторов</returns>
    Vector3[] CreateVectors(Vector3[] change)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            
            if (cubes[i].eulerAngles == CubePositions[0, 0])
            {

            }
            if (cubes[i].eulerAngles == CubePositions[0, 1])
            {
                change[i].y = -change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[0, 2])
            {
                change[i].z = -change[i].z;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[0, 3])
            {
                change[i].y = -change[i].z;
                change[i].z = 0;
                change[i].x *= -1;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[0, 4])
            {
                change[i].y = -change[i].z;
                change[i].z = -change[i].x;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[0, 5])
            {
                change[i].y = -change[i].z;
                change[i].z = change[i].x;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 0])
            {
                float tmp = change[i].z;
                change[i].z = change[i].x;
                change[i].x = -tmp;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 1])
            {
                change[i].y = -change[i].x;
                change[i].x = -change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 2])
            {
                float tmp = change[i].z;
                change[i].z = -change[i].x;
                change[i].x = -tmp;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 3])
            {
                change[i].y = -change[i].x;
                change[i].x = change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 4])
            {
                change[i].y = -change[i].x;
                change[i].z = change[i].z;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[1, 5])
            {
                change[i].y = -change[i].x;
                change[i].z = -change[i].z;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 0])
            {
                change[i].z = -change[i].z;
                change[i].x = -change[i].x;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 1])
            {
                change[i].y = change[i].z;
                change[i].x = -change[i].x;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 2])
            {
                change[i].x = -change[i].x;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 3])
            {
                change[i].y = change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 4])
            {
                change[i].y = change[i].z;
                change[i].z = change[i].x;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[2, 5])
            {
                change[i].y = change[i].z;
                change[i].z = -change[i].x;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 0])
            {
                float tmp = change[i].z;
                change[i].z = -change[i].x;
                change[i].x = tmp;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 1])
            {
                change[i].y = change[i].x;
                change[i].x = change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 2])
            {
                float tmp = change[i].z;
                change[i].z = change[i].x;
                change[i].x = tmp;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 3])
            {
                change[i].y = change[i].x;
                change[i].x = -change[i].z;
                change[i].z = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 4])
            {
                change[i].y = change[i].x;
                change[i].z = -change[i].z;
                change[i].x = 0;
            }
            else
            if (cubes[i].eulerAngles == CubePositions[3, 5])
            {
                change[i].y = change[i].x;
                change[i].z = change[i].z;
                change[i].x = 0;
            }
        }
        
        return change;
    }
}
