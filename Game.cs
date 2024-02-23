using Godot;

public partial class Game : Node
{
	public override void _Ready()
	{
		GetNode<Ball>("Ball").velocity = new Vector2(0,1).Normalized() * GetNode<Ball>("Ball").speed;
	}

	private void OnBallLost() {
		GD.Print("Game over");
		GetNode<Ball>("Ball").QueueFree();
	}
}