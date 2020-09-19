using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WeatherInfo : MonoBehaviour
{
	private int zipCode;
	public string appid; 
	public GameObject tempTextObject; 
	public GameObject humidityTextObject;
	public GameObject waterObject;
	public GameObject tempObject;
	public GameObject windObject;
	public GameObject windSpeedTextObject;
	public GameObject windDegreeTextObject;
	public GameObject weatherTextObject;
	public GameObject[] weatherIcons;
	private float temp = 0.0f;
	private float humidity = 0.0f;
	private float speed = 0.0f;
	private float degree = 0.0f;
	private string description = "";
	private string icon = "01d";
	private int currentActiveWeather = 0;
    // Start is called before the first frame update
    void Start()
    {
		zipCode = 60607;
        InvokeRepeating("startRepeating", 0f, 30f);
    }
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			decWeather();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			incWeather();
		}
		
	}
	void startRepeating()
	{
		StartCoroutine(UpdateWeather());
	}
	
	IEnumerator UpdateWeather()
	{
		using (UnityWebRequest request = UnityWebRequest.Get("api.openweathermap.org/data/2.5/weather?zip="+zipCode+",us&appid="+appid))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError) // Error
            {
                Debug.Log(request.error);
            }
            else // Success
            {
				parseJSON(request.downloadHandler.text);
            }
        }
    }
	
	void incWeather()
	{
		weatherIcons[currentActiveWeather].SetActive(false);
		
		if(currentActiveWeather == 8)
		{
			currentActiveWeather = 0;
		}
		else{
			currentActiveWeather++;
		}
		
		weatherIcons[currentActiveWeather].SetActive(true);
	}
	
	void decWeather()
	{
		weatherIcons[currentActiveWeather].SetActive(false);
		
		if(currentActiveWeather == 0)
		{
			currentActiveWeather = 8;
		}
		else{
			currentActiveWeather--;
		}
		
		weatherIcons[currentActiveWeather].SetActive(true);
		
	}

	void parseJSON(string input)
	{
		char[] delim = {',', '{','}',':'};
		string[] words = input.Split(delim);

		for (int i = 0; i < words.Length; i++)
		{	
			string word = words[i];
			if(word.Contains("temp") && !word.Contains("temp_"))
			{
				//(292.57K − 273.15) × 9/5 + 32 
				temp = (float.Parse(words[i+1]) -273.15f) * (9.0f/5.0f) + 32f;
			}
			else if(word.Contains("humidity"))
			{
				humidity = float.Parse(words[i+1]);
			}
			else if(word.Contains("speed")){
				speed = float.Parse(words[i+1]);
			}
			else if(word.Contains("deg"))
			{
				degree = float.Parse(words[i+1]);
			}
			else if(word.Contains("description"))
			{
				description = words[i+1];
			}
			else if(word.Contains("icon"))
			{
				icon = words[i+1];
			}
		}
		
		tempTextObject.GetComponent<TextMeshPro>().text = temp + "F";
		humidityTextObject.GetComponent<TextMeshPro>().text = humidity + "%";
		float waterScale = (humidity / 100.0f) * .04f;
		float tempScale = ((temp + 32) /180.0f) * .04f;
		waterObject.GetComponent<Transform>().localPosition = new Vector3(0f,waterScale,0.0282f);
		waterObject.GetComponent<Transform>().localScale = new Vector3(.055f,waterScale,0.055f);
		tempObject.GetComponent<Transform>().localPosition = new Vector3(0f,tempScale,-0.0293f);
		tempObject.GetComponent<Transform>().localScale = new Vector3(.01f,tempScale,0.01f);
		
		windSpeedTextObject.GetComponent<TextMeshPro>().text = speed + "m/s";
		windDegreeTextObject.GetComponent<TextMeshPro>().text = degree + "degrees";
		
		windObject.GetComponent<Transform>().localEulerAngles = new Vector3(0,degree,0);
		
		weatherTextObject.GetComponent<TextMeshPro>().text = description;
		
		weatherIcons[currentActiveWeather].SetActive(false);
		
		switch(icon)
		{
			case "01d":
			case "01n":
              currentActiveWeather = 0;
              break;
			case "02d":
			case "02n":
              currentActiveWeather = 1;
              break;
			case "03d":
			case "03n":
              currentActiveWeather = 2;
              break;
			case "04d":
			case "04n":
              currentActiveWeather = 3;
              break;
			case "09d":
			case "09n":
              currentActiveWeather = 4;
              break;
			case "10d":
			case "10n":
              currentActiveWeather = 5;
              break;
			case "11d":
			case "11n":
              currentActiveWeather = 6;
              break;
			case "13d":
			case "13n":
              currentActiveWeather = 7;
              break;
			case "50d":
			case "50n":
              currentActiveWeather = 8;
              break;
          default:
              break;
		}
		weatherIcons[currentActiveWeather].SetActive(true);
	}
	
}
