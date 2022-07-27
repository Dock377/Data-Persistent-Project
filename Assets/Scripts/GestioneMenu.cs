using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GestioneMenu : MonoBehaviour
{
    public static GestioneMenu Richiesta;

    public Text MigliorePunteggio;
    public InputField ScriviNome;
    public string NomeGiocatore;

    public Button Inizio;
    public Button Esci;

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
}
