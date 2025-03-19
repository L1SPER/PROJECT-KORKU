using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Point
{
    public float x, y;
    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
[System.Serializable]
public class Triangle
{
    public Point p1, p2, p3;

    public Triangle(Point p1, Point p2, Point p3)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }

    // Üçgenin çevresini hesapla
    public float GetCircumradius()
    {
        // Basit bir yöntemle üçgenin çevresel yarıçapını hesapla
        float ax = p1.x, ay = p1.y;
        float bx = p2.x, by = p2.y;
        float cx = p3.x, cy = p3.y;

        float d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
        float ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
        float uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

        float radius = Mathf.Sqrt((ux - ax) * (ux - ax) + (uy - ay) * (uy - ay));
        return radius;
    }

    // Üçgenin içinde olup olmadığını kontrol et
    public bool IsPointInside(Point p)
    {
        // Bu basit bir içerik kontrolü değil, sadece üçgenin çevresel sınırını kullanarak
        float radius = GetCircumradius();
        float dist = Mathf.Sqrt((p.x - p1.x) * (p.x - p1.x) + (p.y - p1.y) * (p.y - p1.y));
        return dist <= radius;
    }
}
public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Point[] superTrianglePoints;
    // Delaunay Triangulation'ı başlatmak için kullanılacak metod
    public List<Triangle> GenerateTriangulation(List<Point> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        // 1. Başlangıçta büyük bir dış üçgen oluştur
        // Bu dış üçgen harita sınırlarını kapsayacak şekilde seçilir
        Point p1 = superTrianglePoints[0];
        Point p2 = superTrianglePoints[1];
        Point p3 = superTrianglePoints[2];

        Triangle initialTriangle = new Triangle(p1, p2, p3);
        triangles.Add(initialTriangle);

        // 2. Noktaları sırayla ekleyin
        foreach (Point point in points)
        {
            List<Triangle> badTriangles = new List<Triangle>();

            // 3. Tüm üçgenleri kontrol et ve bu noktayı içerenleri al
            foreach (Triangle triangle in triangles)
            {
                if (triangle.IsPointInside(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            // 4. Kötü üçgenlerden yeni üçgenler oluştur
            foreach (Triangle badTriangle in badTriangles)
            {
                // Yeni üçgenler oluşturulacak
                Triangle newTriangle1 = new Triangle(badTriangle.p1, badTriangle.p2, point);
                Triangle newTriangle2 = new Triangle(badTriangle.p2, badTriangle.p3, point);
                Triangle newTriangle3 = new Triangle(badTriangle.p3, badTriangle.p1, point);
                triangles.Add(newTriangle1);
                triangles.Add(newTriangle2);
                triangles.Add(newTriangle3);
            }
            // 5. Eski üçgenleri sil    
            triangles.RemoveAll(t => badTriangles.Contains(t));
        }
        // Delaunay Triangulation tamamlandıktan sonra büyük üçgene bağlı üçgenleri temizle
        triangles = triangles.Where(t => !(t.p1 == p1||t.p1 == p2||  t.p1 == p3 ||
                                   t.p2 == p1 || t.p2 == p2||  t.p2 == p3 ||
                                   t.p3 == p1 || t.p3 == p2 || t.p3 == p3)).ToList();
        return triangles;
    }

    // Örnek kullanım
    /* void Start()
    {
        List<Point> points = new List<Point>
        {
            new Point(10, 20),
            new Point(30, 40),
            new Point(50, 60),
            new Point(70, 80),
            new Point(90, 100)
        };

        List<Triangle> triangulation = GenerateTriangulation(points);

        // Triangulasyonu çizme işlemi
        foreach (var triangle in triangulation)
        {
            Debug.Log("Triangle: " + triangle.p1.x + ", " + triangle.p1.y + " -> " + triangle.p2.x + ", " + triangle.p2.y + " -> " + triangle.p3.x + ", " + triangle.p3.y);
        }
    } */
}