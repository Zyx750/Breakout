using System;
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class Game : Node
{
	Ball ball;
	Paddle paddle;
	TopBar topBar;
	bool launched = false;
	[Export]
	uint lives = 2;
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
		highscore = file.Get32();
		topBar.updateHighscore(highscore);
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
		GD.Print("Game over");
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
	}

	private void OnBrickBreak(uint score) {
		this.score += score;
		topBar.updateScore(this.score);
		if(this.score > highscore) {
			topBar.updateHighscore(this.score);
		}
	}
}