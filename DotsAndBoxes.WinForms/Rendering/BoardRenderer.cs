using DotsAndBoxes.Core.Models;
using DotsAndBoxes.WinForms.Animations;
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
        LineAnimation? lineAnimation,
        HoverAnimation? hoverAnimation,
        CaptureAnimation? captureAnimation)
    {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.FromArgb(248, 250, 253));

        var geometry = BoardGeometry.Create(
            state.Rows,
            state.Columns,
            canvasSize);

        DrawBoxes(graphics, geometry, state, captureAnimation);
        DrawGuides(graphics, geometry, state);
        DrawHover(graphics, geometry, state, hoveredEdge, hoverAnimation);
        DrawEdges(graphics, geometry, state, lineAnimation);
        DrawCaptures(graphics, geometry, captureAnimation);
        DrawDots(graphics, geometry, state);
    }

    private static void DrawBoxes(Graphics graphics,
        BoardGeometry geometry,
        GameState state,
        CaptureAnimation? animation)
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
            if (animation is not null &&
                animation.Contains(item.Key))
            {
                continue;
            }

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
        Edge? hoveredEdge,
        HoverAnimation? animation)
    {
        if (hoveredEdge is not Edge edge ||
            state.IsFinished ||
            state.DrawnEdges.Contains(edge))
            return;

        Color color = state.CurrentPlayer == Player.First
            ? FirstPlayerColor
            : SecondPlayerColor;

        float width = Math.Clamp(
            geometry.CellSize * 0.075f, 4f, 7f);

        using var basePen = new Pen(
            Color.FromArgb(95, color), width);

        basePen.StartCap = LineCap.Round;
        basePen.EndCap = LineCap.Round;

        DrawEdge(graphics, geometry, edge, basePen);

        if (animation is null || !animation.IsActive)
            return;

        var (start, end) = geometry.GetEdgePoints(edge);

        float progress = animation.Progress;
        float startProgress = Math.Max(0f, progress - 0.25f);

        var waveStart = new PointF(
            start.X + (end.X - start.X) * startProgress,
            start.Y + (end.Y - start.Y) * startProgress);

        var waveEnd = new PointF(
            start.X + (end.X - start.X) * progress,
            start.Y + (end.Y - start.Y) * progress);

        using var glowPen = new Pen(
            Color.FromArgb(55, color), width * 2.5f);

        glowPen.StartCap = LineCap.Round;
        glowPen.EndCap = LineCap.Round;

        using var wavePen = new Pen(
            Color.FromArgb(230, color), width);

        wavePen.StartCap = LineCap.Round;
        wavePen.EndCap = LineCap.Round;

        graphics.DrawLine(glowPen, waveStart, waveEnd);
        graphics.DrawLine(wavePen, waveStart, waveEnd);
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


    private static void DrawCaptures(Graphics graphics, BoardGeometry geometry, CaptureAnimation? animation)
    {
        if (animation is null || !animation.IsActive)
            return;

        float progress = animation.Progress;

        foreach (var (Position, Owner) in animation.Boxes)
        {
            var point = geometry.GetDot(Position.Row, Position.Column);

            float cellSize = geometry.CellSize;
            float centerX = point.X + cellSize / 2f;
            float centerY = point.Y + cellSize / 2f;

            Color color = Owner == Player.First
                ? FirstPlayerColor
                : SecondPlayerColor;

            float fillProgress = Math.Clamp(progress * 4f, 0f, 1f);
            float scale = 0.7f + 0.3f * fillProgress;

            float size = (cellSize - 14f) * scale;

            var bounds = new RectangleF(
                centerX - size / 2f,
                centerY - size / 2f,
                size,
                size);

            using var fillBrush = new SolidBrush(
                Color.FromArgb((int)(190f * fillProgress), color));

            using var borderPen = new Pen(
                Color.FromArgb((int)(255f * fillProgress), color), 2f);

            graphics.FillRectangle(fillBrush, bounds);
            graphics.DrawRectangle(
                borderPen,
                bounds.X,
                bounds.Y,
                bounds.Width,
                bounds.Height);

            DrawCaptureText(
                graphics,
                geometry,
                centerX,
                centerY,
                progress,
                color);

            DrawCaptureParticles(
                graphics,
                geometry,
                centerX,
                centerY,
                progress,
                color);
        }
    }

    private static void DrawCaptureText(
        Graphics graphics,
        BoardGeometry geometry,
        float centerX,
        float centerY,
        float progress,
        Color color)
    {
        if (progress >= 0.85f)
            return;

        float opacity = MathF.Sin(MathF.PI * Math.Clamp(progress / 0.85f, 0f, 1f));

        int alpha = (int)(255f * opacity);

        if (alpha <= 0)
            return;

        float fontSize = Math.Clamp(geometry.CellSize * 0.3f, 12f, 32f);

        using var font = new Font(
            FontFamily.GenericSansSerif,
            fontSize,
            FontStyle.Bold,
            GraphicsUnit.Pixel);

        using var brush = new SolidBrush(Color.FromArgb(alpha, color));

        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        float rise = geometry.CellSize * 0.18f * progress;

        var bounds = new RectangleF(
            centerX - geometry.CellSize / 2f,
            centerY - geometry.CellSize / 2f - rise,
            geometry.CellSize,
            geometry.CellSize);

        graphics.DrawString("+1", font, brush, bounds, format);
    }

    private static void DrawCaptureParticles(
        Graphics graphics,
        BoardGeometry geometry,
        float centerX,
        float centerY,
        float progress,
        Color color)
    {
        if (progress < 0.12f)
            return;

        float t = Math.Clamp((progress - 0.12f) / 0.88f, 0f, 1f);

        int alpha = (int)(220f * (1f - t));

        if (alpha <= 0)
            return;

        using var brush = new SolidBrush(
            Color.FromArgb(alpha, color));

        for (int i = 0; i < 12; i++)
        {
            float angle = 2f * MathF.PI * i / 12f;

            float speed = 0.85f + (i % 3) * 0.12f;

            float distance = geometry.CellSize * (0.05f + 0.38f * t) * speed;

            float particleSize = Math.Max(
                2f,
                geometry.CellSize *
                (0.04f + (i % 2) * 0.015f) *
                (1f - t * 0.5f));

            float x = centerX + MathF.Cos(angle) * distance;
            float y = centerY + MathF.Sin(angle) * distance;

            graphics.FillRectangle(
                brush,
                x - particleSize / 2f,
                y - particleSize / 2f,
                particleSize,
                particleSize);
        }
    }
}
