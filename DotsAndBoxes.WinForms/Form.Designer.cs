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
            chkSoundEnabled = new CheckBox();
            toolTipGame = new ToolTip(components);
            animationTimer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBoard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBoardSize).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxBoard
            // 
            pictureBoxBoard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxBoard.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxBoard.Location = new Point(3, 3);
            pictureBoxBoard.Name = "pictureBoxBoard";
            tableLayoutPanel1.SetRowSpan(pictureBoxBoard, 2);
            pictureBoxBoard.Size = new Size(620, 582);
            pictureBoxBoard.TabIndex = 0;
            pictureBoxBoard.TabStop = false;
            pictureBoxBoard.Paint += pictureBoxBoard_Paint;
            pictureBoxBoard.MouseClick += pictureBoxBoard_MouseClick;
            pictureBoxBoard.MouseLeave += pictureBoxBoard_MouseLeave;
            pictureBoxBoard.MouseMove += pictureBoxBoard_MouseMove;
            // 
            // lblTurn
            // 
            lblTurn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTurn.AutoSize = true;
            lblTurn.Location = new Point(20, 75);
            lblTurn.Name = "lblTurn";
            lblTurn.Size = new Size(38, 20);
            lblTurn.TabIndex = 1;
            lblTurn.Text = "Turn";
            // 
            // lblFirstScore
            // 
            lblFirstScore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblFirstScore.AutoSize = true;
            lblFirstScore.Location = new Point(20, 115);
            lblFirstScore.Name = "lblFirstScore";
            lblFirstScore.Size = new Size(102, 20);
            lblFirstScore.TabIndex = 2;
            lblFirstScore.Text = "Player 1 Score";
            // 
            // lblSecondScore
            // 
            lblSecondScore.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblSecondScore.AutoSize = true;
            lblSecondScore.Location = new Point(186, 115);
            lblSecondScore.Name = "lblSecondScore";
            lblSecondScore.Size = new Size(102, 20);
            lblSecondScore.TabIndex = 3;
            lblSecondScore.Text = "Player 2 Score";
            // 
            // lblResult
            // 
            lblResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblResult.AutoSize = true;
            lblResult.Location = new Point(20, 35);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(86, 20);
            lblResult.TabIndex = 4;
            lblResult.Text = "Game State";
            // 
            // btnNewGame
            // 
            btnNewGame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnNewGame.Location = new Point(117, 79);
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
            btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnUndo.BackgroundImage = (Image)resources.GetObject("btnUndo.BackgroundImage");
            btnUndo.BackgroundImageLayout = ImageLayout.Zoom;
            btnUndo.Location = new Point(81, 79);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(30, 30);
            btnUndo.TabIndex = 6;
            toolTipGame.SetToolTip(btnUndo, "Undo (Ctrl+Z)");
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnRedo
            // 
            btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnRedo.BackgroundImage = (Image)resources.GetObject("btnRedo.BackgroundImage");
            btnRedo.BackgroundImageLayout = ImageLayout.Zoom;
            btnRedo.Location = new Point(217, 79);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(30, 30);
            btnRedo.TabIndex = 7;
            toolTipGame.SetToolTip(btnRedo, "Redo (Ctrl+Y)");
            btnRedo.UseVisualStyleBackColor = true;
            btnRedo.Click += btnRedo_Click;
            // 
            // lblBoardSize
            // 
            lblBoardSize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblBoardSize.AutoSize = true;
            lblBoardSize.Location = new Point(79, 23);
            lblBoardSize.Name = "lblBoardSize";
            lblBoardSize.Size = new Size(81, 20);
            lblBoardSize.TabIndex = 8;
            lblBoardSize.Text = "Board size:";
            // 
            // numBoardSize
            // 
            numBoardSize.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(progressBarGame);
            groupBox1.Controls.Add(lblProgress);
            groupBox1.Controls.Add(lblTurn);
            groupBox1.Controls.Add(lblSecondScore);
            groupBox1.Controls.Add(lblFirstScore);
            groupBox1.Controls.Add(lblResult);
            groupBox1.Location = new Point(629, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(15);
            groupBox1.Size = new Size(306, 321);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Information";
            // 
            // progressBarGame
            // 
            progressBarGame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBarGame.Location = new Point(161, 155);
            progressBarGame.Maximum = 40;
            progressBarGame.Name = "progressBarGame";
            progressBarGame.Size = new Size(125, 20);
            progressBarGame.Step = 1;
            progressBarGame.TabIndex = 6;
            // 
            // lblProgress
            // 
            lblProgress.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(18, 155);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(87, 20);
            lblProgress.TabIndex = 5;
            lblProgress.Text = "Lines: 0 / 40";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(chkSoundEnabled);
            groupBox2.Controls.Add(numBoardSize);
            groupBox2.Controls.Add(btnNewGame);
            groupBox2.Controls.Add(btnUndo);
            groupBox2.Controls.Add(lblBoardSize);
            groupBox2.Controls.Add(btnRedo);
            groupBox2.Location = new Point(629, 330);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(306, 255);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Controls";
            // 
            // chkSoundEnabled
            // 
            chkSoundEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
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
            // animationTimer
            // 
            animationTimer.Interval = 16;
            animationTimer.Tick += animationTimer_Tick;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.73774F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.26226F));
            tableLayoutPanel1.Controls.Add(pictureBoxBoard, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 1);
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 261F));
            tableLayoutPanel1.Size = new Size(938, 588);
            tableLayoutPanel1.TabIndex = 12;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 633);
            Controls.Add(tableLayoutPanel1);
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
            tableLayoutPanel1.ResumeLayout(false);
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
        private TableLayoutPanel tableLayoutPanel1;
    }
}
