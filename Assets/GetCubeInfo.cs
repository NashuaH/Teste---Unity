using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public class GetCubeInfo : MonoBehaviour
{
    public string info;

    public List<float> infoList = new List<float>();

    public List<Color> colorList = new List<Color>();
    public List<float> percentageList = new List<float>();

    public string currentInfo, savedInfo;
    Color color;
    public GameObject startButton;

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://hotelapi.eastus.cloudapp.azure.com/")) 
        {
            yield return www.SendWebRequest();



            //   byte[] results = www.downloadHandler.data;
            //   info = System.Text.Encoding.UTF8.GetString(results);

            info = www.downloadHandler.text;

        }

        ReadInfo(info);
        startButton.SetActive(true);

        /*  for (int i = 0; i < colorList.Count; i++)
          {

              savedInfo = savedInfo + "{ \"randomColor\":\"#"+ "#" + ColorUtility.ToHtmlStringRGB(colorList[i]) + "\",\"percentage\":0." + percentageList[i] + "} ";
          }*/
    }


    public void ReadInfo(string info)
    {

        // currentInfo is to save the info in the field in case of the user want it to save to file
        currentInfo = info;

        //Divide in pairs(color and percentage)
        string[] splitArray = info.Split(char.Parse("}"));
        percentageList.Clear();
        colorList.Clear();

        for (int i = 0; i < splitArray.Length; i++)
        {
           //Split first part to get the color
            string[] expArray = splitArray[i].Split(char.Parse("#"));

            if (expArray.Length > 1)
            {

                string cleanedString = expArray[1];
               
                cleanedString = "#" + cleanedString.Split(char.Parse("\""))[0];

               
                if (ColorUtility.TryParseHtmlString(cleanedString, out color))
                { colorList.Add(color); }
                else
                {
                    for (int e = 4; e < 7; e++)
                    {
                       if(cleanedString.Length == e)
                        {
                            cleanedString = cleanedString + "0";
                        }
                    }
                   
                    if (ColorUtility.TryParseHtmlString(cleanedString, out color))
                    { colorList.Add(color); }
                }
                


                // Divide the percentage
                expArray = expArray[1].Split(char.Parse("."));

                // percentageList.Add(float.Parse(expArray[1]));


                if (expArray.Length > 1)
                {
                    //get percentage with int numbers(instead of fraction as received)
                    if (expArray[1].Length > 4)
                    {
                        percentageList.Add((float)((float)float.Parse(expArray[1].Substring(0, 3)))/10);
                    }
                    else if (expArray[1].Length == 1)
                    {
                        percentageList.Add((float)((float)float.Parse(expArray[1]) * 10));
                    }

                    else
                    {
                        percentageList.Add((float)((float)float.Parse(expArray[1])));
                    }
                }

            }



        }

        
    }


    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        data dataRetrieve = new data(currentInfo);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, dataRetrieve);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination); 
        else
        {
            GetComponent<UILogic>().outputText.text = "No data to \nretrieve";
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
       data dataRetrieve = (data)bf.Deserialize(file);
        file.Close();


        // savedInfo is where is stored the retrieved data
        savedInfo = dataRetrieve.savedData;

       GetComponent<UILogic>().outputText.text = "Loaded saved \ndata";
        ReadInfo(savedInfo);

    }

    public void DeleteFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
       
        if (File.Exists(destination)) File.Delete(destination);
    }

}
