namespace DotsAndBoxes.WinForms
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBoxBoard = new PictureBox();
            lblTurn = new Label();
            lblFirstScore = new Label();
            lblSecondScore = new Label();
            lblResult = new Label();
            btnNewGame = new Button();
            btnUndo = new Button();
            btnRedo = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxBoard
            // 
            pictureBoxBoard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxBoard.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxBoard.Location = new Point(20, 20);
            pictureBoxBoard.Name = "pictureBoxBoard";
            pictureBoxBoard.Size = new Size(620, 580);
            pictureBoxBoard.TabIndex = 0;
            pictureBoxBoard.TabStop = false;
            pictureBoxBoard.Paint += pictureBoxBoard_Paint;
            pictureBoxBoard.MouseClick += pictureBoxBoard_MouseClick;
            pictureBoxBoard.MouseLeave += pictureBoxBoard_MouseLeave;
            pictureBoxBoard.MouseMove += pictureBoxBoard_MouseMove;
            // 
            // lblTurn
            // 
            lblTurn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTurn.AutoSize = true;
            lblTurn.Location = new Point(720, 20);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(38, 20);
            lblTurn.TabIndex = 1;
            lblTurn.Text = "Turn";
            // 
            // lblFirstScore
            // 
            lblFirstScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFirstScore.AutoSize = true;
            lblFirstScore.Location = new Point(720, 60);
            lblFirstScore.Name = "lblFirstScore";
            lblFirstScore.Size = new Size(87, 20);
            lblFirstScore.TabIndex = 2;
            lblFirstScore.Text = "Pler 1 Score";
            // 
            // lblSecondScore
            // 
            lblSecondScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSecondScore.AutoSize = true;
            lblSecondScore.Location = new Point(720, 100);
            lblSecondScore.Name = "lblSecondScore";
            lblSecondScore.Size = new Size(87, 20);
            lblSecondScore.TabIndex = 3;
            lblSecondScore.Text = "Pler 2 Score";
            // 
            // lblResult
            // 
            lblResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblResult.AutoSize = true;
            lblResult.Location = new Point(720, 140);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(49, 20);
            lblResult.TabIndex = 4;
            lblResult.Text = "Result";
            // 
            // btnNewGame
            // 
            btnNewGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNewGame.Location = new Point(720, 190);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(94, 29);
            btnNewGame.TabIndex = 5;
            btnNewGame.Text = "New Game";
            btnNewGame.UseVisualStyleBackColor = true;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // btnUndo
            // 
            btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUndo.Location = new Point(720, 240);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(94, 29);
            btnUndo.TabIndex = 6;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnRedo
            // 
            btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRedo.Location = new Point(720, 290);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(94, 29);
            btnRedo.TabIndex = 7;
            btnRedo.Text = "Redo";
            btnRedo.UseVisualStyleBackColor = true;
            btnRedo.Click += btnRedo_Click;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 633);
            Controls.Add(btnRedo);
            Controls.Add(btnUndo);
            Controls.Add(btnNewGame);
            Controls.Add(lblResult);
            Controls.Add(lblSecondScore);
            Controls.Add(lblFirstScore);
            Controls.Add(lblTurn);
            Controls.Add(pictureBoxBoard);
            MinimumSize = new Size(820, 580);
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dots and Boxes";
            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxBoard;
        private Label lblTurn;
        private Label lblFirstScore;
        private Label lblSecondScore;
        private Label lblResult;
        private Button btnNewGame;
        private Button btnUndo;
        private Button btnRedo;
    }
}
