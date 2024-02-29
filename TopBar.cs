using Godot;
using System;

public partial class TopBar : Control
{
	Label level;
	Label score;
	Label highscore;
	Node hearts;
    public override void _Ready()
    {
        level = GetNode<Label>("HBoxContainer/Level/Number");
        score = GetNode<Label>("HBoxContainer/Score/Current");
        highscore = GetNode<Label>("HBoxContainer/Score/Best");
		hearts = GetNode("HBoxContainer/Lives/MarginContainer/Hearts");
    }
    public void updateLives(int lives) {
		if(lives < 0) return;
		if(lives > 6) lives = 6;
		if(lives < hearts.GetChildCount()) {
			hearts.GetChildren();
			for(int i = hearts.GetChildCount(); i > lives; i--) {
				hearts.GetChild(i-1).QueueFree();
			}
		}
		else {
			while(lives > hearts.GetChildCount()) {
				hearts.AddChild(ResourceLoader.Load<PackedScene>("res://heart.tscn").Instantiate());
			}
		}
	}
	public void changeLevel(int level) {
		this.level.Text = level.ToString();
	}
	public void updateScore(int score) {
		this.score.Text = "Current score: " + score.ToString();
	}
	public void updateHighscore(int score) {
		highscore.Text = "Highscore: " + score.ToString();
	}
}
