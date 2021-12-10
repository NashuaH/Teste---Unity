using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILogic : MonoBehaviour
{
    public InputField userInputField;
    public Text outputText;

   CreateGrid createGridScript;
     GetCubeInfo getinfoScript;

    int indexCurrentsSelected = 99999;


    public void Start()
    {
        createGridScript = GetComponent<CreateGrid>();
       getinfoScript = GetComponent<GetCubeInfo>();
    }
    void CheckInput()
    {
        string userInput = userInputField.text;
        userInput = userInput.ToLower();
        if (int.TryParse(userInput.Substring(0,1), out int number))
        {

            FindCoordinatesColor();
        }
        else
        if (userInput.Substring(0,1) == "#")
        {
            FindCubePercentage();
        }
        else
        if (userInput.Substring(0, 1) == "n" || userInput.Substring(0, 1) == "N")
        {
          
            FindCubeNeighbour();

        }
        else
        if (userInput.Substring(0, 1) == "m" || userInput.Substring(0, 1) == "M")
        {

           FindMirrorCube();
        }else if (userInput == "logs")
        {
            RetrieveCubeToposition();
            LoadSavedInfo();
        }else if(userInput == "delete")
        {

            RetrieveCubeToposition();
            getinfoScript.DeleteFile();
            outputText.text = "Deleted saved \ndata";
        }
        else
        {
            RetrieveCubeToposition();

            outputText.text = "Insert a valid \ninput"; 
        }

    }
  void FindCoordinatesColor()
    {

        string userCoord = userInputField.text;
        string[] splitArray =userCoord.Split(char.Parse(","));


        if (splitArray.Length == 2)
        {
            bool checkY = false;
            if (int.TryParse(splitArray[0], out int x))
            {
                x = int.Parse(splitArray[0]);
            }
            if (int.TryParse(splitArray[1], out int y))
            {
                y = int.Parse(splitArray[1]);
                checkY = true;
            }
           

            // orderInList is the order in the list of the selected coordinates
            int orderInList = x + (y * (int)createGridScript.gridSize.x);
    
            if (orderInList>=0 && x < createGridScript.gridSize.x && y < createGridScript.gridSize.y && checkY)
            {

                outputText.text = "#" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[orderInList].GetComponent<Renderer>().material.color);
                ShowCubeSelected(orderInList);
                indexCurrentsSelected = orderInList;
            }
        }else
        {
            outputText.text = "Insert valid \ncoordinates";
        }

      
    }

  void FindCubePercentage()
    {
        RetrieveCubeToposition();
        string userColor = userInputField.text;
        string[] splitArray = userColor.Split(char.Parse("#"));
        int sameColorCubeNumber = 0;
      
        if (  ColorUtility.TryParseHtmlString("#"+splitArray[1], out Color color)){

            
            if (getinfoScript.colorList.Contains(color))
            {
                foreach(GameObject cube in createGridScript.cubeList)
                {
                    if(cube.GetComponent<Renderer>().material.color == color)
                    {
                        sameColorCubeNumber = sameColorCubeNumber + 1;
                    }
                }
            }
        }

        if (sameColorCubeNumber > 0)
        {
            outputText.text = (100 * (sameColorCubeNumber / (createGridScript.gridSize.x * createGridScript.gridSize.y))) + "%";
        }
        else
        {
            outputText.text = "There is no \ncube with that \ncolor";
        }

    }

