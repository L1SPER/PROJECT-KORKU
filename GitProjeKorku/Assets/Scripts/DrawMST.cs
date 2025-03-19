using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class DrawMST 
{
   public static void DrawEdges(List<Edge> edges)
   {
       foreach (var edge in edges)
       {
           Debug.DrawLine(new Vector3(edge.nodeA.x, 0, edge.nodeA.y), new Vector3(edge.nodeB.x, 0, edge.nodeB.y), Color.red, 1000f);
       }
   }
   
}
