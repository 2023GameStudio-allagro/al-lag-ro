~~~~~~What does this package?~~~~~~

With the scripts in this package you can generate chunks in 2D that have different biomes created by you. Each biome 
has his own objects. The generated objects are placed in random positions. You can choose the amount of objects
place in each chunk within a range. Each chunk will be saved in the Application.persistentDataPath with his own file and 
own name defined by the position of his center.

~~~~~~How it works?~~~~~~

Chunks->

This is the main script of the package.
The size of each chunk and the player to be followed by the chunks must be defined here in the public vars.
In this script you must define each biome in line 18 and the number of spawned objects in line 21,22.

ChunkData->

ChunkData is defined here. 
ChunkData receives a chunk and returns a ChunkData object with two serializables with the name of the objects of that chunk and their positions.

ObjectScript->

Each prefab that will be generated must have a script that defines the radius of a circle
with center in the generate position where colliders of other objects can't be in order to spawn this prefab.
