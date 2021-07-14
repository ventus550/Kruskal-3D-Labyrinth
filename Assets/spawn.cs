using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Edges = System.Collections.Generic.HashSet<UnityEngine.Vector2>;

public class spawn : MonoBehaviour
{

    public GameObject Unit;
    public GameObject Vertex;

    public Vector3 Position;

    public int Size = 10;

    Edges generateEdges() {
        float vertex(Vector2 vect) { return vect[0] * Size + vect[1]; }
        
        Edges Edges = new Edges();
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++) {
                float v = vertex(new Vector2(i, j));

                if (i - 1 >= 0) { Edges.Add(new Vector2(v, vertex(new Vector2(i-1, j)))); }
                if (j - 1 >= 0) { Edges.Add(new Vector2(v, vertex(new Vector2(i, j-1)))); }

            }
        return Edges;
    }

    void Kruskal(int k = 0) {

        

        Edges edges = generateEdges();
        Edges edgesFullSet = new Edges(edges);
        
        Dictionary<float, HashSet<float>> Trees = new Dictionary<float, HashSet<float>>(2 * Size * Size);
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
                Debug.Log(np);
            }
        }


        Vector2 coords(float v) {
            Vector2 virt = new Vector2((int)(v / Size), v % Size);
            return (virt - new Vector2(Size/2, Size/2)) * 2;
        }

        edgesFullSet.ExceptWith(MST);
        foreach (Vector2 p in edgesFullSet) {
            Vector2 pos = (coords(p[0]) + coords(p[1])) / 2;
            Instantiate(Unit, new Vector3(pos[0], k, pos[1]), Quaternion.identity);
        }
    }           


    // Start is called before the first frame update
    void Start()
    {

        for (int i = -10; i < 10; i+=2) {
            for (int j = -10; j < 10; j+=2) {
                // if ((i + j) % 2 != 0)
                Instantiate(Vertex, new Vector3(i, 0, j), Quaternion.identity);
            }
        }

        for (int i = -10; i < 8; i+=2) {
            for (int j = -10; j < 8; j+=2) {
                // if ((i + j) % 2 != 0)
                Instantiate(Unit, new Vector3(i+1, 0, j+1), Quaternion.identity);
            }
        }

        // for (int i = -10; i <= 10; i++) {
        //     for (int j = -10; j <= 10; j++) {
        //         if ( (i*j) % 2 == 0 && (i+j) % 2 != 0)
        //             Instantiate(Unit, new Vector3(i, 0, j), Quaternion.identity);
        //     }
        // }

        Kruskal();
        //Kruskal(1);


    }
}
