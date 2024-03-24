using System;
using Godot;

public partial class Game : Node
{
	Ball ball;
	Paddle paddle;
	TopBar topBar;
	bool launched = false;
	[Export]
	uint lives = 2;
	[Export(PropertyHint.Range, "1, 5,")]
	uint level = 1;
	uint score = 0;
	uint highscore = 0;
	AudioStreamPlayer LevelWinSfx;
	public override void _Ready()
	{
		ball = GetNode<Ball>("Ball");
		paddle = GetNode<Paddle>("Paddle");
		topBar = GetNode<TopBar>("TopBar");
		LevelWinSfx = GetNode<AudioStreamPlayer>("LevelWin");
		topBar.updateLives(lives);

		using var file = FileAccess.Open("highscore.dat", FileAccess.ModeFlags.Read);
		if(file != null) {
			highscore = file.Get32();
			topBar.updateHighscore(highscore);
		}

		if(level > 1) {
			SwitchLevel(level, 1);
		}
	}

    public override void _Process(double delta)
    {
		if(!launched) {
			ball.Position = paddle.Position + new Vector2(0,-40);
			if(Input.IsActionPressed("launch")) {
				float input = Input.GetAxis("left", "right");
				ball.velocity = new Vector2(input, -1).Normalized().Rotated((float)Math.PI * 0.05f * GD.RandRange(-1,1)) * ball.speed;
				launched = true;
			}
		}

		if(Input.IsActionJustPressed("pause")) {
			PauseMenu pauseMenu = ResourceLoader.Load<PackedScene>("pause_menu.tscn").Instantiate() as PauseMenu;
			AddChild(pauseMenu);
			pauseMenu.Pause();
			GetTree().Paused = true;
			pauseMenu.OnResume += () => GetTree().Paused = false;
			pauseMenu.OnRestart += () => {
				if(score > highscore) {
					highscore = score;
					using var file = FileAccess.Open("highscore.dat", FileAccess.ModeFlags.Write);
					file.Store32(highscore);
				}
				RestartGame();
			};
		}
    }

	private void OnBallLost() {
		if(lives > 0) {
			lives--;
			topBar.updateLives(lives);
			ResetBall();
		}
		else {
			ball.QueueFree();
			GameOver();
		}
	}

	private void GameOver() {
		if(score > highscore) {
			highscore = score;
			using var file = FileAccess.Open("highscore.dat", FileAccess.ModeFlags.Write);
			file.Store32(highscore);
		}
	}

	private void ResetBall() {
		launched = false;
		ball.velocity = new Vector2();
	}

	private void OnLevelWin() {
		lives++;
		topBar.updateLives(lives);
		ResetBall();
		LevelWinSfx.Play();
		SwitchLevel(level+1, level);
	}

	private void OnBrickBreak(uint score) {
		this.score += score;
		topBar.updateScore(this.score);
		if(this.score > highscore) {
			topBar.updateHighscore(this.score);
		}
	}

	private void SwitchLevel(uint level, uint prev) {
		if(level > 5) {
			level = 1;
			ball.speed *= 1.2f;
		}
		GetNode("Level"+prev)?.QueueFree();
		Level l = ResourceLoader.Load<PackedScene>($"levels/level{level}.tscn").Instantiate() as Level;
		AddChild(l);
		l.OnBrickBreak += OnBrickBreak;
		l.OnLevelWin += OnLevelWin;
		this.level = level;
		topBar.changeLevel(level);
	}

	private void RestartGame() {
		GD.Print("test");
		score = 0;
		topBar.updateScore(score);
		SwitchLevel(1, level);
		ResetBall();
		lives = 2;
		topBar.updateLives(lives);
		GetTree().Paused = false;
	}
}