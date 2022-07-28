using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text MigliorPunteggio;
    
    private bool m_Started = false;
    private int m_Points;
    public int m_BestPoints = 0;
    
    
    private bool m_GameOver = false;
    public string m_Nome;
    public string m_NomeMigliore;

    private void Awake()
    {
        CaricaMigliore();
    }
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        MigliorPunteggio.text = $"Best Score : {m_NomeMigliore} : {m_BestPoints}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        //MigliorPunteggio.text = $"Best Score : {m_NomeMigliore} : {m_BestPoints}";
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        //Scrivere il punteggio migliore
        m_Nome = GestioneMenu.Richiesta.NomeGiocatore;
        if (m_Points > m_BestPoints)
        {
            m_BestPoints = m_Points;
            m_NomeMigliore = m_Nome;
            MigliorPunteggio.text = $"Best Score : {m_NomeMigliore} : {m_BestPoints}";
            SalvaGiocatore();
        }
    }

    [System.Serializable]
    class SalvaDati
    {
        public string NomeMigliore;
        public int PunteggioMigliore;
    }

    public void SalvaGiocatore()
    {
        SalvaDati dati = new SalvaDati();
        dati.NomeMigliore = m_NomeMigliore;
        dati.PunteggioMigliore = m_BestPoints;

        string json = JsonUtility.ToJson(dati);

        File.WriteAllText(Application.persistentDataPath + "/migliorepunteggio.json", json);
    }

    public void CaricaMigliore()
    {
        string percorso = Application.persistentDataPath + "/migliorepunteggio.json";
        if (File.Exists(percorso))
        {
            string json = File.ReadAllText(percorso);
            SalvaDati dati = JsonUtility.FromJson<SalvaDati>(json);

            m_NomeMigliore = dati.NomeMigliore;
            m_BestPoints = dati.PunteggioMigliore;
        }
    }
}
