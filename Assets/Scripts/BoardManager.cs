using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;



public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }


    public string actualLevel = null;
    public int columns = 15;                                         //Number of columns in our game board.
    public int rows = 15;                                            //Number of rows in our game board.
    public Count wallCount = new Count(5, 9);                      //Lower and upper limit for our random number of walls per level.
    public Count foodCount = new Count(1, 5);                      //Lower and upper limit for our random number of food items per level.
    public GameObject exit;                                         //Prefab to spawn for exit.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    public GameObject[] wallTiles;                                  //Array of wall prefabs.
    public GameObject[] foodTiles;                                  //Array of food prefabs.
    public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
    public GameObject[] outerWallTiles;
    public GameObject Final;//Array of outer tile prefabs.

    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.

    private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.









    private object instance;

    void InitialiseList()
    {

        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }




    public void BoardSetup()
    {

        string mapa = androidDameMapa(actualLevel);

        //string mapa = DameMapa(actualLevel);

        boardHolder = new GameObject("Board").transform;
        String[] mapa1 = mapa.Split(',');
        GameObject toInstantiate;
        int i = 0;



        for (int y = 0; y < columns; y++)
        {


            for (int x = 0; x < rows; x++)
            {
                Debug.Log("i=" + i);
                if (mapa1[i] == "n")
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    pintar(toInstantiate, x, y);
                }
                else if (mapa1[i] == "P")
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    pintar(toInstantiate, x, y);
                    Instantiate(exit, new Vector3(x, y, 0f), Quaternion.identity);
                }
                else if (mapa1[i] == "C")
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    pintar(toInstantiate, x, y);
                    toInstantiate = foodTiles[Random.Range(0, foodTiles.Length)];
                    pintar(toInstantiate, x, y);
                }
                else if (mapa1[i] == "E")
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    pintar(toInstantiate, x, y);
                    toInstantiate = enemyTiles[Random.Range(0, enemyTiles.Length)];
                    pintar(toInstantiate, x, y);
                }
                else if (mapa1[i] == "F")
                {
                    Instantiate(Final, new Vector3(x, y, 0f), Quaternion.identity);
                }
                else
                {

                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    pintar(toInstantiate, x, y);
                }

                i++;
            }
        }
    }

    public void pintar(GameObject toInstantiate, int x, int y)
    {

        GameObject instacia;

        instacia = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        instacia.transform.SetParent(boardHolder);
    }





    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup2()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
            }
        }
    }


    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {

        int randomIndex = Random.Range(0, gridPositions.Count);


        Vector3 randomPosition = gridPositions[randomIndex];


        gridPositions.RemoveAt(randomIndex);


        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }








    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {

        actualLevel = level.ToString();
        BoardSetup();
        InitialiseList();
        


        //LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);


        //LayoutObjectAtRandom (foodTiles,2, 2);


        int enemyCount = (int)Mathf.Log(level, 2f);

        //LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);


        
    }

    private string androidDameMapa(string id)
    {
        string mapa = null;
#if UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaObject("com.example.dsaproyectofront.MenuActivity");     


        if (id.Equals("1"))
        {
            mapa = activity.CallStatic<string>("dameMapa1");

        }
        else if (id.Equals("2"))
        {

            mapa = activity.CallStatic<string>("dameMapa2");

        }
        else
        {
            mapa = activity.CallStatic<string>("dameMapa3");
        }
#endif

        return mapa;
    }


    private string DameMapa(string id)
    {
        string mapa;


        if (id.Equals("1"))
        {
            mapa = "n,n,M,M,M,M,M,M,M,M,M,M,M,M,M,n,n,n,n,n,M,M,n,n,M,n,n,P,n,M,M,n,M,M,n,M,n,n,M,M,n,n,M,M,M,M,n,M,n,n,M,M,n,M,M,M,n,M,n,M,M,n,n,n,M,n,n,C,n,n,M,n,M,n,M,M,M,n,M,M,n,M,M,M,n,M,n,M,n,M,M,n,n,n,M,n,n,n,M,n,n,n,n,n,M,M,n,M,n,M,n,M,n,M,M,M,M,M,M,M,M,n,M,n,n,n,M,M,M,M,M,M,M,M,M,M,n,M,n,M,M,M,n,n,n,n,n,n,n,M,M,M,M,n,n,n,M,n,M,M,M,M,n,M,M,M,M,M,M,M,n,M,n,n,C,n,M,n,M,M,M,n,n,n,n,n,M,M,M,M,n,M,E,M,M,M,M,n,M,M,C,n,n,n,n,n,M,C,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M";

        }
        else if (id.Equals("2"))
        {

            mapa = "n,n,M,M,M,M,M,M,M,M,M,M,M,M,M,n,n,M,M,n,n,n,n,n,M,n,n,n,n,M,n,n,n,n,n,n,M,n,M,M,n,M,C,M,M,M,M,M,M,M,n,M,n,n,n,n,M,n,M,M,M,M,M,n,n,E,M,n,M,M,M,M,n,M,M,M,n,M,n,M,M,M,n,n,n,n,n,n,n,M,M,n,M,C,n,n,M,M,M,M,M,M,M,M,M,M,n,M,n,M,n,M,n,M,M,M,M,M,M,M,M,n,n,n,M,n,n,n,M,E,n,n,n,n,M,M,M,n,M,M,n,M,M,M,n,M,n,M,n,M,M,n,n,n,M,n,n,n,n,n,M,n,M,n,M,M,n,M,n,n,M,M,n,M,M,M,n,M,n,M,M,n,M,M,n,M,n,n,M,M,n,n,M,M,M,M,n,C,n,E,M,M,n,n,M,n,C,P,n,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M";

        }
        else
        {
            mapa = "n,n,M,M,M,M,M,M,M,M,M,M,M,M,M,n,n,M,n,n,n,n,M,n,n,n,M,n,n,M,M,n,n,n,M,M,n,n,n,M,n,M,n,n,M,M,M,M,M,M,n,n,M,M,n,M,n,E,n,M,M,C,n,n,M,n,M,n,n,n,M,M,M,n,M,M,E,n,n,n,n,n,C,M,n,n,n,M,M,M,M,C,n,n,M,n,M,n,M,n,M,M,n,n,M,M,M,M,M,M,n,M,n,M,M,M,n,n,n,M,M,n,n,n,M,M,n,n,M,M,n,n,M,M,M,M,n,M,n,n,n,n,M,n,M,n,n,M,n,M,M,n,n,n,M,n,M,M,n,M,n,n,n,n,M,M,n,n,n,M,n,n,M,E,M,n,C,C,C,M,M,n,n,n,M,M,M,M,n,n,n,C,F,C,M,M,n,n,n,n,C,n,E,n,M,n,C,C,C,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M,M";
        }     
    

        return mapa;
    }
}






