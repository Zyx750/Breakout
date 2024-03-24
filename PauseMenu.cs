using Godot;

public partial class PauseMenu : Control
{
	bool canResume = true;
	[Signal]
	public delegate void OnRestartEventHandler();
	[Signal]
	public delegate void OnResumeEventHandler();
	public void Pause(bool GameOver = false, int score = -1) {
		if(GameOver) {
			GetNode<Label>("Resume").Text = "Final score: " + score;
			GetNode<Label>("GameStatus").Text = "Game Over";
			canResume = false;
		}
		else {
			GetNode<Label>("Resume").Text = "Press Escape to resume";
			GetNode<Label>("GameStatus").Text = "Game Paused";
			canResume = true;
		}
	}

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("pause")) {
			EmitSignal("OnResume");
			QueueFree();
			return;
		}
		if(Input.IsActionJustPressed("restart")) {
			EmitSignal("OnRestart");
			QueueFree();
			return;
		}
    }
}
