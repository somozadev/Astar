using UnityEngine;

public class Node 
{
    public bool walkable; //¿Se trata de un nodo obstáculo? 
    public Vector3 nodeWorldPos; //posición real del nodo (en el mundo 3D)
    public int costG; //coste G (distancia entre el nodo actual y el inicial)
    public int costH; //coste H (distancia entre el nodo actual y el final)
    public int gridX,gridY; //posicion que ocupa el nodo
    public Node parent; //nodo padre del actual

    public Node(bool _walkable, Vector3 _nodeWorldPos, int _gridX, int _gridY) //constructor
    {
        walkable = _walkable;
        nodeWorldPos = _nodeWorldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
    public int CostF { get { return costG + costH;}} //coste F (suma del coste G + H)
}