/*
public class NewBehaviourScript : MonoBehaviour
{
	
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;
		
		public Count (int min, int max){
			maximum = max;
			minimum = min;
		}
	}
	
	
	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count (5,9);
	public Count foodCount = new Count (1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	
	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>(); 
	
	void InitialiseList(){
		
		gridPositions.Clear();
		
		for (int x = 1; x < columns -1; x++){
			for (int y = 1; y < rows -1; y++){
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}
	
   void BoardSetup ()
        {
           
            boardHolder = new GameObject ("Board").transform;
            
           
            for(int x = -1; x < columns + 1; x++)
            {
               
                for(int y = -1; y < rows + 1; y++)
                {
                    
                    GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
                    
                  
                    if(x == -1 || x == columns || y == -1 || y == rows)
                        toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
                    
                   
                    GameObject instance =
                        Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                    
                    
                    instance.transform.SetParent (boardHolder);
                }
            }
        }
        
        
      
        Vector3 RandomPosition ()
        {
      
            int randomIndex = Random.Range (0, gridPositions.Count);
            
            
            Vector3 randomPosition = gridPositions[randomIndex];
            
           
            gridPositions.RemoveAt (randomIndex);
            
          
            return randomPosition;
        }
        
        
       
        void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
        {
         
            int objectCount = Random.Range (minimum, maximum+1);
            
           
            for(int i = 0; i < objectCount; i++)
            {
              
                Vector3 randomPosition = RandomPosition();
                
              
                GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
                
              
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }
        
        
        
        public void SetupScene (int level)
        {
            BoardSetup ();
            
          
            InitialiseList ();
            
            
            LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
            
           
            LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
            
            
            int enemyCount = (int)Mathf.Log(level, 2f);
            
            
            LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
            
           
            Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
        }
    }
*/
