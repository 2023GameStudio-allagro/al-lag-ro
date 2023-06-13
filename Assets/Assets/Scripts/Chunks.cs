using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class Chunk {

	public GameObject chunk_object;
	public Vector2 center;
	public string biome;
	public float chunksize;

				//*************** Insert the names of the biomes here ****************// 
	// Each biome must have a folder with his name in the default Resources folder of Unity where the objects 
	// (prefabs) to be placed will be allocated.
	private List<string> biomes = new List<string>(new string[]{"TestBiome"});

			//*************** Insert the number of objects to be spawned ****************//
	private	int lowest_n = 5;
	private int highest_n = 10;

	public Chunk(GameObject game_o, Vector2 cord, float size)
	{
		// Constructor of the Chunk class
		// GameObject x Vector2 -> Chunk
		// Creates a Chunk object with size x size with center in cord and a gameobject to store all the children objects.
		chunk_object = new GameObject();
		chunk_object.transform.position = cord;
		chunk_object.transform.parent = game_o.transform;
		biome = biomes [Random.Range (0, biomes.Count)];
		center = cord;
		chunksize = size;
	}

	public void GenerateChunk ()
	{
		// Modifier of the Chunk class
		// Chunk -> --
		// Called by a Chunk object and generate the objects inside the chunk according to his properties.
		Object[] items = Resources.LoadAll (this.biome);
		float min_x = this.center.x - (this.chunksize / 2);
		float max_x = this.center.x + (this.chunksize / 2);
		float min_y = this.center.y - (this.chunksize / 2);
		float max_y = this.center.y + (this.chunksize / 2);
		float number_of_objects = Random.Range (lowest_n,highest_n);

		// Create number_of_objects random objects according to the chunk's biome.
		for (int i = 1; i <= number_of_objects; i++) {
			float rand_x = Random.Range (min_x, max_x);
			float rand_y = Random.Range (min_y, max_y);

			GameObject item = (GameObject) items [Random.Range (0, items.Length)];

			// Check if any object is in the position where a new one will spawn
			if (Physics2D.OverlapCircle (new Vector2 (rand_x, rand_y), item.GetComponent<ObjectScript>().spawn_radius) == null) { 
				MonoBehaviour.Instantiate (item , new Vector2 (rand_x, rand_y), Quaternion.identity, this.chunk_object.transform);
			} else {
				i = i - 1;
			}
		}
	}

	public void Load_Chunk()
	{ 
		// Modifier of the Chunk class
		// Chunk -> --
		// Creates all the objects of a chunk previously saved. If a file doesn't exist to the given chunk the function does nothing.
		string load_string = "/chunk" + center.x.ToString ("0.0") + "_" + center.y.ToString ("0.0");

		if (File.Exists(Application.persistentDataPath + load_string))
		{
			int count = 0;
			Object[] items = Resources.LoadAll (this.biome);
			Dictionary<string,GameObject> dictionary = new Dictionary<string,GameObject> ();

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream(Application.persistentDataPath + load_string, FileMode.Open);
			ChunkData data = bf.Deserialize (stream) as ChunkData;
			stream.Close ();
		
			// Runs through the list of names given by ChunkData (name_of_the_object) creating a correspondence between the names and the objects of the items array
			// Creates the object in the position given by ChunkData (objects_position)
			foreach (string name in data.name_of_the_object) {
			    string name_aux = name.Replace ("(Clone)", ""); // Remove the (Clone) part of the object's name in order to identify him
				if (!(dictionary.ContainsKey(name_aux)))
				{
					//Check which of the objects is the object with the given name
					foreach (GameObject objecto in items) {
						if (objecto.transform.name == name_aux) 
						{
							dictionary[name_aux] = objecto;
							break;
						}
					}
				}
				MonoBehaviour.Instantiate(dictionary[name_aux], data.objects_position[count].SerializableToVector3(), Quaternion.identity, this.chunk_object.transform);
				count += 1;
			}
		}
	}

	public void Unload_Chunk()
	{
		// Modifier of the Chunk class
		// Chunk -> --
		// Destroys the gameobject of the given chunk
		MonoBehaviour.Destroy(this.chunk_object);
	}

	public void Save_Chunk()
	{
		// Transformer of the Chunk class
		// Chunk -> File
		// Creates a file with the ChunkData of the given Chunk.
		string save_string = "/chunk" + this.center.x.ToString("0.0") + "_" + this.center.y.ToString("0.0"); // "/x_y"

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream stream = new FileStream (Application.persistentDataPath + save_string, FileMode.Create);
		ChunkData data = new ChunkData (this);
		bf.Serialize (stream, data);
		stream.Close ();
	}

	public bool Chunk_File()
	{
		// Test of the Chunk class
		//Returns true if the file for that chunk exists and false if not.
		string load_string = "/chunk" + center.x.ToString ("0.0") + "_" + center.y.ToString ("0.0");

		return File.Exists (Application.persistentDataPath + load_string);
	}
}

public class Map_Chunks
{
	public float chunk_size;
	public Chunk[] chunk_map;

