using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    //block Stuff
    public enum BlockType : byte
    {
        empty,
        grass,
        soil=3,
        stone,
        coal,
        iron,
        copper,
        tin,
        silver,
        gold, 
        unknown,
        water,
        ///
        border = 255
    }    
    public enum Face : int
    {
        back,
        right,
        front,
        left,
        up,
        down
    }
    BlockType type = BlockType.empty;
    void start() {

    }
   

    //chunk stuff
    public byte size = 16;
    public Vector3 position;
    public Vector3[,,] blocks;
    public BlockType[] blocktypeArray;    
    public Chunk(Vector3 pos) {
        position = pos;        
        FillChunk(size);        
    }
    //fill the chunk with the block information
    void FillChunk(int size) {
        //blocks = new Vector3[size, size, size];
        blocktypeArray = new BlockType[size * size * size];
        for (byte i = 0; i < size; i++)
        {           
            for (byte j = 0; j < size; j++)
            {                
                for (byte w = 0; w < size; w++)
                {
                    //blocks[i,j,w] = new Vector3(i,j,w); 
                    blocktypeArray[i + size * (j + size * w)] = BlockType.empty;
                }
            }     
        }        
    }

    public Vector3[] GetFace(Face f,Vector3 position) {
        Vector3[] face = new Vector3[4];        
        switch (f) {
            case Face.back:
                face[0]=position + Vector3.forward;
                face[1]=position + Vector3.forward + Vector3.up;
                face[2]=position + Vector3.forward + Vector3.up + Vector3.right;
                face[3]=position + Vector3.forward + Vector3.right;
                break;
            case Face.right:
                face[0]=position + Vector3.right ;
                face[1]=position + Vector3.right + Vector3.up ;
                face[2]=position + Vector3.right + Vector3.up + Vector3.forward;
                face[3]=position + Vector3.right + Vector3.forward;
                break;
            case Face.front:
                face[0]=position;
                face[1]=position + Vector3.up;
                face[2]=position + Vector3.up + Vector3.right;
                face[3]=position + Vector3.right;
                break;
            case Face.left:
                face[0]=position;
                face[1]=position + Vector3.up;
                face[2]=position + Vector3.up + Vector3.forward;
                face[3]=position + Vector3.forward;
                break;
            case Face.up:
                face[0]=position + Vector3.up;
                face[1]=position + Vector3.up + Vector3.forward;
                face[2]=position + Vector3.up + Vector3.forward + Vector3.right;
                face[3]=position + Vector3.up + Vector3.right;
                break;
            case Face.down:
                face[0]=position;
                face[1]=position + Vector3.forward;
                face[2]=position + Vector3.forward + Vector3.right;
                face[3]=position + Vector3.right;
                break;
            default:
                Debug.Log("error face nonExistant");
                break;
        }
        return face;
    }

    //needs testing
    public BlockType GetBlockType(Vector3 pos) {
        int x = (int)(pos.x);
        int y = (int)(pos.y);
        int z = (int)(pos.z);      
        if ((x < size)  && (y < size) && (z < size) && (x>-1) && (y>-1) && (z>-1))
        {
            return blocktypeArray[x + size * (y + size * z)];
        }
        else {
            return BlockType.border;
        }        
    }
    public void SetBlockType(Vector3 pos,BlockType type) {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        if ((x < size) && (y < size) && (z < size) && (x > -1) && (y > -1) && (z > -1))
        {
           blocktypeArray[x + size * (y + size * z)]=type;
        }        
    }   
}
