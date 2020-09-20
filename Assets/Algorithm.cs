using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Algorithm : MonoBehaviour
{
    WorldGrid grid; //instancia del gridscript
    public Transform seekerObject; //nodo (objeto) final 
    public Transform targetObject; //nodo (objeto) inicial
    void Awake()
    {
        grid = GetComponent<WorldGrid>();
    }

    void Update()
    {
        FindPath(seekerObject.position,targetObject.position); //función de encontrar el path óptimo
        
    }

    void FindPath(Vector3 pointA, Vector3 pointB)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node initialNode = grid.NodeFromWorldPoint(pointA); //nodo inicial 
        Node finalNode = grid.NodeFromWorldPoint(pointB);//nodo final 

        List<Node> openList = new List<Node>(); //lista abierta con nodos a comprobar
        HashSet<Node> closedList = new HashSet<Node>();//lista cerrada, la final
        openList.Add(initialNode);//añadir el nodo inicial a la lista abierta

        while(openList.Count > 0) //mientras que la lista abierta contenga nodos
        {
            Node currentNode = openList[0]; //nodo actual es el primer nodo de la lista
            for(int i = 1; i < openList.Count; i++) //recorremos la lista abierta
            {
                if(openList[i].CostF < currentNode.CostF || openList[i].CostF == currentNode.CostF) //si el coste f de el nodo en la posición i de la lista es menor o igual que el coste F del primer nodo
                {
                    if(openList[i].costH < currentNode.costH)//y si el coste h de el nodo en la posición i es menor que el coste h del primer nodo de la lista
                    currentNode = openList[i]; //el nodo actual pasa a ser el nodo en la posición i con el coste H menor que el anterior current node
                }
            }
            openList.Remove(currentNode);//eliminar de la lista abierta el nodo actual, el de menor coste 
            closedList.Add(currentNode);//añadimos dicho nodo a la lista cerrada

            if(currentNode == finalNode) //si el nodo añadido corresponde al nodo final
            {
                sw.Stop();
                print("path found in :" + sw.ElapsedMilliseconds + "ms");
                RetracePath(initialNode,finalNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)) //recorremos la lista de nodos vecinos al actual
            {
                if(neighbour.walkable || closedList.Contains(neighbour)) //si el nodo no es un obstáculo y está en la lista cerrada
                    continue;//saltarlo
                int newMovementCostToNeighbour = currentNode.costG + GetDistance(currentNode,neighbour);//coste al neighbour posible nuevo nodo para la lista (costeG del nodo actual mas distancia entre el nodo actual y el neighbour)
                if(newMovementCostToNeighbour < neighbour.costG || !openList.Contains(neighbour))//si este coste es menor que el coste G del vecino o el vecino no está contenido en la lista abierta
                {
                    neighbour.costG = newMovementCostToNeighbour; //el coste G del vecino pasa a ser el coste newMovementCostToNeighbour
                    neighbour.costH = GetDistance(neighbour,finalNode); //el coste H del vecino pasa a ser la distancia entre dicho vecino y el nodo final
                    neighbour.parent = currentNode; //ahora el que era el current node pasa a ser el padre de neighbour
                    if(!openList.Contains(neighbour))//si neighbour no está en la lista abierta lo añadimos
                    {
                        openList.Add(neighbour);
                    }
                }
            }

            

        }
    }
    
    int GetDistance(Node nodeA, Node nodeB) //cálculo del a distancia entre dos nodos 
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);//distancia en el ejeX
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);//distancia en el ejeY

        //calcular distancias diagonales: el coste diagonal es la hipotenusa que vale 1,4 pero multiplicamos todos los costes G y H por 10 para tener enteros
        if(distX > distY)//si la distancia en el ejeX es mayor que la distancia en el ejeY
            return 14*distY + 10* (distX-distY); //aplicar algoritmo en este orden
        return 14*distX + 10* (distY-distX);//si no en este otro
        //ambos return nos dan la distancia (multiplicada por 10) entre el nodoA y el nodoB
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>(); //instanciamos una lista de nodos path, la que será el camino óptimo
        Node currentNode = endNode; //el nodo actual pasa a ser el nodo final

        while(currentNode != startNode) //mientras que el nodo actual no sea el nodo inicial
        {
            path.Add(currentNode);//añadimos el nodo actual a la lista path
            currentNode = currentNode.parent;//y el nodo actual pasa a ser el padre del mismo
        }
        path.Reverse(); //una vez terminado de añadir a la lista hacemos el reverse para crear el camino ordenado del nodo inicial al nodo final
        grid.path = path; //guardamos este path completo en la lista path del script WorldGrid
    }

    
}
