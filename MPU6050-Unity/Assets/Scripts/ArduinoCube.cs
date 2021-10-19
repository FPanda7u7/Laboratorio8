using UnityEngine;
using System.Collections;
using System;
using System.IO.Ports;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ArduinoCube : MonoBehaviour
{
    bool enable = true;
    bool reiniciar = false; 
    string boton;

    public Text titulo;
    public Text contador;
    public Text titulo2;
    public Text contador2;

    float tiempo;
    
    SerialPort serialPort;

    public GameObject target;

    float gyro_normalizer_factor = 1.0f / 32768.0f;

    float curr_angle_x = 0;
    float curr_angle_y = 0;
    float curr_angle_z = 0;

    public float factor = 7;

    void Start()
    {     
        serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        serialPort.Open();
        //serialPort.ReadTimeout = 1000;

        tiempo = 0.0f;
        Time.timeScale = 0;

        titulo.enabled = true;
        contador.enabled = false;
        titulo2.enabled = false;
        contador2.enabled = false;
    }

    void Update()
    {           
        if (!serialPort.IsOpen)
            serialPort.Open();

        if (serialPort.IsOpen)
        {
            string dataString = serialPort.ReadLine();
            char splitChar = ';';
            string[] dataRaw = dataString.Split(splitChar);

            boton = dataRaw[3];

            if (boton == "1" && enable)
            {
                titulo.enabled = false;
                contador.enabled = true;
                Time.timeScale = 1;
                enable = false;
            }

            if (Time.timeScale == 1)
            {
                tiempo += Time.deltaTime;
                contador.text = tiempo.ToString();

                //contador.text = Time.time.ToString();

                float gx = float.Parse(dataRaw[0]) * gyro_normalizer_factor;
                float gy = float.Parse(dataRaw[1]) * gyro_normalizer_factor;
                float gz = float.Parse(dataRaw[2]) * gyro_normalizer_factor;

                if (Mathf.Abs(gx) < 0.025f) gx = 0f;
                if (Mathf.Abs(gy) < 0.025f) gy = 0f;
                if (Mathf.Abs(gz) < 0.025f) gz = 0f;

                curr_angle_x += gx;
                curr_angle_y += gy;
                curr_angle_z += gz;

                target.transform.rotation = Quaternion.Euler(curr_angle_y * factor, -curr_angle_z * factor, -curr_angle_x * factor);
            }

            if (reiniciar)
            {
                titulo2.enabled = true;
                contador2.enabled = true;
                contador2.text = contador.text;
                Time.timeScale = 0;
            }

            if (boton == "1" && reiniciar)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        reiniciar = true;
    }
}
