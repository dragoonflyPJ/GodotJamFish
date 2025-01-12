using Godot;
using System;

public partial class LinePlayerScr : Line2D
{
    // Número de puntos en la línea
    private int numPoints = 20;
    private Vector2[] points;
    private float pointDistance = 10.0f;
    private float maxAngle = Mathf.Pi / 6.0f; // Ángulo máximo permitido entre segmentos (30 grados)

    public override void _Ready()
    {
        // Inicializamos los puntos de la línea
        points = new Vector2[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector2(i * pointDistance, 0);
            AddPoint(points[i]);
        }

        QueueRedraw();
    }

    public override void _Process(double delta)
    {
        // Obtén la posición del mouse
        Vector2 mousePos = GetGlobalMousePosition();

        // Mueve el primer punto a la posición del mouse
        points[0] = mousePos;
        SetPointPosition(0, points[0]);

        // Calcula las nuevas posiciones de los otros puntos con restricción de ángulo
        for (int i = 1; i < numPoints; i++)
        {
            Vector2 direction = (points[i - 1] - points[i]).Normalized();
            Vector2 newPos = points[i - 1] - direction * pointDistance;

            // Calcula el ángulo entre el punto actual y el nuevo
            float angle = points[i - 1].AngleTo(newPos);

            // Ajusta la posición para no exceder el ángulo máximo permitido
            if (Mathf.Abs(angle) > maxAngle)
            {
                float limitedAngle = Mathf.Sign(angle) * maxAngle;
                newPos = points[i - 1] + new Vector2(Mathf.Cos(limitedAngle), Mathf.Sin(limitedAngle)) * pointDistance;
            }

            points[i] = newPos;
            SetPointPosition(i, points[i]);
        }

        // Redibuja la línea y los círculos
        QueueRedraw();
    }

    public override void _Draw()
    {
        // Dibuja los puntos como círculos
        for (int i = 0; i < numPoints; i++)
        {
            DrawCircle(points[i], 5, new Color(1, 0, 0)); // Ajusta el radio y el color del círculo aquí
        }
    }
}







