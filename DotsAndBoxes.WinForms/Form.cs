using DotsAndBoxes.Application.Controllers;
using DotsAndBoxes.Application.Views;
using DotsAndBoxes.Core.Models;
using DotsAndBoxes.WinForms.Rendering;

namespace DotsAndBoxes.WinForms;

public partial class Form : System.Windows.Forms.Form, IGameView
{
    private readonly GameController _controller;

    private GameState? _state;
    private Edge? _hoveredEdge;

    public Form()
    {
        InitializeComponent();

        _controller = new GameController(this);
    }

    public void Render(GameState state)
    {
        _state = state;

        int totalEdges = (state.Rows + 1) * state.Columns +
            state.Rows * (state.Columns + 1);

        int drawnEdges = state.DrawnEdges.Count;

        lblProgress.Text = $"Lines: {drawnEdges} / {totalEdges}";

        progressBarGame.Maximum = totalEdges;
        progressBarGame.Value = drawnEdges;

        lblFirstScore.Text = $"Player 1: {state.FirstPlayerScore}";
        lblSecondScore.Text = $"Player 2: {state.SecondPlayerScore}";

        lblFirstScore.ForeColor = Color.FromArgb(37, 99, 235);
        lblSecondScore.ForeColor = Color.FromArgb(234, 88, 12);

        lblTurn.ForeColor = state.CurrentPlayer == Player.First
            ? Color.FromArgb(37, 99, 235)
            : Color.FromArgb(234, 88, 12);

        if (state.IsFinished)
        {
            lblTurn.Text = "Game finished";

            lblResult.Text = state.Winner switch
            {
                Player.First => "Player 1 wins!",
                Player.Second => "Player 2 wins!",
                _ => "It's a draw!"
            };
        }
        else
        {
            lblTurn.Text = state.CurrentPlayer == Player.First
                ? "Current turn: Player 1"
                : "Current turn: Player 2";

            lblResult.Text = "Game in progress";
        }

        pictureBoxBoard.Invalidate();
    }

    public void SetHistoryAvailability(bool canUndo, bool canRedo)
    {
        btnUndo.Enabled = canUndo;
        btnRedo.Enabled = canRedo;
    }

    public void ShowError(string message)
    {
        MessageBox.Show(
            this,
            message,
            "Invalid move",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
    }

    private void pictureBoxBoard_Paint(object sender, PaintEventArgs e)
    {
        if (_state is null)
            return;

        BoardRenderer.Draw(
            e.Graphics,
            pictureBoxBoard.ClientSize,
            _state,
            _hoveredEdge);
    }

    private void pictureBoxBoard_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;

        var edge = FindAvailableEdge(e.Location);

        if (edge is Edge selectedEdge)
        {
            _controller.DrawLine(selectedEdge);
        }
    }

    private void pictureBoxBoard_MouseMove(object sender, MouseEventArgs e)
    {
        var nextHoveredEdge = FindAvailableEdge(e.Location);

        if (_hoveredEdge != nextHoveredEdge)
        {
            _hoveredEdge = nextHoveredEdge;
            pictureBoxBoard.Invalidate();
        }
    }

    private Edge? FindAvailableEdge(Point position)
    {
        if (_state is null || _state.IsFinished)
            return null;

        var geometry = BoardGeometry.Create(
            _state.Rows,
            _state.Columns,
            pictureBoxBoard.ClientSize);

        if (!geometry.TryFindEdge(position, out var edge))
            return null;

        if (_state.DrawnEdges.Contains(edge))
            return null;

        return edge;
    }

    private void pictureBoxBoard_MouseLeave(object sender, EventArgs e)
    {
        _hoveredEdge = null;
        pictureBoxBoard.Invalidate();
    }

    private void btnNewGame_Click(object sender, EventArgs e)
    {
        StartNewGame();
    }

    private void btnUndo_Click(object sender, EventArgs e) => _controller.Undo();

    private void btnRedo_Click(object sender, EventArgs e) => _controller.Redo();


    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.Z))
        {
            _controller.Undo();
            return true;
        }

        if (keyData == (Keys.Control | Keys.Y))
        {
            _controller.Redo();
            return true;
        }

        if (keyData == (Keys.Control | Keys.N))
        {
            StartNewGame();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }


    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _controller.Dispose();
        base.OnFormClosed(e);
    }

    private void StartNewGame()
    {
        if (_state != null && !_state.IsFinished && _state.DrawnEdges.Count > 0)
        {
            var result = MessageBox.Show(
                this,
                "Are you sure you want to start a new game?",
                "New Game",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                return;
        }

        _hoveredEdge = null;

        int boardSize = (int)numBoardSize.Value;

        _controller.NewGame(boardSize, boardSize);
    }
}
