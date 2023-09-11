public class CommandButtonMove : Command
{
    public CommandButtonMove(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }
    public override void Execute()
    {
        inputMng.OnClickMoveButton();
    }

    private InputManager inputMng = null;
}
