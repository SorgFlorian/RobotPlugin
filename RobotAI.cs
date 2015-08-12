using System;
using TriCAT;

public class SampleRobot : IRobotPlugin
{
	// Name der Lerngruppe, die dieses Plugin entwickelt
	public string GroupName { get { return "Gruppe"; } }
    /**********************/
    // github.com/77adnap
    /**********************/
    // Liste mit Email Adressen der Authoren
    public string[] Authors { get { return new string[] {"Florian Sorg"}; } }

	// Versionsnummer des Plugins
	public int VersionMajor { get { return 1; } }
	public int VersionMinor { get { return 0; } }

    enum RobotState {SearchingForStone, MoveToTargetArea, AllStonesFound, Init, Back};
    RobotState currentRobotState;

    public StoneData [] stones;

	// Diese Interfaces bekommt man beim Start übergeben und sollte sie sich zur weiteren verwendung speichern.
	private IApplication App;
	private IRobot Robot;
	private IPlayingField Field;

	private StoneData currentStone = new StoneData();
    private Vector currentStonePos;
    private int stonesInTargetArea = 0;
    private int back = 0;
    private int push = 0;

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

        currentRobotState = RobotState.Init;

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
       // if (currentStone.Id != -1)
        //{
          //  currentStone = Field.GetStoneById(currentStone.Id);
        //}


        if (currentRobotState == RobotState.Init)
        {
            SearchForNextStone();
            currentRobotState = RobotState.SearchingForStone;
        }
        else if(currentRobotState == RobotState.SearchingForStone)
        {
            MoveToStone();
        }
        else if (currentRobotState == RobotState.MoveToTargetArea)
        {
            MoveToTargetArea(); 
        }
        else if (currentRobotState == RobotState.AllStonesFound)
        {
            App.LogMessage("All Stones Found");
        }
        else if (currentRobotState == RobotState.Back)
        {
            Back();
        }
	}

    public void MoveToStone()
    {
        
        Vector x = currentStonePos - Robot.Data.Position;
        Vector y = x;
        x.Normalize();
        float result = Vector.Dot(Robot.Data.Right, x);

        
        if(Vector.Dot(Robot.Data.Forward, x) < 0 && y.Length() < 0.2 )
        {
            // Collect two stones at once (if two stones are available)
            if (push < 1 && stonesInTargetArea < 3)
            {
                SearchForNextStone();
                push++;
            }
            else
            {
                currentRobotState = RobotState.MoveToTargetArea;
                // Reset so the next collection cycle also collects two stones (if possible)
                push = 0;
            }    
        }
        else
        {
            MoveRobot(result);
        }


    }

    public void MoveToTargetArea()
    {

        Vector x = Field.TargetArea.Position - Robot.Data.Position;
        Vector y = x;
        x.Normalize();
        float result = Vector.Dot(Robot.Data.Right, x);


        if ( y.Length() < Field.TargetArea.Radius )
        {
            currentRobotState = RobotState.Back;  
        }
        else
        {
            MoveRobot(result);
        } 
    }


    public void SearchForNextStone()
    {
        Vector y = new Vector(10000,10000,10000);
        stonesInTargetArea = 0;
        for (int i = 0; i < 4; i++)
        {
            StoneData stone = Field.GetStoneById(i);
            Vector x = stone.Position - Field.TargetArea.Position;
            Vector z = stone.Position - Robot.Data.Position;
            App.LogMessage("Stone with id:" + i + " is " + z.Length() +" away");
            // Dont collect a stone already in the target area
            if (x.Length() > Field.TargetArea.Radius)
            {
                // Check if the stone is the closest to the robot and that it isnt already in the collection "basket"
                if (z.Length() < y.Length() && z.Length()>0.2)
                {
                    y = z;
                    currentStone = stone;
                    currentStonePos = currentStone.Position;
                    App.LogMessage("get id" + Field.GetStoneById(i).Id);
                }
            }
            else
            {
                stonesInTargetArea++;
            }
        }
        App.LogMessage("Next Stone is: " + currentStone.Id);     
    }

    public void Back()
    {
        if (back < 20)
        {
            Robot.GoBackward();
            back++;
        }
        else
        {
            back = 0;
            currentRobotState = RobotState.SearchingForStone;
            SearchForNextStone();
        }
    }

    public void MoveRobot(float result)
    {
        
        
        if (result > 0)
        {
            //rechts drehen
            
            Robot.TurnRight();
            Robot.GoForward();
        }

        if (result < 0)
        {
            //links drehen
            Robot.TurnLeft();
            Robot.GoForward();
          
        }

        if (result == 0)
        {
            Robot.GoForward();
        }
        

    
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
