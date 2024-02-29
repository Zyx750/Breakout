using Godot;

public partial class Game : Node
{
	Ball ball;
	Paddle paddle;
	TopBar topBar;
	bool launched = false;
	[Export]
	int lives = 2;
	public override void _Ready()
	{
		ball = GetNode<Ball>("Ball");
		paddle = GetNode<Paddle>("Paddle");
		topBar = GetNode<TopBar>("TopBar");
		topBar.updateLives(lives);
	}

    public override void _Process(double delta)
    {
		if(!launched) {
			ball.Position = paddle.Position + new Vector2(0,-40);
			if(Input.IsActionPressed("launch")) {
				float input = Input.GetAxis("left", "right");
				ball.velocity = new Vector2(input, -1).Normalized() * ball.speed;
				launched = true;
			}
		}
    }

	private void OnBallLost() {
		if(lives > 0) {
			lives--;
			topBar.updateLives(lives);
			launched = false;
			ball.velocity = new Vector2();
		}
		else {
			GD.Print("Game over");
			ball.QueueFree();
		}
	}
}