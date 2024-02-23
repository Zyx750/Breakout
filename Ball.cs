using Godot;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float speed = 500f;
	private Vector2 screenSize;
    public Vector2 velocity;
    private float height;
    private int dir = 1;
    private AudioStreamPlayer bounce;
    [Signal]
    public delegate void OffscreenEventHandler();


    public override void _Ready()
    {
		screenSize = GetViewportRect().Size;
        height = (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;
        bounce = GetNode<AudioStreamPlayer>("Bounce");
    }

    public override void _PhysicsProcess(double delta)
	{
        var colInfo = MoveAndCollide(velocity * (float)delta);
        //Position += velocity * (float)delta;
        if(Position.Y < height/2+100) {
            velocity = velocity.Bounce(Vector2.Up);
            Position = new Vector2(Position.X, height/2+100);
            bounce.Play();
        }
        else if (Position.Y > screenSize.Y+height*2) {
            EmitSignal("Offscreen");
        }
        if(Position.X < height/2) {
            velocity = velocity.Bounce(Vector2.Right);
            Position = new Vector2(height/2, Position.Y);
            bounce.Play();
        }
        else if(Position.X > screenSize.X - height/2) {
            velocity = velocity.Bounce(Vector2.Left);
            Position = new Vector2(screenSize.X - height/2, Position.Y);
            bounce.Play();
        }

        if (colInfo != null) {
            if(colInfo.GetCollider() is Paddle p) {
                Vector2 dir = new Vector2(Position.X - p.Position.X, -100).Normalized();
                velocity = dir * speed;
                bounce.Play();
            }
            else {

            }
        }

	}
}
