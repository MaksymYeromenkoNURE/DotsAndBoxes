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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            pictureBoxBoard = new PictureBox();
            lblTurn = new Label();
            lblFirstScore = new Label();
            lblSecondScore = new Label();
            lblResult = new Label();
            btnNewGame = new Button();
            btnUndo = new Button();
            btnRedo = new Button();
            lblBoardSize = new Label();
            numBoardSize = new NumericUpDown();
            groupBox1 = new GroupBox();
            progressBarGame = new ProgressBar();
            lblProgress = new Label();
            groupBox2 = new GroupBox();
            toolTipGame = new ToolTip(components);
            animationTimer = new System.Windows.Forms.Timer(components);
            chkSoundEnabled = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBoardSize).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
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
            lblTurn.Location = new Point(18, 75);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(38, 20);
            lblTurn.TabIndex = 1;
            lblTurn.Text = "Turn";
            // 
            // lblFirstScore
            // 
            lblFirstScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFirstScore.AutoSize = true;
            lblFirstScore.Location = new Point(18, 115);
            lblFirstScore.Name = "lblFirstScore";
            lblFirstScore.Size = new Size(102, 20);
            lblFirstScore.TabIndex = 2;
            lblFirstScore.Text = "Player 1 Score";
            // 
            // lblSecondScore
            // 
            lblSecondScore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSecondScore.AutoSize = true;
            lblSecondScore.Location = new Point(184, 115);
            lblSecondScore.Name = "lblSecondScore";
            lblSecondScore.Size = new Size(102, 20);
            lblSecondScore.TabIndex = 3;
            lblSecondScore.Text = "Player 2 Score";
            // 
            // lblResult
            // 
            lblResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblResult.AutoSize = true;
            lblResult.Location = new Point(18, 35);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(86, 20);
            lblResult.TabIndex = 4;
            lblResult.Text = "Game State";
            // 
            // btnNewGame
            // 
            btnNewGame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNewGame.Location = new Point(115, 79);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(94, 30);
            btnNewGame.TabIndex = 5;
            btnNewGame.Text = "New Game";
            toolTipGame.SetToolTip(btnNewGame, "New Game (Ctrl+N)");
            btnNewGame.UseVisualStyleBackColor = true;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // btnUndo
            // 
            btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUndo.BackgroundImage = (Image)resources.GetObject("btnUndo.BackgroundImage");
            btnUndo.BackgroundImageLayout = ImageLayout.Zoom;
            btnUndo.Location = new Point(79, 79);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(30, 30);
            btnUndo.TabIndex = 6;
            toolTipGame.SetToolTip(btnUndo, "Undo (Ctrl+Z)");
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnRedo
            // 
            btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRedo.BackgroundImage = (Image)resources.GetObject("btnRedo.BackgroundImage");
            btnRedo.BackgroundImageLayout = ImageLayout.Zoom;
            btnRedo.Location = new Point(215, 79);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(30, 30);
            btnRedo.TabIndex = 7;
            toolTipGame.SetToolTip(btnRedo, "Redo (Ctrl+Y)");
            btnRedo.UseVisualStyleBackColor = true;
            btnRedo.Click += btnRedo_Click;
            // 
            // lblBoardSize
            // 
            lblBoardSize.AutoSize = true;
            lblBoardSize.Location = new Point(79, 23);
            lblBoardSize.Name = "lblBoardSize";
            lblBoardSize.Size = new Size(81, 20);
            lblBoardSize.TabIndex = 8;
            lblBoardSize.Text = "Board size:";
            // 
            // numBoardSize
            // 
            numBoardSize.Location = new Point(79, 46);
            numBoardSize.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numBoardSize.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            numBoardSize.Name = "numBoardSize";
            numBoardSize.ReadOnly = true;
            numBoardSize.Size = new Size(166, 27);
            numBoardSize.TabIndex = 9;
            numBoardSize.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(progressBarGame);
            groupBox1.Controls.Add(lblProgress);
            groupBox1.Controls.Add(lblTurn);
            groupBox1.Controls.Add(lblSecondScore);
            groupBox1.Controls.Add(lblFirstScore);
            groupBox1.Controls.Add(lblResult);
            groupBox1.Location = new Point(646, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(15);
            groupBox1.Size = new Size(304, 358);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Information";
            // 
            // progressBarGame
            // 
            progressBarGame.Location = new Point(161, 155);
            progressBarGame.Maximum = 40;
            progressBarGame.Name = "progressBarGame";
            progressBarGame.Size = new Size(125, 20);
            progressBarGame.Step = 1;
            progressBarGame.TabIndex = 6;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(18, 155);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(87, 20);
            lblProgress.TabIndex = 5;
            lblProgress.Text = "Lines: 0 / 40";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkSoundEnabled);
            groupBox2.Controls.Add(numBoardSize);
            groupBox2.Controls.Add(btnNewGame);
            groupBox2.Controls.Add(btnUndo);
            groupBox2.Controls.Add(lblBoardSize);
            groupBox2.Controls.Add(btnRedo);
            groupBox2.Location = new Point(646, 384);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(304, 216);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Controls";
            // 
            // animationTimer
            // 
            animationTimer.Interval = 16;
            animationTimer.Tick += animationTimer_Tick;
            // 
            // chkSoundEnabled
            // 
            chkSoundEnabled.AutoSize = true;
            chkSoundEnabled.Checked = true;
            chkSoundEnabled.CheckState = CheckState.Checked;
            chkSoundEnabled.Location = new Point(97, 115);
            chkSoundEnabled.Name = "chkSoundEnabled";
            chkSoundEnabled.Size = new Size(131, 24);
            chkSoundEnabled.TabIndex = 11;
            chkSoundEnabled.Text = "Sound enabled";
            chkSoundEnabled.UseVisualStyleBackColor = true;
            chkSoundEnabled.CheckedChanged += chkSoundEnabled_CheckedChanged;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 633);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(pictureBoxBoard);
            MinimumSize = new Size(820, 580);
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dots and Boxes";
            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBoardSize).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
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
        private Label lblBoardSize;
        private NumericUpDown numBoardSize;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label lblProgress;
        private ProgressBar progressBarGame;
        private ToolTip toolTipGame;
        private System.Windows.Forms.Timer animationTimer;
        private CheckBox chkSoundEnabled;
    }
}
