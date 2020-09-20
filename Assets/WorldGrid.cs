using UnityEngine;
using System.Collections.Generic;

public class WorldGrid : MonoBehaviour
{
    public Transform pointA;
    
    public LayerMask obstacleMask; //la capa propia de los obstáculos 
    public Vector2 gridWorldSize; //tamaño del grid 2D en unidades de unity
    public float nodeRadius; //radio del nodo
    
    public List<Node> path; //declaramos la lista de nodos final; lista path

    Node[,] grid; //grid bidimensional de nodos 
    float nodeDiameter; //dos veces el radio del nodo
    int gridSizeX,gridSizeY; //tamaño X e Y del grid en unidades de nodos


    void Start()
    {
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //guardar el tamaño del grid en función de los nodos en X 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter); //guardar el tamaño del grid en función de los nodos en Y 
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSizeX,gridSizeY]; //crear un grid con las medidas en base nodo
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        //este vector nos sitúa en la esquina inferior, resta al centro del grid la mitad del tamaño del grid en X y en Y dando el punto inferior izquierdo
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter+nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius); //busca la posicion real de cada nodo partiendo de la esquina inferior derecha 
                bool obstacle = (Physics.CheckSphere(worldPoint,nodeRadius,obstacleMask));//comprueba si en el nodo en el que se encuentra hay un objeto en la escena, si es así el nodo en cuestión pasa a not walkeable (nodo obstáculo)
                grid[x,y] = new Node(obstacle,worldPoint,x,y); //actualiza el grid con la nueva información
                
            }
        }
    }

    public List<Node> GetNeighbours(Node node) 
    {
        List<Node> neigbours = new List<Node>();  //instanciar una lista de nodos vecinos al actual

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++) //estos dobles for recorren los nodos colidantes al nodo dado, incluyendo las diagonales
            {
                if(x == 0 && y == 0) //si llegamos al nodo dado saltarlo
                    continue;
                
                int checkX = node.gridX + x; //guardamos la posición X del nodo vecino
                int checkY = node.gridY + y; //guardamos la posición Y del nodo vecino 

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//como empezamos en la esquina derecha inferior sólo nos interesan los nodos en x e y positivos que estén dentro del grid
                {
                    neigbours.Add(grid[checkX,checkY]); //y estos nodos los añadimos a la lista
                }
            }
        }
        return neigbours;
    }




    public Node NodeFromWorldPoint(Vector3 worldPosition) //pasar de la pos 3D de escena al grid 2D para un punto
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y)); //limites del grid
        if(grid!=null) //si existe grid
        {
            Node pointANode = NodeFromWorldPoint(pointA.position); //dibujar nodoA
            foreach (Node node in grid) //dibujar nodos dentro del grid
            {
                Gizmos.color = (node.walkable)?Color.blue:Color.white; //si el nodo es un obstaculo = blue, si no = white
                if(pointANode == node)
                    Gizmos.color = Color.green;
                if(path!=null)
                    if(path.Contains(node))
                        Gizmos.color = Color.red;
                Gizmos.DrawCube(node.nodeWorldPos,Vector3.one * (nodeDiameter-0.1f));
            }
        }
    }

}
