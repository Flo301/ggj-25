using Godot;
using System;
using System.Threading.Tasks;

public partial class FlyingPointsLabel : Label
{
	public float speed = 120f;
	public float width = 30f;
	public float lifespan = 1.1f;

	private Vector2 StartPos;
	public void Init(int points, Vector2 pos)
	{
		GlobalPosition = pos;
		Text = points + "P";
		StartPos = Position - Vector2.Right * GetWidthDifference();

		//autokill
		Task.Delay(TimeSpan.FromSeconds(lifespan)).ContinueWith(_ =>
		{
			QueueFree();
		});
	}

	private float GetWidthDifference()
	{
		return width * (float)Math.Sin(Time.GetTicksMsec() / 100f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = new Vector2(StartPos.X + GetWidthDifference(), Position.Y - speed * (float)delta);
	}
}
