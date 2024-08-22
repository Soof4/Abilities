import { ReactNode } from "react";
import CodeBlock from "../components/CodeBlock";

function ExampleProject(): ReactNode {
  return (
    <>
      <p className="fs-3">Example Project</p>
      <p>
        Abilities can be implemented into your project in many different ways
        but below we'll use a Dictionary&lt;string, AbilityType&gt; object as a
        way to record what type each player chose. We'll use whoopie cushion as
        the ability activator so whenever player uses it, they'll cast their
        ability. We'll also add a command for players that will handle ability
        changes.
      </p>

      <p>
        First of all we would need to hook into PlayerUpdate handler so we can
        keep track of whatever the player has used a whoopie cushion or not.
        Plus, We'll define our ability changer command in here:
      </p>

      <CodeBlock
        content='// ...

public static Dictionary<string, AbilityType> PlayerAbilityTypePairs = new();

public override void Initialize()
{
    GetDataHandlers.PlayerUpdate += OnPlayerUpdate;
    TShockAPI.Commands.ChatCommands.Add(new("exampleproject.changeability", ChangeAbilityCmd, "changeability")
    {
        HelpText = "This command changes your ability. Usage: /changeability <ability name>"
    });
}

// ...'
      />

      <p>
        Now that we have defined our basic needs, we'll hop into implementing
        these two methods. Starting with the easier one, let's implement the
        changeability command. What we trying to achieve here is that if player
        exists in our dictionary then change the ability type to what they want,
        if they don't exist in our dictionary then we'll need to add them to the
        dictionary:
      </p>
      <CodeBlock
        content='// ...

public static void ChangeAbilityCmd(CommandArgs args) {
    // First we need to handle the whatever we have anything in parameter or not
    if (args.Parameters.Count < 1) {
        args.Player.SendErrorMessage("Invalid syntax. Usage: /changeability <ability name>" + 
            "Valid ability types are: dryad, twilight, pentagram"    // We will only use these three abilities in order to keep this simple
            );
        return;
    }
    
    
}

// ...'
      />
    </>
  );
}

export default ExampleProject;
