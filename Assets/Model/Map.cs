using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Map {
    public  Chunk[,,] chunks;    
    public Vector3 size = new Vector3(8,16,8);
    int boundX;
    int boundY;
    int boundZ;
    public Vector3[,] perlinMap;
    float rand ;
    public Map( )
    {        
        boundX = (int)size.x;
        boundY = (int)size.y;
        boundZ = (int)size.z;
        rand = Random.Range(12, 24);
        FillMap(size);
        Debug.Log(rand);

    }
    public void FillMap(Vector3 size) {
        chunks = new Chunk[boundX, boundY, boundZ];        
        for (int i = 0; i < boundX; i++)
        {
            for (int j = 0; j < boundY; j++)
            {                
                for (int w = 0; w < boundZ; w++)
                {
                    chunks[i, j, w] = new Chunk(new Vector3(i*16,j*16,w*16));                    
                }
            }
        }
        Debug.Log("map Stuff created");
        Surface();
        RiverMaker(new Vector3(0, boundY * 8 + 1, boundZ * 8));
        RiverMaker(new Vector3(0, boundY * 8 + 1, boundZ * 8-1));
        RiverMaker(new Vector3(0, boundY * 8 + 1, boundZ * 8+1));
    }
    public void Surface() {        
        for (int i = 0; i < size.x * 16; i++)
        {
            for (int j = 0; j < size.z * 16; j++)
            {
             Vector3 block = new Vector3(i, (int)(size.y*(16)/2) +(int) Mathf.Pow(Mathf.PerlinNoise((i / rand)+300, (j / rand)+300)*4,2), j);
             Chunk chunk = GetChunk(block);
                
                if (chunk != null && block!=null ){                    
                    chunk.SetBlockType(block-chunk.position, Chunk.BlockType.grass);
                    for (int w = (int)block.y-1; w > 0; w--)
                    {
                        Vector3 underblock = new Vector3(block.x, w, block.z);
                        chunk = GetChunk(underblock);
                        chunk.SetBlockType(underblock - chunk.position, SortBlockType(underblock - chunk.position));
                    }
                }              
            }
        }

    }

    public void RiverMaker(Vector3 pos) {
        Vector3 start = pos;
        Vector3 current = start;
        Chunk chunk = GetChunk(current);
        bool issurface = false;

        while (issurface == false)
        {
            if (chunk.GetBlockType(current - chunk.position) != Chunk.BlockType.empty && chunk.GetBlockType(current - chunk.position + Vector3.up) == Chunk.BlockType.empty)
            {
                Debug.Log("ready to build at" + current);
                for (int i = 0; i < (size.x * 16); i++)
                {
                    if (i == 0){
                        chunk.SetBlockType(current - chunk.position, Chunk.BlockType.water);                        
                        CarveUp(current);
                    }
                    else current.x += 1;                    
                    chunk = GetChunk(current);
                    //goes down if the current block is air. water shouldnt float.
                    while (chunk.GetBlockType(current - chunk.position) == Chunk.BlockType.empty){
                        current.y -= 1;
                        chunk = GetChunk(current);
                        chunk.SetBlockType(current - chunk.position, Chunk.BlockType.water);                        
                        CarveUp(current);                        
                    }                    
                    chunk.SetBlockType(current - chunk.position, Chunk.BlockType.water);                    
                    CarveUp(current);
                }
                issurface = true;

            }
            else {
                current = current + Vector3.up;
            }
        }
        
        
    }
    //carves up until upper block is empty. 
    public void CarveUp(Vector3 temp) {
        while (GetBlockType(temp + Vector3.up) != Chunk.BlockType.empty)
        {
            temp.y += 1;
            Chunk tempchunk = GetChunk(temp);
            tempchunk.SetBlockType(temp - tempchunk.position, Chunk.BlockType.empty);
        }
    }


    public Chunk GetChunk(Vector3 pos) {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if ((x < (size.x * 16)) && (y < (size.y * 16)) && (z < (size.z * 16)) && (x > -1) && (y > -1) && (z > -1))
        {
            int indexX = (int)(pos.x / 16);
            int indexY = (int)(pos.y / 16);
            int indexZ = (int)(pos.z / 16);
            return chunks[indexX, indexY, indexZ];
        }
        else {
            return null;
        }
    }

    public Chunk.BlockType SortBlockType(Vector3 pos) {
        int h = (int)pos.y;
        float rand = Random.value;
        if (h >= size.y * 7)
        {
            if (rand < 0.70f)
            {
                return Chunk.BlockType.soil;
            }
            else if (rand >= 0.7f && rand <= 0.9f)
            {
                return Chunk.BlockType.stone;
            }
            else
            {//10% chance
                return Chunk.BlockType.coal;
            }
        }
        else {
            rand = rand + (pos.y / 100);
            if (rand < 0.70f)
            {
                return (Chunk.BlockType) Random.Range(3,5);
            }
            else if (rand >= 0.7f && rand <= 0.9f)
            {
                return(Chunk.BlockType) Random.Range(3, 7);
            }
            else
            {//10% chance
                return (Chunk.BlockType)Random.Range(4, 11);
            }
        }

    }

    /// <summary>
    /// returns the type of a block in the given position (world space);
    /// </summary>
    /// <param name="position"></param>
    /// <returns>Chunk.Blocktype</returns>
    public Chunk.BlockType GetBlockType(Vector3 position) {
        Chunk chunk = GetChunk(position);
        if (chunk != null)
        {
            return chunk.GetBlockType(position - chunk.position);
        }
        else {
            return Chunk.BlockType.border;
        }
    }

}
