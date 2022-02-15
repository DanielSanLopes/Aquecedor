using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//using UnityEditor;  

public class TemperatureManager : MonoBehaviour
{
    public Image barrasCalor;                   //Recebe o objeto Image das barras de calor
   
    public TextMeshProUGUI display;             //Recebe o TextMeshPro do texto do display
    public AudioSource som;                     //Recebe o único AudioSorce em cena 
    public float degrees;                       //Define os graus da "temperatura" numa escala de 100, indo de 0 - 1. Define as cores das barras, o volume e o texto do display

    public Button[] botoes;                     //Recebe os botões de adicionar e subtrair temperatura

    private float g;
    private int G;

     
    private AndroidJavaClass vibrate;           //Objeto resposável por vibrar dispositivos Android
    private AndroidJavaObject vib;

    // Start is called before the first frame update


    void Start()
    {      
        
        if(PlayerPrefs.GetInt("instalado") == 1) {                  //Se o programa foi instalado...

            degrees = PlayerPrefs.GetFloat("degrees");              //Recebe os graus salvos pelo PlayerPrefs
            som.volume = PlayerPrefs.GetFloat("i");                 //Recebe o volume salvo nas corrotinas
            display.text = PlayerPrefs.GetString("display") ;       //Recebe o texto salvo nas corrotinas

            //Recebe o valor das cores salvo nas corrotinas
            barrasCalor.color = new Color(PlayerPrefs.GetFloat("i"), PlayerPrefs.GetFloat("i"), PlayerPrefs.GetFloat("i"));


        } else {

            PlayerPrefs.SetInt("instalado", 1);                     //Diz que o programa está instalado

            degrees = 0;                                            //Os graus ficam em 0
            som.volume = degrees;                                   //O volume recebe os graus
            display.text = degrees.ToString() + "º";                //O texto do display recebe os graus
            barrasCalor.color = new Color(0, 0, 0);                 //As cores recebem 0 numa escala de 100 (0 -1)
            PlayerPrefs.SetFloat("degrees", degrees);               //Salva o valor dos graus
        }

        
        //display.text = degrees.ToString() + "º";
        
    }

    // Update is called once per frame
    void Update()
    {

        if(degrees > 0.9f) {                    //Se a temperatura superar os 90 graus
            Vibrador();
        } 
        
        if(degrees < 0) {                       //Se a temperatura chegar a ser menor que 0
            display.text = "0º";                //O texto do display recebe 0
            degrees = 0;                        //Os graus si igualam a 0

        }
       
    }

    private void Vibrador() {
        Handheld.Vibrate();                 //Vibrar supostamente todo dispositivo vibratório, não sei, pq a merda do meu controle não vibrou porra nenhuma      
        //vibrate.Call("Vibrate");            //Vibrar dispositivo Android

    }

    public void AddTemperature(float graus) {

        g = degrees;

        if (degrees <= 1) {
            degrees += graus;
            StartCoroutine("AddTemp");
        }

    }

    public void SubTemperature(float graus) {

        g = degrees;

        if(degrees > 0) {
            degrees -= graus;

            StartCoroutine("SubTemp");
        }
    }

    IEnumerator AddTemp() {

        botoes[0].interactable = false;
        botoes[1].interactable = false;

        for (float i = g; i < degrees; i += 0.01f) {

            PlayerPrefs.SetFloat("i", i);

            barrasCalor.color = new Color(i, i, i);
            som.volume = i;
            G = (int) (i * 100);
            display.text = G.ToString() + "º";
            PlayerPrefs.SetString("display", display.text);

            yield return new WaitForSeconds(0.05f);
        }
        botoes[0].interactable = true;
        botoes[1].interactable = true;
        PlayerPrefs.SetFloat("degrees", degrees);
        StopCoroutine("AddTemp");
    }

    IEnumerator SubTemp() {
        botoes[1].interactable = false;
        botoes[0].interactable = false;
        for (float i = g; i > degrees; i -= 0.01f) {

            PlayerPrefs.SetFloat("i", i);

            barrasCalor.color = new Color(i, i, i);
            som.volume = i;
            G = (int) (i * 100);
                        
            display.text = G.ToString() + "º";
            PlayerPrefs.SetString("display", display.text);

            yield return new WaitForSeconds(0.05f);
        }
        botoes[1].interactable = true;
        botoes[0].interactable = true;
        PlayerPrefs.SetFloat("degrees", degrees);
        StopCoroutine("SubTemp");
    }
}
