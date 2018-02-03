using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    //variables
    GameObject chunkPool;
    GameObject chunkPrefab;
    bool ChunksDataCreated = false;
    Camera cam;
    Map map;
    List<GameObject> ViewChunks = new List<GameObject>();
    //delegates and events    

    //end of delegates and events   
    void Start () {
        cam = Camera.main;        
        chunkPrefab = (GameObject)Resources.Load("Prefabs/chunk");
        map= new Map();
        Debug.Log("map ready");
        cam.gameObject.transform.position = new Vector3((int)map.size.x * 8, map.size.y * 10 +8.01f, (int)map.size.z * 8);
        chunkPool = gameObject.transform.Find("ChunkPool").gameObject;
        StartCoroutine(FillChunkData(map));        
    }

   



    IEnumerator FillChunkData(Map map) {
        float time = Time.timeSinceLevelLoad;
        GameObject chunk;
        float onepercent = map.chunks.Length * 0.01f;
        //float count = 0f;
        foreach (Chunk c in map.chunks) {
          chunk  = Instantiate(chunkPrefab);
          chunk.transform.SetParent(chunkPool.transform);
          chunk.transform.position = c.position;
          chunk.name=("chunk@"+c.position.x+"_"+c.position.y+"_" + c.position.z);
          yield return new WaitForFixedUpdate();
          chunk.GetComponent<CreateMesh>().CreateFaces(c,map,(int)(map.size.y * 8));
          ViewChunks.Add(chunk);

        }
        Debug.Log("process took"+ (Time.time - time) +"seconds");       
    }
}