	public Map_Chunks(GameObject player, GameObject game_o, float size)
	{
		// Constructor of the Map_Chunks class
		// GameObject x GameObject x Float -> Map_Chunks
		chunk_size = size;
		Vector2 first_chunk = new Vector2 (Chunks.FindClosestMultiple (chunk_size, player.transform.position.x) - chunk_size, 
			Chunks.FindClosestMultiple (chunk_size, player.transform.position.y) + chunk_size);
		chunk_map = new Chunk[9];

		// Chunk [4] -> player's chunk
		// [0] [1] [2]
		// [3] [4] [5]
		// [6] [7] [8]

		for (int i = 0; i <= 8; i++) {
			chunk_map [i] = new Chunk(game_o , first_chunk, chunk_size);

			//Load or Generate the chunk
			if (chunk_map [i].Chunk_File ()) {
				chunk_map [i].Load_Chunk ();
			} else {
				chunk_map [i].GenerateChunk ();
				chunk_map [i].Save_Chunk ();
			}
				
			if ((i + 1) % 3 == 0)
			{
				first_chunk = new Vector3(first_chunk.x - (chunk_size * 2), first_chunk.y - chunk_size);
			}
			else 
			{
				first_chunk = new Vector3(first_chunk.x + chunk_size, first_chunk.y);
			}
		}
	}

	public Chunk Return_Chunk(int n)
	{
		// Selector of the Map_Chunks class
		// Int -> Chunk
		return this.chunk_map [n]; 
	}

	public void Sub_Chunk(Chunk ch, int n)
	{
		// Modifier of the Map_Chunks class
		// Chunk x Int -> --
		// Moves a chunk to a different place in the Map_Chunk
		this.chunk_map [n] = ch;
	}

	public void Move_Two_Chunk(int n1, int n2, int n3)
	{
		// Modifier of the Map_Chunks class
		// Int x Int x Int -> --
		// Move the chunk in the position 1 to the position 2 and the chunk in the position 2 to the position 3
		// If n3 is greater than 8 or lower than 0 only the chunk 1 is moved
		if ((n3 <= 8) && (n3 >= 0)) {
			Chunk chunk_aux = this.chunk_map [n2];
			this.chunk_map [n2] = this.chunk_map [n1];
			this.chunk_map [n3] = chunk_aux;
		} else {
			this.chunk_map [n2] = this.chunk_map [n1];
		}
	}
}
	
public class Chunks : MonoBehaviour {
	
	public GameObject player;
	public float chunk_size = 40;

	private Map_Chunks map;
	private Vector2[] chunk_correction;
	private int[][] numbers = new int[][]{new int[] {0,1,3}, new int[] {0,1,2}, new int[] {1,2,5}, new int[] {0,3,6}, new int[] {4}, 
		new int[] {2,5,8}, new int[] {3,6,7}, new int[] {6,7,8}, new int[] {5,7,8}};
	private int[][] remove_numbers = new int[][]{new int[] {2,5,6,7,8}, new int[] {6,7,8}, new int[] {0,3,6,7,8}, new int[] {2,5,8}, new int[] {4}, 
		new int[] {0,3,6}, new int[] {0,1,2,5,8}, new int[] {0,1,2}, new int[] {0,1,2,3,6}};
	private int[] movement = { 4, 3, 2, 1, 0, -1, -2, -3, -4 };

	// Use this for initialization
	void Start () {
		chunk_correction = new Vector2[] { new Vector2 (-chunk_size, chunk_size), new Vector2 (0, chunk_size), new Vector2 (chunk_size, chunk_size),
			new Vector2 (-chunk_size, 0), new Vector2 (0, 0), new Vector2 (chunk_size, 0), new Vector2 (-chunk_size, -chunk_size), new Vector2 (0, -chunk_size),
			new Vector2 (chunk_size, -chunk_size)};
		map = new Map_Chunks(player,this.gameObject,chunk_size);
	}
	
	// Update is called once per frame
	void Update () {
		Update_Chunks (map, player);
	}

	public void Update_Chunks(Map_Chunks map, GameObject player)
	{
		// Find the chunk where the player is
		Vector2 first_chunk = new Vector2 (FindClosestMultiple (map.chunk_size, player.transform.position.x), 
			FindClosestMultiple (map.chunk_size, player.transform.position.y));

		if (first_chunk != map.chunk_map [4].center) { // Check if the player is still in the center of the Map_Chunks
			int count = 0;

			// Check what is the new chunk in the center
			for (int i = 0; i <= 8; i++){
				if (map.chunk_map [i].center == first_chunk) {
					i = 9;
				} else {
					count += 1;
				}
			}

			// Destroys and saves the chunks that were removed from the Map_Chunks
			foreach (int n in remove_numbers[count]) {
				map.chunk_map [n].Save_Chunk ();
				map.chunk_map [n].Unload_Chunk ();
			}

			// Move the chunks that are still in the Map_Chunks but in different position
			foreach (int n in numbers[count]) {
				map.Move_Two_Chunk (n,n + movement[count], n + movement[count] + movement[count]);
			}

			// Generates or loads the new chunks that are in the Map_Chunks after the change of center
			foreach (int n in numbers[count]) {
				Chunk chunk = new Chunk (this.gameObject, map.chunk_map[4].center + chunk_correction [n],chunk_size);
				map.chunk_map [n] = chunk;

				if (map.chunk_map [n].Chunk_File ()) {
					map.chunk_map [n].Load_Chunk ();
				} else {
					map.chunk_map [n].GenerateChunk ();
					map.chunk_map [n].Save_Chunk ();
				}
			}
		}
	}

	public static float FindClosestMultiple (float factor, float value)
	{
		// Float x Float - > Float
		// Returns the closest multiple of the given factor according to the given value
		float nearestMultiple = (int)System.Math.Round((value / (double)factor),System.MidpointRounding.AwayFromZero) * factor;
		return nearestMultiple;
	}
}
