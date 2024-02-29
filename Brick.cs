using Godot;

[Tool]
public partial class Brick : StaticBody2D
{
	static readonly string[] colors = {"slate blue", "light sky blue", "MediumSeaGreen", "orange", "IndianRed"};

	[Export(PropertyHint.Range, "1, 5,")]
	public int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            GetNode<ColorRect>("ColorRect").Color = new Color(colors[maxHealth-1]);
        }
    }
	int maxHealth = 1;
	int health;

	[Signal]
	public delegate void OnBreakEventHandler(int maxHealth);
	
    public override void _Ready()
    {
        health = maxHealth;
		GetNode<ColorRect>("ColorRect").Color = new Color(colors[health-1]);
    }

	//returns true if the brick is broken
	public bool Hit() {
		health--;
		if(health <= 0) {
			EmitSignal("OnBreak", maxHealth);
			QueueFree();
			return true;
		}
		GetNode<ColorRect>("ColorRect").Color = new Color(colors[health-1]);
		return false;
	}
}