void FindCubeNeighbour()
    {

        string userCoord = userInputField.text;
        string[] splitArray = userCoord.Split(char.Parse("n"));

        if (splitArray.Length == 1)
        {
            splitArray = userCoord.Split(char.Parse("N"));
        }
          splitArray = splitArray[1].Split(char.Parse(","));


        if (splitArray.Length == 2)
        {
            bool checkY = false;
            if (int.TryParse(splitArray[0], out int x))
            {
                x = int.Parse(splitArray[0]);
            }
            if (int.TryParse(splitArray[1], out int y))
            {
                y = int.Parse(splitArray[1]);
                checkY = true;
            }

          
            

            int orderInList = x + (y * (int)createGridScript.gridSize.x);

            if (orderInList >= 0 && x < createGridScript.gridSize.x && y < createGridScript.gridSize.y && checkY)
            {
                outputText.text = "";

                //alternative staggered diamond grid neighbours
                if (createGridScript.gridAltType)
                {

                    //Different y axis lines variation
                    if (y % 2 != 0)
                    {
                        if (x + 1 < createGridScript.gridSize.x && y + 1 < createGridScript.gridSize.y)
                        {
                            outputText.text = outputText.text + "(" + (x + 1) + "," + (y + 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x + 1) + ((y + 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                        }

                        if (x + 1 < createGridScript.gridSize.x && y - 1 >= 0)
                        {
                            outputText.text = outputText.text + "(" + (x + 1) + "," + (y - 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x + 1) + ((y - 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                        }
                    }
                    else
                    {
                        if (x - 1 >= 0 && y - 1 >= 0)
                        {
                            outputText.text = outputText.text + "(" + (x - 1) + "," + (y - 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x - 1) + ((y - 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                        }

                        if (x - 1 >= 0 && y + 1 < createGridScript.gridSize.y)
                        {
                            outputText.text = outputText.text + "(" + (x - 1) + "," + (y + 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x - 1) + ((y + 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                        }
                    }

                    if (y + 1 < createGridScript.gridSize.y)
                    {
                        outputText.text = outputText.text + "(" + x + "," + (y + 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x) + ((y + 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }

                    if (y - 1 >= 0)
                    {
                        outputText.text = outputText.text + "(" + x + "," + (y - 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x) + ((y - 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }
                }
                else
                { //normal diamond grid neighbours

                    if (x + 1 < createGridScript.gridSize.x)
                    {
                        outputText.text = outputText.text + "(" + (x + 1) + "," + y + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x + 1) + (y * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }

                    if (x - 1 >= 0)
                    {
                        outputText.text = outputText.text + "(" + (x - 1) + "," + y + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x - 1) + (y * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }

                    if (y + 1 < createGridScript.gridSize.y)
                    {
                        outputText.text = outputText.text + "(" + x + "," + (y + 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x) + ((y + 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }

                    if (y - 1 >= 0)
                    {
                        outputText.text = outputText.text + "(" + x + "," + (y - 1) + ") - #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[(x) + ((y - 1) * (int)createGridScript.gridSize.x)].GetComponent<Renderer>().material.color) + "\n";
                    }
                }
                ShowCubeSelected(orderInList);
                indexCurrentsSelected = orderInList;
            }
            else
            {
                outputText.text = "Insert valid \ncoordinates";
            }
        }
    }



  void FindMirrorCube()
    {
        string userCoord = userInputField.text;
        string[] splitArray = userCoord.Split(char.Parse("m"));
        if (splitArray.Length == 1)
        {
            splitArray = userCoord.Split(char.Parse("M"));
        }
        bool mx = false, my = false;

        if (splitArray[1].Length > 0)
        {
            if (splitArray[1].Substring(0, 1) == "x") { splitArray = splitArray[1].Split(char.Parse("x")); mx = true; }else
            if (splitArray[1].Substring(0, 1) == "X") { splitArray = splitArray[1].Split(char.Parse("X")); mx = true; }else
            if (splitArray[1].Substring(0, 1) == "y") { splitArray = splitArray[1].Split(char.Parse("y")); my = true; }else
            if (splitArray[1].Substring(0, 1) == "Y") { splitArray = splitArray[1].Split(char.Parse("Y")); my = true; }
            splitArray = splitArray[1].Split(char.Parse(","));
        }
       


        if (splitArray.Length == 2 && (mx ||my))
        {
            bool checkY = false;
            if (int.TryParse(splitArray[0], out int x))
            {
                x = int.Parse(splitArray[0]);
            }
            if (int.TryParse(splitArray[1], out int y))
            {
                y = int.Parse(splitArray[1]);
                checkY = true;
            }


            if (mx)
            {


                // mirror with X Axis align with the square and for the alternative staggered diamond grid
                if (createGridScript.gridAltType)
                {

                     y = ((int)createGridScript.gridSize.y - 1) - y;


                }
                else {

                    // mirror with X Axis world (Normal Diamond Grid Only)

                    if ((y + x) != (createGridScript.gridSize.x - 1))
                    {
                        if ((y + x) < ((createGridScript.gridSize.x - 1) + (createGridScript.gridSize.y - 1) / 2))
                        {


                            int saveX = x;
                            x = x + (((int)createGridScript.gridSize.x - 1) - (y + x));
                            y = y + (((int)createGridScript.gridSize.y - 1) - (y + saveX));

                        }
                        else
                        {

                            int saveX = x;
                            if (y > x)
                            {
                                x = x - (((int)createGridScript.gridSize.x - 1) - (y - x));
                                y = y - (((int)createGridScript.gridSize.y - 1) - (y - saveX));
                            }
                            else if (x > y)
                            {
                                x = x - (((int)createGridScript.gridSize.x - 1) - (x - y));
                                y = y - (((int)createGridScript.gridSize.y - 1) - (saveX - y));

                            }
                        }
                    }
                }

            }

            if (my)
            {
                // mirror with Y Axis align with the square and for the alternative staggered diamond grid
                if (createGridScript.gridAltType)
                {
                       x = ((int)createGridScript.gridSize.x - 1) - x;



                }
                else
                {

                    // mirror with Y Axis world (Normal Diamond Grid Only)
                    if (y != x)
                    {
                        int saveY = y;
                        y = x;
                        x = saveY;
                    }
                }



            }

            int orderInList = x + (y * (int)createGridScript.gridSize.x);

        
            if (orderInList >= 0 && x < createGridScript.gridSize.x && y < createGridScript.gridSize.y && checkY)
            {

                outputText.text = x + "," + y + " #" + ColorUtility.ToHtmlStringRGB(createGridScript.cubeList[orderInList].GetComponent<Renderer>().material.color);
                ShowCubeSelected(orderInList);
                indexCurrentsSelected = orderInList;
            }
            else
            {
                outputText.text = "Insert valid \ncoordinates";
            }

        }
        else
        {
            outputText.text = "Insert axis \nto mirror";
        }





    }



    void LoadSavedInfo()
    {
        //fetch data
        getinfoScript.LoadFile();

        //spawn grid
        if (createGridScript.gridAltType) { 
            

            createGridScript.AlternativeGridInstance();
        
        }
        else
        {
            createGridScript.NormalGridInstance();
        }


 
    }

   void ShowCubeSelected(int indexOfcube)
    {

       
        RetrieveCubeToposition();
        createGridScript.cubeList[indexOfcube].transform.position = new Vector3(createGridScript.cubeList[indexOfcube].transform.position.x, createGridScript.cubeList[indexOfcube].transform.position.y,createGridScript.cubeList[indexOfcube].transform.position.z -0.2f);
    }

    void RetrieveCubeToposition()
    {

        //number 99999 is when no cube is selected
        if(indexCurrentsSelected != 99999)
        {
            createGridScript.cubeList[indexCurrentsSelected].transform.position = new Vector3(createGridScript.cubeList[indexCurrentsSelected].transform.position.x, createGridScript.cubeList[indexCurrentsSelected].transform.position.y, createGridScript.cubeList[indexCurrentsSelected].transform.position.z + 0.2f);
            indexCurrentsSelected = 99999;
        }
    }
}
