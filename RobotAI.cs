using System;
using TriCAT;

public class SampleRobot : IRobotPlugin
{
	// Name der Lerngruppe, die dieses Plugin entwickelt
	public string GroupName { get { return "Test Group"; } }

	// Liste mit Email Adressen der Authoren
	public string[] Authors { get { return new string[] { "max@mustermann.de", "john@doe.com" }; } }

	// Versionsnummer des Plugins
	public int VersionMajor { get { return 1; } }
	public int VersionMinor { get { return 0; } }

	// Diese Interfaces bekommt man beim Start übergeben und sollte sie sich zur weiteren verwendung speichern.
	private IApplication App;
	private IRobot Robot;
	private IPlayingField Field;

	// Zum Speichern des aktuellen Steins
	private StoneData currentStone = new StoneData();

	/// <summary>
	/// Called once, after the plugin has been loaded
	/// </summary>
	/// <param name="app">Provides some debug methods like message logging and line drawing</param>
	/// <param name="robot">Holds the state of the actual robot and exposes some driving controls</param>
	/// <param name="field">Holds the state of the playing field, like active stones and target area</param>
	public void OnInit(IApplication app, IRobot robot, IPlayingField field)
	{
		// Über diese interfaces kann später (OnTick) auf Daten wie die Position der Steine auf dem Feld usw. zugegriffen werden
		App = app;
		Robot = robot;
		Field = field;

		App.LogMessage("Sample Robot initialized");
	}

	/// <summary>
	/// Called everytime, the user clicks the "Reset" button.
	/// </summary>
	public void OnReset()
	{
		App.LogMessage("Sample Robot resetted");
	}

	/// <summary>
	/// Called once for every simulation step.
	/// This is the place to implement your robot steering logic.
	/// </summary>
	public void OnTick()
	{
		// In dieser Funktion kann die KI des Roboters implementiert werden, z.B. als State Machine.
		// Die KI muss jeden Tick die Entscheidung treffen, was der Roboter tun soll, zur Wahl stehen: links / rechts drehen, vorwärts / rückwärts fahren, gar nichts tun
		// z.B. im Kreis zu fahren:

		// update local copy of the stone
		if (currentStone.Id != -1) currentStone = Field.GetStoneById(currentStone.Id);

		Robot.GoForward();
		Robot.TurnLeft();
	}


	/// <summary>
	/// Called once before the plugin is unloaded
	/// </summary>
	public void OnRelease()
	{
		App.LogMessage("Sample Robot released");

		App = null;
		Robot = null;
		Field = null;
	}
}

