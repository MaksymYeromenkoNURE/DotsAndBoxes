using DotsAndBoxes.Core.Models;
using DotsAndBoxes.WinForms.Animations;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotsAndBoxes.WinForms.Rendering;

public static class BoardRenderer
{
    private static readonly Color FirstPlayerColor = Color.FromArgb(37, 99, 235);
    private static readonly Color SecondPlayerColor = Color.FromArgb(234, 88, 12);

    public static void Draw(
        Graphics graphics,
        Size canvasSize,
        GameState state,
        Edge? hoveredEdge,
        LineAnimation? animation)
    {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.FromArgb(248, 250, 253));

        var geometry = BoardGeometry.Create(
            state.Rows,
            state.Columns,
            canvasSize);

        DrawBoxes(graphics, geometry, state);
        DrawGuides(graphics, geometry, state);
        DrawHover(graphics, geometry, state, hoveredEdge);
        DrawEdges(graphics, geometry, state, animation);
        DrawDots(graphics, geometry, state);
    }

    private static void DrawBoxes(Graphics graphics, BoardGeometry geometry, GameState state)
    {
        using var firstBrush = new SolidBrush(Color.FromArgb(219, 234, 254));
        using var secondBrush = new SolidBrush(Color.FromArgb(255, 237, 213));

        using var firstTextBrush = new SolidBrush(FirstPlayerColor);
        using var secondTextBrush = new SolidBrush(SecondPlayerColor);

        using var firstPen = new Pen(FirstPlayerColor, 2f);
        using var secondPen = new Pen(SecondPlayerColor, 2f);

        float fontSize = Math.Clamp(geometry.CellSize * 0.3f, 12f, 34f);

        using var font = new Font(
            FontFamily.GenericSansSerif,
            fontSize,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        foreach (var item in state.BoxOwners)
        {
            var point = geometry.GetDot(item.Key.Row, item.Key.Column);

            float inset = Math.Clamp(geometry.CellSize * 0.09f, 3f, 9f);

            var bounds = new RectangleF(
                point.X + inset,
                point.Y + inset,
                geometry.CellSize - 2 * inset,
                geometry.CellSize - 2 * inset);

            bool isFirst = item.Value == Player.First;

            graphics.FillRectangle(
                isFirst ? firstBrush : secondBrush,
                bounds);

            graphics.DrawRectangle(
                isFirst ? firstPen : secondPen,
                bounds.X,
                bounds.Y,
                bounds.Width,
                bounds.Height);

            graphics.DrawString(
                isFirst ? "1" : "2",
                font,
                isFirst ? firstTextBrush : secondTextBrush,
                bounds,
                format);
        }
    }

    private static void DrawGuides(Graphics graphics, BoardGeometry geometry, GameState state)
    {
        using var pen = new Pen(Color.FromArgb(205, 215, 228), 1.8f);

        pen.DashStyle = DashStyle.Dot;

        for (int row = 0; row <= state.Rows; row++)
        {
            for (int column = 0; column < state.Columns; column++)
            {
                DrawEdge(
                    graphics,
                    geometry,
                    new Edge(
                        row, column, EdgeOrientation.Horizontal),
                    pen);
            }
        }

        for (int row = 0; row < state.Rows; row++)
        {
            for (int column = 0; column <= state.Columns; column++)
            {
                DrawEdge(
                    graphics,
                    geometry,
                    new Edge(
                        row, column, EdgeOrientation.Vertical),
                    pen);
            }
        }
    }

    private static void DrawHover(
        Graphics graphics,
        BoardGeometry geometry,
        GameState state,
        Edge? hoveredEdge)
    {
        if (hoveredEdge is not Edge edge ||
            state.IsFinished ||
            state.DrawnEdges.Contains(edge))
            return;

        Color color = state.CurrentPlayer == Player.First
            ? FirstPlayerColor
            : SecondPlayerColor;

        float width = Math.Clamp(geometry.CellSize * 0.075f, 4f, 7f);

        using var pen = new Pen(Color.FromArgb(190, color), width);

        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;

        DrawEdge(graphics, geometry, edge, pen);
    }


    private static void DrawEdges(
        Graphics graphics,
        BoardGeometry geometry,
        GameState state,
        LineAnimation? animation)
    {
        Edge? animatedEdge = null;

        if (animation is not null &&
            animation.IsActive &&
            animation.AnimatedEdge is Edge candidate &&
            state.DrawnEdges.Contains(candidate))
        {
            animatedEdge = candidate;
        }

        using var pen = new Pen(
            Color.FromArgb(55, 65, 81), 6f);

        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;

        foreach (var edge in state.DrawnEdges)
        {
            if (animatedEdge is Edge current && edge == current)
                continue;

            DrawEdge(graphics, geometry, edge, pen);
        }

        if (animatedEdge is not Edge activeEdge ||
            animation is null)
            return;

        var (start, end) = geometry.GetEdgePoints(activeEdge);

        float progress = animation.Progress;
        float eased = progress * progress * (3f - 2f * progress);

        var currentPoint = new PointF(
            start.X + (end.X - start.X) * eased,
            start.Y + (end.Y - start.Y) * eased);

        graphics.DrawLine(pen, start, currentPoint);

        Color color = animation.PlayedBy == Player.First
            ? FirstPlayerColor
            : SecondPlayerColor;

        using var brush = new SolidBrush(color);

        graphics.FillEllipse(
            brush,
            currentPoint.X - 5f,
            currentPoint.Y - 5f,
            10f,
            10f);
    }

    private static void DrawDots(Graphics graphics, BoardGeometry geometry, GameState state)
    {
        using var brush = new SolidBrush(Color.FromArgb(31, 41, 55));

        float radius = Math.Clamp(geometry.CellSize * 0.055f, 4f, 6f);

        for (int row = 0; row <= state.Rows; row++)
        {
            for (int column = 0; column <= state.Columns; column++)
            {
                var point = geometry.GetDot(row, column);

                graphics.FillEllipse(
                    brush,
                    point.X - radius,
                    point.Y - radius,
                    radius * 2,
                    radius * 2);
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
