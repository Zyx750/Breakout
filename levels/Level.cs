using Godot;

public partial class Level : Node2D
{
	int bricks;
	[Signal]
	public delegate void OnBrickBreakEventHandler(int score);
	[Signal]
	public delegate void OnLevelWinEventHandler();

	public override void _Ready()
	{
		
		foreach(Node c in GetChildren()) {
			if(c is Brick b) {
				bricks++;
				b.OnBreak += OnBreak;
			}
			else {
				foreach(Node c2 in c.GetChildren()) {
					if(c2 is Brick b2) {
						bricks++;
						b2.OnBreak += OnBreak;
					}
				}
			}
		}
	}

	void OnBreak(int maxHealth) {
		EmitSignal("OnBrickBreak", maxHealth);
		bricks--;
		if(bricks == 0) {
			EmitSignal("OnLevelWin");
		}
	}
}
