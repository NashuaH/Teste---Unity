using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public Vector2 gridSize;
    public bool gridAltType = false;
    public Transform startGrid;
    public Transform gridParent;
    public List<GameObject> cubeList = new List<GameObject>();

    int colorsAssigned, colorIndex =0;

    Color colorToassign;



    public  void GridInstance()
    {
        if (gridAltType)
        {
            AlternativeGridInstance();
        }
        else
        {
            NormalGridInstance();
        }
    }
    public void NormalGridInstance()
    {
        gridAltType = false;
        if (cubeList.Count > 0)
        {
            foreach(GameObject cube in cubeList)
            {
                Destroy(cube);
            }
            cubeList.Clear();
            colorsAssigned = 0;
            colorIndex = 0;
            gridParent.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        
        colorToassign = GetComponent<GetCubeInfo>().colorList[colorIndex];
        //create normal grid of cubes(rotate ate the end)
        for (int y = 0; y < gridSize.y; y++)
        {
           

            for (int x = 0; x < gridSize.x; x++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                //the list of cubes is where the app fetch the location of the cube based on the coordinates needed
                //The order in the list MATTERS for UILogic to find cubes 

                cubeList.Add(cube);
                cube.transform.parent = gridParent;
                cube.transform.position = new Vector3(startGrid.position.x + x, startGrid.position.y - y, startGrid.position.z);
                cube.GetComponent<Renderer>().material.color = colorToassign;
                colorsAssigned = colorsAssigned + 1;

                int cubesPerPercentage = (int)((float)(GetComponent<GetCubeInfo>().percentageList[0]/100) * (gridSize.x * gridSize.y));

                if (colorsAssigned < (gridSize.x *gridSize.y))
                {
                    if ((colorsAssigned % cubesPerPercentage) == 0)
                    {
                        colorIndex = colorIndex + 1;

                        if(colorIndex> GetComponent<GetCubeInfo>().colorList.Count - 1)
                        {
                            colorIndex = colorIndex - 1;
                        }

                        if (GetComponent<GetCubeInfo>().colorList[colorIndex] != null)
                        {
                            colorToassign = GetComponent<GetCubeInfo>().colorList[colorIndex];
                        }

                    }
                }
            }

        }


        //rotate the normal grid to transform ir in a diamond
        gridParent.transform.eulerAngles = new Vector3(0, 0, - 45);
       
    }



    public void AlternativeGridInstance()
    {
        if (cubeList.Count > 0)
        {
            foreach (GameObject cube in cubeList)
            {
                Destroy(cube);
            }
            cubeList.Clear();
            colorsAssigned = 0;
            colorIndex = 0;
            gridParent.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        gridAltType = true;
        colorToassign = GetComponent<GetCubeInfo>().colorList[colorIndex];

        //create staggered diamond grid, rotating each cube on instance
       
        for (int y = 0; y < gridSize.y; y++)
        {

            for (int x = 0; x < gridSize.x; x++)
            {

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cubeList.Add(cube);
                cube.transform.parent = gridParent;
               cube.transform.eulerAngles = new Vector3(0, 0, -45);
                cube.transform.position = new Vector3(startGrid.position.x + (x* Mathf.Sqrt(cube.transform.localScale.x + cube.transform.localScale.x)), startGrid.position.y + (y * Mathf.Sqrt(cube.transform.localScale.y + cube.transform.localScale.y) /2), startGrid.position.z);

                cube.GetComponent<Renderer>().material.color = colorToassign;

                //move the cube slighly to give space for every cube depending on the line
                if(y%2 == 0)
                {
                    cube.transform.position = new Vector3(cube.transform.position.x- (Mathf.Sqrt(cube.transform.localScale.y + cube.transform.localScale.y)/2), cube.transform.position.y,cube.transform.position.z);
                }
                colorsAssigned = colorsAssigned + 1;

                int cubesPerPercentage = (int)((float)(GetComponent<GetCubeInfo>().percentageList[0] / 100) * (gridSize.x * gridSize.y));

                if (colorsAssigned < (gridSize.x * gridSize.y))
                {
                    if ((colorsAssigned % cubesPerPercentage) == 0)
                    {
                        colorIndex = colorIndex + 1;


                       
                        if (colorIndex > GetComponent<GetCubeInfo>().colorList.Count - 1)
                        {
                            colorIndex = colorIndex - 1;
                        }

                        if (GetComponent<GetCubeInfo>().colorList[colorIndex] != null)
                        {
                            colorToassign = GetComponent<GetCubeInfo>().colorList[colorIndex];
                           
                        }

                    }
                }
            }

        }


        gridParent.transform.position = new Vector3(gridParent.transform.position.x - 5, gridParent.transform.position.y -15, gridParent.transform.position.z);


    }
   
}
