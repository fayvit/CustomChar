using FayvitUI_10_2020;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TesteMeshCombiner
{
    [SerializeField] private SectionCustomizationManager target;

    private GameObject parentObj;
    private GameObject gameObject;// target GameObject
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<Vector3> normals = new List<Vector3>();
    //private List<Vector4> tangents = new List<Vector4>();
    private List<List<int>> triangles = new List<List<int>>();
    private List<Material> materials = new List<Material>();
    private List<BoneWeight> boneWeights = new List<BoneWeight>();

    private int vertCount;

    private SkinnedMeshRenderer mR;

    public void StartCombiner(SectionCustomizationManager source)
    {
        target.gameObject.SetActive(true);

        triangles.Clear();
        uvs.Clear();
        materials.Clear();
        boneWeights.Clear();
        normals.Clear();
        vertices.Clear();

        CustomizationContainerDates ccd =  source.GetCustomDates();
        UiSupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            target.SetCustomDates(ccd);

            UiSupportSingleton.Instance.InvokeOnEndFrame(() =>
            {

                CombinerReallyStart();

            });
        });

        
    }

    void CombinerReallyStart()
    {
        GameObject paiDeTodos = new GameObject("PaiDeTodos");
        paiDeTodos.transform.position = target.transform.position;
        gameObject = new GameObject();
        gameObject.transform.SetParent(paiDeTodos.transform);
        gameObject.transform.localPosition = Vector3.zero;
        mR = gameObject.AddComponent<SkinnedMeshRenderer>();

        parentObj = target.gameObject;

        Debug.Log("Really starting combine Mesh");

        Transform[] children = parentObj.GetComponentsInChildren<Transform>();

        SkinnedMeshRenderer meshRr = null;
        foreach (var child in children)
        {
            SkinnedMeshRenderer meshR = child.GetComponent<SkinnedMeshRenderer>();

            if (meshR != null && !meshR.gameObject.CompareTag("skCabelo"))
            {
                meshRr = meshR;
                foreach (var mat in meshR.sharedMaterials)
                {
                    if (!materials.Contains(mat))
                    {
                        materials.Add(mat);
                        triangles.Add(new List<int>());
                    }
                }

                for (int i = 0; i < meshR.sharedMesh.vertices.Length; i++)
                {
                    vertices.Add(child.TransformPoint(meshR.sharedMesh.vertices[i]) - parentObj.transform.position);

                    if(meshR.sharedMesh.boneWeights.Length>i)
                        boneWeights.Add(meshR.sharedMesh.boneWeights[i]);
                    else
                        boneWeights.Add(new BoneWeight() { boneIndex0=0,weight0=1});
                }

                for (int i = 0; i < meshR.sharedMesh.subMeshCount; i++)
                {
                    int triIndex = GetTrianglesIndex(meshR.sharedMaterials[i]);
                    
                    int[] tris = meshR.sharedMesh.GetTriangles(i);

                    for (int t = 0; t < tris.Length; t++)
                    {
                        triangles[triIndex].Add(vertCount + tris[t]);
                    }
                }

                //tangents.AddRange(meshR.sharedMesh.tangents);
                int q = normals.Count;
                uvs.AddRange(meshR.sharedMesh.uv);
                normals.AddRange(meshR.sharedMesh.normals);                
                vertCount = vertices.Count;

                for (int i = q; i < normals.Count; i++)
                {
                    normals[i] = child.TransformPoint(normals[i]) - parentObj.transform.position;
                }

            }
        }

        


        Mesh mesh = new Mesh();

        if (vertices.Count > 65535)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.subMeshCount = triangles.Count;
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.boneWeights = boneWeights.ToArray();        

        mesh.normals = normals.ToArray();

        for (int i = 0; i < triangles.Count; i++)
        {
            mesh.SetTriangles(triangles[i].ToArray(), i);
        }

        mR.bones = meshRr.bones;
        mR.rootBone = meshRr.bones[0];

        var bindPoses = new Matrix4x4[mR.bones.Length];

        for (int i = 0; i < bindPoses.Length; i++)
        {
            bindPoses[i] = mR.bones[i].worldToLocalMatrix * gameObject.transform.localToWorldMatrix;
        }

        mesh.bindposes = bindPoses;
        
        mR.sharedMaterials = materials.ToArray();
        
        //mesh.tangents = tangents.ToArray();

        //RemoveDuplicateVertices(mesh);

        mR.sharedMesh = mesh;

        foreach (var child in children)
        {
            SkinnedMeshRenderer meshR = child.GetComponent<SkinnedMeshRenderer>();

            if (meshR != null && !meshR.gameObject.CompareTag("skCabelo"))
            {
                MonoBehaviour.Destroy(meshR.gameObject);
            }
        }

        gameObject.transform.SetParent(parentObj.transform);
        parentObj.GetComponent<Animator>().enabled = true;
        parentObj.SetActive(false);
        UiSupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            parentObj.SetActive(true);
        });
    }

    class MeshDates
    {
        public List<Vector3> lV = new List<Vector3>();
        public List<BoneWeight> lB = new List<BoneWeight>();
        public List<List<int>> lT = new List<List<int>>();

    }
    void RemoveDuplicateVertices(Mesh mesh)
    {
        MeshDates M = new MeshDates();
        
        M.lV.AddRange(mesh.vertices);
        M.lB.AddRange(mesh.boneWeights);
        M.lT.AddRange(triangles);

        bool repita;
        do
        {
            repita = VerifiqueIgualdade(M);
        } while (repita);


        Debug.Log("numero de vertices: " + M.lV.Count+" numero de ossos: "+M.lB.Count);
        mesh.vertices = M.lV.ToArray();
        mesh.boneWeights = M.lB.ToArray();

        for (int i = 0; i < M.lT.Count; i++)
        {
            mesh.SetTriangles(M.lT[i].ToArray(), i);
        }

    }

    bool VerifiqueIgualdade(MeshDates M)
    {
        for (int i = 0; i <M.lV.Count; i++)
        {
            for (int j = 0; j < M.lV.Count; j++)
            {
                if (i != j)
                {
                    if (M.lV[i] == M.lV[j])
                    {
                        M.lV.RemoveAt(j);
                        M.lB.RemoveAt(j);

                        ReplaceTrianglesID(M, i, j);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void ReplaceTrianglesID(MeshDates M,int i, int j)
    {
        for (int I = 0; I < M.lT.Count; I++)
        {
            for (int J = 0; J < M.lT[I].Count; J++)
            {
                if (M.lT[I][J] == j)
                    M.lT[I][J] = i;
            }
        }
    }

    Transform[] MetarigToArray(GameObject toArray,Transform[] reference,string ObjectName,string rigName)
    {
        Transform T = toArray.transform;
        List<Transform> tt = new List<Transform>();

        foreach (var tRef in reference)
        {
            Transform finded = T.Find(CaminhoDoFilho(tRef,ObjectName,rigName));

            if (finded is null)
            {
                Debug.Log("Encontrei um nulo");
            }
            else
            {
                tt.Add(finded);
            }
        }

        return tt.ToArray();
       
    }

    string CaminhoDoFilho(Transform tRef,string objectName,string metaRigName)
    {
        string s = tRef.name;
        Transform otoTref=tRef;
        while (otoTref.parent != null)
        {
            otoTref = otoTref.parent;

            if(otoTref.name!=objectName && otoTref.name!=metaRigName)
                s = otoTref.name + "/" + s;
        }

        Debug.Log(s);

        return s;
    }

    int GetTrianglesIndex(Material material)
    {
        int index = 0;

        for (int i = 0; i < materials.Count; i++)
        {
            if (material == materials[i])
            {
                index = i;
                break;
            }
        }

        return index;
    }
}