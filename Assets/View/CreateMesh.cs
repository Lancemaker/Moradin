using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreateMesh : MonoBehaviour {
    public List<Texture> textures = new List<Texture>();
    MeshFilter filter;
    MeshRenderer meshRenderer;
    Mesh mesh;        
    List<Vector3> visibleVerts = new List<Vector3>();
    List<Vector2> finalUV = new List<Vector2>();
    List<int> tris = new List<int>();
    Map map;
    // Use this for initialization
    void Start () {
        filter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = filter.mesh;
        mesh.uv = finalUV.ToArray();       
	}   

    //render all visible chunks but not the inside chunks Should be replaced.
    public bool CreateFaces(Chunk chunk, Map map, int start) {
        this.map = map;
        for (int i = 0; i < chunk.size; i++)
        {
            for (int j = 0; j < chunk.size; j++)
            {
                for (int w = 0; w < chunk.size; w++)
                {
                    //using local positioning.
                    Vector3 b = new Vector3(i, j, w);
                    if (chunk.GetBlockType(b) != Chunk.BlockType.empty)
                    {
                        //using global positioning
                        //back
                        if (map.GetBlockType(b + Vector3.forward + chunk.position) == Chunk.BlockType.empty)
                        {                            
                        visibleVerts.AddRange(chunk.GetFace(Chunk.Face.back,b));
                        //tris - they are being added backwards using the size of the list at start. A-B-C C-A-B << this can change depending on the direction.
                        tris.Add(visibleVerts.Count - 2);//2
                        tris.Add(visibleVerts.Count - 3);//1
                        tris.Add(visibleVerts.Count - 4);//0
                        tris.Add(visibleVerts.Count - 4);//0
                        tris.Add(visibleVerts.Count - 1);//3
                        tris.Add(visibleVerts.Count - 2);//2
                        //uvs
                        SetUv(b,chunk,Chunk.Face.back,true);
                        }
                        //right
                        if (map.GetBlockType(b + Vector3.right + chunk.position) == Chunk.BlockType.empty)
                        {
                        visibleVerts.AddRange(chunk.GetFace(Chunk.Face.right, b));

                        tris.Add(visibleVerts.Count - 4);
                        tris.Add(visibleVerts.Count - 3);
                        tris.Add(visibleVerts.Count - 2);
                        tris.Add(visibleVerts.Count - 2);
                        tris.Add(visibleVerts.Count - 1);
                        tris.Add(visibleVerts.Count - 4);
                        //uvs
                        SetUv(b, chunk, Chunk.Face.right, true);
                        }
                        
                        //front
                        if (map.GetBlockType(b + Vector3.back + chunk.position) == Chunk.BlockType.empty)
                        {
                        //vertecis
                        visibleVerts.AddRange(chunk.GetFace(Chunk.Face.front, b));

                        tris.Add(visibleVerts.Count - 4);
                        tris.Add(visibleVerts.Count - 3);
                        tris.Add(visibleVerts.Count - 2);
                        tris.Add(visibleVerts.Count - 2);
                        tris.Add(visibleVerts.Count - 1);
                        tris.Add(visibleVerts.Count - 4);
                        //uvs
                        SetUv(b, chunk, Chunk.Face.front, true);

                        }
                        //left
                        if (map.GetBlockType(b + Vector3.left + chunk.position) == Chunk.BlockType.empty)
                        {
                        //face verts
                        visibleVerts.AddRange(chunk.GetFace(Chunk.Face.left, b));
                        //tris
                        tris.Add(visibleVerts.Count - 2);
                        tris.Add(visibleVerts.Count - 3);
                        tris.Add(visibleVerts.Count - 4);
                        tris.Add(visibleVerts.Count - 4);
                        tris.Add(visibleVerts.Count - 1);
                        tris.Add(visibleVerts.Count - 2);
                        //uvs
                        SetUv(b, chunk, Chunk.Face.left, true);
                        }
                        //up
                        if (map.GetBlockType(b + Vector3.up + chunk.position) == Chunk.BlockType.empty)
                        {
                            visibleVerts.AddRange(chunk.GetFace(Chunk.Face.up, b));

                            tris.Add(visibleVerts.Count - 4);
                            tris.Add(visibleVerts.Count - 3);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 1);
                            tris.Add(visibleVerts.Count - 4);
                            //uvs
                            SetUv(b, chunk, Chunk.Face.up, true);
                        }
                        //down
                        if (map.GetBlockType(b + Vector3.down + chunk.position) == Chunk.BlockType.empty)
                        {
                            visibleVerts.AddRange(chunk.GetFace(Chunk.Face.down, b));

                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 3);
                            tris.Add(visibleVerts.Count - 4);
                            tris.Add(visibleVerts.Count - 4);
                            tris.Add(visibleVerts.Count - 1);
                            tris.Add(visibleVerts.Count - 2);
                            //uvs
                            SetUv(b, chunk, Chunk.Face.down, true);
                        }
                        //if the block has a visible side, but have another block on top:                        
                        else if (map.GetBlockType(b + Vector3.up + chunk.position) != Chunk.BlockType.empty &&
                            (map.GetBlockType(b + Vector3.forward + chunk.position) == Chunk.BlockType.empty ||
                            map.GetBlockType(b + Vector3.back + chunk.position) == Chunk.BlockType.empty ||
                            map.GetBlockType(b + Vector3.left + chunk.position) == Chunk.BlockType.empty ||
                            map.GetBlockType(b + Vector3.right + chunk.position) == Chunk.BlockType.empty))
                        {
                            visibleVerts.AddRange(chunk.GetFace(Chunk.Face.up, b));

                            tris.Add(visibleVerts.Count - 4);
                            tris.Add(visibleVerts.Count - 3);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 1);
                            tris.Add(visibleVerts.Count - 4);
                            //uvs
                            SetUv(b, chunk, Chunk.Face.up, true);
                        }
                        else if (map.GetBlockType(b + Vector3.up + chunk.position) != Chunk.BlockType.empty
                            &&
                            (map.GetBlockType(b + Vector3.forward + chunk.position) != Chunk.BlockType.empty &&
                            map.GetBlockType(b + Vector3.back + chunk.position) != Chunk.BlockType.empty &&
                            map.GetBlockType(b + Vector3.left + chunk.position) != Chunk.BlockType.empty &&
                            map.GetBlockType(b + Vector3.right + chunk.position) != Chunk.BlockType.empty)
                            )
                        {
                            visibleVerts.AddRange(chunk.GetFace(Chunk.Face.up, b));

                            tris.Add(visibleVerts.Count - 4);
                            tris.Add(visibleVerts.Count - 3);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 2);
                            tris.Add(visibleVerts.Count - 1);
                            tris.Add(visibleVerts.Count - 4);
                            //uvs
                            SetUv(b, chunk, Chunk.Face.up, false);

                        }
                        
                    }

                }
            }
        }  

        mesh.Clear();
        mesh.vertices = visibleVerts.ToArray();               
        mesh.triangles = tris.ToArray();
        mesh.uv = finalUV.ToArray();
        gameObject.AddComponent<MeshCollider>();
        visibleVerts.Clear();
        tris.Clear();
        finalUV.Clear();
        return true;
    }

    void SetUv(Vector3 b, Chunk c,Chunk.Face f,bool isknown)
    {
        Vector2 step;
        if (isknown)
        {
          step = ToOcta((int)c.GetBlockType(b));
        }
        else {
          step = ToOcta((int)Chunk.BlockType.unknown);
        }
        
        

        if (c.GetBlockType(b) == Chunk.BlockType.grass)
        {
            switch (f)
            {
                case Chunk.Face.back:
                    step.x += 1;
                    break;
                case Chunk.Face.right:
                    step.x += 1;
                    break;
                case Chunk.Face.front:
                    step.x += 1;
                    break;
                case Chunk.Face.left:
                    step.x += 1;
                    break;
                case Chunk.Face.up:
                    break;
                case Chunk.Face.down:
                    step.x += 2;
                    break;
                default:
                    break;
            }
        }
        float fixTex = 0.005f;
        float xMin = 0 + (0.125f * step.x) + fixTex;
        float xMax = 0.125f + (0.125f * step.x) - fixTex;
        float yMin = 0 + (0.125f * step.y) + fixTex;
        float yMax = 0.125f + (0.125f * step.y) - fixTex;

        finalUV.Add(new Vector2(xMin, yMin));
        finalUV.Add(new Vector2(xMin, yMax));
        finalUV.Add(new Vector2(xMax, yMax));
        finalUV.Add(new Vector2(xMax, yMin));
    }

    Vector2 ToOcta(int b) {
        return new Vector2((b % 8) - 1,(int) b/8);
    }

    void HideLayer(int camposY)
    {
        int target = camposY - 10;        
        {
            if (target >= transform.position.y && target < transform.position.y + 15)
            {
                CreateFaces(map.GetChunk(gameObject.transform.position), map, target);
            }
        }
    }

}
