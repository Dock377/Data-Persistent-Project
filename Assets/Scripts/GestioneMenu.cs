using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GestioneMenu : MonoBehaviour
{
    public static GestioneMenu Richiesta;

    public Text ScrittaMiglioreGiocatore;
    public InputField ScriviNome;
    public string NomeGiocatore;

    public Button Inizio;
    public Button Esci;

    //dati migliori
    public string nomeMigliore;
    public int punteggioMigliore;

    private void Awake()
    {
        //distruggi altre copie
        if(Richiesta != null)
        {
            Destroy(gameObject);
            return;
        }

        //una sola copia
        Richiesta = this;
        DontDestroyOnLoad(gameObject);

        CaricaMigliore();
    }

    private void Start()
    {
        if(nomeMigliore != null && punteggioMigliore != 0)
        {
            ScrittaMiglioreGiocatore.text = $"Best Score : {nomeMigliore} : {punteggioMigliore}";
        }        
    }

    public void SceltaNome()
    {
        GestioneMenu.Richiesta.NomeGiocatore = ScriviNome.text;
    }

    public void InizioPartita()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif  
    }

    [System.Serializable]
    class SalvaDati
    {
        public string NomeMigliore;
        public int PunteggioMigliore;
    }

    public void CaricaMigliore()
    {
        string percorso = Application.persistentDataPath + "/migliorepunteggio.json";
        if (File.Exists(percorso))
        {
            string json = File.ReadAllText(percorso);
            SalvaDati dati = JsonUtility.FromJson<SalvaDati>(json);

            nomeMigliore = dati.NomeMigliore;
            punteggioMigliore = dati.PunteggioMigliore;
        }
    }
}
