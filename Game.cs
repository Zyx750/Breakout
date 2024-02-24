using Godot;

public partial class Game : Node
{
	Ball ball;
	Paddle paddle;
	bool launched = false;
	public override void _Ready()
	{
		ball = GetNode<Ball>("Ball");
		paddle = GetNode<Paddle>("Paddle");
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
		GD.Print("Game over");
		GetNode<Ball>("Ball").QueueFree();
	}
}