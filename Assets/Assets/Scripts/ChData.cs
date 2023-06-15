using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
public class ChunkData {

	public string[] name_of_the_object;
	public SerializableVector[] objects_position;

	public ChunkData(Chunk ch) {
		// Constructor of the ChunkData class
		// Chunk -> ChunkData
		name_of_the_object = new string[ch.chunk_object.transform.childCount];
		objects_position = new SerializableVector[ch.chunk_object.transform.childCount];
		int count = 0;

		// Runs through all the object's children of given chunk, placing the name of the object and his position in their respective arrays
		foreach (Transform child in ch.chunk_object.transform) {
			name_of_the_object [count] = child.transform.name;
			objects_position [count] = new SerializableVector(child.transform.position);
			count += 1;
		}
	}
}

[Serializable]
public class SerializableVector {
	public float x,y,z;

	public SerializableVector(Vector3 vector)
	{
		// Constructor of the SerializableVector class
		// Vector3 -> SerializableVector
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}

	public UnityEngine.Vector3 SerializableToVector3()
	{
		// Transformer of the SerializableVector class
		// SerializableVector -> Vector3
		return new Vector3 (this.x, this.y, this.z);
	}
}

public class ChData : MonoBehaviour {
}

