using Godot;
public partial class Paddle : CharacterBody2D
{
	[Export]
	private float speed = 400.0f;
	private Vector2 screenSize;
	private float width;
	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
		width = (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		float input = Input.GetAxis("left", "right");
		Vector2 velocity = new Vector2(input * speed, 0);
		Position += velocity * (float)delta;
		Position = new Vector2(Mathf.Clamp(Position.X, width/2, screenSize.X-width/2), Position.Y);
	}
}
