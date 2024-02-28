using Godot;

public partial class Ball : CharacterBody2D
{
    [Export]
    public float speed = 800f;
    Vector2 screenSize;
    public Vector2 velocity;
    float height;
    int dir = 1;
    AudioStreamPlayer bounceSfx;
	AudioStreamPlayer breakSfx;
    [Signal]
    public delegate void OffscreenEventHandler();


    public override void _Ready()
    {
		screenSize = GetViewportRect().Size;
        height = (GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D).Size.Y;
        bounceSfx = GetNode<AudioStreamPlayer>("Bounce");
		breakSfx = GetNode<AudioStreamPlayer>("Break");
    }

    public override void _PhysicsProcess(double delta)
	{
        var colInfo = MoveAndCollide(velocity * (float)delta);
        //Position += velocity * (float)delta;
        if(Position.Y < height/2+100) {
            velocity = velocity.Bounce(Vector2.Up);
            Position = new Vector2(Position.X, height/2+100);
            bounceSfx.Play();
        }
        else if (Position.Y > screenSize.Y+height*5) {
            EmitSignal("Offscreen");
        }
        if(Position.X < height/2) {
            velocity = velocity.Bounce(Vector2.Right);
            Position = new Vector2(height/2, Position.Y);
            bounceSfx.Play();
        }
        else if(Position.X > screenSize.X - height/2) {
            velocity = velocity.Bounce(Vector2.Left);
            Position = new Vector2(screenSize.X - height/2, Position.Y);
            bounceSfx.Play();
        }

        if (colInfo != null) {
            if(colInfo.GetCollider() is Paddle p) {
                if(colInfo.GetNormal() == Vector2.Up) {
                    Vector2 dir = new Vector2(Position.X - p.Position.X, -100).Normalized();
                    velocity = dir * speed;
                }
                else {
                    velocity = velocity.Bounce(colInfo.GetNormal());
                    Position = Position+colInfo.GetNormal()*5;
                }
                bounceSfx.Play();
            }
            else if(colInfo.GetCollider() is Brick b) {
                velocity = velocity.Bounce(colInfo.GetNormal());
                if(b.Hit()) breakSfx.Play();
                else bounceSfx.Play();
            }
        }

	}
}
