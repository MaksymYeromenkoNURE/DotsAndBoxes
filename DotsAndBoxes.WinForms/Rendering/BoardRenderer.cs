using System.Drawing;
using System.Drawing.Drawing2D;
using DotsAndBoxes.Core.Models;

namespace DotsAndBoxes.WinForms.Rendering;

public static class BoardRenderer
{
    public static void Draw(
        Graphics graphics,
        Size canvasSize,
        GameState state,
        Edge? hoveredEdge)
    {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.FromArgb(248, 250, 253));

        var geometry = BoardGeometry.Create(
            state.Rows,
            state.Columns,
            canvasSize);

        using var firstPlayerBrush = new SolidBrush(Color.FromArgb(205, 228, 255));
        using var secondPlayerBrush = new SolidBrush(Color.FromArgb(255, 224, 196));

        foreach (var item in state.BoxOwners)
        {
            var topLeft = geometry.GetDot(
                item.Key.Row,
                item.Key.Column);

            const float inset = 7f;

            var brush = item.Value == Player.First
                ? firstPlayerBrush
                : secondPlayerBrush;

            graphics.FillRectangle(
                brush,
                topLeft.X + inset,
                topLeft.Y + inset,
                Math.Max(1f, geometry.CellSize - 2 * inset),
                Math.Max(1f, geometry.CellSize - 2 * inset));
        }

        using var guidePen = new Pen(Color.FromArgb(214, 222, 233), 2f);

        guidePen.DashStyle = DashStyle.Dot;

        for (int row = 0; row <= state.Rows; row++)
        {
            for (int column = 0; column < state.Columns; column++)
            {
                var edge = new Edge(
                    row, column, EdgeOrientation.Horizontal);

                DrawEdge(graphics, geometry, edge, guidePen);
            }
        }

        for (int row = 0; row < state.Rows; row++)
        {
            for (int column = 0; column <= state.Columns; column++)
            {
                var edge = new Edge(
                    row, column, EdgeOrientation.Vertical);

                DrawEdge(graphics, geometry, edge, guidePen);
            }
        }

        if (hoveredEdge is Edge hovered &&
            !state.IsFinished &&
            !state.DrawnEdges.Contains(hovered))
        {
            using var hoverPen = new Pen(
                Color.FromArgb(80, 145, 220), 5f);

            hoverPen.StartCap = LineCap.Round;
            hoverPen.EndCap = LineCap.Round;

            DrawEdge(graphics, geometry, hovered, hoverPen);
        }

        using var edgePen = new Pen(Color.FromArgb(49, 72, 112), 6f);

        edgePen.StartCap = LineCap.Round;
        edgePen.EndCap = LineCap.Round;

        foreach (var edge in state.DrawnEdges)
        {
            DrawEdge(graphics, geometry, edge, edgePen);
        }

        using var dotBrush = new SolidBrush(Color.FromArgb(35, 52, 83));

        for (int row = 0; row <= state.Rows; row++)
        {
            for (int column = 0; column <= state.Columns; column++)
            {
                var point = geometry.GetDot(row, column);

                graphics.FillEllipse(
                    dotBrush,
                    point.X - 5,
                    point.Y - 5,
                    10,
                    10);
            }
        }
    }

    private static void DrawEdge(
        Graphics graphics,
        BoardGeometry geometry,
        Edge edge,
        Pen pen)
    {
        var (start, end) = geometry.GetEdgePoints(edge);

        graphics.DrawLine(pen, start, end);
    }
}
