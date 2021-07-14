using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Edges = System.Collections.Generic.HashSet<UnityEngine.Vector2>;

public class Generate : MonoBehaviour
{

    public GameObject Unit;
    public GameObject Vertex;
    public GameObject Player;

    public Vector3 Position;

    public int Size = 10;
    public int Layers = 10;

    void Make(GameObject obj, Vector3 pos) {
        obj = Instantiate(obj, pos, Quaternion.identity);
        obj.GetComponent<Hide>().Player = Player;
    }

    Edges generateEdges(int layers = 2) {
        float vertex(Vector3 vect) { return vect[0]*Size*Size + vect[1]*Size + vect[2]; }
        
        Edges Edges = new Edges();
        for (int h = 0; h < layers; h++)
            for (int i = 0; i < Size; i++) 
                for (int j = 0; j < Size; j++) {
                    float v = vertex(new Vector3(h, i, j));

                    if (h - 1 >= 0) { Edges.Add(new Vector2(v, vertex(new Vector3(h-1, i, j)))); }
                    if (i - 1 >= 0) { Edges.Add(new Vector2(v, vertex(new Vector3(h, i-1, j)))); }
                    if (j - 1 >= 0) { Edges.Add(new Vector2(v, vertex(new Vector3(h, i, j-1)))); }

            }
        return Edges;
    }

    void generateVertices(int layers = 2) {
        for (int h = 0; h < 2*layers; h+=2) {
            for (int i = -10; i < 10; i+=2) {
                for (int j = -10; j < 10; j+=2) {
                    Make(Vertex, new Vector3(i, h - 0.6f, j));
                }
            }

            for (int i = -10; i < 8; i+=2) {
                for (int j = -10; j < 8; j+=2) {
                    Make(Unit, new Vector3(i+1, h, j+1));
                }
            }

            if (h == 0) {continue;}
            for (int i = -10; i < 9; i++) {
                for (int j = -10; j < 9; j++) {
                    if ((i + j) % 2 != 0 || (i*j) % 2 != 0)
                        Make(Unit, new Vector3(i, h-1, j));
                }
            }
        }
    }

    void Kruskal(int layers = 0) {

        generateVertices(layers);
        Edges edges = generateEdges(layers);
        Edges edgesFullSet = new Edges(edges);
        
        Dictionary<float, HashSet<float>> Trees = new Dictionary<float, HashSet<float>>();
        HashSet<float> tree(float v) {
            try { return Trees[v]; }
            catch {
                Trees[v] = new HashSet<float>(){v};
                return Trees[v];
            }
        }

        Edges MST = new Edges();
        Vector2 pop() {
            int r = Random.Range(0, edges.Count);
            Vector2 v = edges.ElementAt(r);
            edges.Remove(v);
            return v;
        } 

        while (edges.Count != 0) {
            Vector2 p = pop();
            float v = p[0], u = p[1];
            if (tree(v) != tree(u)) {
                Vector2 np = new Vector2(v, u);
                MST.Add(np);
                tree(v).UnionWith(tree(u));
                foreach (int r in tree(v)) { Trees[r] = tree(v); }
            }   
        }


        Vector3 coords(float v) {
            float sq = Size*Size;
            float X = (int)((v % sq) / Size),
                  H = (int)(v / sq),
                  Z = v % Size;
            Vector3 virt = new Vector3(X, H, Z);
            return (virt - new Vector3(Size/2, 0, Size/2)) * 2;
        }

        edgesFullSet.ExceptWith(MST);
        foreach (Vector2 p in edgesFullSet) {
            Vector3 pos = (coords(p[0]) + coords(p[1])) / 2;
            Make(Unit, pos);
        }
    }           


    // Start is called before the first frame update
    void Start()
    {

        

        

        // for (int i = -10; i <= 10; i++) {
        //     for (int j = -10; j <= 10; j++) {
        //         if ( (i*j) % 2 == 0 && (i+j) % 2 != 0)
        //             Instantiate(Unit, new Vector3(i, 0, j), Quaternion.identity);
        //     }
        // }

        Kruskal(Layers);


    }
}
