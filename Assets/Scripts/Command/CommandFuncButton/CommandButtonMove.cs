public class CommandButtonMove : Command
{
    public CommandButtonMove(InputManager _inputMng)
    {
        inputMng = _inputMng;
    }

    public override void Execute(params object[] _value)
    {
        inputMng.OnClickMoveButton();

    }

    private InputManager inputMng = null;
}
